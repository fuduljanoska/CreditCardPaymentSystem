using AutoMapper;
using CreditCardPaymentAPI.DTOs;
using CreditCardPaymentAPI.Models;
using CreditCardPaymentAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditCardPaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardEventProducer _creditCardEventProducer;

        public CreditCardController(IMapper mapper, ICreditCardEventProducer creditCardEventProducer)
        {
            _mapper = mapper;
            _creditCardEventProducer = creditCardEventProducer;
        }

        // POST api/<CreditCardController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreditCardPostDto creditCardPostDto, [FromHeader] DateTime dateOfRequest)
        {
            var creditCard = _mapper.Map<CreditCardModel>(creditCardPostDto);

            try
            {
                await _creditCardEventProducer.ProduceCreditCardEventAsync(creditCard);
            }
            catch(HttpRequestException ex)
            {
                return this.Problem(ex.Message);
            }

            return Accepted();
        }
    }
}
