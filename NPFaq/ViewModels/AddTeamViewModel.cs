using DevExpress.Mvvm;
using NPFaq.TestServiceReference;
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
using System.Linq;
using NPFaq.Common.Extensions;
using System.Collections.Generic;
using NPFaq.Common.Interactivity.InteractionRequest;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using NPFaq.Models;
using NPFaq.MatchServiceReference;
using NPFaq.Enums;

namespace NPFaq.ViewModels
{
    public class AddTeamViewModel : ValidationViewModelBase
    {
        private WrapTeam wrapTeam;
        public ICommand SubmitCommand { get; set; }
        public InteractionRequest CloseRequest { get; set; }
        [Required(ErrorMessage = "队伍名称不能为空")]
        public string TeamName
        {
            get { return GetProperty(() => TeamName); }
            set { SetProperty(() => TeamName, value); }
        }

        public string Introduce
        {
            get { return GetProperty(() => Introduce); }
            set { SetProperty(() => Introduce, value); }
        }
        [Required(ErrorMessage = "指导老师不能为空")]
        public string TeacherName
        {
            get { return GetProperty(() => TeacherName); }
            set { SetProperty(() => TeacherName, value); }
        }
        [Required(ErrorMessage = "队长不能为空")]
        public string TeamLeader
        {
            get { return GetProperty(() => TeamLeader); }
            set { SetProperty(() => TeamLeader, value); }
        }

        public string TeamMembers
        {
            get { return GetProperty(() => TeamMembers); }
            set { SetProperty(() => TeamMembers, value); }
        }

        public List<match> Matches
        {
            get { return GetProperty(() => Matches); }
            set { SetProperty(() => Matches, value); }
        }
        [Required(ErrorMessage = "比赛不能为空")]
        public match SelectedMatch
        {
            get { return GetProperty(() => SelectedMatch); }
            set { SetProperty(() => SelectedMatch, value); }
        }

        public AddTeamViewModel()
        {
            SubmitCommand = new DelegateCommand<object>(SubmitExecute);
            CloseRequest = new InteractionRequest();
            TeacherName = App.CurrentUser.Name;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.GetAllMatchesCompleted += (s, e) =>
            {
                Matches = e.Result.Where(l => DateTime.Now > l.StartSign && DateTime.Now < l.EndSign).ToList();
            };
            client.GetAllMatchesAsync();
        }

        protected override void OnParameterChanged(object parameter)
        {
            if (parameter is WrapTeam)
            {
                wrapTeam = parameter as WrapTeam;
                TeamName = wrapTeam.Team.Name;
                Introduce = wrapTeam.Team.Introduce;
                TeacherName = wrapTeam.Teacher.Name;
                TeamLeader = wrapTeam.Team.TeamLeader;
                TeamMembers = wrapTeam.Team.TeamMembers;
                SelectedMatch = wrapTeam.Match;
            }
        }
        private void SubmitExecute(object obj)
        {
            if (!IsValid())
                return;
            if (wrapTeam == null)
            {
                wrapTeam = new WrapTeam();
                wrapTeam.Team.TeacherID = App.CurrentUser.ID;
                wrapTeam.Team.MatchID = SelectedMatch.ID;
            }
            wrapTeam.Team.Name = TeamName;
            wrapTeam.Team.Introduce = Introduce;
            wrapTeam.Team.TeamLeader = TeamLeader;
            wrapTeam.Team.TeamMembers = TeamMembers;

            var client = Utils.ServicesFactory.CreateMatchService();
            client.AddOrUpdateTeamCompleted += (s, e) =>
            {
                wrapTeam.Team.ID = e.Result;
                CloseRequest.Raise();
            };
            client.AddOrUpdateTeamAsync(wrapTeam.Team);
        }
    }
}
