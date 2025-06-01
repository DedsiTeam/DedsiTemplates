using Dedsi.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway;

[ApiController]
[Area(ApiGatewayDomainConsts.ApplicationName)]
[Route("api/ApiGateway/[controller]/[action]")]
[ApiExplorerSettings(GroupName = ApiGatewayDomainConsts.ApplicationName)]
public abstract class ApiGatewayController : DedsiControllerBase;