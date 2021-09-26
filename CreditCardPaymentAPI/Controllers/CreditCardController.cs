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
    /// <summary>
    /// Credit card controller 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardEventProducer _creditCardEventProducer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper">Instance of automapper</param>
        /// <param name="creditCardEventProducer">Instance of credit card event producer</param>
        public CreditCardController(IMapper mapper, ICreditCardEventProducer creditCardEventProducer)
        {
            _mapper = mapper;
            _creditCardEventProducer = creditCardEventProducer;
        }

        /// <summary>
        /// Produces credit card event
        /// </summary>
        /// <param name="creditCardPostDto">Credit card</param>
        /// <param name="dateOfRequest">Date of request</param>
        /// <returns></returns>
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
