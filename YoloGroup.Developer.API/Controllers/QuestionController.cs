using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoloGroup.Developer.API.Models.Response;
using YoloGroup.Developer.API.Services;

namespace YoloGroup.Developer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {

        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }

        /// <summary>
        /// This endpoint inverts the text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <example> Hi there I am Timi</example>
        [HttpPost]
        [Route("question/one")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Question1(string input)
        {
            if(string.IsNullOrEmpty(input))
                input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            return Ok(_questionService.inverseText(input));
        }

        /// <summary>
        /// This endpoint simulates a parrallel processing 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/two")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Question2()
        {
            _questionService.ProcessConcurrent();
            return Ok();
        }


        /// <summary>
        /// Hash file without loading to memory 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/three")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Question3()
        {
           var response = _questionService.LoadAndHashFile();
            return Ok(response);
        }


        /// <summary>
        /// Consume APi and process enteries
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("question/four")]
        [ProducesResponseType(200, Type = typeof(PaginatedResponse<AssetPriceResponse>))]
        public async Task< IActionResult> Question4(int page = 1, int pageSize = 20)
        {
            var response  = await _questionService.GetPaginatedAssetPrices(page, pageSize);
            return Ok(response);
        }
    }
}
