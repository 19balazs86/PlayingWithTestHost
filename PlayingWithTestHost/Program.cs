﻿namespace PlayingWithTestHost;

public sealed class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>());
    }
}
