﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Strilanc.LinqToCollections {
    /// <summary>
    /// Provides optional bounds on the size a collection can be.
    /// Checked for by methods like Take and Skip in order to perform optimizations.
    /// </summary>
    internal interface IBoundedCount {
        int? MaxCount { get; }
        int MinCount { get; }
    }
    internal static class BoundedCount {
        public static int? TryGetMaxCount<T>(this IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentNullException("collection");
            var r = collection as IBoundedCount;
            if (r != null) return r.MaxCount;
            var a = collection as IList;
            if (a != null && a.IsFixedSize) return a.Count;
            return null;
        }
        public static int TryGetMinCount<T>(this IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentNullException("collection");
            var r = collection as IBoundedCount;
            if (r != null) return r.MinCount;
            var a = collection as IList;
            if (a != null && a.IsFixedSize) return a.Count;
            return 0;
        }
    }
}