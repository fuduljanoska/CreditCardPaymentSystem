using CreditCardPaymentProcessor.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CreditCardPaymentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IFileWriterService fileWriterService;

        public TestController(IFileWriterService fileWriterService)
        {
            this.fileWriterService = fileWriterService;
        }
        // POST: TestController/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreditCardModel model)
        {
            try
            {
                await fileWriterService.AppendToFileAsync("Test.txt", model.ToString());
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
            return Ok();
        }
    }
}
