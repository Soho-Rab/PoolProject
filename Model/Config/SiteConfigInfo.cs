using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Pool.Model
{
    public class SiteConfigInfo
    {

        public SiteConfigInfo()
        {
        }

        public SiteConfigInfo(string section)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            configuration.GetSection(section).Bind(this);
        }

        /// <summary>
        /// 大户拥有一个节点需要最小币数
        /// </summary>
        public int MinNodeCoins { get; set; } = 3500;

        /// <summary>
        /// 钱包名称
        /// </summary>
        public string WalletName { get; set; } = "官方轻钱包下载-Windows 64bit(v0.0.4)";

        /// <summary>
        /// 钱包下载地址
        /// </summary>
        public string WalletUrl { get; set; } = "http://downloadwallet.lbtc.io/index.php/s/HvkFNyCqVu3oc0r/download";



    }
}