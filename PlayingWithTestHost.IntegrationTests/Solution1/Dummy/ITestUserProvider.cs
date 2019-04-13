using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution1.Dummy
{
  public interface ITestUserProvider
  {
    UserModel TestUser { get; set; }
  }
}
