namespace CrackHash.Worker.Application.Exceptions;

public class IncorrectHashException : BusinessException
{
    public IncorrectHashException(string message) : base(message)
    {
    }
}