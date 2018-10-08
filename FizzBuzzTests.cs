using FluentAssertions;
using Xunit;

namespace kata_fizzbuzz_csharp
{
    public class FizzBuzzTests
    {
        private readonly INumberTranslator _sut;

        public FizzBuzzTests()
        {
            _sut = new FizzBuzzFactory().Create();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(3, "Fizz")]
        [InlineData(4, "4")]
        [InlineData(5, "Buzz")]
        [InlineData(6, "Fizz")]
        [InlineData(7, "Bang")]
        [InlineData(8, "8")]
        [InlineData(9, "Fizz")]
        [InlineData(10, "Buzz")]
        [InlineData(11, "11")]
        [InlineData(12, "Fizz")]
        [InlineData(13, "13")]
        [InlineData(14, "Bang")]
        [InlineData(15, "FizzBuzz")]
        [InlineData(30, "FizzBuzz")]
        [InlineData(7 * 3, "FizzBang")]
        [InlineData(7 * 3 * 2, "FizzBang")]
        [InlineData(7 * 5, "BuzzBang")]
        [InlineData(7 * 5 * 2, "BuzzBang")]
        [InlineData(7 * 5 * 3, "FizzBuzzBang")]
        [InlineData(7 * 5 * 3 * 2, "FizzBuzzBang")]
        public void ShouldPrintTranslatedInputNumber(int input, string expected)
        {
            _sut.Print(input).Match(
                some: actual => actual.Should().Be(expected),
                none: () => throw new Xunit.Sdk.XunitException("None result")
            );
        }
    }
}