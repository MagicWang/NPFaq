using DevExpress.Mvvm;
using NPFaq.Enums;
using NPFaq.MatchServiceReference;
using System;
using System.Collections.ObjectModel;
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
    public class WrapMatch : BindableBase
    {
        public bool IsChecked
        {
            get { return GetProperty(() => IsChecked); }
            set { SetProperty(() => IsChecked, value); }
        }
        public ObservableCollection<MatchAttach> Attaches
        {
            get { return GetProperty(() => Attaches); }
            set { SetProperty(() => Attaches, value); }
        }

        public match Match
        {
            get { return GetProperty(() => Match); }
            set { SetProperty(() => Match, value); }
        }

        public WrapMatch() : this(null)
        {

        }

        public WrapMatch(match match)
        {
            Match = match ?? new match();
            if (match != null)
                LoadData();
        }
        public void LoadData()
        {
            if (Match == null || Match.ID <= 0)
                return;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.GetAttachsByIDAndTypeCompleted += (s, e) =>
            {
                Attaches = e.Result;
            };
            client.GetAttachsByIDAndTypeAsync(Match.ID,"Match");
        }
    }
}
