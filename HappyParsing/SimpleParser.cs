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

    private static Either<string, decimal?> ParseValue(string valueAmount, string errorMessage)
    {
        if (string.IsNullOrEmpty(valueAmount))
            return new Either<string, decimal?>.Success(null);

        if (decimal.TryParse(valueAmount, out var parsedAmount))
            return new Either<string, decimal?>.Success(parsedAmount);
        
        return new Either<string, decimal?>.Failure(errorMessage);
    }
}

abstract record Either<TLeft, TRight>
{
    internal abstract Either<TLeft, T> Bind<T>(Func<TRight, Either<TLeft, T>> f);
    internal abstract Either<TLeft, T> Map<T>(Func<TRight, T> f);
    internal abstract T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure);

    internal record Success(TRight Amount) : Either<TLeft, TRight>
    {
        internal override Either<TLeft, T> Bind<T>(Func<TRight, Either<TLeft, T>> f) => f(Amount);
        internal override Either<TLeft, T> Map<T>(Func<TRight, T> f) => new Either<TLeft, T>.Success(f(Amount));
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenSuccess(Amount);
    }

    internal record Failure(string Message) : Either<TLeft, TRight>
    {
        internal override Either<TLeft, T> Bind<T>(Func<TRight, Either<TLeft, T>> f) => new Either<TLeft, T>.Failure(Message);
        internal override Either<TLeft, T> Map<T>(Func<TRight, T> f) => new Either<TLeft, T>.Failure(Message);
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenFailure(Message);
    }
}

internal static class Extensions
{
    internal static T? MatchAmount<T>(this Either<string, T?> either) =>
        either.Match(
            d => d,
            _ => default
        );
    internal static string MatchMessage<T>(this Either<string, T> either) =>
        either.Match(
            _ => "",
            m => m
        );

    internal static Either<string, decimal> Or(this Either<string, decimal?> either, decimal fallback) =>
        either.Map(v => v ?? fallback);
    
    internal static Either<string, decimal?> DivideBy(this Either<string, decimal?> dividend, Either<string, decimal> divisor) =>
        dividend.Bind(v1 => divisor.Map(v2 => v1 / v2));
}