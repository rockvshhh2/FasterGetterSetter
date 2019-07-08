using ConsoleApp1.MaskAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class MaskExtensions
    {
        public static List<T> ToMaskByRef<T>(this List<T> source)
            where T : class
        {
            var properties = typeof(T).GetProperties();

            foreach (var data in source)
            {
                foreach (var item in properties)
                {
                    // 根據屬性型別不拉不拉
                    if (item.PropertyType == typeof(string))
                    {
                        // 根據特性的內容 不拉不拉
                        if (item.GetValue(data) is string oldData)
                        {
                            item.SetValue(data, "Reflection");
                        }
                    }
                }
            }

            return source;
        }

        #region Expression

        public static List<T> ToMaskByExpression<T>(this List<T> source)
            where T : class
        {
            var properties = typeof(T).GetProperties();

            var setters = new Dictionary<string, Action<T, object>>();
            var getters = new Dictionary<string, Func<T, object>>();

            foreach (var item in properties)
            {
                var target = Expression.Parameter(typeof(T));
                var value = Expression.Parameter(typeof(object));

                #region get

                getters.Add(item.Name, Expression.Lambda<Func<T, object>>(Expression.Property(target, item.GetMethod), target)
                    .Compile());

                #endregion

                #region set

                setters.Add(item.Name, Expression.Lambda<Action<T, object>>(Expression.Property(target, item.SetMethod), target, value)
                    .Compile());

                #endregion
            }

            foreach (var data in source)
            {
                foreach (var item in properties)
                {
                    // 根據屬性型別不拉不拉
                    if (item.PropertyType == typeof(string))
                    {
                        //根據特性的內容 不拉不拉
                        if (getters[item.Name](data) is string oldData)
                        {
                            setters[item.Name](data, "Expression");
                        }
                    }
                }
            }

            return source;
        }

        #endregion

        #region Delegate

        public static List<T> ToMaskByDelegate<T>(this List<T> source)
    where T : class
        {
            var properties = typeof(T).GetProperties();

            var setters = new Dictionary<string, Delegate>();
            var getters = new Dictionary<string, Delegate>();

            foreach (var item in properties)
            {
                if (item.PropertyType == typeof(string))
                {
                    setters.Add(item.Name, item.SetMethod.CreateDelegate(typeof(Action<T, string>)));
                    getters.Add(item.Name, item.GetMethod.CreateDelegate(typeof(Func<T, string>)));
                }
            }

            foreach (var data in source)
            {
                foreach (var item in properties)
                {
                    // 根據屬性型別不拉不拉
                    if (item.PropertyType == typeof(string))
                    {
                        // 根據特性的內容 不拉不拉
                        if (((Func<T,string>)getters[item.Name])(data) is string oldData)
                        {
                            ((Action<T,string>)setters[item.Name])(data, "Delegate");
                        }
                    }
                }
            }

            return source;
        }

        #endregion







    }
}
