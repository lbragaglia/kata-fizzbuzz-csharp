using Optional;

namespace kata_fizzbuzz_csharp
{
    internal class NumberTranslator : INumberTranslator
    {
        public Option<string> Print(int number) => number.ToString().Some();
    }
}