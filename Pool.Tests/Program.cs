using System;
using Pool.BLL;
using Pool.Common;
using Pool.Model;
using Pool.Service;

namespace Pool.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //Console.WriteLine(PoolNodesBLL.GetPoolNodeCount());
            long x = 10123;
            decimal y = 23.00m;
            Console.WriteLine(x/y);
            Console.WriteLine(LBTCNodesBLL.GetAllNodes().Count);
            Console.WriteLine(PoolNodesBLL.GetPoolNodesByGroupID(0).Count);
            Console.ReadKey();
        }
    }
}
