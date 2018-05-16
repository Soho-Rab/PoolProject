using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 节点挖矿新块
    /// </summary>
    [SugarTable("nodenewblocks")]
    public class NodeNewBlocks
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string NewID { get; set; }

        /// <summary>
        /// 当前块Hash值
        /// </summary>
        public string BlockHash { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        ///[SugarColumn(IsIgnore = true)]
        ///public string NodeName { get; set; }

        /// <summary>
        /// 获取币数
        /// </summary>
        public decimal GetCoins { get; set; }

        /// <summary>
        /// 块高
        /// </summary>
        public int BlockHeight { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime BlockTime { get; set; }

        /// <summary>
        /// 创建方式
        /// </summary>
        public string CreateWay { get; set; }
    }

}
