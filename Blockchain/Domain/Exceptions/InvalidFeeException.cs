namespace Domain.Exceptions;

public class InvalidFeeException : Exception
{
    public InvalidFeeException(string message) : base(message)
    {
    }

    public InvalidFeeException() : base("Invalid transaction fee.")
    {
    }
}