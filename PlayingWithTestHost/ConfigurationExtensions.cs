using Microsoft.Extensions.Configuration;

namespace PlayingWithTestHost
{
  public static class ConfigurationExtensions
  {
    public static T BindTo<T>(this IConfiguration configuration) where T : new()
    {
      T bindingObject = new T();

      configuration.GetSection(nameof(T)).Bind(bindingObject);

      return bindingObject;
    }
  }
}
