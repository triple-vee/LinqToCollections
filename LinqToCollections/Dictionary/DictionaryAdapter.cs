﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Strilanc.LinqToCollections {
    ///<summary>Exposes a readonly dictionary as a mutable dictionary that doesn't support mutation.</summary>
    internal sealed class DictionaryAdapter<TKey, TValue> : AbstractReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue> {
        public readonly IReadOnlyDictionary<TKey, TValue> SubDictionary;
        
        private DictionaryAdapter(IReadOnlyDictionary<TKey, TValue> dict) {
            this.SubDictionary = dict;
        }
        public static IReadOnlyDictionary<TKey, TValue> From(IReadOnlyDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            return new DictionaryAdapter<TKey, TValue>(dictionary);
        }
        public static IReadOnlyDictionary<TKey, TValue> Adapt(IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            if (dictionary.IsReadOnly) {
                // if it's an adapter, then we can unwrap it
                var c = dictionary as DictionaryAdapter<TKey, TValue>;
                if (c != null) return c.SubDictionary;

                // if it's already what we need, great!
                var r = dictionary as IReadOnlyDictionary<TKey, TValue>;
                if (r != null) return r;
            }

            // use existing readonly adapter
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
        public static IDictionary<TKey, TValue> Adapt(IReadOnlyDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            // if it's already a dictionary, we can just return it
            var r = dictionary as IDictionary<TKey, TValue>;
            if (r != null) return r;

            // otherwise we need to adapt it
            return new DictionaryAdapter<TKey, TValue>(dictionary);
        }

        public override int Count { get { return SubDictionary.Count; } }
        public override bool TryGetValue(TKey key, out TValue value) { return SubDictionary.TryGetValue(key, out value); }
        public override IEnumerable<TKey> Keys { get { return SubDictionary.Keys; } }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            TValue value;
            return SubDictionary.TryGetValue(item.Key, out value)
                && Equals(item.Value, value);
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            if (array == null) throw new ArgumentNullException("array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex < 0");
            if (arrayIndex + Count > array.Length) throw new ArgumentOutOfRangeException("array", "arrayIndex + Count > array.Length");
            foreach (var e in this) {
                array[arrayIndex] = e;
                arrayIndex += 1;
            }
        }
        ICollection<TKey> IDictionary<TKey, TValue>.Keys {
            get {
                return new AnonymousReadOnlyCollection<TKey>(
                    () => Count,
                    () => SubDictionary.Keys.GetEnumerator()
                ).AsICollection();
            }
        }
        ICollection<TValue> IDictionary<TKey, TValue>.Values {
            get {
                return new AnonymousReadOnlyCollection<TValue>(
                    () => Count,
                    () => SubDictionary.Values.GetEnumerator()
                ).AsICollection();
            }
        }
        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get { return SubDictionary[key]; }
            set { throw new NotSupportedException("Dictionary is read-only."); }
        }

        public bool IsReadOnly { get { return true; } }
        public bool Remove(TKey key) { throw new NotSupportedException("Dictionary is read-only."); }
        public void Add(KeyValuePair<TKey, TValue> item) { throw new NotSupportedException("Dictionary is read-only."); }
        public void Clear() { throw new NotSupportedException("Dictionary is read-only."); }
        public bool Remove(KeyValuePair<TKey, TValue> item) { throw new NotSupportedException("Dictionary is read-only."); }
        public void Add(TKey key, TValue value) { throw new NotSupportedException("Dictionary is read-only."); }
    }
}
