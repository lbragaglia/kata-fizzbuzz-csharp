using Optional;

namespace kata_fizzbuzz_csharp
{
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
}