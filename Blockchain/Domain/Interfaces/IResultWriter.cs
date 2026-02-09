using System.Buffers;
using Domain.Transaction;

namespace Domain.Interfaces;

public interface IResultWriter
{
    String WriteBlock(Block block);

    string WriteTransaction(TransactionEntry transactionEntry);

    string WriteMempool(bool ascending = false);
}