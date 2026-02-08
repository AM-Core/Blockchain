namespace Application.Exceptions;

public class NotFoundTransactionByIdException : Exception
{
    public NotFoundTransactionByIdException(string id, string entityType)
        : base($"{entityType} with ID '{id}' was not found.")
    {
    }

    public NotFoundTransactionByIdException(string message) : base(message)
    {
    }

    public NotFoundTransactionByIdException() : base("Transaction with the specified ID was not found.")
    {
    }
}
