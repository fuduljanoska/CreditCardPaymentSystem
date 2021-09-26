using CreditCardPaymentAPI.Models;
using CreditCardPaymentAPI.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreditCardPaymentAPITests
{
    public class CreditCardEventProducerTests
    {
        private Mock<IHttpClientFactory> _mockIHttpClientFactory;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        CreditCardEventProducer _creditCardEventProducer;

        [SetUp]
        public void Init()
        {
            _mockIHttpClientFactory = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        }
        [Test]
        public void ProduceCreditCardEventAsync_ShouldPass()
        {
            //Arrange
            var creditCardModel = new CreditCardModel();


            //Ako kreiras prazen HttpClient, ke pravi realni povici
            //Mu prakame mocked handler za da pusta mocked povici
            SetupHttpMessageHandler(HttpStatusCode.OK);
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _mockIHttpClientFactory.Setup(x => x.CreateClient("Test")).Returns(httpClient);
            _creditCardEventProducer = new CreditCardEventProducer(_mockIHttpClientFactory.Object);

            //Act
            Assert.DoesNotThrowAsync(async () => await _creditCardEventProducer.ProduceCreditCardEventAsync(creditCardModel));

            //Verify
            _mockIHttpClientFactory.Verify(x => x.CreateClient("Test"), Times.Once);
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void ProduceCreditCardEventAsync_SendAsyncShouldFail()
        {
            //Arrange
            var creditCardModel = new CreditCardModel();


            //Ako kreiras prazen HttpClient, ke pravi realni povici
            //Mu prakame mocked handler za da pusta mocked povici
            SetupHttpMessageHandler(HttpStatusCode.NotFound);
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _mockIHttpClientFactory.Setup(x => x.CreateClient("Test")).Returns(httpClient);
            _creditCardEventProducer = new CreditCardEventProducer(_mockIHttpClientFactory.Object);

            //Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await _creditCardEventProducer.ProduceCreditCardEventAsync(creditCardModel));

            //Verify
            _mockIHttpClientFactory.Verify(x => x.CreateClient("Test"), Times.Once);
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        private void SetupHttpMessageHandler(HttpStatusCode code)
        {
            _httpMessageHandlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = code,
                   Content = new StringContent("[{'id':1,'value':'1'}]"),
               })
               .Verifiable();
        }
    }
}
