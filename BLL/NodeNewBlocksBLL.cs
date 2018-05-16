/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.BLL
 * 文件名：        NodeNewBlocksBLL
 * 唯一标识：      2717dad1-3391-4b76-9446-542b31056521
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/3/1 15:56:22
 * 描述：          
 *
 *=====================================================================*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SqlSugar;
using Pool.Model;
using Pool.Model.Json;

namespace Pool.BLL
{
    public class NodeNewBlocksBLL
    {

        public static int InsertNewBlocks(List<NodeNewBlocks> blocks)
        {
            //return SugarBase.DBAutoClose.Insertable(nodes.ToArray()).ExecuteCommand();
            return SugarBase.DBAutoClose.Insertable(blocks).ExecuteCommand();
        }

        public static List<NowCoinsReports> GetBlocksByAddressAndWay(string address,string way,DateTime? begin,DateTime? end,int pagesize,int pageindex,ref int totalCount)
        {
            if (begin == null) begin = DateTime.MinValue;
            if (end == null) end = DateTime.MaxValue;
            totalCount = 0;
            return SugarBase.DBAutoClose.Queryable<NodeNewBlocks,PoolNodes>((x, y)=> new object[] {
        JoinType.Left,x.NodeAddress==y.NodeAddress}).Where(x => (string.IsNullOrWhiteSpace(address) || x.NodeAddress == address) && SqlFunc.Between(x.BlockTime, begin.Value, end.Value) && x.CreateWay == way).OrderBy(x => x.BlockTime, OrderByType.Desc).Select<NowCoinsReports>((x,y)=>new NowCoinsReports { BlockHash = x.BlockHash, UserAddress = "NoUserAddress", NodeAddress = x.NodeAddress, NodeName = y.NodeName, GetCoins = x.GetCoins, BlockHeight = x.BlockHeight, BlockTime = x.BlockTime }).ToPageList(pageindex, pagesize, ref totalCount);
        }

    }
}