namespace HappyParsing;

internal class SimpleParser
{
    internal static DecimalDataObject ParseDecimal(InputDataObject value)
    {
        var parsedAmount = ParseValue(value.Amount, "invalid Amount");
        var parsedSize = ParseValue(value.Size, "invalid Size");

        var fullMessage = parsedAmount.MatchMessage() + 
                          (parsedAmount.MatchMessage() == "" || parsedSize.MatchMessage() == "" ? "" : "|") +
                          parsedSize.MatchMessage();
        
        return new DecimalDataObject(
            parsedAmount.MatchAmount(),
            parsedSize.MatchAmount(),
            parsedAmount.MatchAmount() * parsedSize.MatchAmount(),
            fullMessage
        );
    }

    private static Result<decimal?> ParseValue(string valueAmount, string errorMessage)
    {
        if (string.IsNullOrEmpty(valueAmount))
            return new Result<decimal?>.Success(null);

        if (decimal.TryParse(valueAmount, out var parsedAmount))
            return new Result<decimal?>.Success(parsedAmount);
        
        return new Result<decimal?>.Failure(errorMessage);
    }
}

abstract record Result<TRight>
{
    internal abstract Result<T> Map<T>(Func<TRight, T> f);
    internal abstract Result<T> Bind<T>(Func<TRight, Result<T>> f);
    internal abstract T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure);

    internal record Success(TRight Right) : Result<TRight>
    {
        internal override Result<T> Map<T>(Func<TRight, T> f) => new Result<T>.Success(f(Right));
        internal override Result<T> Bind<T>(Func<TRight, Result<T>> f) => f(Right);
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenSuccess(Right);
    }

    internal record Failure(string Message) : Result<TRight>
    {
        internal override Result<T> Map<T>(Func<TRight, T> f) => new Result<T>.Failure(Message);
        internal override Result<T> Bind<T>(Func<TRight, Result<T>> f) => new Result<T>.Failure(Message);
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenFailure(Message);
    }
}

internal static class Extensions
{
    internal static T? MatchAmount<T>(this Result<T> result) =>
        result.Match(
            d => d,
            _ => default(T)
        );
    internal static string MatchMessage<T>(this Result<T> result) =>
        result.Match(
            _ => "",
            m => m
        );

    internal static Result<decimal> Or(this Result<decimal?> result, decimal fallback) =>
        result.Map(d => d ?? fallback);

    internal static Result<decimal?> DivideBy(this Result<decimal?> dividend, Result<decimal> divisor) =>
        dividend.Bind(d1 => divisor.Map(d2 => d1 / d2));
}