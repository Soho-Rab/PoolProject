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

namespace Pool.BLL
{
    public class NodeVotesBLL
    {

        public static int InsertNodeVotes(List<NodeVotes> votes)
        {
            //return SugarBase.DBAutoClose.Insertable(nodes.ToArray()).ExecuteCommand();
            return SugarBase.DBAutoClose.Insertable(votes).ExecuteCommand();
        }

        public static List<NodeVotes> GetVotesByNodeAddress(string nodeaddress)
        {
            return SugarBase.DBAutoClose.Queryable<NodeVotes>().Where(x => x.NodeAddress==nodeaddress).OrderBy(x => x.VoteCoins, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 获取精英节点所有投票信息
        /// </summary>
        public static List<SmallNodeVotes> GetVotesByGroupID(int gid)
        {
            return SugarBase.DBAutoClose.Queryable<NodeVotes,PoolNodes>((x,y)=>new object[] { JoinType.Inner,x.NodeAddress==y.NodeAddress}).Where((x, y) => y.NodeGroupID == gid).Select<SmallNodeVotes>(x=>new SmallNodeVotes { NodeAddress=x.NodeAddress, VotesAddress=x.VotesAddress }).ToList();
        }

        public static int UpdateNodeVotes(List<NodeVotes> votes)
        {
            //return SugarBase.DBAutoClose.Insertable(nodes.ToArray()).ExecuteCommand();
            return SugarBase.DBAutoClose.Updateable(votes).ExecuteCommand();
        }


        /// <summary>
        /// 所有矿池节点投票数
        /// </summary>
        public static long GetPoolNodeVotesSum()
        {
            return SugarBase.DBAutoClose.Queryable<NodeVotes>().Sum(x => x.VoteCoins);
        }

    }
}