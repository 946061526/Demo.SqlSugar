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
        /// <param name="entity">实体内容</param>
        /// <param name="pk">要忽略的主键</param>
        /// <param name="isIdentity">是否返回生成的ID</param>
        /// <returns></returns>
        long Insert<T>(T entity, Expression<Func<T, object>> ignorePk = null, bool isReturnIdentity = true) where T : class, new();
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entitys">实体集合，List<T></param>
        /// <param name="pk">要忽略的主键</param>
        /// <returns>最大id</returns>
        long Insert<T>(T[] entitys, Expression<Func<T, object>> ignorePk = null) where T : class, new();
        /// <summary>
        /// 逻辑删除(状态修改为删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <param name="fields">要修改的字段</param>
        /// <param name="where">修改条件</param>
        /// <returns>受影响行数</returns>
        int Delete<T>(T entity, Expression<Func<T, object>> fields, Expression<Func<T, bool>> where = null) where T : class, new();
        /// <summary>
        /// 按条件删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <param name="where">删除条件</param>
        /// <returns>受影响行数</returns>
        int Delete<T>(T entity, Expression<Func<T, bool>> where) where T : class, new();
        /// <summary>
        /// 按主键批量删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>受影响行数</returns>
        int Delete<T>(int[] primaryKeys) where T : class, new();
        /// <summary>
        /// 按住键删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="primaryKey">主键值</param>
        /// <returns>受影响行数</returns>
        int Delete<T>(int primaryKey) where T : class, new();
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity"></param>
        /// <param name="fileds">需要更新的字段</param>
        /// <param name="where">条件</param>
        /// <returns>受影响行数</returns>
        int Update<T>(T entity, Expression<Func<T, object>> fileds = null, Expression<Func<T, bool>> where = null) where T : class, new();
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <returns>受影响行数</returns>
        int Update<T>(T entity) where T : class, new();
        /// <summary>
        /// 获取实体数量
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> predicate) where T : class, new();
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
        /// 根据表达式获取单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] orderBy) where T : class, new();
        /// <summary>
        /// 根据表达式获取实体集合
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] orderBy) where T : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        IEnumerable<T> GetPageList<T>(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, Sorting<T>[] orderBy, ref int total) where T : class, new();


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
