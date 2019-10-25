using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAssessmentSystem.Models;

namespace DataAccessLayer
{
    public interface IUserService
    {
        IEnumerable<user> DemoTest();
        IEnumerable<category> GetAllCategories();
        IEnumerable<subcategory> GetSubcategoriesByUser(int userID, int catID);
        test GetUserTest(int subcategoryID, int categoryID);
        user GetUserbyID(int userID);
        int StartTest(int userID, int testID);
        bool PopulateTestAnswer(int uTestID);
        userTestAnswer GetQuestionBySrNo(int serialno, int testID);
        bool MarkAnswer(int serialno, int utestID, string answer);
        IEnumerable<testLog> GetHistory(int utID);
        userTest GetUserTestResult(int userTestID);
        List<userTest> GetTestLogs(int userID);
        user UserLogin(string uname, string pwd);
        bool UpdateTestLog(int userTestID);
        userTest MarksCalculate(int userTestID);
        bool RegisterUser(user newUser);
        bool AddTotestlog(int utID, int tID);
        List<List<ReportModel>> Reports(int userID, int categoryID);
    }
}
