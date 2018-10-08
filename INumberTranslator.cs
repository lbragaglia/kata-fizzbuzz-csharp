using Optional;

namespace kata_fizzbuzz_csharp
{
    internal interface INumberTranslator
    {
        Option<string> Print(int number);
    }
}