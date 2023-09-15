using System;
using System.Collections.Generic;

namespace CollectionHelper
{
	public static class Generic
	{
		public static int IndexOf<T, TValue>
		(this IEnumerable<T> collection,
		Func<T, TValue> selector,
		Func<int, bool> predicate)
		where TValue : IComparable<TValue>
		{
			if (collection == null)
				return -1;
			
			TValue min = default;
			int minIndex = -1;
			bool start = false;
			int index = 0;
			
			foreach (T item in collection)
			{
				TValue value = selector(item);
				
				if (!start || predicate(value.CompareTo(min)))
				{
					start = true;
					min = value;
					minIndex = index;
				}
				
				index++;
			}
			
			return minIndex;
		}
		
		static bool IndexOfMinPredicate(int value) => value < 0;
		
		static bool IndexOfMaxPredicate(int value) => value > 0;

		public static int IndexOfMin<T, TValue>
		(this IEnumerable<T> collection,
		Func<T, TValue> selector)
		where TValue : IComparable<TValue> =>
			IndexOf(
				collection,
				selector,
				IndexOfMinPredicate
			);

		public static int IndexOfMax<T, TValue>
		(this IEnumerable<T> collection,
		Func<T, TValue> selector)
		where TValue : IComparable<TValue> =>
			IndexOf(
				collection,
				selector,
				IndexOfMaxPredicate
			);
		
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection) =>
			collection == null || collection.Count == 0;

		public static IEnumerable<TKey> Keys<TKey, TValue>
			(this ICollection<KeyValuePair<TKey, TValue>> collection)
		{
			foreach (KeyValuePair<TKey, TValue> item in collection)
				yield return item.Key;
		}

		public static bool TryGet<TKey, TValue>
			(this ICollection<KeyValuePair<TKey, TValue>> collection,
			TKey key,
			out TValue value)
		{
			if (collection == null)
			{
				value = default;
				return false;
			}	

			if (collection is Dictionary<TKey, TValue> dictionary)
				if (dictionary.TryGetValue(key, out value))
					return true;

			foreach (KeyValuePair<TKey, TValue> pair in collection)
				if (pair.Key.Equals(key))
				{
					value = pair.Value;
					return true;
				}

			value = default;
			return false;
		}

		public static T Get<T>
			(this ICollection<KeyValuePair<string, T>> collection,
			string key,
			T @default = default) =>
			TryGet(collection, key, out T value) ? value : @default;

		public static float Float
			(this ICollection<KeyValuePair<string, string>> collection,
			string key,
			float @default = 0f) =>
			TryFloat(collection, key, out float value)
			? value
			: @default;

		public static bool TryFloat
			(this ICollection<KeyValuePair<string, string>> collection,
			string key,
			out float value)
		{
			value = default;
			return TryGet(collection, key, out string text) &&
				float.TryParse(text, out value);
		}

		public static bool TryInt
			(this ICollection<KeyValuePair<string, string>> collection,
			string key,
			out int value)
		{
			value = default;
			return TryGet(collection, key, out string text) &&
				int.TryParse(text, out value);
		}

		/// <summary>
		/// Try getting 1 item that passes the predicate,
		/// then remove it from the collection.
		/// </summary>
		public static bool TryPopOne<T>
			(this ICollection<T> collection,
			Func<T, bool> predicate,
			out T value)
		{
			bool flag = false;
			value = default;

			foreach (T item in collection)
				if (predicate(item))
				{
					value = item;
					flag = true;
					break;
				}

			if (flag)
				collection.Remove(value);

			return flag;
		}
	}
}
