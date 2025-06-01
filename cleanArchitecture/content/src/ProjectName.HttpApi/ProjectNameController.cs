using Dedsi.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ProjectName;

[ApiController]
[Area(ProjectNameCoreConsts.ApplicationName)]
[Route("api/ProjectName/[controller]/[action]")]
[ApiExplorerSettings(GroupName = ProjectNameCoreConsts.ApplicationName)]
public abstract class ProjectNameController : DedsiControllerBase;