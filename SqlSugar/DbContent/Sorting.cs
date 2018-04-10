using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarEx
{
    /// <summary>
    /// 字段排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Sorting<T> where T : class, new()
    {
        /// <summary>
        /// 排序字段表达式
        /// </summary>
        public Expression<Func<T, object>> Parameter { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType Direction { get; set; }

        public Sorting(Expression<Func<T, object>> parameter, SortType direct)
        {
            Parameter = parameter;
            Direction = direct;
        }
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        Asc,
        Desc
    }
}
