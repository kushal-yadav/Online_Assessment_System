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
    public class UserTestResultController : ApiController
    {
        IBLLService bllService;
        public UserTestResultController()
        {
            bllService = new BLLService();
        }

        public UserTestResultController(IBLLService bService)
        {
            bllService = bService;
        }

        [AuthorizeUser]
        [ResponseType(typeof(userTest))]
        public IHttpActionResult GetuserTest(int id)
        {
            var result = bllService.GetUserTestResult(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}