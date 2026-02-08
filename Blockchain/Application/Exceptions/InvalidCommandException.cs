namespace Application.Exceptions;

public class InvalidCommandException: Exception
{
    public InvalidCommandException(string message) :base(message)
    {
        
    }

    public InvalidCommandException() : base("Invalid Command Type !")
    {
        
    }
}