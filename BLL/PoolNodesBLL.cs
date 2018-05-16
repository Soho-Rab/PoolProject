/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.BLL
 * 文件名：        PoolNodesBLL
 * 唯一标识：      30e57e86-c601-49bf-b17d-d6a554be0b53
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/3/2 13:25:38
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
    public class PoolNodesBLL
    {
        public static int UpdatePoolNodes(List<PoolNodes> nodes)
        {
            return SugarBase.DBAutoClose.Updateable(nodes).ExecuteCommand();
        }

        public static PoolNodes GetPoolNodeByAddress(string nodeaddress)
        {
            return SugarBase.DBAutoClose.Queryable<PoolNodes>().Where(x=>x.NodeAddress== nodeaddress).First();
        }

        /// <summary>
        /// 根据群组ID获取节点信息，传0的时候获取所有
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static List<PoolNodes> GetPoolNodesByGroupID(int groupid)
        {
            return SugarBase.DBAutoClose.Queryable<PoolNodes>().Where(x => x.NodeGroupID == groupid || groupid == 0).ToList();
        }

        public static int UpdateOnePoolNode(PoolNodes node)
        {
            return SugarBase.DBAutoClose.Updateable(node).ExecuteCommand();
        }

        public static int CleanOnlineCoins()
        {
            return SugarBase.DBAutoClose.Updateable<PoolNodes>().UpdateColumns(x => new PoolNodes() { ThisNewCoins = 0.00m, ThisShareCoins = 0.00m }).Where(x => x.NodeID > 0).ExecuteCommand();
        }

        /// <summary>
        /// 所有节点
        /// </summary>
        public static int GetPoolNodeCount()
        {
            return SugarBase.DBAutoClose.Queryable<PoolNodes>().Count();
        }

        /// <summary>
        /// 所有节点
        /// </summary>
        public static int GetPoolNodeCountByGroupID(int gid)
        {
            return SugarBase.DBAutoClose.Queryable<PoolNodes>().Where(x => x.NodeGroupID == gid).Count();
        }

        public static int DelOnePoolNodeByIDs(int[] ids)
        {
            return SugarBase.DBAutoClose.Deleteable<PoolNodes>().In(ids).ExecuteCommand();
        }

        public static int CreatePoolNodes(List<PoolNodes> nodes)
        {
            return SugarBase.DBAutoClose.Insertable<PoolNodes>(nodes).ExecuteCommand();
        }
    }
}
