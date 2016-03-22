using NPFaq.Common.Extensions;
using NPFaq.TestServiceReference;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Models
{
    public class Question : faq_question
    {
        private ObservableCollection<Attach> attachs;
        /// <summary>
        /// 带缩略图的附件
        /// </summary>
        public ObservableCollection<Attach> Attachs
        {
            get { return attachs; }
            set { attachs = value; RaisePropertyChanged("Attachs"); }
        }
        private ObservableCollection<Attach> otherAttachs;
        /// <summary>
        /// 非图片附件
        /// </summary>
        public ObservableCollection<Attach> OtherAttachs
        {
            get { return otherAttachs; }
            set { otherAttachs = value; RaisePropertyChanged("OtherAttachs"); }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged("IsChecked"); }
        }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value; RaisePropertyChanged("IsSelected");
                if (value)
                    Load();
            }
        }
        private bool isExpander;

        public bool IsExpander
        {
            get { return isExpander; }
            set
            {
                isExpander = value;
                RaisePropertyChanged("IsExpander");
                if (value)
                    Load();
            }
        }
        private bool isAnswerLoaded;

        public bool IsAnswerLoaded
        {
            get { return isAnswerLoaded; }
            set { isAnswerLoaded = value; RaisePropertyChanged("IsAnswerLoaded"); }
        }
        private bool isAttachLoaded;

        public bool IsAttachLoaded
        {
            get { return isAttachLoaded; }
            set { isAttachLoaded = value; RaisePropertyChanged("IsAttachLoaded"); }
        }
        private void Load()
        {
            if (!this.IsAnswerLoaded)
            {
                var client = Utils.ServicesFactory.CreateTestService();
                client.GetAnswersByIDCompleted += (s, e) =>
                {
                    this.IsAnswerLoaded = true;
                    this.faq_answer = e.Result;
                };
                client.GetAnswersByIDAsync(this.ID);
            }
            if (!this.IsAttachLoaded)
            {
                var client = Utils.ServicesFactory.CreateTestService();
                client.GetAttachsByQIDCompleted += (s, e) =>
                {
                    this.IsAttachLoaded = true;
                    this.Attachs = e.Result.ToObservableCollection();
                    this.OtherAttachs = e.Result.Where(l => l.Type == "Other").ToObservableCollection();
                };
                client.GetAttachsByQIDAsync(this.ID);
            }
        }
    }
}
