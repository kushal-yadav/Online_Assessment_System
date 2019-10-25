using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Entities;
using BusinessLogicLayer;
using OnlineAssessmentSystem.CustomAuthorize;

namespace OnlineAssessmentSystem.Controllers
{
    public class TestLogsController : ApiController
    {

        IBLLService bllService;
        public TestLogsController()
        {
            bllService = new BLLService();
        }

        public TestLogsController(IBLLService bService)
        {
            bllService = bService;
        }

        [AuthorizeUser]
        [ResponseType(typeof(userTest))]
        public IHttpActionResult GetTestLogs(int id)
        {
            var result = bllService.GetTestLogs(id)
                .Select(res => new userTest {
                    userTestID = res.userTestID,
                    userID = res.userID,
                    testID = res.testID,
                    marksScored = res.marksScored,
                    statusOfTest = res.statusOfTest,
                    created_by = res.created_by,
                    modified_by = res.modified_by,
                    is_Active = res.is_Active,
                    test = new test { subcategory= new subcategory { subcategoryName = res.test.subcategory.subcategoryName} }                    
                });
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [AuthorizeUser]
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttestLog(int id)
        {
            try
            {
                var testlog = bllService.UpdateTestLog(id);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}