/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      LBTCGetData
 * 文件名：        TimeVoid
 * 唯一标识：      25a10b79-f695-4e46-a260-8958605f4487
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/3/3 11:56:29
 * 描述：          
 *
 *=====================================================================*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;//加入，使用进程类，创建独立进程
using log4net;
using Newtonsoft.Json;
using Pool.Common;
using Pool.Model;
using Pool.Model.BitCore;
using Pool.Service;
using Pool.BLL;
using System.Timers;

namespace Pool.LBTCGetData
{
    /// <summary>
    /// 单节点监控程序
    /// </summary>
    public class TimerVoid
    {
        private static ILog log = Logs.Logger;
        private static NodeConfigInfo conf = new NodeConfigInfo("NodeConfig");
        private static List<GroupFees> fees=new List<GroupFees>();
        private static GroupTwoFees fee2 = new GroupTwoFees();

        /// <summary>
        /// 监测节点收益，建议5分钟一次
        /// </summary>
        public static void CheckGetCoins(object source, ElapsedEventArgs e)
        {
            #region 原理说明
            /*
             所有节点
             查询节点当前投票人和每个人的投票数量，更新nodevotes三张表

             查询节点出块信息
             */
            #endregion
            log.Info("循环执行时间：" + DateTime.Now.ToString() + "");
            string listaddressfornode = RunScript("listreceivedvotes \"" + conf.NodeName + "\"");
            var group = PoolGroupBLL.GetPoolGroupByID(conf.NodeGroup);
            if (group.GoupRole == 1)
            {
                string[] gs = group.PublicWay.Split('-');
                for (int k = 0; k < gs.Length; k++)
                {
                    fees.Add(new GroupFees(gs[k]));
                }
                //list排序：https://www.cnblogs.com/dare/p/7215246.html
                fees = fees.OrderByDescending(x => x.Coins).ToList();
            }
            else if (group.GoupRole == 2)
            {
                fee2 = new GroupTwoFees(group.PublicWay);
            }
            var poolNode = PoolNodesBLL.GetPoolNodeByAddress(conf.NodeAddress);
            var hasads = NodeVotesBLL.GetVotesByNodeAddress(conf.NodeAddress);//之前已经投票的地址
            var newads = new List<NodeVotes>();
            var hasadds = UserAddressesBLL.GetAllUserAddress();
            var newadds = new List<UserAddresses>();
            var datarunInt = -1;
            if (Strings.ScriptTrue(listaddressfornode))
            {
                try
                {
                    var lists = JsonConvert.DeserializeObject<string[]>(listaddressfornode);//投票节点集合
                    var adsvotes = new long[lists.Length];//投票人币数集合
                    string onevotess = "";
                    long onevotesd = 0;//投票节点投票数集合
                    long allvotes = 0;//投票节点所有投票数
                    bool yes = false;
                    for(int i=0;i< lists.Length;i++)
                    {
                        onevotess= RunScript("getaddressbalance " + lists[i]);
                        if (Strings.ScriptTrue(onevotess))
                        {
                            yes = long.TryParse(onevotess, out onevotesd);
                            if (yes)
                            {
                                adsvotes[i] = onevotesd;
                            }
                            else
                            {
                                onevotess = "";
                                onevotesd =0;
                                adsvotes[i] = onevotesd;
                            }
                            allvotes += onevotesd;
                            yes = false;
                            for (int j=0;j< hasads.Count;j++)
                            {
                                if(hasads[i].VotesAddress == lists[i])
                                {
                                    yes = true;
                                    hasads[i].VoteCoins = onevotesd;
                                    break;
                                }
                            }
                            if (!yes)
                            {
                                newads.Add(new NodeVotes
                                {
                                    VotesID = Guid.NewGuid().ToString(),
                                    VotesAddress = lists[i],
                                    NodeAddress = conf.NodeAddress,
                                    VoteCoins = onevotesd,
                                    JoinTime = DateTime.Now
                                });
                                if (hasadds.FindAll(x => x.UserAddress == lists[i]).Count <= 0)
                                {
                                    newadds.Add(new UserAddresses
                                    {
                                        UserAddress = lists[i],
                                        JoinTime = DateTime.Now,
                                        JoinUserID = 0
                                    });
                                }
                            }
                        }
                    }
                    if (newads.Count > 0)
                    {
                        NodeVotesBLL.InsertNodeVotes(newads);//节点投票信息
                        if (newadds.Count > 0) UserAddressesBLL.CreateUserAddresses(newadds);//用户信息
                    }
                    if (hasads.Count > 0) NodeVotesBLL.UpdateNodeVotes(hasads);//节点投票信息
                    if (newads.Count > 0) hasads = hasads.Concat(newads).ToList();
                    //开始获取收益，首先查询全节点钱包内地址所有交易
                    string thisnodecoinss = RunScript("getaddressbalance " + conf.NodeAddress);//节点当前持币数
                    decimal thisnodecoins = 0.00m;
                    decimal thisnodegetcoins = 0.00m;//当前节点周期收益
                    decimal thisnodesharecoins = 0.00m;//当前节点周期分红
                    if (Strings.ScriptTrue(thisnodecoinss)) thisnodecoins = decimal.Parse(thisnodecoinss)/ Strings.CoinToCong;//129085 21116581

                    
                    string thisnodetxs = RunScript("listsinceblock " + poolNode.MakeNewBlockHash);//节点当前周期交易记录
                    if (Strings.ScriptTrue(thisnodetxs))
                    {
                        try
                        {
                            var thistxs = JsonConvert.DeserializeObject<ListSinceBlock>(thisnodetxs);
                            poolNode.MakeNewBlockHash = thistxs.lastblock;
                            var txs = thistxs.transactions;
                            var checktime = DateTime.Now;
                            poolNode.CheckTime = checktime;
                            var listnewblocks = new List<NodeNewBlocks>();
                            var newblockheight = 0;
                            if (txs != null)
                            {
                                foreach(var tx in txs)
                                {
                                    //表nodenewblocks插入数据
                                    string blockinfoss = RunScript("getblock " + tx.blockhash);//块信息
                                    int blockheight = -1;
                                    if (Strings.ScriptTrue(blockinfoss))
                                    {
                                        try
                                        {
                                            var blockinfo = JsonConvert.DeserializeObject<SimpleBlockInfo>(blockinfoss);
                                            blockheight = blockinfo.height;
                                        }
                                        catch (Exception ee)
                                        {
                                            log.Error(ee.ToString());
                                        }
                                    }
                                    if(tx.category.ToLower()== "generate")
                                    {
                                        thisnodegetcoins += tx.amount;
                                        newblockheight = blockheight;
                                    }
                                    listnewblocks.Add(new NodeNewBlocks {
                                        NewID = Guid.NewGuid().ToString(),
                                        BlockHash = tx.blockhash,
                                        NodeAddress = conf.NodeAddress,
                                        GetCoins = tx.amount,
                                        BlockHeight = blockheight,
                                        BlockTime = Strings.ConvertIntDateTime(tx.blocktime),
                                        CreateWay=tx.category
                                    });
                                }

                            }
                            //将出块信息写入nodenewblocks和nodeincomes中
                            if (listnewblocks.Count > 0)datarunInt = NodeNewBlocksBLL.InsertNewBlocks(listnewblocks);
                            datarunInt=NodeInComesBLL.InsertComes(new NodeInComes {
                                NodeAddress=conf.NodeAddress,
                                CheckTime= checktime,
                                GetCoins= thisnodegetcoins,
                                NowCoins= thisnodecoins
                            });

                            if (thisnodegetcoins > 0)
                            {
                                //块分红记录，保留8位小数8位后全部舍去
                                var listincomeonliness = new List<UserInComesOnline>();
                                var onesharecoins = 0.00m;
                                var IsManPriNode = false;//是否为管理方私有节点
                                var grouptwovotelist = new List<SmallNodeVotes>();
                                var grouptwolist = new List<PoolNodes>();
                                string bigusersvoteaddresses = "|";//大户所有投票地址，这些地址不参与25%分红
                                if (group.GoupRole == 2)
                                {
                                    if (conf.NodeAddress == fee2.ManAddress) IsManPriNode = true;
                                    grouptwovotelist = NodeVotesBLL.GetVotesByGroupID(conf.NodeGroup);
                                    grouptwolist = PoolNodesBLL.GetPoolNodesByGroupID(conf.NodeGroup);
                                    foreach(var gn in grouptwolist)
                                    {
                                        bigusersvoteaddresses += gn.OwerVoteAddress.Trim() + "|";
                                    }
                                }
                                if (!IsManPriNode)
                                {
                                    //精英模式首先分红给大户，如果大户没有投满则将收益转自基金
                                    decimal jijincoins = 0.00m;//基金分红
                                    decimal dahucoins = 0.00m;
                                    long jinyinsanhuallvotes = 0;//精英节点散户投票总数
                                    decimal sanhucoins = 0.00m;//散户分红
                                    if (group.GoupRole == 2)
                                    {
                                        jijincoins = thisnodegetcoins * fee2.FeeTwo / (fee2.FeeOne + fee2.FeeTwo + fee2.FeeThree);
                                        dahucoins= thisnodegetcoins * fee2.FeeOne / (fee2.FeeOne + fee2.FeeTwo + fee2.FeeThree);
                                        sanhucoins = thisnodegetcoins * fee2.FeeThree / (fee2.FeeOne + fee2.FeeTwo + fee2.FeeThree);
                                        //大户没有投满,或撤票，也会将收益暂时转至基金账户
                                        string bighucoinss = RunScript("getaddressbalance " + poolNode.OwerVoteAddress);//大户当前持币数
                                        decimal bighucoins = 0.00m;
                                        if (Strings.ScriptTrue(bighucoinss)) bighucoins = decimal.Parse(bighucoinss);
                                        if (grouptwovotelist.FindAll(x => x.VotesAddress == poolNode.OwerVoteAddress).Count < grouptwolist.Count || bighucoins < poolNode.VoteMinCoins.Value)
                                        {
                                            jijincoins += dahucoins;
                                            dahucoins = 0.00m;
                                            if(bighucoins < poolNode.VoteMinCoins.Value){
                                                log.Info(poolNode.OwerName + "：" + poolNode.OwerVoteAddress + "锁仓币不够");
                                            }
                                            if (grouptwovotelist.FindAll(x => x.VotesAddress == poolNode.OwerVoteAddress).Count < grouptwolist.Count)
                                            {
                                                log.Info(poolNode.OwerName + "：" + poolNode.OwerVoteAddress + "未投满");
                                            }
                                        }
                                        if (dahucoins > 0)
                                        {
                                            thisnodesharecoins += dahucoins;
                                            listincomeonliness.Add(new UserInComesOnline
                                            {
                                                InComeID = Guid.NewGuid().ToString(),
                                                NodeAddress = conf.NodeAddress,
                                                UserAddress = poolNode.OwerSendAddress,
                                                CheckTime = checktime,
                                                GetCoins = dahucoins,
                                                BlockHeight = newblockheight
                                            });
                                        }

                                        //找出可以享受分红的散户节点,基金管理账户分红
                                        var issanhuvoteman = false;
                                        foreach (var vv in hasads)
                                        {
                                            issanhuvoteman = grouptwovotelist.FindAll(x => x.NodeAddress == fee2.ManAddress || x.VotesAddress == vv.VotesAddress).Count > 0;
                                            if (bigusersvoteaddresses.IndexOf("|" + vv.VotesAddress + "|") < 0&& issanhuvoteman)
                                            {
                                                jinyinsanhuallvotes += vv.VoteCoins;
                                            }
                                            else
                                            {
                                                if(!issanhuvoteman) log.Info("散户：" + vv.VotesAddress + "未投管理方节点");
                                            }
                                        }
                                        if (jinyinsanhuallvotes <= 0) jijincoins += sanhucoins;
                                        /*基金分红不记入分红列表中，统一自动将余额转至管理账户
                                        thisnodesharecoins += jijincoins;
                                        listincomeonliness.Add(new UserInComesOnline
                                        {
                                            InComeID = Guid.NewGuid().ToString(),
                                            NodeAddress = conf.NodeAddress,
                                            UserAddress = group.ManagerAddress,
                                            CheckTime = checktime,
                                            GetCoins = jijincoins,
                                            BlockHeight = newblockheight
                                        });*/
                                    }
                                    for (int i = 0; i < lists.Length; i++)
                                    {
                                        if (group.GoupRole == 1)//阶梯分红
                                        {
                                            onesharecoins = Strings.CutDecimalWithN(thisnodegetcoins * adsvotes[i] * (100.00m - GetFees(adsvotes[i])) / (100 * allvotes), 8);
                                            thisnodesharecoins += onesharecoins;
                                            listincomeonliness.Add(new UserInComesOnline
                                            {
                                                InComeID = Guid.NewGuid().ToString(),
                                                NodeAddress = conf.NodeAddress,
                                                UserAddress = lists[i],
                                                CheckTime = checktime,
                                                GetCoins = onesharecoins,
                                                BlockHeight = newblockheight
                                            });
                                        }
                                        else if (group.GoupRole == 2)//精英模式
                                        {
                                            //首先分管理节点和非管理节点
                                            //排除大户地址,且有散户有投票
                                            var issanhuvoteman = grouptwovotelist.FindAll(x => x.NodeAddress == fee2.ManAddress || x.VotesAddress == lists[i]).Count > 0;
                                            if (bigusersvoteaddresses.IndexOf("|" + lists[i] + "|") < 0 && jinyinsanhuallvotes > 0 && issanhuvoteman)
                                            {
                                                onesharecoins = Strings.CutDecimalWithN(sanhucoins * adsvotes[i] / jinyinsanhuallvotes, 8);
                                                thisnodesharecoins += onesharecoins;
                                                listincomeonliness.Add(new UserInComesOnline
                                                {
                                                    InComeID = Guid.NewGuid().ToString(),
                                                    NodeAddress = conf.NodeAddress,
                                                    UserAddress = lists[i],
                                                    CheckTime = checktime,
                                                    GetCoins = onesharecoins,
                                                    BlockHeight = newblockheight
                                                });
                                            }
                                        }

                                    }
                                }
                                else//管理员节点
                                {
                                    var mfees = fee2.ManCoins;
                                    var allint = 0;
                                    foreach (KeyValuePair<string, int> kvp in mfees)
                                    {
                                        allint += kvp.Value;
                                    }
                                    foreach (KeyValuePair<string, int> kvp in mfees)
                                    {
                                        onesharecoins = Strings.CutDecimalWithN(thisnodegetcoins * kvp.Value / allint, 8);
                                        thisnodesharecoins += onesharecoins;
                                        listincomeonliness.Add(new UserInComesOnline
                                        {
                                            InComeID = Guid.NewGuid().ToString(),
                                            NodeAddress = conf.NodeAddress,
                                            UserAddress = kvp.Key,
                                            CheckTime = checktime,
                                            GetCoins = onesharecoins,
                                            BlockHeight = newblockheight
                                        });
                                    }
                                }
                                poolNode.MakeNewCoins += thisnodegetcoins;
                                poolNode.MakeShareCoins += thisnodesharecoins;
                                poolNode.ThisNewCoins += thisnodegetcoins;
                                poolNode.ThisShareCoins += thisnodesharecoins;
                                datarunInt = UserInComesOnlineBLL.InsertInComesOnlines(listincomeonliness);
                            }
                            
                            //更新节点信息
                            PoolNodesBLL.UpdateOnePoolNode(poolNode);

                        }
                        catch (Exception es)
                        {
                            log.Error(es.ToString());
                        }
                    }
                    /*8点转账到管理账户，要考虑新出块10000块锁定周期，大约5-6个币会被锁定，取大值*/
                    var publictime = DateTime.Now;
                    bool isintime = publictime.Hour == conf.SetCoinsHour && publictime.Minute < conf.TimerSpan;
                    if (isintime)
                    {
                        var lockcoins = 6.66m;
                        var setcoins = thisnodecoins - lockcoins;
                        if (setcoins > 0.1m)
                        {
                            string txhash = RunScript("sendtoaddress " + group.ManagerAddress + " " + setcoins.ToString());
                            log.Info("转节点币到管理地址：sendtoaddress " + group.ManagerAddress + " " + setcoins.ToString() + "------txid：" + txhash);
                        }
                    }

                }
                catch (Exception ess)
                {
                    log.Error(ess.ToString());
                }
            }
            log.Info("循环结束时间：" + DateTime.Now.ToString() + "");
        }


        private static decimal GetFees(decimal mycoins)
        {
            var returnFee = 0.00m;
            foreach(var fee in fees)
            {
                if (mycoins >= Strings.CoinToCong * fee.Coins)
                {
                    returnFee = fee.Fees;
                    break;
                }
            }
            return returnFee;
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