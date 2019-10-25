using BusinessLogicLayer;
using Entities;
using OnlineAssessmentSystem.CustomAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace OnlineAssessmentSystem.Controllers
{
    public class UserController : ApiController
    {
        IBLLService bllService;
        public UserController()
        {
            bllService = new BLLService();
        }

        public UserController(IBLLService bService)
        {
            bllService = bService;
        }

        [AuthorizeUser]
        public IEnumerable<category> Get()
        {
            return bllService.GetCategories().Select(c => new category{ categoryID = c.categoryID, categoryName = c.categoryName  } );
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/user/Register")]
        public IHttpActionResult Register(user newUser)
        { 
            try
            {
                if (ModelState.IsValid)
                {
                    if (bllService.RegisterUser(newUser))
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("username exists");
                    }
                }
                else
                {
                    return BadRequest("Invalid modelstate");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/subcategory")]
        public IEnumerable<subcategory> Getsubcategory(int userID,int catID)
        {
            return bllService.GetSubCategories(userID,catID).Select(c => new subcategory { subcategoryID = c.subcategoryID, subcategoryName = c.subcategoryName });
        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("Test")]
        public IHttpActionResult GetTest(int subcategoryid, int categoryid)
        {
            try
            {
                test test = bllService.GetUserTest(subcategoryid, categoryid);
                if (test != null)
                {
                    return Ok(test);
                }
                else
                {
                    return BadRequest("Test Not Found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/result")]
        //[ResponseType(typeof(userTest))]
        public IHttpActionResult GetuserTest(int id)
        {
            var result = bllService.GetUserTestResult(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [AuthorizeUser]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/reports")]
        public IHttpActionResult GetReports(int userID, int categoryID)
        {
            var result = bllService.Reports(userID, categoryID);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}