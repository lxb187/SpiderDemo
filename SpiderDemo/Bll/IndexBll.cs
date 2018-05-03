using System.Data;
using MySql.Data.MySqlClient;
using SpiderDemo.DAL;

namespace SpiderDemo.Bll
{
    /// <summary>
    /// 数据库操作bll层
    /// </summary>
    class IndexBll
    {
        /// <summary>
        /// dll层类对象
        /// </summary>
        private IndexDll dll = new IndexDll();

        /// <summary>
        /// 数据库连接
        /// </summary>
        private string conn = string.Empty;

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="connStr">数据库连接</param>
        public IndexBll(string connStr)
        {
            conn = connStr;
        }

        /// <summary>
        /// 数据库查询操作，返回结果集第一行的第一列
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>Object结果集</returns>
        public object SelectOne(string sql, params MySqlParameter[] parameters)
        {
            return dll.SelectOne(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库查询操作
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Select(string sql, params MySqlParameter[] parameters)
        {
            return dll.Select(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库更新操作
        /// </summary>
        /// <param name="sql">更新语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为更新失败</returns>
        public int Update(string sql, params MySqlParameter[] parameters)
        {
            return dll.Update(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        /// <param name="sql">插入语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为插入失败</returns>
        public int Insert(string sql, params MySqlParameter[] parameters)
        {
            return dll.Insert(conn, sql, parameters);
        }

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        /// <param name="sql">插入语句</param>
        /// <param name="errorFile">错误记录的文件名</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为插入失败</returns>
        public int Insert(string sql, string errorFile, params MySqlParameter[] parameters)
        {
            return dll.Insert(conn, sql, errorFile, parameters);
        }

        /// <summary>
        /// 数据库删除操作
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <param name="parameters">参数列表化</param>
        /// <returns>-101为删除失败</returns>
        public int Delete(string sql, params MySqlParameter[] parameters)
        {
            return dll.Delete(conn, sql, parameters);
        }
    }
}
