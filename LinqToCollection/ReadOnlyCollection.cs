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

		public static IReadOnlyCollection<object> AsReadOnlyCollection( this ICollection collection )
		{
			return collection.Cast<object>().WithCount( collection.Count );
		}

		public static IReadOnlyCollection<T> AsReadOnlyCollection<T>( this ICollection<T> collection )
		{
			return collection.WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> OrderBy<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector )
		{
			return collection.OrderBy( keySelector, null );
		}

		public static IOrderedReadOnlyCollection<T> OrderBy<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return collection.AsEnumerable().OrderBy( keySelector, comparer ).WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> OrderByDescending<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector )
		{
			return collection.OrderByDescending( keySelector, null );
		}

		public static IOrderedReadOnlyCollection<T> OrderByDescending<T, TKey>( this IReadOnlyCollection<T> collection,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return collection.AsEnumerable().OrderByDescending( keySelector, comparer ).WithCount( collection.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenBy<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector )
		{
			return source.ThenBy( keySelector, null );
		}

		public static IOrderedReadOnlyCollection<T> ThenBy<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector, IComparer<TKey> comparer )
		{
			return source.AsOrderedEnumerable().ThenBy( keySelector, comparer ).WithCount( source.Count );
		}

		public static IOrderedReadOnlyCollection<T> ThenByDescending<T, TKey>( this IOrderedReadOnlyCollection<T> source,
			Func<T, TKey> keySelector )
		{
			return source.ThenByDescending( keySelector, null );
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
			var array = new T[collection.Count];
			if ( collection.Count == 0 )
			{
				return array;
			}

			int index = 0;
			foreach ( var element in collection )
			{
				array[index] = element;
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
			var array = collection.ToArray();

			return Reversed( array ).WithCount( array.Length );
		}

		private static IEnumerable<T> Reversed<T>( T[] source )
		{
			for ( var i = source.Length - 1; i >= 0; i-- )
			{
				yield return source[i];
			}
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

		public static IReadOnlyCollection<T> Concat<T>( this IReadOnlyCollection<T> first, IReadOnlyCollection<T> second )
		{
			return first.AsEnumerable().Concat( second ).WithCount( first.Count + second.Count );
		}

		public static bool Contains<T>( this IReadOnlyCollection<T> collection, T value )
		{
			return collection.Contains( value, null );
		}

		public static bool Contains<T>( this IReadOnlyCollection<T> collection, T value, IEqualityComparer<T> comparer )
		{
			if ( collection.Count == 0 )
			{
				return false;
			}

			return collection.AsEnumerable().Contains( value, comparer );
		}

		public static bool SequenceEqual<T>( this IReadOnlyCollection<T> first, IReadOnlyCollection<T> second )
		{
			return SequenceEqual( first, second, null );
		}

		public static bool SequenceEqual<T>( this IReadOnlyCollection<T> first, IReadOnlyCollection<T> second,
			IEqualityComparer<T> comparer )
		{
			if ( first.Count != second.Count )
			{
				return false;
			}

			return first.AsEnumerable().SequenceEqual( second, comparer );
		}

		public static IReadOnlyCollection<TResult> Zip<TFirst, TSecond, TResult>( this IReadOnlyCollection<TFirst> first,
			IReadOnlyCollection<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector )
		{
			return first.AsEnumerable().Zip( second, resultSelector ).WithCount( Math.Min( first.Count, second.Count ) );
		}

		public static IReadOnlyCollection<T> Empty<T>()
		{
			return Enumerable.Empty<T>().WithCount( 0 );
		}

		public static IReadOnlyCollection<int> Range( int start, int count )
		{
			return Enumerable.Range( start, count ).WithCount( count );
		}

		public static IReadOnlyCollection<T> Repeat<T>( T element, int count )
		{
			return Enumerable.Repeat( element, count ).WithCount( count );
		}

		public static IReadOnlyCollection<T> Yield<T>( T element )
		{
			return Enumerable.Repeat( element, 1 ).WithCount( 1 );
		}

		// ElementAt
		// First/OrDefault/Single...
		// DefaultIfEmpty
		// ElementAtOrDefault
		// Last
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
			if ( count < 0 )
			{
				throw new ArgumentOutOfRangeException( "count" );
			}
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
