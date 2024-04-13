namespace HappyParsing;

public class ParsingDecimals
{
    [Theory]
    [InlineData("2", 2)]
    [InlineData("12", 12)]
    [InlineData("123.456", 123.456)]
    public void is_valid(string input, decimal result)
    {
        var expected = new DecimalDataObject(result, 3.1m, 3.1m * result, "");
        var obj = new InputDataObject(input, "3.1");
        
        var actual = SimpleParser.ParseDecimal(obj);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2", 2)]
    [InlineData("12", 12)]
    [InlineData("123.456", 123.456)]
    public void parses_partially_amount(string input, decimal result)
    {
        var expected = new DecimalDataObject(result, null, null, "invalid Size");
        var obj = new InputDataObject(input, "abc");
        
        var actual = SimpleParser.ParseDecimal(obj);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2", 2)]
    [InlineData("12", 12)]
    [InlineData("123.456", 123.456)]
    public void parses_partially_size(string input, decimal result)
    {
        var expected = new DecimalDataObject(null, result, null, "invalid Amount");
        var obj = new InputDataObject("abc", input);
        
        var actual = SimpleParser.ParseDecimal(obj);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void does_not_parse()
    {
        var expected = new DecimalDataObject(null, null, null, "invalid Amount|invalid Size");
        var obj = new InputDataObject("abc", "efg");
        
        var actual = SimpleParser.ParseDecimal(obj);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void accepts_null()
    {
        var expected = new DecimalDataObject(null, null, null, "");
        var obj = new InputDataObject("", "");
        
        var actual = SimpleParser.ParseDecimal(obj);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, 1, 0, null)]
    [InlineData(null, null, 1, null)]
    [InlineData(4, null, 2, 2)]
    [InlineData(14, 7, 3, 2)]
    void divides_parsed_results_with_default(int? left, int? right, decimal defaultValue, int? expected)
    {
        var dividend = new ParsingResult<decimal?>.Success(left);
        var divisor = new ParsingResult<decimal?>.Success(right);

        var actual = dividend.DivideBy(divisor.Or(defaultValue));
        
        Assert.Equal(expected, actual.MatchValue());
    }

    [Fact]
    void maps_success()
    {
        var original = new ParsingResult<int>.Success(42);

        var actual = original.Map(x => x / 2);

        Assert.Equal(21, actual.MatchValue());
    }

    [Fact]
    void maps_failure()
    {
        var original = new ParsingResult<int>.Failure("error");

        var actual = original.Map(x => x / 2);

        Assert.Equal("error", actual.MatchMessage());
    }
}