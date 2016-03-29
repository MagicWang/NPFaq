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
    public class WrapTeam : BindableBase
    {
        public bool IsChecked
        {
            get { return GetProperty(() => IsChecked); }
            set { SetProperty(() => IsChecked, value); }
        }
        public bool IsCurrentUser { get { return Team == null ? false : Team.TeacherID == App.CurrentUser.ID; } }
        public ObservableCollection<MatchAttach> Attaches
        {
            get { return GetProperty(() => Attaches); }
            set { SetProperty(() => Attaches, value); }
        }

        public user Teacher
        {
            get { return GetProperty(() => Teacher); }
            set { SetProperty(() => Teacher, value); }
        }

        public match Match
        {
            get { return GetProperty(() => Match); }
            set { SetProperty(() => Match, value); }
        }

        public team Team
        {
            get { return GetProperty(() => Team); }
            set { SetProperty(() => Team, value); }
        }

        public WrapTeam() : this(null)
        {

        }

        public WrapTeam(team team)
        {
            Team = team ?? new team();
            if (team != null)
                LoadData();
        }
        public void LoadData()
        {
            if (Team == null || Team.ID <= 0)
                return;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.GetAttachsByIDAndTypeCompleted += (s, e) =>
            {
                Attaches = e.Result;
            };
            client.GetAttachsByIDAndTypeAsync(Team.ID, "Team");
            if (Team.TeacherID.HasValue)
            {
                client.GetUserByIDCompleted += (s, e) => Teacher = e.Result;
                client.GetUserByIDAsync(Team.TeacherID.Value);
            }
            if (Team.MatchID.HasValue)
            {
                client.GetMatchByIDCompleted += (s, e) => Match = e.Result;
                client.GetMatchByIDAsync(Team.MatchID.Value);
            }
        }
    }
}
