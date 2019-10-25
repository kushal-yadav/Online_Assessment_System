using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BusinessLogicLayer;
using Entities;
using System.Net.Http;
using System.Web;
using OnlineAssessmentSystem.CustomAuthorize;

namespace OnlineAssessmentSystem.Controllers
{
    public class AdminController : ApiController
    {
        IBLLService bllService;
        public AdminController()
        {
            bllService = new BLLService();
        }

        public AdminController(IBLLService bService)
        {
            bllService = bService;
        }

        [CustomAuthorization]
        [HttpGet]
        [Route("api/Admin/category")]
        public IEnumerable<category> Get()
        {
            return bllService.GetCategories().Select(c => new category { categoryID = c.categoryID, categoryName = c.categoryName });
        }

        [CustomAuthorization]
        [HttpPost]
        [Route("api/Admin/AddCategory")]
        public IHttpActionResult AddCategory(category category1)
        {

          
            int result = bllService.AddCategory(category1);
            if (result!=null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Cannot add same existing category");
            }
        }



        [CustomAuthorization]
        [HttpGet]
        [Route("api/Admin/SubCategory")]
        public IEnumerable<subcategory> SubCategory(int id)
        {
            return bllService.GetSubcategoryByCategory(id).Select(c => new subcategory { subcategoryID = c.subcategoryID, subcategoryName = c.subcategoryName });
        }


        [CustomAuthorization]
        [HttpPost]
        [Route("api/Admin/AddSubCategory")]
        public IHttpActionResult AddSubCategory(subcategory subCategory)
        {


            int result = bllService.AddSubCategory(subCategory);
            return Ok();
        }


        [CustomAuthorization]
        [HttpGet]
        [Route("api/Admin/Questions")]
        public IEnumerable<question> Questions(int id)
        {
            return bllService.GetQuestionsBySubcategory(id).Select(q => new question
            {
                questionID = q.questionID,
                questionText = q.questionText,
                option1 = q.option1,
                option2 = q.option2,
                option3 = q.option3,
                option4 = q.option4,
                marks = q.marks,
                answer = q.answer,
                answerType = q.answerType,
                complexity = q.complexity
            });
        }

        [CustomAuthorization]
        [HttpPut]
        [Route("api/Admin/EditQuestion")]
        public int EditQuestion(int id,question EditQuestion)
        {
            return bllService.EditQuestion(EditQuestion,id);
        }

        [CustomAuthorization]
        [HttpGet]
        [ActionName("Question")]
        public IHttpActionResult GetQuestion(int id)
        {
            try
            {
                question retrieved = bllService.GetQuestion(id);


                if (retrieved != null)
                {
                    question question = new question()
                    {
                        answerType = retrieved.answerType,
                        complexity = retrieved.complexity,
                        answer = retrieved.answer,
                        marks = retrieved.marks,
                        option1 = retrieved.option1,
                        option2 = retrieved.option2,
                        option3 = retrieved.option3,
                        option4 = retrieved.option4,
                        testid = retrieved.testid,
                        topicID = retrieved.topicID,
                        questionID = retrieved.questionID,
                        questionText = retrieved.questionText,
                    };
                    return Ok(question);
                }
                else
                {
                    return BadRequest("Question Not Found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorization]
        [HttpPost]
        [Route("api/Admin/UploadFile")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                string filePath;
                var httpRequest = HttpContext.Current.Request;
                
                var subcategoryID = httpRequest.Form["subcategoryID"];
                var available = httpRequest.Form["available"];
                var questionPaperName = httpRequest.Form["questionPaperName"];
                var duration = httpRequest.Form["duration"];

                if (httpRequest.Files.Count < 1)
                {
                    return BadRequest("Invalid Request");
                }

                test newTest = new test();
                if (available == "true")
                    newTest.available = true;
                else
                    newTest.available = false;

                newTest.subcategoryID = Convert.ToInt32(subcategoryID);
                newTest.questionPaperName = questionPaperName;
                newTest.duration = TimeSpan.FromMinutes(Convert.ToInt32(duration));
                newTest.created_on = DateTime.Now;

                var postedFile = httpRequest.Files[0];
                filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + postedFile.FileName);
                postedFile.SaveAs(filePath);

                bllService.AddTest(newTest, filePath);
                return Ok("Test Created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

    }
}
