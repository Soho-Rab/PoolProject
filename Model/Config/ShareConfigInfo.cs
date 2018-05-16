using System;
using Microsoft.Extensions.Configuration;

namespace Pool.Model
{
    /// <summary>
    /// 分红程序配置
    /// </summary>
    public class ShareConfigInfo
    {
        public ShareConfigInfo()
        {

        }

        public ShareConfigInfo(string section)
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
        /// 分红最小值
        /// </summary>
        public decimal ShareMin { get; set; } = 0.01m;

        /// <summary>
        /// 轮询时间间隔，秒单位
        /// </summary>
        public int GetNodesTimerSpan { get; set; } =5;

        /// <summary>
        /// 每天分红时间
        /// </summary>
        public int ShareCoinsHour { get; set; } = 9;


        /// <summary>
        /// 一周内那天分红
        /// </summary>
        public int ShareCoinsWeekDay { get; set; } = 5;
    }
}
