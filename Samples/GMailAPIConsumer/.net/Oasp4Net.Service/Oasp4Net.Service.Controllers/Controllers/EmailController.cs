
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oasp4Net.Arquitecture.CommonTools.Source.Enums;
using Oasp4Net.Business.Repositories.EmailManagement.Implementation;
using Oasp4Net.Business.Repositories.EmailManagement.Interfaces;
using Oasp4Net.Business.Views.Views;

namespace Oasp4Net.Service.Controllers.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Send(EmailView emailView)
        {
            bool result;
            try
            {
                IEmailRepository emailRepo = new EmailRepository();
                result= emailRepo.Send(emailView);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"{ex.Message} : {ex.InnerException}");                
            }
            return Content(HttpStatusCode.OK, result);
        }
    }
}