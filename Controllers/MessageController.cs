using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RandomnessnessAPI.Models;

namespace RandomnessnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public readonly IHubContext<messageHub> _hubContext;
        private static string json = System.IO.File.ReadAllText("Data/data.json");
        private static Message message;

        public MessageController(IHubContext<messageHub> hubContext)
        {
            _hubContext = hubContext;
        }

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
        public async Task<ActionResult<Message>> PostMessage([FromBody] Message newMessage)
        {
            json = JsonConvert.SerializeObject(newMessage);
            await System.IO.File.WriteAllTextAsync("Data/data.json", json);
            await _hubContext.Clients.All.SendAsync("ReceiveNewMessage", newMessage);
            return CreatedAtAction("GetMessage", newMessage);
        }
    }
}
