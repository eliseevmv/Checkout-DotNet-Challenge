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
            CreateMap<PaymentGateway.Services.Entities.Payment, PaymentGateway.Models.PaymentDetails>();
        }
    }
}
