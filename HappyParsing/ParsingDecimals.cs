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
}