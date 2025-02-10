using AutoMapper;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Domain.Transaction, TransactionDto>()
            .ForMember(dest => dest.WalletIncomingId, opt => opt.MapFrom(src => src.WalletIncoming.Id))
            .ForMember(dest => dest.WalletOutgoingId, opt => opt.MapFrom(src => src.WalletOutgoing.Id));

        CreateMap<TransactionDto, Domain.Transaction>();
        CreateMap<Domain.Wallet, WalletDto>();
        CreateMap<WalletDto, Domain.Wallet>();
    }
}
