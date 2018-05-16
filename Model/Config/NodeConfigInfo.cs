using System;
using Microsoft.Extensions.Configuration;


namespace Pool.Model
{
    /// <summary>
    /// 节点配置属性
    /// </summary>
    public class NodeConfigInfo
    {
        public NodeConfigInfo()
        {
            
        }

        public NodeConfigInfo(string section)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            configuration.GetSection(section).Bind(this);
        }

        /// <summary>
        /// 钱包命令行文件地址
        /// </summary>
        public string BitCoinFile { get; set; } = "/root/bitcoin-cli";

        /// <summary>
        /// 挖矿节点地址
        /// </summary>
        public string NodeAddress { get; set; } = "";

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }= "";

        /// <summary>
        /// 节点收益自动检测时间间隔（分钟）
        /// </summary>
        public int TimerSpan { get; set; } = 6;

        /// <summary>
        /// 节点所在组
        /// </summary>
        public int NodeGroup { get; set; } = 1;

        /// <summary>
        /// 节点将收益转至管理账户时间
        /// </summary>
        public int SetCoinsHour { get; set; } = 8;


    }
}
