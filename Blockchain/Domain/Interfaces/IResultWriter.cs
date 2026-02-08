using System.Buffers;

namespace Domain.Interfaces;

public interface IResultWriter
{
    String WriteBlock(Block block);

    string WriteTransaction(Transaction.Transaction transaction);

    string WriteMempool(bool ascending = false);
}