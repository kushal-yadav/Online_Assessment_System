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
    class AdminServiceTest
    {
        IAdminService adminService;
        IOASEntities context;
        IQueryable<test> test;
        IQueryable<question> question;
        IQueryable<user> user;
        IQueryable<userTest> userTest;
        IQueryable<category> category;
        [OneTimeSetUp]
        public void Setup()
        {
            context = Substitute.For<IOASEntities>();
            IDbSet<user> customerDbSet = Substitute.For<IDbSet<user>>();

            adminService = new AdminService(context);

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

            IDbSet<test> testDbSet = Substitute.For<IDbSet<test>>();
            test = new List<test>
            {
                new test()
                {
                    testID = 1,
                    available = true,
                    passingMarks = 30,
                    totalMarks = 100,
                    subcategoryID = 1,
                    duration = TimeSpan.Parse("1"),
                    questionPaperName = "Core Java",
                    
                }

            }.AsQueryable();
            testDbSet.Provider.Returns(test.Provider);
            testDbSet.Expression.Returns(test.Expression);
            testDbSet.ElementType.Returns(test.ElementType);
            testDbSet.GetEnumerator().Returns(test.GetEnumerator());
            context.tests.Returns(testDbSet);

            IDbSet<question> questionDbSet = Substitute.For<IDbSet<question>>();
            question = new List<question>
            {
                new question()
                {
                    questionID = 1,
                    option1 = "p",
                    option2 = "q",
                    option3 = "r",
                    option4 = "s",
                    topicID = 1,
                    marks = 3,
                    testid = 1
                }

            }.AsQueryable();
            questionDbSet.Provider.Returns(question.Provider);
            questionDbSet.Expression.Returns(question.Expression);
            questionDbSet.ElementType.Returns(question.ElementType);
            questionDbSet.GetEnumerator().Returns(question.GetEnumerator());
            context.questions.Returns(questionDbSet);

            IDbSet<userTest> userTestDbSet = Substitute.For<IDbSet<userTest>>();
            userTest = new List<userTest>
            {
                new userTest()
                {
                    testID = 1,
                    statusOfTest = "fail",
                    userTestID = 1,
                    userID = 1,
                    user = user.First(x=>x.userID == 1),
                    is_Active = true
                }

            }.AsQueryable();
            userTestDbSet.Provider.Returns(userTest.Provider);
            userTestDbSet.Expression.Returns(userTest.Expression);
            userTestDbSet.ElementType.Returns(userTest.ElementType);
            userTestDbSet.GetEnumerator().Returns(userTest.GetEnumerator());
            context.userTests.Returns(userTestDbSet);

            IDbSet<category> categoryDbSet = Substitute.For<IDbSet<category>>();
            category = new List<category>
            {
                new category()
                {
                   categoryID = 1,
                   categoryName = "test",
                }

            }.AsQueryable();
            categoryDbSet.Provider.Returns(category.Provider);
            categoryDbSet.Expression.Returns(category.Expression);
            categoryDbSet.ElementType.Returns(category.ElementType);
            categoryDbSet.GetEnumerator().Returns(category.GetEnumerator());
            context.categories.Returns(categoryDbSet);
        }

        [Test]
        public void AddTest_Test()
        {
            context.SaveChanges().Returns(1);
            var result = adminService.AddTest(test.First(), question.ToList());

            Assert.That(result);
        }

        [Test]
        public void GetFailedStudents_Test()
        {
            
            var result = adminService.GetFailedStudents();

            Assert.AreEqual(typeof(List<userTest>), result.GetType());
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void UpdateFailedStudentList_Test()
        {
            
            context.SaveChanges().Returns(1);
            var result = adminService.UpdateFailedStudentList(adminService.GetFailedStudents().Select(x=>x.userID.Value).ToList());

            Assert.That(result);
        }

        [Test]
        public void GetQuestion_Test()
        {
            var result = adminService.GetQuestion(1);

            Assert.AreEqual(question.First(),result);
        }

        [Test]
        public void GetAllCategories_Test()
        {
            var result = adminService.GetAllCategories();

            Assert.AreEqual(category.ToList(), result);
        }
    }
    

}
