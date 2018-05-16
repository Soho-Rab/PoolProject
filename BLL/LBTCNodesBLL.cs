/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.BLL
 * 文件名：        LBTCNodesBLL
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

namespace Pool.BLL
{
    public class LBTCNodesBLL
    {

        public static int InsertNodes(List<LBTCNodes> nodes)
        {
            //return SugarBase.DBAutoClose.Insertable(nodes.ToArray()).ExecuteCommand();
            return SugarBase.DBAutoClose.Insertable(nodes).ExecuteCommand();
        }

        public static LBTCNodes GetOneNodeByID(int id)
        {
            return SugarBase.DBAutoClose.Queryable<LBTCNodes>().InSingle(id);
        }

        public static LBTCNodes GetOneNodeByName(string name)
        {
            return SugarBase.DBAutoClose.Queryable<LBTCNodes>().Where(x=>x.NodeName==name).First();
        }

        public static LBTCNodes GetOneNodeByAddress(string address)
        {
            return SugarBase.DBAutoClose.Queryable<LBTCNodes>().Where(x => x.NodeAddress == address).First();
        }

        public static int UpdateNodesWithAll(List<LBTCNodes> nodes)
        {
            return SugarBase.DBAutoClose.Updateable(nodes).ExecuteCommand();
        }

        //更新除收益率其它列
        public static int UpdateNodesWithOutRate(List<LBTCNodes> nodes)
        {
            return SugarBase.DBAutoClose.Updateable<LBTCNodes>(nodes).IgnoreColumns(x => x.GetRate).ExecuteCommand();
        }

        public static List<LBTCNodes> GetAllNodes()
        {
            return SugarBase.DBAutoClose.Queryable<LBTCNodes>().OrderBy(x => x.NodeVotes, OrderByType.Desc).OrderBy(x => x.NodeID).ToList();
        }

        /// <summary>
        /// 获取前101节点,-1是101内所有矿池节点，传0所有101节点，传组ID是组内进101节点的
        /// </summary>
        public static List<LBTCNodes> Get101Nodes(int group)
        {
            var lists = SugarBase.DBAutoClose.Queryable<LBTCNodes>().OrderBy(x => x.NodeVotes, OrderByType.Desc).OrderBy(x => x.NodeID).Take(101).ToList();
            if (group > 0)
            {
                lists = lists.FindAll(x => x.GroupID == group);
            }
            else if (group < 0)
            {
                lists = lists.FindAll(x => x.GroupID > 0);
            }
            return lists;
        }

    }
}