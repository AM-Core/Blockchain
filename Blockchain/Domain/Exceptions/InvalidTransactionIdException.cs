namespace Domain.Exceptions;

public class InvalidTransactionIdException : Exception
{
    public InvalidTransactionIdException() : base("Invalid transaction ID.")
    {

    }

    public InvalidTransactionIdException(string message) : base(message)
    {
        
    }
}