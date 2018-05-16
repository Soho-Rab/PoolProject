using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pool.Web.Models
{
    public class IndexViewModel
    {
        /// <summary>
        /// 历史总收益
        /// </summary>
        public decimal HisSumCoins { get; set; }

        /// <summary>
        /// 当前周期收益
        /// </summary>
        public decimal NowSumCoins { get; set; }

        /// <summary>
        /// 矿池节点数量
        /// </summary>
        public int NodeAllCount { get; set; }

        /// <summary>
        /// 矿池节点在101内数量
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        /// 节点总票数
        /// </summary>
        public decimal VotesSum { get; set; }

        /// <summary>
        /// 投票总人数
        /// </summary>
        public int UsersSum { get; set; }
    }


    /// <summary>
    /// 矿池票数信息
    /// </summary>
    public class IndexGroupVotes
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }


        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 节点投币数
        /// </summary>
        public decimal NodeVotes { get; set; }

        /// <summary>
        /// 历史总分红
        /// </summary>
        public decimal MakeShareCoins { get; set; }

        /// <summary>
        /// 收益率
        /// </summary>
        public decimal GetRate { get; set; }

        /// <summary>
        /// 节点排名
        /// </summary>
        public int NodeOrderIndex { get; set; }

        /// <summary>
        /// 是否在挖矿
        /// </summary>
        //public int IsForging { get; set; }

    }
}
