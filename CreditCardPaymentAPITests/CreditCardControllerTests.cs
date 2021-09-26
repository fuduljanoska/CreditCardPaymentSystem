using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using AutoMapper;
using CreditCardPaymentAPI.Services;
using CreditCardPaymentAPI.Controllers;
using CreditCardPaymentAPI.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CreditCardPaymentAPI.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CreditCardPaymentAPITests
{
    public class CreditCardControllerTests
    {

        private Mock<IMapper> _mockIMapper;
        private Mock<ICreditCardEventProducer> _mockICreditCardEventProducer;
        CreditCardController _controler;

        [SetUp]
        public void Init()
        {
            _mockIMapper = new Mock<IMapper>();
            _mockICreditCardEventProducer = new Mock<ICreditCardEventProducer>();
        }

        [Test]
        public async Task Post_ShouldAccept()
        {
            var creditCardPostDto = new CreditCardPostDto();
            var creditCardModel = new CreditCardModel();

            _mockIMapper.Setup(x => x.Map<CreditCardModel>(creditCardPostDto)).Returns(creditCardModel);
            _mockICreditCardEventProducer.Setup(x => x.ProduceCreditCardEventAsync(creditCardModel)).Returns(Task.CompletedTask);

            _controler = new CreditCardController(_mockIMapper.Object, _mockICreditCardEventProducer.Object);
            var result = await _controler.Post(creditCardPostDto, DateTime.Now);

            Assert.IsTrue(result is AcceptedResult);
        }

        [Test]
        public async Task Post_ShouldHandleException()
        {
            var creditCardPostDto = new CreditCardPostDto();
            var creditCardModel = new CreditCardModel();

            //Arrange
            var problemDetails = new ProblemDetails()
            {
                //...populate as needed
            };
            var mock = new Mock<ProblemDetailsFactory>();
            mock.Setup(x => x.CreateProblemDetails(
                    It.IsAny<HttpContext>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                )
                .Returns(problemDetails);

            _mockIMapper.Setup(x => x.Map<CreditCardModel>(creditCardPostDto)).Returns(creditCardModel);
            _mockICreditCardEventProducer.Setup(x => x.ProduceCreditCardEventAsync(creditCardModel)).Throws(new HttpRequestException());

            _controler = new CreditCardController(_mockIMapper.Object, _mockICreditCardEventProducer.Object)
            {
                ProblemDetailsFactory = mock.Object
            };

            var result = await _controler.Post(creditCardPostDto, DateTime.Now);

            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).Value is ProblemDetails);
        }
    }
}
