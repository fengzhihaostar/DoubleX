using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DoubleX.Module.Common
{
    public class DBHelper
    {
        /// <summary>
        /// 生成Sql语句参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MySqlParameter GetParameter(string key, object value)
        {
            return new MySqlParameter(key, value);
        }
    }
}
