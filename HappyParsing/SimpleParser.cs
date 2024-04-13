namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimalOld(InputDataObject value)
    {
        var (amount, returnedMessageAmount) = ParseOptionalDecimal(value.Amount, "Amount");
        var (size, returnedMessageSize) = ParseOptionalDecimal(value.Size, "Size");
        
        var message = string.Join("|", new [] { returnedMessageAmount, returnedMessageSize }.Where(x => !string.IsNullOrEmpty(x)));
        
        return new DecimalDataObject(
            amount,
            size,
            amount * size,
            message
        );
    }

    internal static DecimalDataObject ParseDecimal(InputDataObject value)
    {
        var amount = ParseOptionalDecimalRecord(value.Amount, "Amount");
        var size = ParseOptionalDecimalRecord(value.Size, "Size");
        
        var message = new [] { amount, size }.GetMessage();
        
        return new DecimalDataObject(
            amount.MatchValue(),
            size.MatchValue(),
            amount.MatchValue() * size.MatchValue(),
            message
        );
    }

    private static (decimal?, string) ParseOptionalDecimal(string value, string fieldName) =>
        !string.IsNullOrEmpty(value)
            ? decimal.TryParse(value, out var parsedAmount)
                ? (parsedAmount, "")
                : (null, $"invalid {fieldName}")
            : (null, "");

    private static ParsingResult<decimal?> ParseOptionalDecimalRecord(string value, string fieldName) =>
        !string.IsNullOrEmpty(value)
            ? decimal.TryParse(value, out var parsedAmount)
                ? new ParsingResult<decimal?>.Success(parsedAmount)
                : new ParsingResult<decimal?>.Failure($"invalid {fieldName}")
            : new ParsingResult<decimal?>.Success(null);
    
    
}