using ApiGateway.AgClusterConfigs.CommamdHandlers;
using ApiGateway.Queries;
using Dedsi.Ddd.CQRS.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.AgClusterConfigs;

/// <summary>
/// AgClusterConfig
/// </summary>
/// <param name="agClusterConfigQuery"></param>
/// <param name="dedsiMediator"></param>
public class AgClusterConfigController(
    IAgClusterConfigQuery agClusterConfigQuery,
    IDedsiMediator dedsiMediator) : ApiGatewayController
{
    [HttpPost]
    public Task<bool> CreateAsync(AgClusterConfigRequestDto request)
    {
        return dedsiMediator.SendAsync(new CreateAgClusterConfigCommamd(request.ClusterId, request.Destinations), HttpContext.RequestAborted);
    }

    [HttpGet]
    public Task<string[]> GetAllClusterIdAsync()
    {
        return agClusterConfigQuery.GetAllClusterIdAsync(HttpContext.RequestAborted);
    }

    [HttpPost]
    public Task<bool> DeleteByClusterIdAsync(DeleteByClusterIdRequestDto request)
    {
        return dedsiMediator.SendAsync(new DeleteByClusterIdCommamd(request.ClusterId), HttpContext.RequestAborted);
    }
}

public record DeleteByClusterIdRequestDto(string ClusterId);