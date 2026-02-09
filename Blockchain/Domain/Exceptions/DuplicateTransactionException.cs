namespace DomainService;

public class DuplicateTransactionException : Exception
{
    public DuplicateTransactionException(string message) : base(message)
    {
        
    }

    public DuplicateTransactionException() : base("Transaction ID is Exist !")
    {
            
    }
}