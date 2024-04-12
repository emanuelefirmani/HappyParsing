namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimal(InputDataObject value)
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

    private static (decimal?, string) ParseOptionalDecimal(string value, string fieldName) =>
        !string.IsNullOrEmpty(value)
            ? decimal.TryParse(value, out var parsedAmount)
                ? (parsedAmount, "")
                : (null, $"invalid {fieldName}")
            : (null, "");
}