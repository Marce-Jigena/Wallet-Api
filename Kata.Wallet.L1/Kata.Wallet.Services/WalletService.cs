﻿using System.Net;
using AutoMapper;
using Kata.Wallet.Database;
using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    public readonly IMapper _mapper;

    public WalletService(
        IMapper mapper, 
        IWalletRepository walletRepository)
    {
        _mapper = mapper;
        _walletRepository = walletRepository;
    }

    public async Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument)
    {
        return await _walletRepository.GetAllAsync(currency, userDocument);
    }

    public async Task<List<Transaction>> GetTransactionsAsync(int walletId)
    {
        var wallet = await _walletRepository.GetByIdAsync(walletId);
        return await _walletRepository.GetTransactionsAsync(wallet);
    }

    //modificar para que devuelva wallet
    public async Task NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description) 
    {
        var originWallet = await _walletRepository.GetByIdAsync(originWalletId);
        var destinationWallet = await _walletRepository.GetByIdAsync(destinationWalletId);

        if (Exists(originWallet) && Exists(destinationWallet))
        {
            if (originWallet.Currency != destinationWallet.Currency)
            {
                throw new Exception("Invalid Currency");
            }
            else
            {
                //Se crea transferencia y transaccion.
                await _walletRepository.NewTransfer(originWallet, destinationWallet, amount, description);
            }
        }
        else
        {
            //Generar Bad Request
        }
    }

    public async Task CreateAsync(WalletDto walletDto)
    {
        var wallet = _mapper.Map<Domain.Wallet>(walletDto);
        await _walletRepository.AddAsync(wallet);
    }

    public bool Exists(Domain.Wallet wallet) 
    {
        if (wallet == null) return false;
        else return true;
    }
}

