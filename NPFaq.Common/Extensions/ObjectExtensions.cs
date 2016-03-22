using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;

namespace NPFaq.Common.Extensions
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
        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oSource"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T oSource)
        {
            T oClone;
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                dcs.WriteObject(ms, oSource);
                ms.Position = 0;
                oClone = (T)dcs.ReadObject(ms);
            }
            return oClone;
        }
    }
}
