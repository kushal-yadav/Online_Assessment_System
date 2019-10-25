using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Class1 cl = new Class1();
            ////    var x = cl.mymethod();

            UserService us = new UserService();
            //us.GetHistory(2);
            //us.GetTestLogs(4);
            //us.GetSubCategoryName(1);
            //us.GetSubCatName(2);
            //us.MarksCalculate(2);
            //us.GetUserResult(2);
            //us.UpdateTestLog(2);
            //us.GetUserTestResult(2);
            us.Reports(4, 2);
            //us.GetSubcategoriesByUser(4, 2);

            //category cat01 = new category { categoryName = "Java" };
            //subcategory subcat01 = new subcategory { subcategoryName = "Java01" };

            BLLService _bllService = new BLLService();
            //_bllService.GetUserTestResult(2);
            AdminService ads = new AdminService();
            //ads.AddCategory(cat01);
            //ads.AddSubCategory(subcat01);
            //ads.GetQuestionsBySubcategory(2);
            //ads.RemoveTest(3);
            // _bllService.AddExcelToDb(new test { testID = 1 }, "D:\\Major Project 01\\BackEnd\\OnlineAssessmentSystem\\OnlineAssessmentSystem\\Uploads\\SampleNew.xlsx");
            // _bllService.UpdateTestLog(2);
            // var data = _bllService.GetSubcategoryByCategory(2).ToList();
            //var data = _bllService.GetUserTest(1, 1);
        }
    }
}
