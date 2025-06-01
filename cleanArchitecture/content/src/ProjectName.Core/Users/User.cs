using SqlSugar;

namespace ProjectName.Users;

[SugarTable("Users")]
public class User
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Account { get; set; }
    
    public string PassWord { get; set; }
    
    public string Email { get; set; }
}