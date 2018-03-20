using System;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace kata_fizzbuzz_csharp
{
    public class FizzBuzzTests
    {
        private readonly FizzBuzzer _sut;

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
        [InlineData(7, "7")]
        [InlineData(8, "8")]
        [InlineData(9, "Fizz")]
        [InlineData(10, "Buzz")]
        [InlineData(11, "11")]
        [InlineData(12, "Fizz")]
        [InlineData(13, "13")]
        [InlineData(14, "14")]
        [InlineData(15, "FizzBuzz")]
        public void ShouldPrintInputNumber(int input, string expected)
        {
            _sut.Print(input).Should().Be(expected);

        }
    }

    internal class FizzBuzzFactory
    {
        public FizzBuzzFactory()
        {
        }

        internal FizzBuzzer Create()
        {
            return new FizzBuzzer(new DefaultTranslator(), new Fizzer(), new Buzzer());
        }
    }

    internal class Buzzer:INumberTranslator
    {
        public Buzzer()
        {
        }

        public string Print(int number)
        {
            return number % 5 == 0 ? "Buzz" : null;
        }
    }

    internal class DefaultTranslator:INumberTranslator
    {
        public DefaultTranslator()
        {
        }

        public string Print(int number)
        {
            return number.ToString();
        }
    }

    internal class Fizzer: INumberTranslator
    {
        public Fizzer()
        {
        }

        public string Print(int number)
        {
            return number % 3 == 0 ? "Fizz" : null;
        }
    }

    internal interface INumberTranslator
    {
        string Print(int number);
    }

    internal class FizzBuzzer
    {
        private readonly INumberTranslator[] _fizzer;
        private readonly INumberTranslator _default;

        public FizzBuzzer(INumberTranslator @default, params INumberTranslator[] fizzer)
        {
            this._fizzer = fizzer;
            this._default = @default;
        }

        internal string Print(int number)
        {
            var result = _fizzer.Select(f => f.Print(number) ?? "").Aggregate((a,b) => a + b);
            return string.IsNullOrEmpty(result) ? _default.Print(number) : result;
        }
    }
}
