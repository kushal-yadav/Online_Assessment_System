using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccessLayer;
using NUnit.Framework;
using NSubstitute;
using BusinessLogicLayer;

namespace OnlineAssessmentSystem.Tests
{
    [TestFixture]
    public class BLLTests
    {
        IBLLService bService;
        IUserService userService;
        IAdminService adminService;

        IEnumerable<question> questions;
        IEnumerable<topic> topics;
        IEnumerable<user> user;
        IEnumerable<userTestAnswer> userTestAnswer;
        IEnumerable<testLog> testLogs;
        IEnumerable<userTest> userTest;
        IEnumerable<category> category;
        IEnumerable<test> test;
        IEnumerable<subcategory> subcat;

        [OneTimeSetUp]
        public void SetUp()
        {
            subcat = new List<subcategory>
            {
                new subcategory { categoryID =1, subcategoryID = 1, subcategoryName="TestSub1"},
                new subcategory { categoryID =1, subcategoryID = 2, subcategoryName="TestSub2"},
                new subcategory { categoryID =1, subcategoryID = 3, subcategoryName="TestSub3"},

            };
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
            };

            category = new List<category>
            {
                new category() {
                    categoryID = 1,
                    categoryName="cat1",

                },
                new category() {
                     categoryID = 12,
                    categoryName="cat2",

                }
            };

            userTest = new List<userTest>
            {
                new userTest() {
                    is_Active = true,
                    marksScored = 2,
                    statusOfTest = "fail",
                    testID = 1,
                    userID = 1,
                    user = new user {userID =1, name = "Test1" },
                    userTestID = 1,
                    test = new test {testID =1, available =true, duration = TimeSpan.Zero, questions=new List<question> { new question { questionID= 1, testid =1} } },

                }
            };

            test = new List<test>
            {
                new test() {
                    is_Active = true,
                    testID =1,
                    available =true,
                    duration = TimeSpan.Zero,
                    subcategoryID = 1

                },
            };

            userTestAnswer = new List<userTestAnswer> {
                new userTestAnswer { srno = 3, userTestID = 9, questionID = 1, question=new question { questionID=1,
                    questionText="your name?",
                    answerType=1,
                    option1="a",
                    option2="b",
                    option3="c",
                    option4="d",
                    topicID=2,
                    testid=1,} }
                };

            testLogs = new List<testLog> {
                new testLog {
                    testStarted =  new DateTime(2014, 12, 1),
                    testEnded = new DateTime(2015, 12, 1),
                    marksScored = 15,
                    topicID = 2,
                    userTestID = 1,
                    testLogID = 2
                }
            };

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
            };

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
            };

            userService = Substitute.For<IUserService>();
            adminService = Substitute.For<IAdminService>();

            bService = new BLLService(userService, adminService);
        }

        [Test]
        public void GetCategoryTest()
        {
            userService.GetAllCategories().Returns(category);

            IEnumerable<category> result = bService.GetCategories();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        [TestCase(1, 1)]
        public void GetSubCategoryTest(int uid, int cid)
        {
            userService.GetSubcategoriesByUser(1, 1).Returns(subcat);

            IEnumerable<subcategory> result = bService.GetSubCategories(uid, cid);

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void FailedListTest()
        {
            adminService.GetFailedStudents().Returns(userTest);

            IEnumerable<userTest> result = bService.FailedStudents();

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        [TestCase(new[] { 1, 2, 3 })]
        public void UpdateFailedTest(int[] a)
        {
            adminService.UpdateFailedStudentList(new System.Collections.Generic.List<int> { 1, 2, 3 }).Returns(true);

            bool result = bService.UpdateFailed(a.ToList());

            Assert.AreEqual(true, result);
        }

        [Test]
        [TestCase(3, 9, 1)]
        public void GetQuestionTest(int srno, int utID, int currID)
        {
            userService.GetQuestionBySrNo(3, 9).Returns(userTestAnswer.First());
            userService.AddTotestlog(9, 2).Returns(true);

            Entities.userTestAnswer result = bService.GetNextQuestion(srno, utID, currID);

            Assert.AreEqual(9, result.userTestID);
        }
    }
}
