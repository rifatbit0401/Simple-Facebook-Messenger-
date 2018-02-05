using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FacebookTest
{
    public class Test : Controller
    {

        private string fbToken = "EAAEr0ZAgmeE0BADBhduVAR0VpZAoYmvqXz4cXngXZATDYhuCkljJ7ewF3lnQYgWM5Q4UZBhCgHtYEZCtn4uGH2TDpSsc2EmZCpsPmMLCCSWZA0uFgJXy9rES2ZAAmAgLhDzbUbQkFOZC6ZAw38XlxT3abeikTDJjjIqeDRHwpWz30WadlkV927AyXo";
        private string postUrl = "https://graph.facebook.com/v2.6/me/messages";

        [HttpGet]
        [Route("Test/Get")]
        public string Get()
        {
            return "okkk";
        }

        [HttpGet]
        [Route("Test/Webhook")]
        public string Webhook([FromQuery(Name = "hub.mode")] string mode,
                    [FromQuery(Name = "hub.challenge")] string challenge,
                    [FromQuery(Name = "hub.verify_token")] string verify_token)
        {

            if (verify_token.Equals("my_token_is_great"))
            {
                return challenge;
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        [Route("Test/Webhook")]
        public void Webhook()
        {
            var json = (dynamic)null;
            try
            {
                using (StreamReader sr = new StreamReader(this.Request.Body))
                {
                    json = sr.ReadToEnd();
                }
                dynamic data = JsonConvert.DeserializeObject(json);

                postToFB((string)data.entry[0].messaging[0].sender.id, (string)data.entry[0].messaging[0].message.text);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void postToFB(string recipientId, string messageText)
        {
            //Post to ApiAi
            string messageTextAnswer = "Hello !!!!";
            string postParameters = string.Format("access_token={0}&recipient={1}&message={2}",
                                            fbToken,
                                            "{ id:" + recipientId + "}",
                                            "{ text:\"" + messageTextAnswer + "\"}");

            //Response from ApiAI or answer to FB question from user post it to FB back.
            var client = new HttpClient();
            client.PostAsync(postUrl, new StringContent(postParameters, Encoding.UTF8, "application/json"));
        }


    }
}
