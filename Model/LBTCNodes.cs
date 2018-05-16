using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// LBTC所有节点
    /// </summary>
    [SugarTable("lbtcnodes")]
    public class LBTCNodes
    {
        /// <summary>
        /// 节点ID，直接使用接口传过的值
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int NodeID { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 节点投票数
        /// </summary>
        public long NodeVotes { get; set; } = 0;

        /// <summary>
        /// 节点所在组
        /// </summary>
        public int GroupID { get; set; } = 0;

        /// <summary>
        /// 收益率
        /// </summary>
        public decimal GetRate { get; set; } = 0.0000m;

        /// <summary>
        /// 是否在101序列中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool Isin { get; set; } = false;
    }

}
