using Domain.Contracts;
using Domain.Transaction;

namespace Domain.Interfaces;

public interface IResultWriter
{
    string WriteBlock(BlockDto block);

    string WriteTransaction(TransactionDto transaction);

    string WriteMempool(MempoolDto mempool, bool ascending = false);
}