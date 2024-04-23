namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimal(InputDataObject value)
    {
        var parsedAmount = ParseValue(value.Amount, "invalid Amount");
        var parsedSize = ParseValue(value.Size, "invalid Size");

        var fullMessage = parsedAmount.Message + (parsedAmount.Message == "" || parsedSize.Message == "" ? "" : "|") + parsedSize.Message;
        
        return new DecimalDataObject(
            parsedAmount.Amount,
            parsedSize.Amount,
            parsedAmount.Amount * parsedSize.Amount,
            fullMessage
        );
    }

    private static Result ParseValue(string valueAmount, string errorMessage)
    {
        if (string.IsNullOrEmpty(valueAmount))
            return new Result.Success(null);

        if (decimal.TryParse(valueAmount, out var parsedAmount))
            return new Result.Success(parsedAmount);
        
        return new Result.Failure(errorMessage);
    }
}

abstract record Result
{
    internal abstract decimal? Amount { get; }
    internal abstract string Message { get; }
    
    internal record Success(decimal? Amount) : Result
    {
        internal override decimal? Amount { get; } = Amount;
        internal override string Message => "";
    }

    internal record Failure(string Message) : Result
    {
        internal override decimal? Amount => null;
        internal override string Message { get; } = Message;
    }
}