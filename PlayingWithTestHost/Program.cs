﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PlayingWithTestHost
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webHostBuilder =>
          webHostBuilder
            .UseStartup<Startup>());
    }
  }
}
