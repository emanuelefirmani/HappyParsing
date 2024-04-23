namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimal(InputDataObject value)
    {
        var (amount, message) = ParseValue(value.Amount, "invalid Amount");
        var (size, errorMessageSize) = ParseValue(value.Size, "invalid Size");

        message += (message == "" || errorMessageSize == "" ? "" : "|") + errorMessageSize;
        
        return new DecimalDataObject(
            amount,
            size,
            amount * size,
            message
        );
    }

    private static (decimal? amount, string message) ParseValue(string valueAmount, string errorMessage)
    {
        if (string.IsNullOrEmpty(valueAmount))
            return (null, "");
        
        if (decimal.TryParse(valueAmount, out var parsedAmount))
            return (parsedAmount, "");
        
        return (null, errorMessage);

    }
}