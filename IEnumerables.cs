﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionHelper
{
	public static class IEnumerables
	{
		public static IEnumerable<T> New<T>
			(params T[] values) =>
			values.AsEnumerable();

		/// <summary>
		/// Returns the index of the given value in the enumerable.
		/// </summary>
		public static int IndexOf<T>
			(this IEnumerable<T> enumerable,
			T value,
			int @default = -1)
			where T : class
		{
			int index = 0;

			foreach (T item in enumerable)
				if (item == value)
					return index;
				else
					index++;

			return @default;
		}

		public static T Get<T>
			(this IEnumerable<T> collection,
			Func<T, bool> predicate,
			T @default = default) =>
			collection.TryFirst(predicate, out T value)
			? value
			: @default;
		
		/// <summary>
		/// If all of the items are present in the enumerable.
		/// </summary>
		public static bool ContainsAll<T>
			(this IEnumerable<T> enumerable,
			T[] items)
		{
			foreach (T item in items)
				if (!enumerable.Contains(item))
					return false;

			return true;
		}

		/// <summary>
		/// Checks if the 2 enumerables are intersecting.
		/// </summary>
		public static bool Intersects<T>
			(this IEnumerable<T> a,
			IEnumerable<T> b)
		{
			foreach (T item in a)
				if (b.Contains(item))
					return true;

			return false;
		}

		public static SortedDictionary<TKey, TValue> ToSortedDictionary<T, TKey, TValue>
			(this IEnumerable<T> collection,
			Func<T, TKey> keyPredicate,
			Func<T, TValue> valuePredicate,
			IComparer<TKey> keyComparer = null)
		{
			SortedDictionary<TKey, TValue> dictionary =
				keyComparer != null
				? new SortedDictionary<TKey, TValue>(keyComparer)
				: new SortedDictionary<TKey, TValue>();

			foreach (T item in collection)
				dictionary.Add(keyPredicate(item), valuePredicate(item));

			return dictionary;
		}

		public static bool TryFirst<T>
			(this IEnumerable<T> self,
			out T value)
		{
			foreach (T item in self)
			{
				value = item;
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryFirst<T>
			(this IEnumerable<T> self,
			Func<T, bool> predicate,
			out T value)
		{
			foreach (T item in self)
				if (predicate(item))
				{
					value = item;
					return true;
				}

			value = default;
			return false;
		}

		public static IEnumerable<T> Repeat<T>
			(this IEnumerable<T> enumerable,
			int count)
		{
			if (count <= 0)
				yield break;

			for (int i = 0; i < count; i++)
				foreach (T item in enumerable)
					yield return item;
		}

		/// <summary>
		/// Take an exact count of elements, ignoring any excess elements.
		/// If there is not enough elements, it will use the default values.
		/// </summary>
		public static T[] TakeExactly<T>
			(this IEnumerable<T> self,
			int count,
			params T[] defaultValues)
		{
			T defaultValue = default;
			T[] list = new T[count];
			int n = 0;

			if (defaultValues.Length > 0)
				defaultValue = defaultValues[^1];

			foreach (T item in self)
			{
				if (n < count)
					list[n] = item;
				else
					break;

				n++;
			}

			while (n < count)
			{
				if (n < defaultValues.Length)
					list[n] = defaultValues[n];
				else
					list[n] = defaultValue;

				n++;
			}

			return list;
		}

		public static T[] TakeAtLeast<T>
			(this IEnumerable<T> enumerable,
			int count,
			params T[] defaultValues) =>
			TakeExactly(
				enumerable,
				Math.Max(enumerable.Count(), count),
				defaultValues
			);

		/// <summary>
		/// Returns `true` if there is at least
		/// the given number of elements in the enumerable.
		/// </summary>
		public static bool AtLeast<T>
			(this IEnumerable<T> enumerable,
			int minimum)
		{
			foreach (T _ in enumerable)
				if (minimum > 0)
					minimum--;
				else
					break;

			return minimum <= 0;
		}

		public static int RemoveValues<T1, T2>
			(this IDictionary<T1, T2> self,
			T2 value)
		{
			KeyValuePair<T1, T2>[] pairs = self.ToArray();
			int i = 0;

			foreach (KeyValuePair<T1, T2> pair in pairs)
				if (pair.Value.Equals(value) && self.Remove(pair.Key))
					i++;

			return i;
		}
	}
}
