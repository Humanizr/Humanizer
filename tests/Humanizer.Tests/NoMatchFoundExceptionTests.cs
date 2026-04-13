namespace Humanizer.Tests;

public class NoMatchFoundExceptionTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        var ex = new NoMatchFoundException();

        Assert.NotNull(ex.Message);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void MessageConstructor_SetsMessage()
    {
        var ex = new NoMatchFoundException("test message");

        Assert.Equal("test message", ex.Message);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void MessageAndInnerConstructor_SetsBoth()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new NoMatchFoundException("outer message", inner);

        Assert.Equal("outer message", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void IsException_DerivesFromException()
    {
        var ex = new NoMatchFoundException("derived check");
        Assert.IsAssignableFrom<Exception>(ex);
    }
}