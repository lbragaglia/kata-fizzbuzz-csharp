namespace kata_fizzbuzz_csharp
{
    static class IntExtensions
    {
        internal static bool MultipleOf(this int number, int divisor) => number % divisor == 0;
    }
}