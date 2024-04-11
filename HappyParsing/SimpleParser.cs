namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimal(InputDataObject value)
    {
        var message = "";
        decimal? amount = null;
        if(!string.IsNullOrEmpty(value.Amount))
        {
            if (decimal.TryParse(value.Amount, out var parsedAmount))
                amount = parsedAmount;
            else
                message = "invalid Amount";
        }

        decimal? size = null;
        if(!string.IsNullOrEmpty(value.Size))
        {
            if (decimal.TryParse(value.Size, out var parsedSize))
                size = parsedSize;
            else
                message += (message == "" ? "" : "|") + "invalid Size";
        }
        
        return new DecimalDataObject(
            amount,
            size,
            amount * size,
            message
        );
    }
}