using Optional;

namespace kata_fizzbuzz_csharp
{
    internal class ConcatenatingTranslator : INumberTranslator
    {
        private readonly INumberTranslator _left;
        private readonly INumberTranslator _right;

        public ConcatenatingTranslator(INumberTranslator left, INumberTranslator right)
        {
            _left = left;
            _right = right;
        }

        public Option<string> Print(int number) => _left.Print(number).Concat(_right.Print(number), Concat);

        private static string Concat(string left, string right) => left + right;
    }
}