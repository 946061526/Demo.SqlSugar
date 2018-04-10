using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarEx
{
    public interface IDbContext : IDisposable
    {
        #region 单表操作
        /// <summary>
        /// 插入一个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity"></param>
        /// <param name="pk">要忽略的主键</param>
        /// <param name="isIdentity">是否返回生成的ID</param>
        /// <returns></returns>
        long Insert<T>(T entity, Expression<Func<T, object>> pk = null, bool isReturnIdentity = true) where T : class, new();
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entitys">表实体集合，List<T></param>
        /// <param name="pk">要忽略的主键</param>
        /// <returns>最大id</returns>
        long Insert<T>(T[] entitys, Expression<Func<T, object>> pk = null) where T : class, new();
        int Delete<T>(T entity, Expression<Func<T, object>> expression, bool isLogicDelete) where T : class, new();
        int Delete<T>(int[] ids) where T : class, new();
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity"></param>
        /// <param name="fileds">需要更新的字段</param>
        /// <returns></returns>
        int Update<T>(T entity, Expression<Func<T, object>> fileds) where T : class, new();
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <typeparam name="TValue">主键的值</typeparam>
        /// <param name="pk">主键</param>
        /// <param name="value">主键值</param>
        /// <returns></returns>
        T Get<T, TValue>(Expression<Func<T, object>> pk, TValue value) where T : class, new() where TValue : struct;
        /// <summary>
        /// 获取实体数量
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        /// <summary>
        /// 根据表达式获取单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="sorts">排序字段</param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] sorts) where T : class, new();
        /// <summary>
        /// 根据表达式获取实体集合
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="sorts">排序字段</param>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] sorts) where T : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="page">页数</param>
        /// <param name="resultsPerPage">每页条数</param>
        /// <param name="sorts">排序字段</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        IEnumerable<T> GetPageList<T>(Expression<Func<T, bool>> predicate, int page, int resultsPerPage, Sorting<T>[] sorts, ref int total) where T : class, new();
        #endregion

        #region Ado操作
        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="TReturn">查询返回实体</typeparam>
        /// <param name="sql">sql字符串</param>
        /// <param name="dic">字典</param>
        /// <returns>返回实体</returns>
        IEnumerable<TReturn> SqlQuery<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new();
        /// <summary>
        /// sql查询
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        TReturn SqlQuerySingle<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new();
        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="dic">字典</param>
        /// <returns>返回datatable</returns>
        DataTable GetDataTable(string sql, IDictionary<string, object> dic = null);
        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="dic">字典</param>
        /// <returns>返回dataset</returns>
        DataSet GetDataSet(string sql, IDictionary<string, object> dic = null);

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <param name="isTran"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, IDictionary<string, object> dic = null, bool isTran = false);

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        int GetInt(string sql, IDictionary<string, object> dic = null);
        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        string GetString(string sql, IDictionary<string, object> dic = null);
        #endregion
    }
}
