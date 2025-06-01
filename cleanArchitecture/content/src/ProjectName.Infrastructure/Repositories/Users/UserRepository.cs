using ProjectName.Users;
using SqlSugar;

namespace ProjectName.Repositories.Users;

public class UserRepository(ISqlSugarClient sqlSugarClient) : SimpleClient<User>(sqlSugarClient), IUserRepository;