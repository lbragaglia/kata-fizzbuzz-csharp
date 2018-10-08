namespace kata_fizzbuzz_csharp
{
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
}