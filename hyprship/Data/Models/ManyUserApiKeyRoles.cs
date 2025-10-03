namespace Hyprship.Data.Models;

public class ManyUserApiKeyRoles : Many<UserApiKey, Role, UserApiKeyRole>
{
    public ManyUserApiKeyRoles(UserApiKey owner, ICollection<UserApiKeyRole> values)
        : base(owner, values, k => k.Role, Factory)
    {
    }

    private static UserApiKeyRole Factory(UserApiKey key, Role role)
    {
        return new UserApiKeyRole(key.Id, role.Id)
        {
            UserApiKey = key,
            Role = role,
        };
    }
}