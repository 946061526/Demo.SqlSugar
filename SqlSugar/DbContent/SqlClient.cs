using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace SqlSugarEx
{
    public class SqlClient
    {
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "server=.;uid=sa;pwd=sa123!@#;database=yz", DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
            //db.Aop.OnLogExecuting = (sql, pars) =>
            //{
            //    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            //    Console.WriteLine();
            //};
            return db;
        }

        private static Tuple<string, SqlSugar.DbType> GetConfig()
        {
            return new Tuple<string, SqlSugar.DbType>("server=.;uid=sa;pwd=sa123!@#;database=yz", SqlSugar.DbType.SqlServer);
        }
    }
}
