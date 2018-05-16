using System;
using System.Timers;
using log4net;
using Pool.Common;
using Pool.Model;

namespace Pool.LBTCGetData
{
    public class Program
    {
        private static NodeConfigInfo conf = new NodeConfigInfo("NodeConfig");

        public static void Main(string[] args)
        {
            //http://www.ruanyifeng.com/blog/2016/02/linux-daemon.html
            //Screen 命令,让控制台一直运行
            //Console.WriteLine("Hello World!");
            //Console.WriteLine(RunScript("getaddressbalance 1MHawh1LPdi3ZMWns9XsfaBwLhYFeKMnvZ"));
            //RunScript("");
            //JsonConvert.DeserializeObject<>();
            //Console.ReadLine();
            //./bitcoin-cli listsinceblock
            //http://www.360doc.com/content/16/0912/08/12545397_590179174.shtml
            /*43 listsinceblock 列出指定块之后的交易 ★ 交易*/
            //./ bitcoin - cli getdelegatevotes QQ26165891    节点得票数

            //listdelegates   获取所有节点
            //getdelegatevotes QQ26165891    节点得票数
            //listreceivedvotes QQ26165891   当前节点投票地址，string[]
            //getaddressbalance 1BgEszHUTyQuHGxmtwJyrtfe4tk9LVzyQB  地址持币数，聪单位
            //listsinceblock lastblockhash 节点最新块
            //getblock blockhash  块信息
            //Console.WriteLine(conf.TimerSpan);
            var TimerSpan = conf.TimerSpan > 5 ? 5 : conf.TimerSpan;
            Timer t = new Timer(TimerSpan * 60 * 1000);
            //Timer t = new Timer(1000);
            t.Elapsed += new ElapsedEventHandler(TimerVoid.CheckGetCoins); //到达时间的时候执行事件；   
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
        
    }
}
