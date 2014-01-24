using System.Collections.Generic;

namespace Thecentury.Linq
{
	public static class EnumerableEx
	{
		public static IEnumerable<T> Yield<T>( T value )
		{
			yield return value;
		}
	}
}