namespace HappyParsing;

internal abstract record ParsingResult<T>
{
    public abstract TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed);
    public abstract ParsingResult<TOut> Map<TOut>(Func<T, TOut> f);
    public abstract ParsingResult<TOut> Bind<TOut>(Func<T, ParsingResult<TOut>> f);
    private ParsingResult() { }
    
    public record Success(T Value) : ParsingResult<T>
    {
        public override TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed) =>
            whenSuccessful(Value);

        public override ParsingResult<TOut> Map<TOut>(Func<T, TOut> f) =>
            new ParsingResult<TOut>.Success(f(Value));

        public override ParsingResult<TOut> Bind<TOut>(Func<T, ParsingResult<TOut>> f) =>
            f(Value);
    }

    public record Failure(string Message) : ParsingResult<T>
    {
        public override TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed) =>
            whenFailed(Message);

        public override ParsingResult<TOut> Map<TOut>(Func<T, TOut> f) =>
            new ParsingResult<TOut>.Failure(Message);

        public override ParsingResult<TOut> Bind<TOut>(Func<T, ParsingResult<TOut>> f) =>
            new ParsingResult<TOut>.Failure(Message);
    }
}

internal static class ParsingResultExtensions
{
    internal static string GetMessage<T>(this IEnumerable<ParsingResult<T>> results) =>
        string.Join("|",
            results.Select(MatchMessage).Where(x => !string.IsNullOrEmpty(x)));

    internal static string MatchMessage<T>(this ParsingResult<T> x) =>
        x.Match(_ => "", m => m);

    internal static T? MatchValue<T>(this ParsingResult<T> result) =>
        result.Match(v => v, _ => default(T?));

    internal static ParsingResult<decimal> Or(this ParsingResult<decimal?> dividend, decimal fallback) =>
        dividend.Map(v => v ?? fallback);

    internal static ParsingResult<decimal?> DivideBy(this ParsingResult<decimal?> dividend,
        ParsingResult<decimal> divisor) =>
        dividend.Bind(
            p1 => divisor.Map(
                p2 => p1 / p2
            )
        );
}