using System.Data;
using MySql.Data.MySqlClient;
using SpiderHelp.SaveModule;

namespace SpiderDemo.DAL
{
    /// <summary>
    /// 数据库访问dal层
    /// </summary>
    class IndexDll
    {
        /// <summary>
        /// 数据库查询操作，返回结果集第一行的第一列
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">查询语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>DataTable结果集</returns>
        public object SelectOne(string conn, string sql, params MySqlParameter[] parameters)
        {
            return MySqlHelp.SelectOne(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库查询操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">查询语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Select(string conn, string sql, params MySqlParameter[] parameters)
        {
            return MySqlHelp.Select(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库更新操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">更新语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为更新失败</returns>
        public int Update(string conn, string sql, params MySqlParameter[] parameters)
        {
            return MySqlHelp.Update(conn, sql);
        }

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">插入语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为插入失败</returns>
        public int Insert(string conn, string sql, params MySqlParameter[] parameters)
        {
            return MySqlHelp.Insert(conn, sql);
        }

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">插入语句</param>
        /// <param name="errorFile">错误记录的文件名</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为插入失败</returns>
        public int Insert(string conn, string sql, string errorFile, params MySqlParameter[] parameters)
        {
            return MySqlHelp.Insert(conn, sql, errorFile);
        }

        /// <summary>
        /// 数据库删除操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sql">删除语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为删除失败</returns>
        public int Delete(string conn, string sql, params MySqlParameter[] parameters)
        {
            return MySqlHelp.Delete(conn, sql);
        }
    }
}
