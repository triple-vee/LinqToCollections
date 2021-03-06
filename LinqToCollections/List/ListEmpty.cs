﻿using System;
using System.Collections.Generic;

namespace Strilanc.LinqToCollections {
    ///<summary>A singleton for empty readable lists.</summary>
    internal sealed class ListEmpty<T> : AbstractReadOnlyList<T>, IBoundedCount {
        public static readonly ListEmpty<T> Empty = new ListEmpty<T>();
        private ListEmpty(){}
        public override T this[int index] { get { throw new ArgumentOutOfRangeException("index"); } }
        public override int Count { get { return 0; } }
        public override IEnumerator<T> GetEnumerator() { yield break; }
        public int? MaxCount { get { return 0; } }
        public int MinCount { get { return 0; } }
    }
}
