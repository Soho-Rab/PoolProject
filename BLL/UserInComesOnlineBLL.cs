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
using Pool.Model.Json;

namespace Pool.BLL
{
    public class UserInComesOnlineBLL
    {
        public static int InsertInComesOnlines(List<UserInComesOnline> incomeses)
        {
            return SugarBase.DBAutoClose.Insertable<UserInComesOnline>(incomeses).ExecuteCommand();
        }

        public static List<NowCoinsReports> GetInComesOnlineBySome(string useraddress, string nodeaddress, DateTime? begin, DateTime? end, int pagesize, int pageindex, ref int totalCount)
        {
            if (begin == null) begin = DateTime.MinValue;
            if (end == null) end = DateTime.MaxValue;
            totalCount = 0;
            return SugarBase.DBAutoClose.Queryable<UserInComesOnline, LBTCNodes, NodeNewBlocks>((x, y, z) => new object[] {
        JoinType.Left,x.NodeAddress==y.NodeAddress,JoinType.Left,x.BlockHeight==z.BlockHeight}).Where(x => (x.NodeAddress == nodeaddress || string.IsNullOrWhiteSpace(nodeaddress)) && (x.UserAddress == useraddress || string.IsNullOrWhiteSpace(useraddress)) && SqlFunc.Between(x.CheckTime, begin, end)).OrderBy(x => x.CheckTime, OrderByType.Desc).Select<NowCoinsReports>((x, y, z) => new NowCoinsReports { BlockTime = z.BlockTime, NodeAddress = x.NodeAddress, NodeName = y.NodeName, GetCoins = x.GetCoins, BlockHeight = x.BlockHeight, UserAddress = x.UserAddress, BlockHash = z.BlockHash }).ToPageList(pageindex, pagesize, ref totalCount);
        }

        public static decimal GetOnlineInComesSum(string useraddress, string nodeaddress)
        {
            return SugarBase.DBAutoClose.Queryable<UserInComesOnline>().Where(x => (x.NodeAddress == nodeaddress || string.IsNullOrWhiteSpace(nodeaddress)) && (x.UserAddress == useraddress || string.IsNullOrWhiteSpace(useraddress))).Sum(x => x.GetCoins);
        }

        public static int DelOnlineInComesByUserAddress(string useraddress)
        {
            return SugarBase.DBAutoClose.Deleteable<UserInComesOnline>().Where(x => (x.UserAddress == useraddress || string.IsNullOrWhiteSpace(useraddress))).ExecuteCommand();
        }
    }

}
