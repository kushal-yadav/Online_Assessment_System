using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using OnlineAssessmentSystem.Models;

namespace BusinessLogicLayer
{
    public class BLLService: IBLLService
    {
        IUserService userData;
        IAdminService adminData;

        #region Constructor
        public BLLService()
        {
            userData = new UserService();
            adminData = new AdminService();
        }

        public BLLService(IUserService user, IAdminService admin)
        {
            userData = user;
            adminData = admin;
        }
        #endregion
        
        public IEnumerable<category> GetCategories()
        {
            try
            {
                return userData.GetAllCategories().Select(c => new category { categoryID = c.categoryID, categoryName = c.categoryName });

            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public IEnumerable<subcategory> GetSubCategories(int uID,int cID)
        {
            try
            {
                return userData.GetSubcategoriesByUser(uID, cID).Select(s => new subcategory { subcategoryID = s.subcategoryID, subcategoryName = s.subcategoryName, categoryID = s.categoryID });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int  AddCategory(category category)
        {
            return adminData.AddCategory(category);
        }

        public int AddSubCategory(subcategory subcategory)
        {
            return adminData.AddSubCategory(subcategory);
        }

        public int AddTopic(topic topic)
        {
            return adminData.AddTopic(topic);
        }


        public int EditQuestion(question editQuestion, int questionId)
        {
            return adminData.EditQuestion(editQuestion,questionId);
        }


        public IEnumerable<subcategory> GetSubcategoryByCategory(int categoryId)
        {
            return adminData.GetSubcategoryByCategory(categoryId);
        }
        
        public IEnumerable<testLog> GetHistory(int utID)
        {
            try
            {
                return userData.GetHistory(utID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public userTest GetUserTestResult(int userTestID)
        {
            try
            {
                var result = userData.GetUserTestResult(userTestID);
                var m = (new userTest {
                    userTestID = result.userTestID,
                    userID = result.userID,
                    testID = result.testID,
                    marksScored = result.marksScored,
                    statusOfTest = result.statusOfTest,
                    created_by = result.created_by,
                    modified_by = result.modified_by,
                    is_Active = result.is_Active,
                    test = new test {
                        passingMarks = result.test.passingMarks,
                        totalMarks = result.test.totalMarks,
                        subcategory = new subcategory {
                            subcategoryName = result.test.subcategory.subcategoryName
                        }
                    }
                });
                return m;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<userTest> GetTestLogs(int userID)
        {
            try
            {
                return userData.GetTestLogs(userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        

        public user UserLogin(string uname, string pwd)
        {
            try
            {
                if(string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pwd))
                {
                    return null;
                }
                else
                {
                    return userData.UserLogin(uname, pwd);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateTestLog(int userTestID)
        {
            try
            {
                if(userData.UpdateTestLog(userTestID))
                {
                    if (userData.MarksCalculate(userTestID) != null)
                    {
                        return true;
                    }

                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public IEnumerable<question> GetQuestionsBySubcategory(int subcategoryId)
        {
            return adminData.GetQuestionsBySubcategory(subcategoryId);
        }

        public bool RegisterUser(user newUser)
        {
            try
            {
                newUser.createdDate = DateTime.Now;
                newUser.is_Active = true;
                return userData.RegisterUser(newUser);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public IEnumerable<userTest> FailedStudents()
        {
            try
            {
                return adminData.GetFailedStudents().Select(ut => new userTest { marksScored = ut.marksScored,
                    statusOfTest = ut.statusOfTest,
                    testID = ut.testID,
                    userID = ut.userID,
                    userTestID = ut.userTestID,
                    user = new user { userID = ut.user.userID, name = ut.user.name, },
                    test = new test { testID=ut.test.testID,questionPaperName=ut.test.questionPaperName, }, });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateFailed(List<int> userTestIDList)
        {
            return adminData.UpdateFailedStudentList(userTestIDList);
        }

        public userTestAnswer GetNextQuestion(int srno, int utestID, int currentsrno)
        {
            try
            {
                userTestAnswer ques = userData.GetQuestionBySrNo(srno, utestID);
                if(userData.AddTotestlog(utestID,ques.question.topicID.Value))
                {
                    return new userTestAnswer
                    {
                        question = new question
                        {
                            marks = ques.question.marks,
                            option1 = ques.question.option1,
                            option2 = ques.question.option2,
                            option3 = ques.question.option3,
                            option4 = ques.question.option4,
                            questionID = ques.question.questionID,
                            questionText = ques.question.questionText,
                            testid = ques.question.testid,
                            topicID = ques.question.topicID,
                            topic = ques.question.topic
                        },
                        questionID = ques.questionID,
                        srno = ques.srno,
                        userTestID = ques.userTestID,
                        answerMarked= ques.answerMarked,

                    };
                }
                else
                {
                    throw new Exception("Could'nt add topic to logs");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool MarkAnswer(int serialno, int utestID, string answer)
        {
            try
            {
                return userData.MarkAnswer(serialno, utestID, answer);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AddExcelToDb(test addedTest, string filePath)
        {
            List<question> questionList = new List<question>();
            try
            {
                Excel.Application xlApp = new Excel.Application();
                //Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                int srno = 0;
                var x = xlRange.Cells[2, 2].Value2;
                for (int i = 2; i <= rowCount; i++)
                {
                    questionList.Add(new question
                    {
                        questionID = ++srno,
                        questionText = xlRange.Cells[i, 3].Value2,
                        answerType = 1,
                        option1 = xlRange.Cells[i, 4].Value2,
                        option2 = xlRange.Cells[i, 5].Value2,
                        option3 = xlRange.Cells[i, 6].Value2,
                        option4 = xlRange.Cells[i, 7].Value2,
                        testid = addedTest.testID,
                        topicID = adminData.GetTopicByTopicName(xlRange.Cells[i, 2].Value2).topicID,
                        answer = xlRange.Cells[i, 8].Value2,
                        marks = Convert.ToInt32(xlRange.Cells[i, 9].Value2),
                        complexity = 1,
                        created_on = DateTime.Now,
                        is_Active = true
                    });

                }
                xlApp.Workbooks.Close();
                adminData.AddTest(questionList);
                return true;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public bool AddTest(test newTest, string filePath)
        {
            try
            {
                test addedTest = adminData.AddTest(newTest);
                if (addedTest != null)
                {
                    try
                    {
                        return AddExcelToDb(addedTest, filePath);
                    }
                    catch (Exception e)
                    {
                        adminData.RemoveTest(addedTest.testID);
                        throw new Exception("error in excel file");
                    }
                }
                else
                {
                    throw new Exception("error in test add");
                }
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        public question GetQuestion(int questionID)
        {
            try
            {
                if(questionID > 0)
                {
                    return adminData.GetQuestion(questionID);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public user GetUserbyID(int userID)
        {
            try
            {
               return userData.GetUserbyID(userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public test GetTestDetails(int subCategoryID)
        {
            try
            {
                return userData.GetUserTest(subCategoryID,1);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public int SetUpTest(int userID, int testID)
        {
            try
            {
                int userTestID = userData.StartTest(userID, testID);
                if (userData.PopulateTestAnswer(userTestID))
                {
                    return userTestID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public test GetUserTest(int subcategoryID, int categoryID)
        {
            try
            {
                test testInfo = userData.GetUserTest(subcategoryID, categoryID);
                if (testInfo != null)
                {
                    test testModel = new test()
                    {
                        testID = testInfo.testID,
                        questionPaperName = testInfo.questionPaperName,
                        totalMarks = testInfo.totalMarks,
                        duration = testInfo.duration,
                        subcategoryID = testInfo.subcategoryID
                    };
                    return testModel;
                }
                else
                {
                    throw new Exception("Test not Found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<List<ReportModel>> Reports(int userID, int categoryID)
        {
            try
            {
                return userData.Reports(userID, categoryID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
