using Dedsi.ApiGateway;
using Yarp.ReverseProxy.Configuration;

namespace ApiGateway.Apis;

public static class ApiGatewayHttpApis
{
    public static void MapApiGatewayHttpApis(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/gateway/RefreshConfig", (IProxyConfigProvider proxyConfigProvider) =>
        {
            if (proxyConfigProvider is ApiGatewayProxyConfigProvider apiGatewayProxyConfigProvider)
            {
                apiGatewayProxyConfigProvider.RefreshConfig();
                return Results.Ok(true);
            }

            return Results.Ok(false);
        });
    }
}
