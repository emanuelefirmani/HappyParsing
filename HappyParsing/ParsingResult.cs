namespace HappyParsing;

internal abstract record ParsingResult<T>
{
    public abstract T Value { get; }
    public abstract string Message { get; }
    public abstract TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed);
    
    public record Success(T SuccessfulValue) : ParsingResult<T>
    {
        public override T Value => SuccessfulValue;
        public override string Message => "";
        public override TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed) =>
            whenSuccessful(Value);
    }

    public record Failure(string FailureMessage) : ParsingResult<T>
    {
        public override T Value => throw new Exception("Cannot access Value because in Failure");
        public override string Message => FailureMessage;
        public override TOut Match<TOut>(Func<T, TOut> whenSuccessful, Func<string, TOut> whenFailed) =>
            whenFailed(Message);
    }
}

internal static class ParsingResultExtensions
{
    internal static string GetMessage<T>(this IEnumerable<ParsingResult<T>> results) =>
        string.Join("|",
            results.Where(x => x.GetType() == typeof(ParsingResult<T>.Failure)).Select(x => x.Message));

    internal static T? MatchValue<T>(this ParsingResult<T> result) =>
        result.Match(
            v => v,
            _ => default(T?)
        );
}