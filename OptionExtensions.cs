using System;
using Optional;

namespace kata_fizzbuzz_csharp
{
    static class OptionExtensions
    {
        internal static Option<T> Concat<T>(this Option<T> left, Option<T> right, Func<T, T, T> concat)
        {
            if (!left.HasValue) return right;
            if (!right.HasValue) return left;

            return left.FlatMap(leftValue => right.Map(rightValue => concat(leftValue, rightValue)));
        }
    }
}