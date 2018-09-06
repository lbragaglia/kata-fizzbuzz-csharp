using Xunit;
using FluentAssertions;
using System.Linq;
using Optional;

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
        [InlineData(7*3, "FizzBang")]
        [InlineData(7*3*2, "FizzBang")]
        [InlineData(7*5, "BuzzBang")]
        [InlineData(7*5*2, "BuzzBang")]
        [InlineData(7*5*3, "FizzBuzzBang")]
        [InlineData(7*5*3*2, "FizzBuzzBang")]
        public void ShouldPrintInputNumber(int input, string expected)
        {
            _sut.Print(input).ValueOr("").Should().Be(expected);
        }
    }

    internal class FizzBuzzFactory
    {
        internal INumberTranslator Create() => new OrNumberTranslator(
            new AndNumberTranslator(
                new Fizzer(),
                new AndNumberTranslator(
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

    internal class OrNumberTranslator : INumberTranslator
    {
        private readonly INumberTranslator _left;
        private readonly INumberTranslator _right;

        public OrNumberTranslator(INumberTranslator left, INumberTranslator right)
        {
            _left = left;
            _right = right;
        }

        public Option<string> Print(int number)
        {
            return _left.Print(number).Else(() => _right.Print(number));
        }
    }

    internal class AndNumberTranslator : INumberTranslator
    {
        private readonly INumberTranslator _left;
        private readonly INumberTranslator _right;

        public AndNumberTranslator(INumberTranslator left, INumberTranslator right)
        {
            _left = left;
            _right = right;
        }

        public Option<string> Print(int number) => _left.Print(number).Concat(_right.Print(number));
    }

    internal class FactorTranslator : INumberTranslator
    {
        private readonly int _factor;
        private readonly string _translation;

        public FactorTranslator(int factor, string translation)
        {
            _factor = factor;
            _translation = translation;
        }

        public Option<string> Print(int number) => _translation.SomeWhen(_ => number.MultipleOf(_factor));
    }

    internal class Buzzer : FactorTranslator
    {
        public Buzzer() : base(5, "Buzz") { }
    }

    internal class Fizzer : FactorTranslator
    {
        public Fizzer() : base(3, "Fizz") { }
    }

    internal class Banger : FactorTranslator
    {
        public Banger() : base(7, "Bang") { }
    }

    internal class NumberTranslator: INumberTranslator
    {
        public Option<string> Print(int number) => number.ToString().Some();
    }

    static class IntExtensions {
        internal static bool MultipleOf(this int number, int factor) => number % factor == 0;
    }

    static class OptionExtensions {
        internal static Option<string> Concat(this Option<string> left, Option<string> right) {
            if (!left.HasValue) return right;
            if (!right.HasValue) return left;

            return left.FlatMap(leftValue => right.Map(rightValue => leftValue + rightValue));
        }
    }
}
