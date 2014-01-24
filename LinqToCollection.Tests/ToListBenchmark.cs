using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Thecentury.Linq;

namespace LinqToCollections.Tests
{
	[TestFixture]
	public sealed class ToListBenchmark
	{
		[TestCase( 100 * 1000 * 1000 )]
		public void ToList( int count )
		{
			IReadOnlyCollection<int> source = Enumerable.Range( 0, count ).ToList().Select( i => i * i );

			Stopwatch timer = Stopwatch.StartNew();

			List<int> list = source.ToList();

			timer.Stop();

			Console.WriteLine( "Collection: {0} ms", timer.ElapsedMilliseconds );

			timer.Start();

			var l2 = source.AsEnumerable().ToList();

			timer.Stop();

			Console.WriteLine( "Enumerable: {0} ms", timer.ElapsedMilliseconds );

			Assert.NotNull( list );
			Assert.NotNull( l2 );

			Assert.That( list.SequenceEqual( l2 ) );
		}
	}
}
