namespace ProjectNameCQRS.Users.Dtos;

public record CreateUserRequestDto(string UserName, string Account, string PassWord, string Email);