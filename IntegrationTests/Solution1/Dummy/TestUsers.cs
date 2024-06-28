using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution1.Dummy;
public static class TestUsers
{
    public static readonly UserModel User  = new UserModel("Test user",  ["User"]);
    public static readonly UserModel Admin = new UserModel("Test user",  ["Admin"]);
}
