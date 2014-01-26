using System.Collections.Generic;
using System.Linq;

namespace Thecentury.Linq
{
	public static class EnumerableExtensions
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
}