using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlayingWithTestHost.IntegrationTests
{
  public static class HttpContentExtensions
  {
    // The response can be quite big. Deserialize the response directly from stream
    // to avoid allocating more memory, than necessary.
    public static async Task<T> Deserialize<T>(this HttpContent httpContent) where T : class
      => await httpContent.Deserialize(typeof(T)) as T;

    public static async Task<object> Deserialize(this HttpContent httpContent, Type objectType)
    {
      using (StreamReader streamReader = new StreamReader(await httpContent.ReadAsStreamAsync()))
        return new JsonSerializer().Deserialize(streamReader, objectType);
    }
  }
}
