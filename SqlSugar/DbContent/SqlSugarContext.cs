using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace SqlSugarEx
{
    public class SqlSugarContext : IDbContext
    {
        SqlSugarClient _db;
        public SqlSugarContext(string connstr, DbType dbType = DbType.SqlServer)
        {
            SqlSugar.DbType _dbType = SqlSugar.DbType.SqlServer;
            switch (dbType)
            {
                case DbType.MySql:
                    _dbType = SqlSugar.DbType.MySql;
                    break;
                case DbType.SqlServer:
                default:
                    _dbType = SqlSugar.DbType.SqlServer;
                    break;
            }
            _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = connstr, DbType = _dbType, IsAutoCloseConnection = true });
#if DEBUG
            _db.Ado.IsEnableLogEvent = true;
            _db.Ado.LogEventStarting = (sql, pars) =>
            {
                System.Diagnostics.Debug.Write(sql);
            };
#endif
            _db = SqlClient.GetInstance();
        }
        public SqlSugarContext()
        {
            _db = SqlClient.GetInstance();
        }

        #region 单表操作

        #region 查询

        /// <summary>
        /// 获取实体数量
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _db.Queryable<T>().Where(predicate).Count();
        }
        /// <summary>
        /// 按住键查询单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="pkValue">主键</param>
        /// <returns></returns>
        T Get<T>(object pkValue) where T : class, new()
        {
            return _db.Queryable<T>().Single(null);
        }
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <typeparam name="TValue">主键的值</typeparam>
        /// <param name="pk">主键</param>
        /// <param name="value">主键值</param>
        /// <returns></returns>
        public T Get<T, TValue>(Expression<Func<T, object>> pk, TValue value)
            where T : class, new()
        where TValue : struct
        {
            var filed = ExpressionUtils.GetProperty(pk);
            SugarParameter parameter = new SugarParameter(filed, value);
            return _db.Queryable<T>().Where(string.Concat(filed, "=@", filed), parameter).Single();
        }
        /// <summary>
        /// 根据表达式获取单个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] orderBy) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>();
            if (orderBy != null)
            {
                foreach (var sort in orderBy)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.Single(predicate);
        }
        /// <summary>
        /// 根据表达式获取实体集合
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] orderBy) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>().Where(predicate);
            if (orderBy != null)
            {
                foreach (var sort in orderBy)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.ToList();
        }
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
        public IEnumerable<T> GetPageList<T>(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, Sorting<T>[] orderBy, ref int total) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>().Where(predicate);
            if (orderBy != null)
            {
                foreach (var sort in orderBy)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.ToPageList(pageIndex, pageSize, ref total);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 插入一个实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <param name="ignorePk">要忽略的主键</param>
        /// <param name="isIdentity">是否返回生成的ID</param>
        public long Insert<T>(T entity, Expression<Func<T, object>> ignorePk = null, bool isReturnIdentity = true) where T : class, new()
        {
            IInsertable<T> insertObj = _db.Insertable(entity);
            if (ignorePk != null)
            {
                insertObj = insertObj.IgnoreColumns(ignorePk);
            }
            if (isReturnIdentity)
            {
                return insertObj.ExecuteReturnBigIdentity();
            }
            return insertObj.ExecuteCommand();
        }
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entitys">实体集合，List<T></param>
        /// <param name="ignorePk">要忽略的主键</param>
        /// <returns>最大id</returns>
        public long Insert<T>(T[] entitys, Expression<Func<T, object>> ignorePk = null) where T : class, new()
        {
            IInsertable<T> insertObj = _db.Insertable(entitys);
            if (ignorePk != null)
            {
                insertObj = insertObj.IgnoreColumns(ignorePk);
            }
            var t = insertObj.ExecuteReturnBigIdentityAsync();
            t.Wait();
            return t.Result;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 逻辑删除(状态修改为删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <param name="fields">要修改的字段</param>
        /// <param name="where">修改条件</param>
        /// <returns>受影响行数</returns>
        public int Delete<T>(T entity, Expression<Func<T, object>> fields, Expression<Func<T, bool>> where = null) where T : class, new()
        {
            var upd = _db.Updateable(entity).UpdateColumns(fields);
            if (where != null)
            {
                upd.Where(where);
            }
            return upd.ExecuteCommand();
        }
        /// <summary>
        /// 按条件删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <param name="where">删除条件</param>
        /// <returns>受影响行数</returns>
        public int Delete<T>(T entity, Expression<Func<T, bool>> where) where T : class, new()
        {
            return _db.Deleteable(entity).Where(where).ExecuteCommand();
        }
        /// <summary>
        /// 按主键批量删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>受影响行数</returns>
        public int Delete<T>(int[] primaryKeys) where T : class, new()
        {
            return _db.Deleteable<T>(primaryKeys).ExecuteCommand();
        }
        /// <summary>
        /// 按住键删除(物理删除)
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="primaryKey">主键值</param>
        /// <returns>受影响行数</returns>
        public int Delete<T>(int primaryKey) where T : class, new()
        {
            return _db.Deleteable<T>(primaryKey).ExecuteCommand();
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity"></param>
        /// <param name="fileds">需要更新的字段</param>
        /// <param name="where">条件</param>
        /// <returns>受影响行数</returns>
        public int Update<T>(T entity, Expression<Func<T, object>> fileds = null
            , Expression<Func<T, bool>> where = null) where T : class, new()
        {
            IUpdateable<T> upd = _db.Updateable(entity);
            if (fileds != null)
            {
                upd.UpdateColumns(fileds);
            }
            if (where != null)
            {
                upd.Where(where);
            }
            return upd.ExecuteCommand();
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">表实体</typeparam>
        /// <param name="entity">实体内容</param>
        /// <returns>受影响行数</returns>
        public int Update<T>(T entity) where T : class, new()
        {
            return _db.Updateable(entity).ExecuteCommand();
        }
        #endregion

        #endregion

        #region Ado操作
        public IEnumerable<TReturn> SqlQuery<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new()
        {
            return _db.Ado.SqlQuery<TReturn>(sql, dic);
        }

        public TReturn SqlQuerySingle<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new()
        {
            return _db.Ado.SqlQuerySingle<TReturn>(sql, dic);
        }

        public DataSet GetDataSet(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetDataSetAll(sql, dic);
        }

        public DataTable GetDataTable(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetDataTable(sql, dic);
        }

        public int ExecuteSql(string sql, IDictionary<string, object> dic = null, bool isTran = false)
        {
            if (isTran)
            {
                var result = _db.Ado.UseTran(() =>
                {
                    return _db.Ado.ExecuteCommand(sql, dic);
                });
                return result.IsSuccess ? result.Data : 0;
            }
            return _db.Ado.ExecuteCommand(sql, dic);
        }


        public int GetInt(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetInt(sql, dic);
        }

        public string GetString(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetString(sql, dic);
        }
        #endregion

        ~SqlSugarContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }


    }
}
