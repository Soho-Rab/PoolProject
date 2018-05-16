using System;
using SqlSugar;
using System.Collections.Generic;


//http://www.codeisbug.com/Doc/8/1153


namespace Pool.Model
{
    /// <summary>
    /// 矿池联盟组别
    /// </summary>
    [SugarTable("poolgroup")]
    public class PoolGroup
    {

        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int GroupID { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupDesc { get; set; }

        /// <summary>
        /// 管理（基金）费地址
        /// </summary>
        public string ManagerAddress { get; set; }
        /*
        /// <summary>
        /// 公益方分成
        /// </summary>
        public decimal WelfareCost { get; set; }

        /// <summary>
        /// 公益方分成地址
        /// </summary>
        public string WelfareAddress { get; set; }

        /// <summary>
        /// 散户杠杆
        /// </summary>
        public decimal VoteLever { get; set; } = 1m;

        /// <summary>
        /// 是否区分大户和小散，区分的化大户需要给散户补贴，VoteLever>1。1区分，0不区分
        /// </summary>
        public string IsCheck { get; set; } = "1";

        /// <summary>
        /// 大户分成address-coins|address-coins
        /// </summary>
        public string BigShare { get; set; }



        /// <summary>
        /// 发币地址
        /// </summary>
        public string PublicAddress { get; set; }*/


        /// <summary>
        /// 规则，1是阶梯分红，2是节点认领
        /// </summary>
        public int GoupRole { get; set; } = 1;

        /// <summary>
        /// 手续费,coins|fee-coins|fee是阶梯模式，address|fee|fee|fee|ad1:coin1-ad2:coin2表示管理方节点，大户收益，基金收益，散户收益，管理方收入分配
        /// </summary>
        public string PublicWay { get; set; }


        /// <summary>
        /// 是否开放
        /// </summary>
        public string IsUsed { get; set; }

    }

    /// <summary>
    /// 精英模式地址
    /// </summary>
    public class GroupTwoFees
    {

        public GroupTwoFees(string way)
        {
            string[] wayArray = way.Split('|');
            ManAddress = wayArray[0];
            FeeOne = decimal.Parse(wayArray[1]);
            FeeTwo = decimal.Parse(wayArray[2]);
            FeeThree = decimal.Parse(wayArray[3]);
            string[] mm = wayArray[4].Split('-');
            ManCoins = new Dictionary<string, int>();
            for (int i=0;i< mm.Length; i++)
            {
                string[] mmi = mm[i].Split(':');
                ManCoins.Add(mmi[0], int.Parse(mmi[1]));
            }
        }

        public GroupTwoFees()
        {
            FeeOne = 70.00m;
            FeeTwo = 5.00m;
            FeeThree = 25.00m;
        }

        /// <summary>
        /// 大户收益
        /// </summary>
        public decimal FeeOne { get; set; }

        /// <summary>
        /// 基金收益
        /// </summary>
        public decimal FeeTwo { get; set; }

        /// <summary>
        /// 散户收益
        /// </summary>
        public decimal FeeThree { get; set; }

        /// <summary>
        /// 管理方节点地址
        /// </summary>
        public string ManAddress { get; set; }

        /// <summary>
        /// 管理方分红
        /// </summary>
        public Dictionary<string,int> ManCoins { get; set; }

    }

    /// <summary>
    /// 阶梯分红规则
    /// </summary>
    public class GroupFees
    {

        public GroupFees(string way)
        {
            string[] wayArray = way.Split('|');
            Coins = int.Parse(wayArray[0]);
            Fees = decimal.Parse(wayArray[0]);
        }

        public GroupFees()
        {
            Coins = 0;
            Fees = 0;
        }

        /// <summary>
        /// 用户持币数门槛
        /// </summary>
        public int Coins { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Fees { get; set; }

    }
}
