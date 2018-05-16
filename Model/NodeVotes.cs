using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 节点投票和票数集合
    /// </summary>
    [SugarTable("nodevotes")]
    public class NodeVotes
    {

        /// <summary>
        /// 投票人地址
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string VotesID { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 投票人地址
        /// </summary>
        public string VotesAddress { get; set; }

        /// <summary>
        /// 投票
        /// </summary>
        public long VoteCoins { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime JoinTime { get; set; }

    }

    public class SmallNodeVotes
    {

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 投票人地址
        /// </summary>
        public string VotesAddress { get; set; }

    }

}
