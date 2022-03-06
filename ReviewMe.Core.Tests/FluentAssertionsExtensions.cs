namespace ReviewMe.Core.Tests;

public static class FluentAssertionsExtensions
{
    public static bool IsEquivalentTo<T>(this T actual, T expected)
    {
        using var scope = new AssertionScope();

        actual.Should().BeEquivalentTo(expected);

        return scope.Discard().Length == 0;
    }
}