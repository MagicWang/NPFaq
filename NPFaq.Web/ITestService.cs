using NPFaq.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NPFaq.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ITestService”。
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_category> GetAllCategorys();
        /// <summary>
        /// 增加类别
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int AddCategory(string categoryName);
        /// <summary>
        /// 删除类别
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCategory(int id);
        /// <summary>
        /// 根据类别获取问题总数
        /// </summary>
        /// <param name="categoryID">类别ID</param>
        /// <returns></returns>
        [OperationContract]
        int GetCountOfCategory(int categoryID);
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <param name="pageSize">每页几条</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="categoryID">类别ID</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_question> GetPagedQuestions(int pageSize, int currentPage, int categoryID);
        /// <summary>
        /// 根据问题ID获取答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_answer> GetAnswersByID(int id);
        /// <summary>
        /// 根据关键字搜索常见问题
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_question> SearchFaqs(string keyWord);
        /// <summary>
        /// 获取所有问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_question> GetAllQuestion();
        /// <summary>
        /// 根据ID获取问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        faq_question GetQuestionByID(int id);
        /// <summary>
        /// 根据问题ID获取附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Attach> GetAttachsByQID(int id);
        /// <summary>
        /// 根据问题ID获取图片附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns>绝对url集合</returns>
        [OperationContract]
        IEnumerable<string> GetImagesByQuestionID(int id);
        /// <summary>
        /// 新增或更新问题(更新问题时会先清空回答和附件，不会重复上传文件)
        /// </summary>
        /// <param name="faq"></param>
        /// <returns></returns>
        [OperationContract]
        int AddOrUpdateQuestion(faq_question question);
        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [OperationContract]
        int AddAnswer(faq_answer answer);
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="attachs"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddFaq_Attachs(IEnumerable<faq_attach> attachs);
        /// <summary>
        /// 根据ID批量删除问题
        /// </summary>
        /// <param name="faqID"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteFaqByID(IEnumerable<int> faqID);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        bool Login(string userName, string password);
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<faq_user> GetAllUsers();
    }
}
