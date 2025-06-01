using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectName.Users.Dtos;

namespace ProjectName.Authorizations;

/// <summary>
/// 授权
/// </summary>
/// <param name="configuration"></param>
public class AuthorizationController(IConfiguration configuration) : ProjectNameController
{
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<LoginResultDto> LoginAsync(LoginInputDto input)
    {

        var claims = new Claim[]
        {
            
        };

        var secretByte = Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!);
        var signingKey = new SymmetricSecurityKey(secretByte);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: configuration["JwtOptions:Issuer"],
            audience: configuration["JwtOptions:Audience"],
            claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new LoginResultDto()
        {
            Token = token
        };
    }
}