using System;
using System.Collections.Generic;

namespace Pool.Model.BitCore
{

    /// <summary>
    /// ./bitcoin-cli listdelegates获取的所有节点信息，更新lbtcnodes表
    /// </summary>
    public class NodeInfo
    {

        public int id { get; set; }

        public string address { get; set; }

        public string name { get; set; }

    }


    /// <summary>
    /// ./bitcoin-cli listsinceblock lastblockhash，监测节点产生的最新交易信息
    /// </summary>
    public class ListSinceBlock
    {
        /// <summary>
        /// 所有交易
        /// </summary>
        public List<SimpleTransaction> transactions { get; set; }

        /// <summary>
        /// 当前节点最新块Hash
        /// </summary>
        public string lastblock { get; set; }

    }

    /// <summary>
    /// ./bitcoin-cli listsinceblock lastblockhash，监测节点产生的最新交易信息-某个交易信息情况
    /// </summary>
    public class SimpleTransaction
    {
        /// <summary>
        /// 交易地址，不一定是节点地址，如果节点导入了多个私钥的话
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 交易分类，coinbase是挖矿所得
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 交易币数
        /// </summary>
        public decimal amount{ get; set; }

        /// <summary>
        /// 块hash值
        /// </summary>
        public string blockhash { get; set; }

        /// <summary>
        /// 块产生的timespan值
        /// </summary>
        public double blocktime { get; set; }

        /// <summary>
        /// 交易txid值
        /// </summary>
        public string txid { get; set; }

    }


    /// <summary>
    /// ./bitcoin-cli getblock blockhash，块信息
    /// </summary>
    public class SimpleBlockInfo
    {

        public string hash { get; set; }

        /// <summary>
        /// 块高
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 块产生的timespan值
        /// </summary>
        public string time { get; set; }

    }

}
