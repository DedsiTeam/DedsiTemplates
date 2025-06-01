using Dedsi.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ProjectNameCQRS;

[ApiController]
[Area(ProjectNameCQRSDomainConsts.ApplicationName)]
[Route("api/ProjectNameCQRS/[controller]/[action]")]
[ApiExplorerSettings(GroupName = ProjectNameCQRSDomainConsts.ApplicationName)]
public abstract class ProjectNameCQRSController : DedsiControllerBase;