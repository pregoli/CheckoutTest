using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Application.Common.Dto;
using Checkout.Application.Common.Interfaces;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Checkout.Application.Services
{
    public class TransactionsHistoryService : ITransactionsHistoryService
    {
        private readonly ITransactionsHistoryRepository _transactionsHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsHistoryService> _logger;

        public TransactionsHistoryService(
            ITransactionsHistoryRepository transactionsHistoryRepository,
            IMapper mapper,
            ILogger<TransactionsHistoryService> logger)
        {
            _transactionsHistoryRepository = transactionsHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TransactionItemDto> AddAsync(TransactionItemDto transactionItem)
        {
            var transaction = _mapper.Map<TransactionHistory>(transactionItem);
            await _transactionsHistoryRepository.AddAsync(transaction);
            return transactionItem;
        }

        public async Task<TransactionItemDto> GetByTransactionIdAsync(Guid id)
        {
            var transaction = await _transactionsHistoryRepository.GetAsync(id);
            return _mapper.Map<TransactionItemDto>(transaction);
        }

        public async Task<List<TransactionItemDto>> GetByMerchantIdAsync(Guid id)
        {
            var transactions = await _transactionsHistoryRepository.Query(x => x.MerchantId == id).ToListAsync();
            return _mapper.Map<List<TransactionItemDto>>(transactions);
        }
    }
}
