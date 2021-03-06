﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Services.Entities.Payment, API.Models.Payment>();

            CreateMap<API.Models.ProcessPaymentRequest, Services.Entities.Payment>();

            CreateMap<Services.Entities.Payment, Services.ServiceClients.AcquiringBankClient.Models.BankPaymentRequest>()
                .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentCurrency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.PaymentCardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.PaymentExpiryMonthAndDate, opt => opt.MapFrom(src => src.ExpiryMonthAndDate))
                .ForMember(dest => dest.PaymentCvv, opt => opt.MapFrom(src => src.Cvv));

            CreateMap<Services.Entities.Payment, API.Models.ProcessPaymentResponse>();

        }
    }
}
