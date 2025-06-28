using AutoMapper;
using B3DeveloperEvaluation.Application.Dtos;
using B3DeveloperEvaluation.Domain.Entities;

namespace B3DeveloperEvaluation.Application.Mappings;

public class InvestmentProfile : Profile
{
    public InvestmentProfile()
    {
        CreateMap<InvestmentRequestDto, Investment>(MemberList.Source);
        CreateMap<Investment, InvestmentResponseDto>(MemberList.Destination);
    }
}