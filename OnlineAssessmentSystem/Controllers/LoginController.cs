using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineAssessmentSystem.Models;
using Entities;

namespace OnlineAssessmentSystem.Controllers
{
    public class LoginController : ApiController
    {
        IBLLService bllService;
        public LoginController()
        {
            bllService = new BLLService();
        }

        public LoginController(IBLLService bService)
        {
            bllService = bService;
        }


        [System.Web.Http.HttpPost] //comment
        [System.Web.Http.Route("api/Login/Login")]
        public IHttpActionResult LoginUser(LoginModel loginData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user existingUser = bllService.UserLogin(loginData.Username, loginData.Password);
                    existingUser.userTests = null;
                    existingUser.role = null;
                    if (existingUser != null)
                    {
                        return Ok(existingUser);
                    }
                    else
                    {
                        return BadRequest("Invalid username or password");
                    }
                }
                else
                {
                    return BadRequest("Invalid Request");
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
