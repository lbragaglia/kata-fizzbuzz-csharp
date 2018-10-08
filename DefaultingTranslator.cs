using Optional;

namespace kata_fizzbuzz_csharp
{
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
}