using Domain.Transaction;

namespace Domain.Interfaces;

public interface IResultWriter
{
    string WriteBlock(Block block);

    string WriteTransaction(TransactionEntry transactionEntry);

    string WriteMempool(bool ascending = false);
}