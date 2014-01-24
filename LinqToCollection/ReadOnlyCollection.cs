using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Thecentury.Linq
{
	public static class ReadOnlyCollection
	{
		public static IReadOnlyCollection<TResult> Select<T, TResult>( this IReadOnlyCollection<T> collection, Func<T, TResult> selector )
		{
			return collection.AsEnumerable().Select( selector ).WithCountOf( collection );
		}

		public static IOrderedReadOnlyCollection<T> OrderBy<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector )
		{
			return collection.AsEnumerable().OrderBy( keySelector ).WithCountOf( collection );
		}

		public static IOrderedReadOnlyCollection<T> OrderBy<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return collection.AsEnumerable().OrderBy( keySelector, comparer ).WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> OrderByDescending<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector )
		{
			return collection.AsEnumerable().OrderByDescending( keySelector ).WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> OrderByDescending<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return collection.AsEnumerable().OrderByDescending( keySelector, comparer ).WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenBy<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector )
		{
			return source.AsOrderedEnumerable().ThenBy( keySelector ).WithCount( source.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenBy<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return source.AsOrderedEnumerable().ThenBy( keySelector, comparer ).WithCount( source.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenByDescending<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector )
		{
			return source.AsOrderedEnumerable().ThenByDescending( keySelector ).WithCount( source.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenByDescending<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return source.AsOrderedEnumerable().ThenByDescending( keySelector, comparer ).WithCount( source.Count );
		}

		public static IOrderedEnumerable<T> AsOrderedEnumerable<T>( this IOrderedReadOnlyCollection<T> collection )
		{
			return collection;
		}

		public static List<T> ToList<T>( this IReadOnlyCollection<T> collection )
		{
			var list = new List<T>( collection.Count );

			if ( collection.Count == 0 )
			{
				return list;
			}

			list.AddRange( collection );
			return list;
		}

		public static T[] ToArray<T>( this IReadOnlyCollection<T> collection )
		{
			var array = new T[ collection.Count ];
			if ( collection.Count == 0 )
			{
				return array;
			}

			int index = 0;
			foreach ( var element in collection )
			{
				array[ index ] = element;
				index++;
			}

			return array;
		}

		public static int Count<T>( this IReadOnlyCollection<T> collection )
		{
			return collection.Count;
		}

		public static long LongCount<T>( this IReadOnlyCollection<T> collection )
		{
			return collection.Count;
		}

		public static bool Any<T>( this IReadOnlyCollection<T> collection )
		{
			return collection.Count > 0;
		}

		public static bool Any<T>( this IReadOnlyCollection<T> collection, Func<T, bool> predicate )
		{
			if ( collection.Count == 0 )
			{
				return false;
			}

			return collection.AsEnumerable().Any( predicate );
		}

		public static bool All<T>( this IReadOnlyCollection<T> collection, Func<T, bool> predicate )
		{
			if ( collection.Count == 0 )
			{
				return true;
			}

			return collection.AsEnumerable().All( predicate );
		}

		public static IReadOnlyCollection<TResult> Cast<T, TResult>( this IReadOnlyCollection<T> collection )
		{
			return collection.AsEnumerable().Cast<TResult>().WithCountOf( collection );
		}

		public static IReadOnlyCollection<T> Reverse<T>( this IReadOnlyCollection<T> collection )
		{
			// todo brinchuk reverse with awareness of count
			return collection.AsEnumerable().Reverse().WithCountOf( collection );
		}

		public static IReadOnlyCollection<T> Skip<T>( this IReadOnlyCollection<T> collection, int count )
		{
			if ( collection.Count <= count )
			{
				return new ReadOnlyCollection<T>( Enumerable.Empty<T>(), 0 );
			}

			return collection.AsEnumerable().Skip( count ).WithCount( collection.Count - count );
		}

		public static IReadOnlyCollection<T> Take<T>( this IReadOnlyCollection<T> collection, int count )
		{
			return collection.AsEnumerable().Take( count ).WithCount( Math.Min( collection.Count, count ) );
		}

		// ElementAt
		// Empty
		// Repeat
		// ToLookup
		// SequenceEqual
		// First/OrDefault/Single...
		// DefaultIfEmpty
		// ElementAtOrDefault
	}

	internal static class EnumerableExtensions
	{
		public static IReadOnlyCollection<T> WithCount<T>( this IEnumerable<T> enumerable, int count )
		{
			return new ReadOnlyCollection<T>( enumerable, count );
		}

		public static IReadOnlyCollection<T1> WithCountOf<T1, T2>( this IEnumerable<T1> enumerable, IReadOnlyCollection<T2> collection )
		{
			return new ReadOnlyCollection<T1>( enumerable, collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> WithCount<T>( this IOrderedEnumerable<T> enumerable, int count )
		{
			return new OrderedReadOnlyCollection<T>( enumerable, count );
		}

		public static IOrderedReadOnlyCollection<T1> WithCountOf<T1, T2>( this IOrderedEnumerable<T1> enumerable, IReadOnlyCollection<T2> collection )
		{
			return new OrderedReadOnlyCollection<T1>( enumerable, collection.Count );
		}
	}

	public interface IOrderedReadOnlyCollection<T> : IOrderedEnumerable<T>, IReadOnlyCollection<T>
	{
	}

	internal sealed class OrderedReadOnlyCollection<T> : IOrderedReadOnlyCollection<T>
	{
		private readonly IOrderedEnumerable<T> _enumerable;
		private readonly int _count;

		public OrderedReadOnlyCollection( IOrderedEnumerable<T> enumerable, int count )
		{
			// todo brinchuk count >=0
			_count = count;
			_enumerable = enumerable;
		}

		public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>( Func<T, TKey> keySelector, IComparer<TKey> comparer, bool @descending )
		{
			return _enumerable.CreateOrderedEnumerable( keySelector, comparer, @descending );
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _enumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return _count; }
		}
	}

	internal sealed class ReadOnlyCollection<T> : IReadOnlyCollection<T>
	{
		private readonly IEnumerable<T> _enumerable;
		private readonly int _count;

		public ReadOnlyCollection( IEnumerable<T> enumerable, int count )
		{
			if ( enumerable == null )
			{
				throw new ArgumentNullException( "enumerable" );
			}
			// todo brinchuk count >=0
			_count = count;
			_enumerable = enumerable;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _enumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return _count; }
		}
	}
}
