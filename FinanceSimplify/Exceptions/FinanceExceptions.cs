namespace FinanceSimplify.Exceptions;


public class FinanceNotFoundException : Exception
{
    public FinanceNotFoundException(string message) : base(message)
    {
    }
}

public class FinanceInternalErrorException : Exception
{
    public FinanceInternalErrorException(string message) : base(message)
    {
    }

    public FinanceInternalErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
