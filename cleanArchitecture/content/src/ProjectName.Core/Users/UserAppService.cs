using Dedsi.Ddd.Application.Contracts.Services;
using Dedsi.Ddd.Application.Services;
using ProjectName.Users.Dtos;

namespace ProjectName.Users;

public interface IUserAppService : IDedsiApplicationService
{
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public class UserAppService(IUserRepository userRepository) : DedsiApplicationService, IUserAppService
{
    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstAsync(a => a.Id == id, cancellationToken);

        return new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Account = user.Account,
            Email = user.Email,
        };
    }
}