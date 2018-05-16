using SqlSugar;
using System;
using System.Data;
using Pool.Model;


namespace Pool.BLL
{
    /// <summary>
    /// Sugar ORM父类, 封装一些基本的操作
    /// </summary>
    public class SugarBase
    {
        public static ConnConfigInfo ConnConfig{ get; set; } = new ConnConfigInfo("ConnConfig");

        /// <summary>
        /// 获取ORM数据库连接对象(只操作数据库一次的使用, 否则会进行多次数据库连接和关闭)
        /// 默认超时时间为30秒
        /// 默认为MySql数据库
        /// 默认自动关闭数据库链接, 多次操作数据库请勿使用该属性, 可能会造成性能问题
        /// 要自定义请使用GetIntance()方法或者直接使用Exec方法, 传委托
        /// </summary>
        public static SqlSugarClient DBAutoClose
        {
            get
            {
                return InitDB(ConnConfig.TimeOut, true);
            }
        }

        /// <summary>
        /// 数据库默认打开，手动关闭一下
        /// </summary>
        /// <value>The DBD ef open.</value>
        public static SqlSugarClient DBDefOpen
        {
            get
            {
                return InitDB(ConnConfig.TimeOut, false);
            }
        }

        private static SqlSugar.DbType GetByString(string type){
            switch(type.ToLower()){
                case "mysql":
                    return SqlSugar.DbType.MySql;
                case "sqlserver":
                    return SqlSugar.DbType.SqlServer;
                case "oracle":
                    return SqlSugar.DbType.Oracle;
                case "sqllite":
                    return SqlSugar.DbType.Sqlite;
                default:
                   return SqlSugar.DbType.MySql; 
            }
        }

        /// <summary>
        /// 获得SqlSugarClient(使用该方法, 默认请手动释放资源, 如using(var db = SugarBase.GetIntance()){你的代码}, 如果把isAutoCloseConnection参数设置为true, 则无需手动释放, 会每次操作数据库释放一次, 可能会影响性能, 请自行判断使用)
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        /// <returns></returns>
        /// <author>旷丽文</author>
        public static SqlSugarClient GetIntance(int commandTimeOut = 30, bool isAutoCloseConnection = false)
        {
            return InitDB(commandTimeOut, isAutoCloseConnection);
        }

        /// <summary>
        /// 初始化ORM连接对象, 一般无需调用, 除非要自己写很复杂的数据库逻辑
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        private static SqlSugarClient InitDB(int commandTimeOut = 30, bool isAutoCloseConnection = false)
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnConfig.ConnString,
                DbType = GetByString(ConnConfig.Type),
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = isAutoCloseConnection
            });
            db.Ado.CommandTimeOut = commandTimeOut;
            return db;
        }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型</typeparam>
        /// <param name="func">方法体</param>
        /// <returns></returns>
        public static Result Exec<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = InitDB(commandTimeOut))
            {
                try
                {
                    return func(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// 带事务处理的执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型</typeparam>
        /// <param name="func">方法体</param>
        /// <returns></returns>
        /// <author>旷丽文</author>
        public static Result ExecTran<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = InitDB(commandTimeOut))
            {
                try
                {
                    db.Ado.BeginTran(IsolationLevel.Unspecified);
                    var result = func(db);
                    db.Ado.CommitTran();
                    return result;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }

}
