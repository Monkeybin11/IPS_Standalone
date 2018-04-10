using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;
using System.Collections.Immutable;

namespace ModelLib.AmplifiedType
{
	using static ModelLib.AmplifiedType.Handler;
	public static class EnumerableExt
	{
		public static IEnumerable<T> Append<T>( this IEnumerable<T> source
		 , params T [ ] ts ) => source.Concat( ts );

		static IEnumerable<T> Prepend<T>( this IEnumerable<T> source , T val )
		{
			yield return val;
			foreach ( T t in source ) yield return t;
		}

		public static IEnumerable<R> Lift<T, R>
		 ( this IEnumerable<T> list , Func<T , R> func )
		  => list.Select( func );

		public static R Match<T, R>( this IEnumerable<T> list
		, Func<R> Empty , Func<T , IEnumerable<T> , R> Otherwise )
		=> list.Head().Match(
		   Nothing: Empty ,
		   Just: head => Otherwise( head , list.Skip( 1 ) ) );

		public static Maybe<T> Head<T>( this IEnumerable<T> list )
		{
			if ( list == null ) return None;
			var enumerator = list.GetEnumerator();
			return enumerator.MoveNext() ? Just( enumerator.Current ) : None;
		}

		// Linq Extension
		public static IEnumerable<R> Bind<T, R>( this IEnumerable<T> list , Func<T , IEnumerable<R>> func )
		 => list.SelectMany( func );

		public static IEnumerable<T> Flatten<T>( this IEnumerable<IEnumerable<T>> list )
			=> list.SelectMany( x => x );

		public static IEnumerable<Unit> ForEach<A>
			( this IEnumerable<A> self , Action<A> act )
			=> self.Lift( act.ToFunc() ).ToImmutableList();

	}
}
