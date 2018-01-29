using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oasp4Net.Business.Common.GmailManagement.Dto;
using Oasp4Net.Business.Common.GmailManagement.Service;

namespace Oasp4Net.Business.Common.GmailManagement.Controller
{
    /// <summary>
    /// API Methods for sending emails
    /// </summary>
    public class GmailController : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public GmailController()
        {
        }

        /// <summary>
        /// Sends the email
        /// </summary>
        /// <param name="emailDto"></param>
        /// <returns>True if the email has been sent</returns>
        /// <response code="201">Account created</response>
        /// <response code="400">Username already in use</response>
        [HttpOptions]
        [HttpPost]
        [AllowAnonymous]
        [Route("/mythaistar/services/rest/EmailManagement/v1/Email/Send")]
        public  IActionResult Send([FromBody] EmailDto emailDto)
        {
            bool result;
            try
            {
                IOaspGmailService gmailService = new OaspGmailService();
                result =  gmailService.Send(emailDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message} : {ex.InnerException}");

            }
            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
