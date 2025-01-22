namespace FinanceSimplify.Exceptions;


public class FinanceNotFoundException(string message) : Exception(message)
{
}

public class FinanceInternalErrorException(string message) : Exception(message)
{
}

public class FinanceUnauthorizedException(string message) : Exception(message)
{
}
