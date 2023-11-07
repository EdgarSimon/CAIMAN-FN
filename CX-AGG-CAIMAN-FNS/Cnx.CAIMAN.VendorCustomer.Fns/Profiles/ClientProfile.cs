using AutoMapper;
using Cnx.CAIMAN.VendorCustomer.Fns.JSONModels;
using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<RootAddressList, Root>()
                .ForPath(dest => dest.MT_ServiceBusJobSite, opt => opt.MapFrom(src => src.MT_ServiceBusJobSite))
                .ForPath(dest => dest.MT_ServiceBusJobSite.JobSitesList, opt => opt.MapFrom(src => src.MT_ServiceBusJobSite.JobSites))
                .ForPath(dest => dest.MT_ServiceBusCustomer, opt => opt.MapFrom(src => src.MT_ServiceBusCustomer))
                .ForPath(dest => dest.MT_ServiceBusCustomer.CustomersList, opt => opt.MapFrom(src => src.MT_ServiceBusCustomer.Customers));

            CreateMap<RootSingleAddress,Root >()
                .ForPath(dest => dest.MT_ServiceBusJobSite, opt => opt.MapFrom(src => src.MT_ServiceBusJobSite))
                .ForPath(dest => dest.MT_ServiceBusJobSite.JobSitesList, opt => opt.MapFrom(src => src.MT_ServiceBusJobSite.JobSites))
                .ForPath(dest => dest.MT_ServiceBusCustomer, opt => opt.MapFrom(src => src.MT_ServiceBusCustomer))
                .ForPath(dest => dest.MT_ServiceBusCustomer.CustomersList, opt => opt.MapFrom(src => src.MT_ServiceBusCustomer.Customers));
        }
    }
}
