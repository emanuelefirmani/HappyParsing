﻿namespace HappyParsing;

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
    internal abstract T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure);

    internal record Success(TRight Amount) : Result<TRight>
    {
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenSuccess(Amount);
    }

    internal record Failure(string Message) : Result<TRight>
    {
        internal override T Match<T>(Func<TRight, T> whenSuccess, Func<string, T> whenFailure) =>
            whenFailure(Message);
    }
}

internal static class Extensions
{
    internal static decimal? MatchAmount(this Result<decimal?> result) =>
        result.Match(
            d => d,
            _ => null
        );
    internal static string MatchMessage(this Result<decimal?> result) =>
        result.Match(
            _ => "",
            m => m
        );
}