using AutoMapper;
using CreditCardPaymentAPI.DTOs;
using CreditCardPaymentAPI.Models;

namespace CreditCardPaymentAPI.Profiles
{
    public class CreditCardProfiles:Profile
    {
        public CreditCardProfiles()
        {
            CreateMap<CreditCardPostDto, CreditCardModel>();
        }
    }
}
