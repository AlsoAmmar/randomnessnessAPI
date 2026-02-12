using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RandomnessnessAPI.Models;

namespace RandomnessnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private static string json = System.IO.File.ReadAllText("Data/data.json");
        private static Message message;

        [HttpGet]
        public ActionResult<string> GetMessage()
        {
            return Ok(json);
        }

        [HttpGet("text")]
        public ActionResult<string> GetMessageText()
        {
            message = JsonConvert.DeserializeObject<Message>(json);
            return message.text;
        }

        [HttpPost]
        public ActionResult<Message> PostMessage([FromBody] Message newMessage)
        {
            json = JsonConvert.SerializeObject(newMessage);
            System.IO.File.WriteAllText("Data/data.json", json);
            return CreatedAtAction("GetMessage", newMessage);
        }
    }
}
