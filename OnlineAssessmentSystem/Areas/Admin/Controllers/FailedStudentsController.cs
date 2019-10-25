using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using BusinessLogicLayer;
using System.Web;
using System.Web.Mvc;
using OnlineAssessmentSystem.CustomAuthorize;

namespace OnlineAssessmentSystem.Areas.Admin.Controllers
{
    public class FailedStudentsController : ApiController
    {
        IBLLService bllService;
        public FailedStudentsController()
        {
            bllService = new BLLService();
        }

        public FailedStudentsController(IBLLService bService)
        {
            bllService = bService;
        }
     
        [CustomAuthorization]
        public IEnumerable<userTest> GetFailed()
        {
            return bllService.FailedStudents().ToList();
        }

        [CustomAuthorization]
        public HttpResponseMessage PostUpdateFailed(List<int> uTestID)
        {
            try
            {
                if (bllService.UpdateFailed(uTestID))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Retests made available!");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Something Went Wrong, PLease retry");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
