using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 会员收益历史记录
    /// </summary>
    [SugarTable("userincomeshis")]
    public class UserInComesHis
    {

        /// <summary>
        /// 主键ID，GUID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]//, IsIdentity = true
        public string InComeID { get; set; }

        /// <summary>
        /// 节点地址收益address-coins|address-coins
        /// </summary>
        public string NodeAddresses { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string UserAddress { get; set; }

        /// <summary>
        /// 分红时间
        /// </summary>
        public DateTime SetTime { get; set; }

        /// <summary>
        /// 获取分红
        /// </summary>
        public decimal GetCoins { get; set; }


        /// <summary>
        /// 交易Hash
        /// </summary>
        public string TransactionHash { get; set; }
    }

}
