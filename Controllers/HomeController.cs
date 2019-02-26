using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            // If the user is authenticated, then this is how you can get the access_token and id_token


            if (User.Identity.IsAuthenticated)
            {

                string accessToken = await HttpContext.GetTokenAsync("access_token");

                // if you need to check the access token expiration time, use this value
                // provided on the authorization response and stored.
                // do not attempt to inspect/decode the access token
                DateTime accessTokenExpiresAt = DateTime.Parse(
                    await HttpContext.GetTokenAsync("expires_at"),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

                string idToken = await HttpContext.GetTokenAsync("id_token");

                // Now you can use them. For more info on when and how to use the
                // access_token and id_token, see https://auth0.com/docs/tokens

                try
                {
                    HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3010/api/private");
                    wbRequest.Method = "GET";
                    wbRequest.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
                    wbRequest.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                    using (Stream responseStream = wbResponse.GetResponseStream())
                    {
                        using (StreamReader sReader = new StreamReader(responseStream))
                        {
                           ViewBag.jjj = sReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View();
                }

            }

            return View();
        }
        public IActionResult Table()
        {
            return View();
        }
        [Authorize]
        public IActionResult Detail(int i)
        {
            ViewBag.id = i;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
