using Microsoft.WindowsAPICodePack.Shell;
using NPFaq.Web.Extensions;
using NPFaq.Web.Models;
using NPFaq.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;

namespace NPFaq.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MatchService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MatchService.svc 或 MatchService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MatchService : IMatchService
    {
        public void DoWork()
        {

        }
        public IEnumerable<faq_category> GetAllCategorys()
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_category.ToList();
            }
        }

        public int AddCategory(string categoryName)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                var result = context.faq_category.Add(new faq_category() { CategoryName = categoryName });
                context.SaveChanges();
                return result.ID;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                var c = context.faq_category.FirstOrDefault(l => l.ID == id);
                var result = context.faq_category.Remove(c);
                context.SaveChanges();
                return true;
            }
        }
        public int GetCountOfCategory(int categoryID)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_question.Where(l => l.CategoryID == categoryID).Count();
            }
        }

        public IEnumerable<faq_question> GetPagedQuestions(int pageSize, int currentPage, int categoryID)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_question.Where(l => l.CategoryID == categoryID).OrderByDescending(l => l.SearchCount).Skip(pageSize * currentPage).Take(pageSize).ToList();
            }
        }

        public IEnumerable<faq_answer> GetAnswersByID(int id)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_answer.Where(l => l.QuestionID == id).OrderBy(l => l.Order).ToList();
            }
        }

        public IEnumerable<faq_question> SearchFaqs(string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
                return null;
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                var result = context.faq_question.Where(l => l.Question.Contains(keyWord) || l.KeyWord.Contains(keyWord)).OrderByDescending(l => l.SearchCount).ToList();
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    using (var context1 = ContextHelper.CreateContext<TestEntities>())
                    {
                        result.ToList().ForEach(l =>
                        {
                            var q = context1.faq_question.FirstOrDefault(p => p.ID == l.ID);
                            if (q != null)
                                q.SearchCount = q.SearchCount.HasValue ? ++q.SearchCount : 1;
                        });
                        context1.SaveChanges();
                    }
                });
                return result;
            }
        }
        public faq_question GetQuestionByID(int id)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_question.FirstOrDefault(l => l.ID == id);
            }
        }

        public int AddOrUpdateQuestion(faq_question question)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                context.faq_question.AddOrUpdate(question);
                if (question.ID > 0)
                {
                    context.faq_answer.Where(l => l.QuestionID == question.ID).ToList().ForEach(l => context.faq_answer.Remove(l));
                    context.faq_attach.Where(l => l.QuestionID == question.ID).ToList().ForEach(l => context.faq_attach.Remove(l));
                    context.faq_answer.AddRange(question.faq_answer);
                    context.faq_attach.AddRange(question.faq_attach);
                }
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                }
                return question.ID;
            }
        }
        public int AddAnswer(faq_answer answer)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                context.faq_answer.Add(answer);
                context.SaveChanges();
                return answer.ID;
            }
        }
        public IEnumerable<MatchAttach> GetAttachsByIDAndType(int id, string type)
        {
            var context = ContextHelper.CreateContext<MatchEntities>();
            var result = new List<MatchAttach>();
            context.attaches.Where(l => l.Type == type && l.TypeID == id).ToList().ForEach(l =>
            {
                var attach = l.CopyTo<MatchAttach>();
                try
                {
                    attach.URL = new Uri(HttpContext.Current.Request.Url, l.Path).AbsoluteUri;
                    var path = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~"), l.Path);
                    attach.Size = new System.IO.FileInfo(path).Length;
                    var file = ShellFile.FromFilePath(path);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    file.Thumbnail.MediumBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    attach.Thumbnail = ms.ToArray();
                }
                catch (Exception e) { }
                finally
                {
                    result.Add(attach);
                }
            });
            return result;
        }
        public IEnumerable<string> GetImagesByQuestionID(int id)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                var baseuri = System.Web.HttpContext.Current.Request.Url;
                return context.faq_attach.Where(l => l.QuestionID == id && l.Type == "Image").ToList().Select(l => new Uri(baseuri, "ClientBin/Attach/" + id + "/" + l.Path).AbsoluteUri);
            }
        }

        public bool AddFaq_Attachs(IEnumerable<faq_attach> attachs)
        {
            if (attachs == null)
                return false;
            TestEntities context = ContextHelper.CreateContext<TestEntities>();
            attachs.ToList().ForEach(l => context.faq_attach.Add(l));
            context.SaveChanges();
            return true;
        }

        public bool DeleteFaqByID(IEnumerable<int> faqID)
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                context.faq_question.ToList().ForEach(l =>
                {
                    if (faqID.Contains(l.ID))
                        context.faq_question.Remove(l);
                });
                context.SaveChanges();
                var root = System.Web.HttpContext.Current.Server.MapPath("~");
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    faqID.ToList().ForEach(l =>
                    {
                        var dir = root + @"\ClientBin\Attach\" + l;
                        if (System.IO.Directory.Exists(dir))
                            System.IO.Directory.Delete(dir, true);
                    });
                });
                return true;
            }
        }
        private string MD5(string origin)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(origin))).Replace("-", "");
        }
        public bool Login(string userName, string password)
        {
            var context = ContextHelper.CreateContext<MatchEntities>();
            password = MD5(password);
            var user = context.users.Where(l => l.Name == userName && l.Password == password).FirstOrDefault();
            return user == null ? false : true;
        }

        public IEnumerable<faq_question> GetAllQuestion()
        {
            using (var context = ContextHelper.CreateContext<TestEntities>())
            {
                return context.faq_question.ToList();
            }
        }

        public IEnumerable<user> GetAllUsers()
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.users.ToList();
            }
        }
        public user GetUserByName(string userName)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.users.FirstOrDefault(l => l.Name == userName);
            }
        }
        public void AddOrUpdateUser(user user)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                if (user.ID <= 0)
                    user.Password = MD5(user.Password);
                context.users.AddOrUpdate(user);
                context.SaveChanges();
            }
        }
        public IEnumerable<user> SearchUser(string name)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.users.Where(l => l.Name.Contains(name) || l.Info.Contains(name)).ToList();
            }
        }

        public void DeleteUserByID(params int[] id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.users.Where(l => id.Contains(l.ID)).ToList().ForEach(l =>
                {
                    context.users.Remove(l);
                });
                context.SaveChanges();
            }
        }
        public bool AlterPassword(int id, string oldPwd, string newPwd)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                var pwd = MD5(oldPwd);
                var user = context.users.FirstOrDefault(l => l.ID == id && l.Password == pwd);
                if (user != null)
                {
                    user.Password = MD5(newPwd);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public IEnumerable<match> GetAllMatches()
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.matches.ToList();
            }
        }

        public int AddOrUpdateMatch(match match)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.matches.AddOrUpdate(match);
                context.SaveChanges();
                return match.ID;
            }
        }

        public IEnumerable<match> SearchMatch(string name)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.matches.Where(l => l.Name.Contains(name)).ToList();
            }
        }

        public void DeleteMatchByID(params int[] id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.matches.Where(l => id.Contains(l.ID)).ToList().ForEach(l =>
                {
                    context.matches.Remove(l);
                });
                context.SaveChanges();
            }
        }
        public IEnumerable<team> GetAllTeams()
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.teams.ToList();
            }
        }
        public int AddOrUpdateTeam(team team)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.teams.AddOrUpdate(team);
                context.SaveChanges();
                return team.ID;
            }
        }
        public IEnumerable<team> SearchTeam(string name)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.teams.Where(l => l.Name.Contains(name)).ToList();
            }
        }
        public void DeleteTeamByID(params int[] id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.teams.Where(l => id.Contains(l.ID)).ToList().ForEach(l =>
                {
                    context.teams.Remove(l);
                });
                context.SaveChanges();
            }
        }
        public void AddAttaches(IEnumerable<attach> attaches)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.attaches.AddRange(attaches);
                context.SaveChanges();
            }
        }
        public void DeleteAttachesByMID(int id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.attaches.Where(l => l.Type == "Match" && l.TypeID == id).ToList().ForEach(l =>
                {
                    context.attaches.Remove(l);
                });
                context.SaveChanges();
            }
        }
        public user GetUserByID(int id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.users.FirstOrDefault(l => l.ID == id);
            }
        }
        public match GetMatchByID(int id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                return context.matches.FirstOrDefault(l => l.ID == id);
            }
        }
        public void ReviewTeam(params int[] id)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                context.teams.Where(l => id.Contains(l.ID)).ToList().ForEach(l =>
                {
                    l.IsChecked = true;
                });
                context.SaveChanges();
            }
        }

        public void MarkTeam(int id, string result, string rank)
        {
            using (var context = ContextHelper.CreateContext<MatchEntities>())
            {
                var team = context.teams.FirstOrDefault(l => l.ID == id);
                if (team != null)
                {
                    team.Result = result;
                    team.Rank = rank;
                    context.SaveChanges();
                }
            }
        }
    }
}
