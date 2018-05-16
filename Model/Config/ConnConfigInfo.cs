using System;
using Microsoft.Extensions.Configuration;


namespace Pool.Model
{
    public class ConnConfigInfo
    {
        
        public ConnConfigInfo()
        {
        }

        public ConnConfigInfo(string section)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            configuration.GetSection(section).Bind(this);
        }

        public string ConnString { get; set; } = "";
        public string Type { get; set; } = "MySQL";
        public int TimeOut { get; set; } = 30;


       
    }
}
