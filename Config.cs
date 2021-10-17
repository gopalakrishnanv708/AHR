using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class Config
{
	private readonly IConfiguration configuration;
	public Config()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

		configuration = builder.Build();
	}

	public IConfiguration Configuration { get { return configuration; } }
}
