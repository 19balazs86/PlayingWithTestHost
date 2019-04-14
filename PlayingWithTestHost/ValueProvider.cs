namespace PlayingWithTestHost
{
  public interface IValueProvider
  {
    string GetValue();
  }

  public class ValueProvider : IValueProvider
  {
    public string GetValue() => "PlayingWithTestHost";
  }
}
