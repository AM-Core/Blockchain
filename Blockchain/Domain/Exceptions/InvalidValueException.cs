namespace Domain.Exceptions;

public class InvalidValueException : Exception
{
    public InvalidValueException(string message) : base(message)
    {
    }

    public InvalidValueException() : base("Invalid Value Exception !")
    {
    }
}