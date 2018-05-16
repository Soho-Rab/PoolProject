using log4net;
using Pool.Common;
using Pool.Model;
using Pool.BLL;
using System;
using System.Diagnostics;
using System.Timers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Pool.Model.BitCore;

namespace Pool.ShareCoins
{
    public class Program
    {
        private static ILog log = Logs.Logger;
        private static ShareConfigInfo conf = new ShareConfigInfo("ShareConfig");
        private static List<LBTCNodes> listhasnodes;
        private static List<PoolNodes> poolnodes;
        //private static List<PoolGroup> poolgroups;

        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //http://www.ruanyifeng.com/blog/2016/02/linux-daemon.html
            //Screen 命令,让控制台一直运行
            Timer t = new Timer(conf.GetNodesTimerSpan * 1000);
            //Timer t = new Timer(1000);
            t.Elapsed += new ElapsedEventHandler(ShareCoins); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；
            t.Start();
            Console.WriteLine("启动服务成功，输入exit退出!");
            while (true)
            {
                var str = Console.ReadLine();
                //Console.WriteLine(str);
                if (str.ToLower().Equals("exit"))
                {
                    break;
                }
            }
            Console.WriteLine();
            t.Stop();
            Console.WriteLine("服务已停止，按任意键退出!");
            Console.ReadKey();
        }

        public static void ShareCoins(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("Share Begin!");
            log.Info("循环执行时间：" + DateTime.Now.ToString() + "");
            string listnodess = RunScript("listdelegates");
            listhasnodes = LBTCNodesBLL.GetAllNodes();
            var newlisthasnodes = new List<LBTCNodes>();
            var addlist = new List<LBTCNodes>();
            var alllist = new List<LBTCNodes>();
            var onenode = new LBTCNodes();
            poolnodes = PoolNodesBLL.GetPoolNodesByGroupID(0);
            var datarunInt = -1;

            #region 30秒更新一次节点投票信息
            if (Strings.ScriptTrue(listnodess))
            {
                try
                {
                    try
                    {
                        var listnodes = JsonConvert.DeserializeObject<List<NodeInfo>>(listnodess);
                        string nodevotess = "";
                        long nodevotes = 0;//节点投票数
                        foreach(var node in listnodes)
                        {
                            var thisname = node.name.Replace(Environment.NewLine, "");
                            //thisname = thisname.Trim();
                            if (!string.IsNullOrWhiteSpace(thisname))
                            {
                                nodevotess = RunScript("getdelegatevotes \"" + thisname + "\"");
                                if (Strings.ScriptTrue(nodevotess))
                                {
                                    nodevotes = long.Parse(nodevotess);
                                }
                                var GroupID = GetGorupID(node.address);
                                alllist.Add(new LBTCNodes
                                {
                                    NodeID = node.id,
                                    NodeAddress = node.address,
                                    NodeName = thisname,
                                    NodeVotes = nodevotes,
                                    GroupID = GroupID,
                                    GetRate = 0.0000m
                                });
                                if (IsInHasList(node.address))
                                {
                                    newlisthasnodes.Add(new LBTCNodes
                                    {
                                        NodeID = node.id,
                                        NodeAddress = node.address,
                                        NodeName = thisname,
                                        NodeVotes = nodevotes,
                                        GroupID = GroupID
                                    });
                                }
                                else
                                {
                                    addlist.Add(new LBTCNodes
                                    {
                                        NodeID = node.id,
                                        NodeAddress = node.address,
                                        NodeName = thisname,
                                        NodeVotes = nodevotes,
                                        GroupID = GroupID,
                                        GetRate = 0.0000m
                                    });
                                }
                            }
                        }
                        if (newlisthasnodes.Count > 0) datarunInt = LBTCNodesBLL.UpdateNodesWithOutRate(newlisthasnodes);
                        if (addlist.Count > 0) datarunInt = LBTCNodesBLL.InsertNodes(addlist);

                    }
                    catch (Exception er)
                    {
                        log.Error(er.ToString());
                    }
                }
                catch (Exception ee)
                {
                    log.Error(ee.ToString());
                }
            }
            #endregion

            #region 每周五9点按时发放分红
            var publictime = DateTime.Now;
            bool isintime = Strings.WeekDayToInt(publictime.DayOfWeek) == conf.ShareCoinsWeekDay && publictime.Hour == conf.ShareCoinsHour && publictime.Minute <= conf.GetNodesTimerSpan / 60 && publictime.Second < conf.GetNodesTimerSpan % 60;
            //isintime = publictime.Hour == 13 && publictime.Minute == 43 && publictime.Second < 30;
            if (isintime)
            {
                //统计节点分红率
                var allpoolnodes = PoolNodesBLL.GetPoolNodesByGroupID(0);
                var publicnodes = new List<LBTCNodes>();
                foreach(var node in allpoolnodes)
                {
                    var getcoinsAll = UserInComesOnlineBLL.GetOnlineInComesSum(null, node.NodeAddress);
                    var thisnode = alllist.Find(x => x.NodeAddress == node.NodeAddress);
                    if (thisnode != null)
                    {
                        if (thisnode.NodeVotes > 0)
                        {
                            thisnode.GetRate = Math.Round(getcoinsAll * Strings.CoinToCong * 10000.00m / thisnode.NodeVotes, 4);
                        }
                        else
                        {
                            thisnode.GetRate = 0.0000m;
                        }
                    }
                    publicnodes.Add(thisnode);
                }
                datarunInt = LBTCNodesBLL.UpdateNodesWithAll(publicnodes);
                //发放分红
                var listusers = UserAddressesBLL.GetAllUserAddress();
                //poolgroups = PoolGroupBLL.GetAllPoolGroup();
                var hislist = new List<UserInComesHis>();
                foreach(var user in listusers)
                {
                    var getcoinsAll = UserInComesOnlineBLL.GetOnlineInComesSum(user.UserAddress, null);
                    if (getcoinsAll >= conf.ShareMin)
                    {
                        //7044 5ad6 5891 8824 85d2 1c6c 2014 35cc 921b 9e17 1e30 3cb1 30f5 1539 0649 3a2e
                        //执行转账脚本
                        string txhash = RunScript("sendtoaddress " + user.UserAddress + " " + getcoinsAll.ToString());
                        //string txhash = RunScript("sendtoaddress 12CacXV2WvdUNBu9K3Z1CUBvfEizPq9r4D 0.005");
                        log.Info("分红：sendtoaddress " + user.UserAddress + " " + getcoinsAll.ToString() + "------txid：" + txhash);
                        if (Strings.ScriptTrue(txhash)&&txhash.Length == 64)
                        {
                            string listhis = "";
                            foreach (var poolnode in poolnodes)
                            {
                                var getcoinsone = UserInComesOnlineBLL.GetOnlineInComesSum(user.UserAddress, poolnode.NodeAddress);
                                listhis += "|" + poolnode.NodeAddress + "-" + getcoinsone.ToString();
                            }
                            listhis = listhis.Trim('|');
                            hislist.Add(new UserInComesHis {
                                InComeID=Guid.NewGuid().ToString(),
                                NodeAddresses= listhis,
                                UserAddress= user.UserAddress,
                                SetTime= publictime,
                                GetCoins= getcoinsAll,
                                TransactionHash= txhash
                            });
                            datarunInt = UserInComesOnlineBLL.DelOnlineInComesByUserAddress(user.UserAddress);
                        }
                    }
                }
                if (hislist.Count > 0) datarunInt = UserInComesHisBLL.InsertInComesHis(hislist);
                datarunInt = PoolNodesBLL.CleanOnlineCoins();//还原所有矿池节点周期统计


            }
            #endregion

            log.Info("循环结束时间：" + DateTime.Now.ToString() + "");
        }

        /// <summary>
        /// 是否在节点中
        /// </summary>
        private static bool IsInHasList(string nodeaddress)
        {
            var ishas = false;
            foreach(var node in listhasnodes)
            {
                if(node.NodeAddress== nodeaddress)
                {
                    ishas = true;
                    break;
                }
            }
            return ishas;
        }

        /// <summary>
        /// 是否在矿池节点中
        /// </summary>

        private static int GetGorupID(string nodeaddress)
        {
            var id = 0;
            foreach (var node in poolnodes)
            {
                if (node.NodeAddress == nodeaddress)
                {
                    id = node.NodeGroupID;
                    break;
                }
            }
            return id;
        }

        //执行bitcoin-cli脚本
        private static string RunScript(string code)
        {
            #region Process用法注释
            //http://blog.csdn.net/first_sight/article/details/53957918
            //http://blog.csdn.net/neok/article/details/816770
            /*
            这些注释的大部分是Windows写法
            实例一个process类
            Process process = new Process();
            //设定程序名
            process.StartInfo.FileName = "/root/bitcoin-cli";
            process.StartInfo.Arguments = " getaddressbalance 1MHawh1LPdi3ZMWns9XsfaBwLhYFeKMnvZ";*/
            /*
            //关闭Shell的使用
            process.StartInfo.UseShellExecute = false;
            //重新定向标准输入，输入，错误输出
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //设置cmd窗口不显示
            process.StartInfo.CreateNoWindow = false;
            //开始
            process.Start();
            //输入命令，退出
            process.StandardInput.WriteLine("ping 192.168.0.1");
            //process.StandardInput.WriteLine("netstat");
            //process.StandardInput.WriteLine("exit");
            //获取结果
            string strRst = process.StandardOutput.ReadToEnd();*/

            //Console.WriteLine(strRst+"wocao");
            //return strRst;
            /*
            process.StartInfo.CreateNoWindow = true; // 获取或设置指示是否在新窗口中启动该进程的值（不想弹出powershell窗口看执行过程的话，就=true）
            process.StartInfo.ErrorDialog = true; // 该值指示不能启动进程时是否向用户显示错误对话框
            process.StartInfo.UseShellExecute = false;

            process.Start();
            string strOutput = process.StandardOutput.ReadToEnd();


            process.WaitForExit();*/
            #endregion

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = conf.BitCoinFile;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.Arguments = " " + code;
            try
            {
                Process p = Process.Start(psi);
                string strOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                //Console.WriteLine(strOutput);
                
                p.Close();
                return strOutput.Trim();
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                log.Info("bitcode:" + conf.BitCoinFile + " " + code);
                return "error";
            }
        }
    }
}
