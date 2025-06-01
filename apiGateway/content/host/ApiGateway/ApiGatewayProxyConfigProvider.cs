using ApiGateway.Queries;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Dedsi.ApiGateway;

public class ApiGatewayProxyConfigProvider : IProxyConfigProvider
{
    private readonly Timer _timer;

    private ApiGatewayConfig _config;

    private readonly IAgRouteConfigQuery _agRouteConfigQuery;

    private readonly IAgClusterConfigQuery _agClusterConfigQuery;

    public ApiGatewayProxyConfigProvider(
        IConfiguration configuration,
        IAgRouteConfigQuery agRouteConfigQuery,
        IAgClusterConfigQuery agClusterConfigQuery)
    {
        _agRouteConfigQuery = agRouteConfigQuery ?? throw new ArgumentNullException(nameof(agRouteConfigQuery));
        _agClusterConfigQuery = agClusterConfigQuery ?? throw new ArgumentNullException(nameof(agClusterConfigQuery));

        _config = CreateConfigAsync().GetAwaiter().GetResult();

        // 刷新频率：分钟
        var timerMinutes = configuration.GetValue<int>("Apps:TimerMinutes");
        _timer = new Timer(_ => RefreshConfig(), null, TimeSpan.FromMinutes(timerMinutes), TimeSpan.FromMinutes(timerMinutes));
    }

    /// <summary>
    /// 刷新配置
    /// </summary>
    public void RefreshConfig()
    {
        var newConfig = CreateConfigAsync().GetAwaiter().GetResult();
        var oldConfig = Interlocked.Exchange(ref _config, newConfig);
        oldConfig.SignalChange();
    }

    public void Dispose() => _timer.Dispose();

    public IProxyConfig GetConfig() => _config;

    private async Task<ApiGatewayConfig> CreateConfigAsync()
    {
        var routeConfigs = (await _agRouteConfigQuery.GetAllAsync())
            .Select(a => new RouteConfig()
            {
                RouteId = a.RouteId,
                ClusterId = a.ClusterId,
                Match = new RouteMatch
                {
                    Path = a.Match.Path
                }
            })
            .ToArray();

        var clusterConfigs = (await _agClusterConfigQuery.GetAllAsync())
            .Select(a => new ClusterConfig
            {
                ClusterId = a.ClusterId,
                Destinations = a.Destinations.ToDictionary(
                    d => d.DestinationId,
                    d => new DestinationConfig { Address = d.Address }
                )
            })
            .ToArray();

        return new ApiGatewayConfig(routeConfigs, clusterConfigs, Guid.CreateVersion7().ToString());
    }

    private sealed class ApiGatewayConfig : IProxyConfig
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ApiGatewayConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, string revisionId)
        {
            RevisionId = revisionId ?? throw new ArgumentNullException(nameof(revisionId));
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        /// <inheritdoc/>
        public string RevisionId { get; }

        public IReadOnlyList<RouteConfig> Routes { get; }

        /// <summary>
        /// A snapshot of the list of Clusters which are collections of interchangeable destination endpoints
        /// </summary>
        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken { get; }

        internal void SignalChange()
        {
            _cts.Cancel();
        }
    }
}
