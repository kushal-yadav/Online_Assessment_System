using Entities;
using OnlineAssessmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserService:IUserService
    {
        IOASEntities context;

        public UserService(IOASEntities user)
        {
            context = user;
        }

        public UserService()
        {
            context = new OASEntities();
        }

        public IEnumerable<user> DemoTest()
        {
            throw new NotImplementedException();
        }
        
        public bool RegisterUser(user newUser)
        {
            try
            {
                user u = context.users.FirstOrDefault(x => x.username == newUser.username);

                if(u == null)
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    var verifyPassword = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(newUser.passw));
                    newUser.passw = BitConverter.ToString(verifyPassword).Substring(0, 15);
                    newUser.roleID = 1;
                    newUser.userID = context.users.Count() + 1;
                    context.users.Add(newUser);
                    return context.SaveChanges() > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException e)
            {

                throw;
            }
        }
        
        public IEnumerable<category> GetAllCategories()
        {
            try 
            { 
                return context.categories.Where(c=>c.is_Active==true).ToList();
            }
            catch (SqlException e)
            {
                throw;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public IEnumerable<subcategory> GetSubcategoriesByUser(int userID,int catID)
        {
            try
            {
                IQueryable<int> subcategoryIDs = context.userTests.Where(ut => ut.userID == userID && ut.is_Active == true).Select(ut => ut.test).Where(t=>t.available==true).Select(t=>t.subcategoryID.Value);
                IQueryable<subcategory> sub = context.subcategories.Where(sc => !subcategoryIDs.Contains(sc.subcategoryID) && sc.is_Active == true && sc.categoryID == catID);
                List<subcategory> subcategoryList = new List<subcategory>();
                foreach (var subcategory in sub)
                {
                    if(context.tests.FirstOrDefault(t=>t.subcategoryID == subcategory.subcategoryID) != null)
                    {
                        subcategoryList.Add(subcategory);
                    }
                }
                return subcategoryList;
            }
            catch (SqlException e)
            {
                throw;
            }
            catch(Exception e)
            {
                throw;
            }
        }
        
        public test GetUserTest(int subcategoryID, int categoryID)
        {
            try
            {
                test currentTest = context.tests.FirstOrDefault(t => t.subcategoryID == subcategoryID && t.is_Active==true && t.available==true);
                return new test
                {
                    duration = currentTest.duration,
                    passingMarks = currentTest.passingMarks,
                    questionPaperName = currentTest.questionPaperName,
                    testID = currentTest.testID,
                    totalMarks = currentTest.totalMarks,
                    subcategoryID = currentTest.questions.Count
                };
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public user GetUserbyID(int userID)
        {
            try
            {
                return context.users.FirstOrDefault(u => u.userID == userID);
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int StartTest(int userID,int testID)
        {
            try
            {
                int id = context.userTests.Count()+1;
                context.userTests.Add(new userTest
                {
                    created_by = GetUserbyID(userID).name,
                    is_Active = true,
                    testID = testID,
                    userID = userID,
                    userTestID = id,
                    test = context.tests.FirstOrDefault(x=>x.testID == testID)
                });
                var result = (context.SaveChanges() > 0) ? id : 0;
                return result;
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public bool PopulateTestAnswer(int uTestID)
        {
            try
            {
                int serialno = 1;
                int testAnswerID = context.userTestAnswers.Count() + 1;
                IEnumerable<int> questionIds = context.userTests.FirstOrDefault(u => u.userTestID == uTestID).test.questions.OrderBy(q=>q.topicID).Select(q => q.questionID);
                foreach (int question in questionIds)
                {
                    context.userTestAnswers.Add(new userTestAnswer
                    {
                        questionID = question,
                        srno = serialno++,
                        userTestAnswerID = testAnswerID++,
                        userTestID = uTestID,
                    });
                }
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public userTestAnswer GetQuestionBySrNo(int serialno, int testID)
        {
            try
            {
                return context.userTestAnswers.FirstOrDefault(ut => ut.srno.Value == serialno && ut.userTestID == testID);
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public bool MarkAnswer(int serialno, int utestID,string answer)
        {
            try
            {
                userTestAnswer option = context.userTestAnswers.FirstOrDefault(ans => ans.srno == serialno && ans.userTestID == utestID);
                if(option.question.answerType == 1)
                {
                    option.answerMarked = answer;
                }
                if (context.SaveChanges()>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }
        
        public IEnumerable<testLog> GetHistory(int utID)
        {
            try
            {
                return context.userTests.FirstOrDefault(u=>u.userTestID == utID).testLogs.ToList();
            }
            catch (SqlException)
            {
                throw;
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
                var result = context.userTests.FirstOrDefault(x => x.userTestID == userTestID);
                var tests = context.tests.FirstOrDefault(x => x.testID == result.testID);
                result.test = tests;

                return result;
            }
            catch (SqlException)
            {
                throw;
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
                var result = context.userTests.Where(x => x.userID == userID).ToList();//var res2 = context.userTests.Where(x => x.userID == userID).Select(x => x.test.subcategory.subcategoryName);
                var res = context.userTests.Where(x => x.userID == userID).Select(x => x.test).ToList();
                var date = context.userTests.Where(x => x.userID == userID).Select(x => x.testLogs.Select(tlog => tlog.testStarted.Value)).ToList();
                int i = 0;
                foreach (var usertest in result)
                {
                    usertest.testLogs.First().testStarted = date[i].First();
                    usertest.test = res[i];
                    i++;
                }

                return result;
            }
            catch (SqlException)
            {
                throw;
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
                user u = context.users.FirstOrDefault(x => x.username == uname);
                if(u == null)
                {
                    return null;
                }

                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                var verifyPassword = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pwd));
                var result = (u.passw == BitConverter.ToString(verifyPassword).Substring(0, 15)) ? u.userID : -1;
                return context.users.FirstOrDefault(x => x.userID == result);
                
            }
            catch (SqlException e)
            {

                throw;
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
                var query = from u in context.users
                            join ut in context.userTests
                            on u.userID equals ut.userID
                            join uta in context.userTestAnswers
                            on ut.userTestID equals uta.userTestID
                            join q in context.questions
                            on uta.questionID equals q.questionID
                            join t in context.topics
                            on q.topicID equals t.topicID
                            where uta.answerMarked == q.answer && ut.userTestID==userTestID
                            group q.marks by t.topicID into g
                            select new MarksModel { topicID = g.Key, Marks = g.Sum() };

                int count = context.testLogs.Count();
                var testLog = context.testLogs.FirstOrDefault(tl => tl.userTestID == userTestID);
                if(testLog!=null)
                {
                    foreach (var item in context.testLogs.Where(ut=>ut.userTestID == userTestID))
                    {
                        MarksModel topicMarks = query.Where(tpcID => tpcID.topicID == item.topicID).FirstOrDefault();
                        if (topicMarks==null)
                        {
                            item.marksScored = 0;
                        }
                        else
                        {
                            item.marksScored = topicMarks.Marks;
                        }
                    }
                    var testLog01 = context.testLogs.FirstOrDefault(tl => tl.userTestID == userTestID && tl.testEnded == null);
                    testLog01.testEnded = DateTime.Now;
                    if (context.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (SqlException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
        public userTest MarksCalculate(int userTestID)
        {
            try
            {
                int count = context.userTests.Count();
                var totalMarks = context.testLogs.Where(t => t. userTestID == userTestID).Select(m => m.marksScored).Sum();
                userTest userTests = context.userTests.FirstOrDefault(utID => utID.userTestID == userTestID);
                userTests.marksScored = totalMarks;
                var result = context.SaveChanges();
                List<test> tests = context.tests.Where(p=>p.testID == userTests.testID).ToList();

                if (totalMarks >= tests.Select(p=>p.passingMarks).First())
                {
                    userTests.statusOfTest = "pass";
                    result = context.SaveChanges();
                }
                else
                {
                    userTests.statusOfTest = "fail";
                    result = context.SaveChanges();
                }

                if (result > 0)
                {
                    return context.userTests.FirstOrDefault(ut => ut.userTestID == userTestID);
                }
                else
                {
                    return context.userTests.FirstOrDefault(ut => ut.userTestID == userTestID);
                }
            }
            catch (SqlException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AddTotestlog(int utID,int tID)
        {
            try
            {
                bool result = true;
                if (context.testLogs.FirstOrDefault(t => t.userTestID == utID && t.topicID == tID) == null)
                {
                    testLog testlogs = context.testLogs.FirstOrDefault(t => t.testEnded == null && t.userTestID==utID);
                    if (testlogs!=null)
                    {
                        testlogs.testEnded = DateTime.Now;

                    }
                    context.testLogs.Add(new testLog
                    {
                        testStarted = DateTime.Now,
                        topicID = tID,
                        userTestID = utID,
                        testLogID = context.testLogs.Count() + 1
                    });

                    if (context.SaveChanges() <= 0)
                    {
                        result = false;
                    }
                }
                return result;
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

                var query01 = (from ut in context.userTests
                               join t in context.tests
                               on ut.testID equals t.testID
                               join s in context.subcategories
                               on t.subcategoryID equals s.subcategoryID
                               join c in context.categories
                               on s.categoryID equals c.categoryID
                               where ut.userID == userID && s.categoryID == categoryID
                               group new { ut.marksScored, t.totalMarks, c.categoryName } by s.categoryID into g
                               select new ReportModel
                               {
                                   TotalMarks = g.Sum(x => x.totalMarks).Value,
                                   Marks = (g.Sum(x => x.marksScored).Value)*100 / (g.Sum(x => x.totalMarks).Value),
                                   Name = g.FirstOrDefault().categoryName
                               }).ToList();

                var query02 = (from ut in context.userTests
                               join t in context.tests
                               on ut.testID equals t.testID
                               join s in context.subcategories
                               on t.subcategoryID equals s.subcategoryID
                               join c in context.categories
                               on s.categoryID equals c.categoryID
                               where s.categoryID == categoryID
                               group new { ut.marksScored, t.totalMarks, c.categoryName } by s.categoryID into g
                               select new ReportModel
                               {
                                   TotalMarks = g.Sum(x => x.totalMarks).Value,
                                   Marks = (g.Sum(x => x.marksScored).Value)*100 / (g.Sum(x => x.totalMarks).Value),
                                   Name = g.FirstOrDefault().categoryName
                               }).ToList();

                var query03 = (from ut in context.userTests
                               join t in context.tests
                               on ut.testID equals t.testID
                               join s in context.subcategories
                               on t.subcategoryID equals s.subcategoryID
                               where ut.userID == userID && s.categoryID == categoryID
                               group new { s.subcategoryName, t.totalMarks, ut.marksScored } by t.subcategoryID into g
                               select new ReportModel
                               {
                                   TotalMarks = g.Sum(x => x.totalMarks).Value,
                                   Marks = (g.Sum(x => x.marksScored).Value)*100 / (g.Sum(x => x.totalMarks).Value),
                                   Name = g.FirstOrDefault().subcategoryName
                               }).ToList();

                var query04 = from q in context.questions
                              join t in context.tests
                              on q.testid equals t.testID
                              join ut in context.userTests
                              on t.testID equals ut.testID
                              join tp in context.topics
                              on q.topicID equals tp.topicID
                              join s in context.subcategories
                              on t.subcategoryID equals s.subcategoryID
                              where ut.userID == userID && s.categoryID == categoryID
                              group new { q.marks, t.testID, q.topicID, tp.topicName } by new { t.testID, q.topicID } into g
                              select new
                              {
                                  testID = g.FirstOrDefault().testID,
                                  total = g.Sum(x => x.marks),
                                  topicID = g.FirstOrDefault().topicID.Value,
                                  topicName = g.FirstOrDefault().topicName
                              };

                var query05 = (from ut in context.userTests
                               join tl in context.testLogs
                               on ut.userTestID equals tl.userTestID
                               from q in query04
                               where ut.testID.Value == q.testID
                               && q.topicID == tl.topicID.Value
                               group new { tl.marksScored, q.total, q.topicName } by tl.topicID into g
                               select new ReportModel
                               {
                                   Marks = (g.Sum(x => x.marksScored).Value) * 100 / (g.Sum(x => x.total)),
                                   TotalMarks = g.Sum(x => x.total),
                                   Name = g.FirstOrDefault().topicName
                               }).ToList();

                List<List<ReportModel>> list = new List<List<ReportModel>>();
                list.Add(query01);
                list.Add(query02);
                list.Add(query03);
                list.Add(query05);
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
