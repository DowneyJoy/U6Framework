using System.Collections.Generic;
using ZLinq;

namespace Downey.ZLinq
{
    public static class ZlinqExtensions
    {
        public static IEnumerable<T> AsEnumerable<TEnumerator, T>(this ValueEnumerable<TEnumerator, T> valueEnumerable)
        where TEnumerator : struct, IValueEnumerator<T>
        {
            using (var e = valueEnumerable.Enumerator)
            {
                while (e.TryGetNext(out var next))
                {
                    yield return next;
                }
            }
        }
    }
}