using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pool.Web.Models;
using Pool.Model;
using Pool.Model.Json;
using Pool.Common;
using Pool.BLL;

namespace Pool.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var thismodel = GetModel(null);
            ViewData.Add("SiteConfig", new SiteConfigInfo("SiteConfig"));
            return View(thismodel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        /// <summary>
        /// 查询当前分红
        /// </summary>
        public JsonResult GetNowCoins(string useraddress,int? page,int? rows)
        {
            if (page == null) page = 1;
            if (rows == null) rows = 20;
            var total = 0;
            var list = new List<NowCoinsReports>();
            if (string.IsNullOrWhiteSpace(useraddress))
            {
                list = NodeNewBlocksBLL.GetBlocksByAddressAndWay(null, "generate", null, null, rows.Value, page.Value, ref total);
            }
            else
            {
                list = UserInComesOnlineBLL.GetInComesOnlineBySome(useraddress, null, null, null, rows.Value, page.Value, ref total);
            }
            var result = new { total = total, rows = list };
            return Json(result);
        }

        /// <summary>
        /// 查询历史分红
        /// </summary>
        public JsonResult GetHisCoins(string useraddress, int? page, int? rows)
        {
            if (page == null) page = 1;
            if (rows == null) rows = 20;
            var total = 0;
            var list = UserInComesHisBLL.GetInComesHisBySome(useraddress, null, null, rows.Value, page.Value, ref total);
            var result = new { total = total, rows = list };
            return Json(result);
        }

        /// <summary>
        /// 查询矿池票数
        /// </summary>
        public JsonResult GetGroupVotes()
        {
            var list = PoolNodesBLL.GetPoolNodesByGroupID(0);
            var allnodes = LBTCNodesBLL.GetAllNodes();
            var rlist = new List<IndexGroupVotes>();
            foreach(var node in list)
            {
                var NodeOrderIndex = 0;
                var GetRate = 0.00m;
                var NodeVotes = 0.00m;
                foreach (var nn in allnodes)
                {
                    NodeOrderIndex++;
                    if (nn.NodeAddress == node.NodeAddress)
                    {
                        GetRate = nn.GetRate;
                        NodeVotes = Math.Round(nn.NodeVotes / Strings.CoinToCong, 2);
                        break;
                    }
                }
                //var IsForging = NodeOrderIndex < 101 ? 1 : 0;
                rlist.Add(new IndexGroupVotes
                {
                    NodeName = node.NodeName,
                    NodeAddress = node.NodeAddress,
                    NodeVotes = NodeVotes,
                    MakeShareCoins = node.MakeShareCoins,
                    GetRate = GetRate,
                    NodeOrderIndex = NodeOrderIndex
                });
            }
            var in101s = rlist.FindAll(x => x.NodeOrderIndex < 101).OrderByDescending(x => x.NodeOrderIndex);
            var out101s = rlist.FindAll(x => x.NodeOrderIndex >= 101).OrderBy(x => x.NodeOrderIndex);
            var returns = in101s.Concat(out101s).ToList();
            return Json(returns);
        }

        /// <summary>
        /// 查询101节点情况
        /// </summary>
        public JsonResult Get101Votes()
        {
            return Json(LBTCNodesBLL.Get101Nodes(0));
        }

        /// <summary>
        /// 获取分红统计
        /// </summary>
        public JsonResult GetView(string useraddress)
        {
            var list = LBTCNodesBLL.Get101Nodes(0);
            return Json(GetModel(useraddress));
        }



        private IndexViewModel GetModel(string useraddress)
        {
            var list = LBTCNodesBLL.Get101Nodes(-1);
            decimal votessum = NodeVotesBLL.GetPoolNodeVotesSum();
            votessum = votessum / Strings.CoinToCong;
            votessum = Math.Round(votessum, 2);
            return new IndexViewModel
            {
                HisSumCoins = Math.Round(UserInComesHisBLL.GetInComesSum(useraddress, null, null),2),
                NowSumCoins = Math.Round(UserInComesOnlineBLL.GetOnlineInComesSum(useraddress, null), 6),
                NodeAllCount = PoolNodesBLL.GetPoolNodeCount(),
                NodeCount = list.Count,
                VotesSum = votessum,
                UsersSum = UserAddressesBLL.GetAllUserCount()
            };
        }
    }
}
