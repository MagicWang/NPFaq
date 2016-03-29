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
    public enum MatchStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 无效
        /// </summary>
        Invalid,
        /// <summary>
        /// 进行中
        /// </summary>
        Running,
        /// <summary>
        /// 评审中
        /// </summary>
        Reviewing,
        /// <summary>
        /// 结束
        /// </summary>
        Finished
    }
}
