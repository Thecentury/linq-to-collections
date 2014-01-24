using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Thecentury.Linq
{
	[DebuggerDisplay( "Count = {Count}" )]
	internal sealed class OrderedReadOnlyCollection<T> : IOrderedReadOnlyCollection<T>
	{
		private readonly IOrderedEnumerable<T> _enumerable;
		private readonly int _count;

		public OrderedReadOnlyCollection( IOrderedEnumerable<T> enumerable, int count )
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
}