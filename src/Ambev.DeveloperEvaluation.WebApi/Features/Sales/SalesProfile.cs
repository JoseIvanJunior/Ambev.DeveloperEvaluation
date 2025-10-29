using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    public class SalesProfile : Profile
    {
        public SalesProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
        }
    }
}