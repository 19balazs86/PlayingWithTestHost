using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution1.Dummy
{
  public interface ITestUserProvider
  {
    UserModel TestUser { get; set; }
  }
}
