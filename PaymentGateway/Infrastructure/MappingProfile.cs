using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Services.Entities.Payment, Models.PaymentDetails>();

            CreateMap<Models.ProcessPaymentRequest, Services.Entities.Payment>();

            CreateMap<Services.Entities.Payment, Services.ServiceClients.AcquiringBankClient.Models.BankPaymentRequest>();
            // todo map properties

            CreateMap<Services.ServiceClients.AcquiringBankClient.Models.BankPaymentResponse, Services.Entities.Payment>();
           //     .ForMember(dest => dest., opt => opt.MapFrom(src => src.PaymentIdentifier;
            // todo map properties

        }
    }
}
