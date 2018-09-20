namespace Sales.API.Controllers
{
    using System;
    using System.IO;
    using System.Web.Http;
    using Common.Models;
    using Helpers;
    using Newtonsoft.Json.Linq;

    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        public IHttpActionResult PostUser(UserRequest userRequest)
        {
            if (userRequest.ImageArray != null && userRequest.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(userRequest.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Users";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    userRequest.ImagePath = fullPath;
                }
            }

            var answer = UsersHelper.CreateUserASP(userRequest);
            if (answer.IsSuccess)
            {
                return Ok(answer);
            }

            return BadRequest(answer.Message);
        }

        [HttpPost]
        [Authorize]
        [Route("GetUser")]
        public IHttpActionResult GetUser(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
