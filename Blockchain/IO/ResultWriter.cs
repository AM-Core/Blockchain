using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DomainService;

namespace IO;

public class ResultWriter : IResultWriter
{
    public string WriteBlock(Block block)
    {
        throw new NotImplementedException();
    }

    public string WriteTransaction(Transaction transaction)
    {
        throw new NotImplementedException();
    }
    public string WriteMempool(bool ascending = false)
    {
        throw new NotImplementedException();
    }
}