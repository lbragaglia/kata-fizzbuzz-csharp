using System.Linq;
using FluentAssertions;
using Optional;
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

    internal class FizzBuzzFactory
    {
        internal INumberTranslator Create() => new DefaultingTranslator(
            new ConcatenatingTranslator(
                new Fizzer(),
                new ConcatenatingTranslator(
                    new Buzzer(),
                    new Banger()
                )
            ),
            new NumberTranslator()
        );
    }

    internal interface INumberTranslator
    {
        Option<string> Print(int number);
    }

    internal class DefaultingTranslator : INumberTranslator
    {
        private readonly INumberTranslator _primary;
        private readonly INumberTranslator _default;

        public DefaultingTranslator(INumberTranslator primary, INumberTranslator @default)
        {
            _primary = primary;
            _default = @default;
        }

        public Option<string> Print(int number) => _primary.Print(number).Else(() => _default.Print(number));
    }

    internal class ConcatenatingTranslator : INumberTranslator
    {
        private readonly INumberTranslator _left;
        private readonly INumberTranslator _right;

        public ConcatenatingTranslator(INumberTranslator left, INumberTranslator right)
        {
            _left = left;
            _right = right;
        }

        public Option<string> Print(int number) => _left.Print(number).Concat(_right.Print(number));
    }

    internal class DivisorTranslator : INumberTranslator
    {
        private readonly int _divisor;
        private readonly string _translation;

        public DivisorTranslator(int divisor, string translation)
        {
            _divisor = divisor;
            _translation = translation;
        }

        public Option<string> Print(int number) => _translation.SomeWhen(_ => number.MultipleOf(_divisor));
    }

    internal class Buzzer : DivisorTranslator
    {
        public Buzzer() : base(5, "Buzz") { }
    }

    internal class Fizzer : DivisorTranslator
    {
        public Fizzer() : base(3, "Fizz") { }
    }

    internal class Banger : DivisorTranslator
    {
        public Banger() : base(7, "Bang") { }
    }

    internal class NumberTranslator : INumberTranslator
    {
        public Option<string> Print(int number) => number.ToString().Some();
    }

    static class IntExtensions
    {
        internal static bool MultipleOf(this int number, int divisor) => number % divisor == 0;
    }

    static class OptionExtensions
    {
        internal static Option<string> Concat(this Option<string> left, Option<string> right)
        {
            if (!left.HasValue) return right;
            if (!right.HasValue) return left;

            return left.FlatMap(leftValue => right.Map(rightValue => leftValue + rightValue));
        }
    }
}