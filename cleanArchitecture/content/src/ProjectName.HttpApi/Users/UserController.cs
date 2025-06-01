using Microsoft.AspNetCore.Mvc;
using ProjectName.Users.Dtos;

namespace ProjectName.Users;

/// <summary>
/// 用户
/// </summary>
/// <param name="userAppService"></param>
public class UserController(IUserAppService userAppService) : ProjectNameController
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<UserDto> GetByIdAsync(Guid id)
    {
        return userAppService.GetByIdAsync(id, HttpContext.RequestAborted);
    }
}