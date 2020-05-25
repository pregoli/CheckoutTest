using AutoMapper;
using Checkout.Application.Common.Dto;
using Checkout.Domain.Entities;

namespace Checkout.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapTransaction();
        }

        private void MapTransaction()
        {
            CreateMap<TransactionHistory, TransactionItemDto>();
            CreateMap<TransactionItemDto, TransactionHistory>();
        }
    }
}
