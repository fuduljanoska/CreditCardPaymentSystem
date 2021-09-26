using CreditCardPaymentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCardPaymentAPI.Services
{
    public interface ICreditCardEventProducer
    {
        Task ProduceCreditCardEventAsync(CreditCardModel creditCardModel);
    }
}
