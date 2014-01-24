using System.Collections.Generic;
using System.Linq;

namespace Thecentury.Linq
{
	public interface IOrderedReadOnlyCollection<T> : IOrderedEnumerable<T>, IReadOnlyCollection<T>
	{
	}
}