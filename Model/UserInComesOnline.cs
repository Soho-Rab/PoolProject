using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 会员即时收益历史记录
    /// </summary>
    [SugarTable("userincomesonline")]
    public class UserInComesOnline
    {
        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]//, IsIdentity = true
        public string InComeID { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        /// 监测时间
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 分红收益
        /// </summary>
        public decimal GetCoins { get; set; }

        /// <summary>
        /// 分红块高
        /// </summary>
        public int BlockHeight { get; set; }
    }

}
