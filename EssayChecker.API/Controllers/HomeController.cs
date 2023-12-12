using EssayChecker.API.Brokers.OpenAI;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using System.Threading.Tasks;

namespace EssayChecker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        private readonly IOpenAIBroker openAIBroker;

        public HomeController(IOpenAIBroker openAIBroker)
        {
            this.openAIBroker = openAIBroker;
        }

        [HttpGet]
        public ActionResult<string> GetHomeMessage() => 
            Ok("Tarteeb is running");

        [HttpPost]
        public async ValueTask<ActionResult<ChatCompletion>> Post(
            ChatCompletion chatCompletion)
        {
            return await openAIBroker
                .AnalyseEssayAsync(chatCompletion); 
        }

    }
}
