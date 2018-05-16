using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 每次巡检时矿池节点收益，初步定5检测一次
    /// </summary>
    [SugarTable("nodeincomes")]
    public class NodeInComes
    {
        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
        public int InComeID { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 检测时间，页面呈现精确到小时即可
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 周期内收益
        /// </summary>
        public decimal GetCoins { get; set; }

        /// <summary>
        /// 节点检测时。地址持币数
        /// </summary>
        public decimal NowCoins { get; set; }
    }

}
