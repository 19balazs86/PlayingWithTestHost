using System.Security.Claims;

namespace PlayingWithTestHost.Model;

public sealed class UserModel
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = [];

    public UserModel() // This constructor need for response.Content.ReadFromJsonAsync<UserModel>()
    {
    }

    public UserModel(string name, IEnumerable<string> roles)
    {
        Name = name;
        Roles = roles;
    }

    public UserModel(IEnumerable<Claim> claims)
    {
        var roles = new List<string>();

        foreach (Claim claim in claims)
        {
            switch (claim.Type)
            {
                case ClaimTypes.Name:
                    Name = claim.Value;
                    break;
                case ClaimTypes.Role:
                    roles.Add(claim.Value);
                    break;
            }
        }

        Roles = roles;
    }

    public static UserModel CreateFromClaims(IEnumerable<Claim> claims)
    {
        return new UserModel(claims);
    }

    public IEnumerable<Claim> ToClaims()
    {
        var claims = new List<Claim>(Roles.Select(role => new Claim(ClaimTypes.Role, role)))
        {
            new Claim(ClaimTypes.Name, Name)
        };

        return claims;
    }
}
