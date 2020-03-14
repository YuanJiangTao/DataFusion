using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 将List<T>转成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="ignoreColumns"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> models, string dataTableName, params string[] ignoreColumns)
            where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            DataTable dt = new DataTable(dataTableName);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetIndexParameters().Length != 0
                    || propertyInfo.IsDefined(typeof(NotMappedAttribute))
                    || ignoreColumns.Any(o => o.ToUpper().Equals(propertyInfo.Name.ToUpper()))
                    || propertyInfo.GetSetMethod() == null)
                {
                    continue;
                }

                var destName = propertyInfo.Name;
                if (propertyInfo.IsDefined(typeof(ColumnAttribute)))
                {
                    var attr = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));
                    if (!string.IsNullOrEmpty(attr.Name))
                    {
                        destName = attr.Name;
                    }
                }

                dt.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }

            foreach (var model in models)
            {
                var modelType = model.GetType();

                var row = dt.NewRow();

                foreach (DataColumn column in dt.Columns)
                {
                    var p = modelType.GetProperty(column.ColumnName);

                    var value = p?.GetValue(model, null);

                    row[column.ColumnName] = value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 判断List<T>是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list == null || !list.Any();
        }




        /// <summary>
        /// 对Distinct进行扩展，调用方式:Distinct(p=>p.ID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector, comparer));
        }
    }
    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;
        private IEqualityComparer<V> comparer;
        public CommonEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }
        public CommonEqualityComparer(Func<T, V> keySelector) : this(keySelector, EqualityComparer<V>.Default)
        {

        }
        public bool Equals(T x, T y)
        {
            return comparer.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(keySelector(obj));
        }
    }
}
