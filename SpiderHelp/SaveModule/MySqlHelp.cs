#region 版权信息
/* ======================================================================== 
 * 描述信息 
 *  
 * 作者  ：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间  ：2018年3月12日14:45:04
 * 功能  ：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace SpiderHelp.SaveModule
{
    /// <summary>
    /// 数据库操作帮助类
    /// </summary>
    public class MySqlHelp
    {
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="connstr">用户连接信息</param>
        /// <returns>打开失败返回null</returns>
        public static MySqlConnection OpenConnection(string connstr)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connstr);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.OpenConnection>>>{ex.Message}", "MySqlDbOpen");
                return null;
            }          
        }

        /// <summary>
        /// 执行查询，返回查询结果集中第一行的第一列，支持参数列表化
        /// 用完自动关闭数据库连接
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>查询失败返回null</returns>
        public static object SelectOne(string connStr, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);                        
                        object result = cmd.ExecuteScalar();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.SelectOne>>>{ex.Message}", "MySqlDbSelect");
                return null;
            }
        }

        /// <summary>
        /// 执行查询，返回查询结果集中第一行的第一列，支持参数列表化
        /// 用完自己控制数据库连接关闭
        /// </summary>
        /// <param name="con">已打开的数据库连接对象</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>查询失败返回null</returns>
        public static object SelectOne(MySqlConnection con, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    object result = cmd.ExecuteScalar();
                    return result;
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.SelectOne>>>{ex.Message}", "MySqlDbSelect");
                return null;
            }
        }

        /// <summary>
        /// 查询操作，返回DataTable，支持参数列表化
        /// 用完自动关闭数据库连接
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>查询失败返回null</returns>
        public static System.Data.DataTable Select(string connStr, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using(MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }                   
                }
            }
            catch(Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Select>>>{ex.Message}", "MySqlDbSelect");
                return null;               
            }
        }

        /// <summary>
        /// 查询操作，返回DataTable，支持参数列表化
        /// 用完自己控制数据库连接关闭
        /// </summary>
        /// <param name="con">已打开的数据库连接对象</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>查询失败返回null</returns>
        public static System.Data.DataTable Select(MySqlConnection con, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Select>>>{ex.Message}", "MySqlDbSelect");
                return null;
            }
        }

        /// <summary>
        /// 通用数据库插入方法
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">插入语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Insert(string connStr, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }              
            }
            catch(Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Insert>>>{ex.Message}", "MySqlDbInsert");
                return -101;                
            }          
        }

        /// <summary>
        /// 通用数据库插入方法，用完自己控制数据库连接关闭
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="cmdText">插入语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Insert(MySqlConnection con, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {              
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }               
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Insert>>>{ex.Message}", "MySqlDbInsert");
                return -101;
            }
        }

        /// <summary>
        /// 通用数据库插入方法
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">插入语句</param>
        /// <param name="errorfile">错误记录的文件名</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Insert(string connStr, string cmdText, string errorfile, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Insert>>>{ex.Message}", errorfile);
                return -101;
            }
        }

        /// <summary>
        /// 通用数据库更新方法
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">更新语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Update(string connStr, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Update>>>{ex.Message}", "MySqlDbUpdate");
                return -101;
            }
        }

        /// <summary>
        /// 通用数据库更新方法，用完自己控制数据库连接关闭
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="cmdText">更新语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Update(MySqlConnection con, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Update>>>{ex.Message}", "MySqlDbUpdate");
                return -101;
            }
        }

        /// <summary>
        /// 通用数据库删除操作
        /// </summary>
        /// <param name="connStr">用户连接信息</param>
        /// <param name="cmdText">删除语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Delete(string connStr, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Delete>>>{ex.Message}", "MySqlDbDelete");
                return -101;
            }
        }

        /// <summary>
        /// 通用数据库删除操作，用完自己控制数据库连接关闭
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="cmdText">删除语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>失败返回-101</returns>
        public static int Delete(MySqlConnection con, string cmdText, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
            catch (Exception ex)
            {
                CLog.DbLog($"MySqlHelp.Delete>>>{ex.Message}", "MySqlDbDelete");
                return -101;
            }
        }

        /// <summary>
        /// 普通对象转数据库对象取值
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>对象值</returns>
        public static object ToDBValue(object value)
        {
            return value ?? DBNull.Value;
        }

        /// <summary>
        /// 数据库对象转普通对象取值
        /// </summary>
        /// <param name="dbValue">对象</param>
        /// <returns>对象值</returns>
        public static object FromDBValue(object dbValue)
        {
            return dbValue == DBNull.Value ? null : DBNull.Value;
        }
    }
}
