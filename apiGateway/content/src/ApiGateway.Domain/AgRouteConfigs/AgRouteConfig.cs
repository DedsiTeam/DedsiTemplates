using Volo.Abp.Domain.Entities;

namespace ApiGateway.AgRouteConfigs;

/// <summary>
/// RouteConfig
/// </summary>
public class AgRouteConfig : AggregateRoot
{
    protected AgRouteConfig()
    {
    }

    public AgRouteConfig(string routeId, string clusterId, AgRouteMatch match)
    {
        RouteId = routeId;
        ClusterId = clusterId;
        Match = match;
    }

    public string RouteId { get; private set; }
    public string ClusterId { get; private set; }

    public AgRouteMatch Match { get; private set; }

    public override object?[] GetKeys()
    {
        return [RouteId];
    }
}

public class AgRouteMatch
{
    protected AgRouteMatch()
    {
    }

    public AgRouteMatch(string routeId, string path)
    {
        RouteId = routeId;
        Path = path;
    }

    public string RouteId { get; private set; }

    public string Path { get; private set; }
}