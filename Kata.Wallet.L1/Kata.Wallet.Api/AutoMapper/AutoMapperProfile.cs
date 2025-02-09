using AutoMapper;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Domain.Transaction, TransactionDto>()
            .ForMember(dest => dest.WalletIncoming, opt => opt.MapFrom(src => src.WalletIncoming))
            .ForMember(dest => dest.WalletOutgoing, opt => opt.MapFrom(src => src.WalletOutgoing));

        CreateMap<TransactionDto, Domain.Transaction>();
        CreateMap<Domain.Wallet, WalletDto>();
        CreateMap<WalletDto, Domain.Wallet>();
    }
}
