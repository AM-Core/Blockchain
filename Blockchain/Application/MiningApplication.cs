using Domain.Interfaces;

namespace Application;

public class MiningApplication
{
    private readonly IResultWriter _resultWriter;
    private readonly ITransactionReader _transactionReader;
    public MiningApplication(IResultWriter resultWriter, 
        ITransactionReader transactionReader)
    {
        _resultWriter = resultWriter;
        _transactionReader = transactionReader;
    }
    
    public void GetCommand(string command)
    {
    }
}