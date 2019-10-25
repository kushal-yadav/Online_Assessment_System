using DataAccessLayer;
using Entities;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAssessmentSystem.Tests
{

    public class DemoTest
    {

        IUserService userService;
        IOASEntities context;
        IQueryable<testLog> testLogs;
        IQueryable<userTest> userTest;
        IQueryable<question> questions;
        IQueryable<topic> topics;
        IQueryable<userTestAnswer> userTestAnswer;
        IQueryable<user> user;
        IQueryable<subcategory> subCategories;

        [OneTimeSetUp]
        public void Setup()
        {
            context = Substitute.For<IOASEntities>();
            IDbSet<user> customerDbSet = Substitute.For<IDbSet<user>>();

            userService = new UserService(context);
            user = new List<user>
            {
                new user() {
                    userID = 1,
                    email = "test@test.com",
                    gender = "male",
                    name = "test",
                    passw = "test",
                    username = "test",
                    roleID = 1
                },
                new user() {
                    userID = 2,
                    email = "test1@test.com",
                    gender = "male",
                    name = "test1",
                    passw = "test",
                    username = "test1",
                    roleID = 1
                }
            }.AsQueryable();
            customerDbSet.Provider.Returns(user.Provider);
            customerDbSet.Expression.Returns(user.Expression);
            customerDbSet.ElementType.Returns(user.ElementType);
            customerDbSet.GetEnumerator().Returns(user.GetEnumerator());
            context.users.Returns(customerDbSet);

            IDbSet<category> categoryDbSet = Substitute.For<IDbSet<category>>();
            IQueryable<category> category = new List<category>
            {
                new category() {
                    categoryID = 1,
                    categoryName="cat1",

                },
                new category() {
                     categoryID = 12,
                    categoryName="cat2",

                }
            }.AsQueryable();
            categoryDbSet.Provider.Returns(category.Provider);
            categoryDbSet.Expression.Returns(category.Expression);
            categoryDbSet.ElementType.Returns(category.ElementType);
            categoryDbSet.GetEnumerator().Returns(category.GetEnumerator());
            context.categories.Returns(categoryDbSet);

            IDbSet<userTest> userTestDbSet = Substitute.For<IDbSet<userTest>>();
            userTest = new List<userTest>
            {
                new userTest() {
                    is_Active = true,
                    marksScored = 2,
                    statusOfTest = "fail",
                    testID = 1,
                    userID = 1,
                    userTestID = 1,
                    test = new test {
                        testID =1,
                        available =true,
                        duration = TimeSpan.Zero,
                        questions = new List<question> { new question { questionID= 1, testid =1} },
                        subcategory = new subcategory { categoryID =1, subcategoryID = 1, subcategoryName="TestSub1"}
                    } 
                }
            }.AsQueryable();
            userTestDbSet.Provider.Returns(userTest.Provider);
            userTestDbSet.Expression.Returns(userTest.Expression);
            userTestDbSet.ElementType.Returns(userTest.ElementType);
            userTestDbSet.GetEnumerator().Returns(userTest.GetEnumerator());
            context.userTests.Returns(userTestDbSet);

            IDbSet<test> testDbSet = Substitute.For<IDbSet<test>>();
            IQueryable<test> test = new List<test>
            {
                new test() {
                    is_Active = true,
                    testID =1,
                    available =true,
                    duration = TimeSpan.Zero,
                    subcategoryID = 1,
                    subcategory = new subcategory { categoryID =1, subcategoryID = 1, subcategoryName="TestSub1"}

                },
            }.AsQueryable();
            testDbSet.Provider.Returns(test.Provider);
            testDbSet.Expression.Returns(test.Expression);
            testDbSet.ElementType.Returns(test.ElementType);
            testDbSet.GetEnumerator().Returns(test.GetEnumerator());
            context.tests.Returns(testDbSet);

            IDbSet<userTestAnswer> userTestAnswerDbSet = Substitute.For<IDbSet<userTestAnswer>>();
            userTestAnswer = new List<userTestAnswer> {
                new userTestAnswer { srno = 2, answerMarked="a", userTestID = 1, questionID=1 },
                new userTestAnswer { srno = 2, userTestID = 9, },
                new userTestAnswer { srno = 3, userTestID = 9, questionID = 12, question=new question { questionID=12,testid=2} }
                }.AsQueryable();
            userTestAnswerDbSet.Provider.Returns(userTestAnswer.Provider);
            userTestAnswerDbSet.Expression.Returns(userTestAnswer.Expression);
            userTestAnswerDbSet.ElementType.Returns(userTestAnswer.ElementType);
            userTestAnswerDbSet.GetEnumerator().Returns(userTestAnswer.GetEnumerator());
            context.userTestAnswers.Returns(userTestAnswerDbSet);

            IDbSet<testLog> testLogDbSet = Substitute.For<IDbSet<testLog>>();
            testLogs = new List<testLog> {
                new testLog {
                    testStarted =  new DateTime(2014, 12, 1),
                    testEnded = new DateTime(2015, 12, 1),
                    marksScored = 15,
                    topicID = 2,
                    userTestID = 1,
                    testLogID = 2
                }
            }.AsQueryable();
            testLogDbSet.Provider.Returns(testLogs.Provider);
            testLogDbSet.Expression.Returns(testLogs.Expression);
            testLogDbSet.ElementType.Returns(testLogs.ElementType);
            testLogDbSet.GetEnumerator().Returns(testLogs.GetEnumerator());
            context.testLogs.Returns(testLogDbSet);

            IDbSet<question> questionDbSet = Substitute.For<IDbSet<question>>();
            questions = new List<question> {
                new question {
                    questionID=1,
                    questionText="your name?",
                    answerType=1,
                    option1="a",
                    option2="b",
                    option3="c",
                    option4="d",
                    topicID=2,
                    testid=1,
                    answer="a",
                    marks=1,
                    complexity=2,
                    created_by="xyz",
                    modified_by="abc",
                    created_on=new DateTime(2014, 12, 1),
                    modified_on=new DateTime(2015, 12, 1),
                    is_Active=true
                }
            }.AsQueryable();
            questionDbSet.Provider.Returns(questions.Provider);
            questionDbSet.Expression.Returns(questions.Expression);
            questionDbSet.ElementType.Returns(questions.ElementType);
            questionDbSet.GetEnumerator().Returns(questions.GetEnumerator());
            context.questions.Returns(questionDbSet);

            IDbSet<topic> topicDbSet = Substitute.For<IDbSet<topic>>();
            topics = new List<topic> {
                new topic {
                    topicID=2,
                    topicName="oops",
                    created_by="xyz",
                    modified_by="abc",
                    created_on=new DateTime(2014, 12, 1),
                    modified_on=new DateTime(2015, 12, 1),
                    is_Active=true
                }
            }.AsQueryable();
            topicDbSet.Provider.Returns(topics.Provider);
            topicDbSet.Expression.Returns(topics.Expression);
            topicDbSet.ElementType.Returns(topics.ElementType);
            topicDbSet.GetEnumerator().Returns(topics.GetEnumerator());
            context.topics.Returns(topicDbSet);

            IDbSet<subcategory> subCategoryDbSet = Substitute.For<IDbSet<subcategory>>();
            subCategories = new List<subcategory> {
                new subcategory { categoryID =1, subcategoryID = 1, subcategoryName="TestSub1"},
                new subcategory { categoryID =1, subcategoryID = 2, subcategoryName="TestSub2"},
                new subcategory { categoryID =1, subcategoryID = 3, subcategoryName="TestSub3"},
            }.AsQueryable();
            subCategoryDbSet.Provider.Returns(subCategories.Provider);
            subCategoryDbSet.Expression.Returns(subCategories.Expression);
            subCategoryDbSet.ElementType.Returns(subCategories.ElementType);
            subCategoryDbSet.GetEnumerator().Returns(subCategories.GetEnumerator());
            context.subcategories.Returns(subCategoryDbSet);

        }

        [Test]
        public void GetCustomer()
        {

            var result = userService.DemoTest();

            Assert.AreEqual(result.GetType(), typeof(IEnumerable<user>));
            Assert.AreEqual(2,result.Count());
        }

        [Test]
        public void GetAllCategoryTest()
        {
            IEnumerable<category>cats = userService.GetAllCategories();

            Assert.AreEqual(2,cats.Count());
        }

        [Test]
        [TestCase(1,1)]
        public void SubCategoryByUserTest(int id,int cID)
        {
            IEnumerable<subcategory> count = userService.GetSubcategoriesByUser(id,cID);

            Assert.AreEqual(0, count.Count());
        }

        [Test]
        [TestCase(1)]
        public void GetUserTest(int id)
        {
            user getUser = userService.GetUserbyID(id);

            Assert.AreEqual(1, getUser.userID);
        }

        [Test]
        [TestCase(1,1)]
        public void StartTestTest(int a, int b)
        {
            int testID = userService.StartTest(a, b);

            Assert.AreEqual(2, testID);
        }

        [Test]
        [TestCase(1)]
        public void PopulateUserTestTest(int id)
        {
            bool result = userService.PopulateTestAnswer(id);

            Assert.AreEqual(true, result);
        }

        [Test]
        [TestCase(2,9,"a")]
        public void MarkAnswerTest(int sr,int utid,string ans)
        {
            bool isMarked = userService.MarkAnswer(sr, utid, ans);

            Assert.AreEqual(true, isMarked);
        }

        [Test]
        [TestCase(1)]
        public void GetHistory_Test(int utID)
        {
            userTest.First().testLogs = testLogs.ToList();
            var getHistory = userService.GetHistory(utID);
            Assert.AreEqual(1, getHistory.Count());
        }

        [Test]
        [TestCase("test", "test")]
        public void UserLogin_Test(string uname, string pwd)
        {
            user u = context.users.FirstOrDefault();
            Assert.AreEqual(u.username, uname);
            Assert.AreEqual(u.passw, pwd);
        }

        [Test]
        [TestCase(1)]
        public void GetUserTestResult_Test(int userTestID)
        {
            var result = userService.GetUserTestResult(userTestID);
            Assert.AreEqual("fail", result.statusOfTest);
        }

        [Test]
        [TestCase(1)]
        public void GetTestLogs_Test(int userID)
        {
            var result = userService.GetTestLogs(userID);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        [TestCase(1)]
        public void MarksCalculate_Test(int userTestID)
        {
            questions.First().userTestAnswers = userTestAnswer.ToList();
            userTest.First().userTestAnswers = userTestAnswer.ToList();
            var result = userService.MarksCalculate(userTestID);
            Assert.AreEqual(1, result.marksScored);
        }

        [Test]
        [TestCase(1)]
        public void UpdateTestLog_Test(int userTestID)
        {
            var result = userService.UpdateTestLog(userTestID);
            Assert.AreEqual(true, result);
        }
    }
    
}
