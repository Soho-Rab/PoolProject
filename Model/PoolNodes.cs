using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 矿池联盟内节点信息
    /// </summary>
    [SugarTable("poolnodes")]
    public class PoolNodes
    {

        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int NodeID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 节点所属组
        /// </summary>
        public int NodeGroupID { get; set; }

        /// <summary>
        /// 最新出块hash值，作为下次监测的起点
        /// </summary>
        public string MakeNewBlockHash { get; set; } = "38d4172bdcafcffc1dfd88df0c6060de26a834f6bab00064889d1daca866ef76";//920064块

        /// <summary>
        /// 总出块奖励
        /// </summary>
        public decimal MakeNewCoins { get; set; }

        /// <summary>
        /// 监测时间
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 总分红
        /// </summary>
        public decimal MakeShareCoins { get; set; }


        /// <summary>
        /// 周期内累计收益，分红后归零
        /// </summary>
        public decimal ThisShareCoins { get; set; }

        /// <summary>
        /// 周期内累计出币，分红后归零
        /// </summary>
        public decimal ThisNewCoins { get; set; }

        /// <summary>
        /// 节点所有者
        /// </summary>
        public string OwerName { get; set; }

        /// <summary>
        /// 节点所有者投票地址
        /// </summary>
        public string OwerVoteAddress { get; set; }

        /// <summary>
        /// 节点所有者收益地址
        /// </summary>
        public string OwerSendAddress { get; set; }

        /// <summary>
        /// 节点所有者兜底票数
        /// </summary>
        public int? VoteMinCoins { get; set; }



    }

}
