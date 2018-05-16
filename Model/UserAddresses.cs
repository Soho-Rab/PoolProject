using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 会员地址信息
    /// </summary>
    [SugarTable("useraddresses")]
    public class UserAddresses
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string UserAddress { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 后继开通注册后绑定用户ID
        /// </summary>
        public int JoinUserID { get; set; } = 0;


    }

}
