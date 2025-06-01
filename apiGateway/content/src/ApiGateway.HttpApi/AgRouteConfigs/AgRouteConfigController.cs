using ApiGateway.AgRouteConfigs.CommamdHandlers;
using Dedsi.Ddd.CQRS.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.AgRouteConfigs;

/// <summary>
/// AgRouteConfig
/// </summary>
/// <param name="dedsiMediator"></param>
public class AgRouteConfigController(IDedsiMediator dedsiMediator) : ApiGatewayController
{
    [HttpPost]
    public Task<bool> CreateAsync(AgRouteConfigRequestDto request)
    {
        return dedsiMediator.SendAsync(new CreateAgRouteConfigCommamd(request.RouteId, request.ClusterId, request.Match), HttpContext.RequestAborted);
    }

    [HttpPost]
    public Task<bool> DeleteByRouteIdAsync(DeleteByRouteIdRequestDto request)
    {
        return dedsiMediator.SendAsync(new DeleteByRouteIdCommamd(request.RouteId), HttpContext.RequestAborted);
    }
}

public record DeleteByRouteIdRequestDto(string RouteId);
