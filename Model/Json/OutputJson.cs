using System;
using System.Collections.Generic;
using System.Text;

namespace Pool.Model.Json
{
    /// <summary>
    /// 首页：节点当前分红情况
    /// </summary>
    public class NowCoinsReports
    {
        /// <summary>
        /// 出块节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 出块节点地址
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 收益人地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        /// 出块时间
        /// </summary>
        public DateTime BlockTime { get; set; }

        /// <summary>
        /// 出块奖励
        /// </summary>
        public decimal GetCoins { get; set; }

        /// <summary>
        /// 块高
        /// </summary>
        public int BlockHeight { get; set; }


        /// <summary>
        /// 块Hash值
        /// </summary>
        public string BlockHash { get; set; }

    }
}
