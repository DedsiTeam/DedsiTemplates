using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ProjectNameCQRS.Users;

public class User : AggregateRoot<Guid>
{
    protected User() { }

    public User(Guid id, string userName, string account, string passWord, string email) : base(id)
    {
        ChangeUserName(userName);
        ChangeAccount(account);
        ChangePassWord(passWord);
        ChangeEmail(email);
    }

    public string UserName { get; private set; }

    public void ChangeUserName(string newUserName)
    {
        UserName = Check.NotNullOrWhiteSpace(newUserName, nameof(newUserName));
    }

    public string Account { get; private set; }

    public void ChangeAccount(string newAccount)
    {
        Account = Check.NotNullOrWhiteSpace(newAccount, nameof(newAccount));
    }

    public string PassWord { get; private set; }

    public void ChangePassWord(string newPassWord)
    {
        PassWord = Check.NotNullOrWhiteSpace(newPassWord, nameof(newPassWord));
    }

    public string Email { get; private set; }

    public void ChangeEmail(string newEmail)
    {
        Email = Check.NotNullOrWhiteSpace(newEmail, nameof(newEmail));
    }

}