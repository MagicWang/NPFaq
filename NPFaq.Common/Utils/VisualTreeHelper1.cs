using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Utils
{
    public static class VisualTreeHelper1
    {
        /// <summary>
        /// 获取对象在可视树上的根
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DependencyObject GetRoot(DependencyObject obj)
        {
            while (VisualTreeHelper.GetParent(obj) != null)
                obj = VisualTreeHelper.GetParent(obj);
            return obj;
        }
        /// <summary>
        /// 获取对象的指定类型的父对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj, string name = "") where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name || string.IsNullOrEmpty(name)))
                    return (T)parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
        /// <summary>
        /// 获取对象的指定类型的子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name = "") where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                    return (T)child;
                else
                {
                    var grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取对象的指定类型的所有子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetChildObjects<T>(DependencyObject obj, string name = "") where T : FrameworkElement
        {
            var result = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                    result.Add(child as T);
                else
                    result.AddRange(GetChildObjects<T>(child, name));
            }
            return result;
        }
        /// <summary>
        /// 获取对象的指定类型的所有同级对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSiblingObjects<T>(T obj, string name = "") where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                    yield return child as T;
            }
        }
    }
}
