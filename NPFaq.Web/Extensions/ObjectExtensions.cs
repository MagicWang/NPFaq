using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPFaq.Web.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将对象的属性复制到另一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CopyTo<T>(this object obj) where T : new()
        {
            T target = new T();
            var originalType = obj.GetType();
            var targetType = target.GetType();
            var commonProperty = originalType.GetProperties().Select(l => l.Name).Intersect(targetType.GetProperties().Select(l => l.Name));
            commonProperty.ToList().ForEach(l =>
            {
                var originalValue = originalType.GetProperty(l).GetValue(obj, null);
                targetType.GetProperty(l).SetValue(target, originalValue, null);
            });
            return target;
        }
    }
}