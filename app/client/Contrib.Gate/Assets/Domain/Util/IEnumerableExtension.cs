using System.Collections.Generic;
using System.Linq;
using System;

namespace Util
{
    // IEnumerable に拡張メソッド定義
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            return collection.OrderBy(i => Guid.NewGuid());
        }
    }
}