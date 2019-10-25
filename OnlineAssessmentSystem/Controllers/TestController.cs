using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using BusinessLogicLayer;
using OnlineAssessmentSystem.CustomAuthorize;

namespace OnlineAssessmentSystem.Controllers
{
    public class TestController : ApiController
    {
        IBLLService bllService;
        public TestController()
        {
            bllService = new BLLService();
        }

        public TestController(IBLLService bService)
        {
            bllService = bService;
        }

        [AuthorizeUser]
        public IHttpActionResult Get(int id, int utID, int currentQues)
        {
            try
            {
                return Ok(bllService.GetNextQuestion(id, utID, currentQues));

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        [Route("api/test/mark")]
        public HttpResponseMessage MarkAnswer(int serialno, int utestID, string answer)
        {
            try
            {
                if (bllService.MarkAnswer(serialno, utestID, answer))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Error marking answer.");
                }
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            
        }

        [AuthorizeUser]
        [HttpGet]
        [Route("api/test/set")]
        public IHttpActionResult SetTest(int userID, int testID)
        {
            try
            {
                int userTestID = bllService.SetUpTest(userID, testID);
                if (userTestID > 0)
                {
                    return Ok(userTestID);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {

                return BadRequest("Error Occured");
            }   

        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/testlogdata")]
        public IHttpActionResult GetTestLogs(int id)
        {
            var result = bllService.GetTestLogs(id)
                .Select(res => new userTest
                {
                    userTestID = res.userTestID,
                    userID = res.userID,
                    testID = res.testID,
                    marksScored = res.marksScored,
                    statusOfTest = res.statusOfTest,
                    created_by = res.created_by,
                    modified_by = res.modified_by,
                    is_Active = res.is_Active,
                    test = new test {
                        passingMarks = res.test.passingMarks,
                        totalMarks = res.test.totalMarks,
                        subcategory = new subcategory {
                            subcategoryName = res.test.subcategory.subcategoryName
                        }
                    },
                    testLogs = new List<testLog>
                    {
                        new testLog
                        {                            
                            testStarted = res.testLogs.First().testStarted,
                        }
                    }
                });
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/puttestlog")]
        public IHttpActionResult PuttestLog(int id)
        {
            try
            {
                bool testlog = bllService.UpdateTestLog(id);
                if (testlog)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest("Exception has occured");
            }

        }

    }
}
