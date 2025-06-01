namespace ProjectNameCQRS.Users.Dtos;

public class UserInfoResponseDto
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string Account { get; set; }
    public string Email { get; set; }
}