using System;
using SqlSugar;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 矿池整个配置
    /// </summary>
    [SugarTable("poolconfig")]
    public class PoolConfig
    {

        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ConfigID { get; set; }

        /// <summary>
        /// 矿池名称
        /// </summary>
        public string PoolName { get; set; }

        /// <summary>
        /// 矿池上一次分红时间
        /// </summary>
        public DateTime PoolLastTime { get; set; }


        /// <summary>
        /// 监测时间间隔，60分钟
        /// </summary>
        public int LoopMins { get; set; } = 60;

        /// <summary>
        /// 每天发送分红时间，几点
        /// </summary>
        public int PublicHour { get; set; } = 9;

        /// <summary>
        /// 是否开放发币
        /// </summary>
        public string IsPublic { get; set; } = "1";
    }

}
