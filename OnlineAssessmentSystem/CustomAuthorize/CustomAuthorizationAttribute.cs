using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using BusinessLogicLayer;
using System.Net.Http;
using System.Net;

namespace OnlineAssessmentSystem.CustomAuthorize
{
    public class CustomAuthorizationAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var x = actionContext.Request.Headers.GetValues("Authorization").First();
            if (actionContext.Request.Headers.GetValues("Authorization").First() != null)
            {
                
                // get value from header
                int authenticateUserID = Convert.ToInt32(
                actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault());
                
                BLLService bllService = new BLLService();
                var roleID = bllService.GetUserbyID(authenticateUserID).roleID;
                if (roleID != 2)
                {
                    HttpContext.Current.Response.AddHeader("authenticationToken", authenticateUserID+"");
                    HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    return;
                }
                HttpContext.Current.Response.AddHeader("authenticationToken", authenticateUserID+"");
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "Authorized");
                return;
            }
            actionContext.Response =
              actionContext.Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            actionContext.Response.ReasonPhrase = "Please provide valid inputs";
        }
    }
}