using Entities;
using OnlineAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLLService
    {
        IEnumerable<category> GetCategories();
        IEnumerable<subcategory> GetSubCategories(int uID,int cID);
        int AddCategory(category category);
        int AddSubCategory(subcategory subcategory);
        int AddTopic(topic topic);
        int EditQuestion(question editQuestion, int questionId);
        IEnumerable<subcategory> GetSubcategoryByCategory(int categoryId);
        IEnumerable<question> GetQuestionsBySubcategory(int subcategoryId);
        IEnumerable<userTest> FailedStudents();
        IEnumerable<testLog> GetHistory(int utID);
        userTest GetUserTestResult(int utID);
        List<userTest> GetTestLogs(int userID);
        user UserLogin(string uname, string pwd);
        bool UpdateTestLog(int userTestID);
        bool RegisterUser(user newUser);
        bool UpdateFailed(List<int> userTestIDList);
        userTestAnswer GetNextQuestion(int srno, int utestID, int currentQuesID);
        bool MarkAnswer(int serialno, int utestID, string answer);
        bool AddTest(test newTest, string filePath);
        question GetQuestion(int questionID);
        int SetUpTest(int userID, int testID);
        test GetUserTest(int subcategoryID, int categoryID);
        user GetUserbyID(int userID);
        test GetTestDetails(int subCategoryID);
        List<List<ReportModel>> Reports(int userID, int categoryID);
    }
}