/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.BLL
 * 文件名：        UserInComesHisBLL
 * 唯一标识：      68f11d5f-9bfb-4ca4-9e4a-c123c4c63bda
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/3/2 16:59:30
 * 描述：          
 *
 *=====================================================================*/


using System;
using System.Collections.Generic;
using System.Text;
using Pool.Model;
using SqlSugar;

namespace Pool.BLL
{
    public class UserInComesHisBLL
    {
        public static int InsertInComesHis(List<UserInComesHis> hises)
        {
            return SugarBase.DBAutoClose.Insertable<UserInComesHis>(hises).ExecuteCommand();
        }

        public static List<UserInComesHis> GetInComesHisBySome(string useraddress,DateTime? begin,DateTime? end, int pagesize, int pageindex, ref int totalCount)
        {
            if (begin == null) begin = DateTime.MinValue;
            if (end == null) end = DateTime.MaxValue;
            totalCount = 0;
            return SugarBase.DBAutoClose.Queryable<UserInComesHis>().Where(x=>(x.UserAddress == useraddress || string.IsNullOrWhiteSpace(useraddress))&& SqlFunc.Between(x.SetTime,begin,end)).OrderBy(x => x.SetTime, OrderByType.Desc).ToPageList(pageindex, pagesize, ref totalCount);
        }

        public static decimal GetInComesSum(string useraddress, DateTime? begin, DateTime? end)
        {
            if (begin == null) begin = DateTime.MinValue;
            if (end == null) end = DateTime.MaxValue;
            return SugarBase.DBAutoClose.Queryable<UserInComesHis>().Where(x => (x.UserAddress == useraddress || string.IsNullOrWhiteSpace(useraddress)) && SqlFunc.Between(x.SetTime, begin, end)).Sum(x => x.GetCoins);
        }
    }
}
