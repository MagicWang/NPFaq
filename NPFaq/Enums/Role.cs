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

namespace NPFaq.Enums
{
    public enum Role
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 超级管理员(用户名：admin,密码：admin)
        /// </summary>
        SuperAdministrator,
        /// <summary>
        /// 管理员
        /// </summary>
        Administrator,
        /// <summary>
        /// 教师
        /// </summary>
        Teacher
    }
}
