using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;




namespace ModelLib.AmplifiedType
{ 
		using Maybe;
	using static ModelLib.AmplifiedType.Handler;
		public static partial class Handler
		{
			public static Maybe<A> Just<A>( A value ) => new Maybe.Just<A>( value );
			public static Maybe.Nothing None => Maybe.Nothing.Default;
		}
	

	public struct Maybe<A> : IEquatable<Maybe.Nothing>, IEquatable<Maybe<A>>// Define TypeClass 
	{
		public readonly A Value;
		public readonly bool isJust;
		bool isNothing => isJust;

		Maybe( A value )
		{
			if ( value == null ) throw new ArgumentNullException(
				"Can't initilize with null " );
			isJust = true;
			Value = value;
		} 

		public static implicit operator Maybe<A>(Maybe.Nothing _ ) => new Maybe<A>();
		public static implicit operator Maybe<A>(Maybe.Just<A> just) => new Maybe<A>(just.Value);
		public static implicit operator Maybe<A>(A value) => value == null ? None : Just(value);

		public B Match<B>( Func<B> Nothing , Func<A , B> Just )
		  =>         isJust ? Just( Value ) : Nothing();

		public IEnumerable<A> AsEnumerable()
		{
			if ( isJust ) yield return Value;
		}

		public bool Equals( Maybe.Nothing other )
			=> this.isJust ? false : true;

		public bool Equals( Maybe<A> other )
			=> this.isJust == other.isJust
			&& ( this.Value.Equals( other.Value ) );
	}   

	namespace Maybe // TypeClass Instance Impelemnt 
	{
		public struct Nothing
		{
			internal static readonly Nothing Default = new Nothing();
		}

		public struct Just<T>
		{
			internal T Value { get; }
			internal Just( T value )
			{
				if ( value == null )
					throw new ArgumentNullException( nameof( value )
					   , "Just can't be created with null, use 'Nothing' instead" );
				Value = value;
			}
		}
	}

	public static class MaybeExt
	{
		public static Maybe<B> Bind<A, B>
			( this Maybe<A> self , Func<A , Maybe<B>> f )
			=> self.Match(
				Nothing: () => None ,
				Just: x => f( x ) );

		public static IEnumerable<B> Bind<A, B>
			( this Maybe<A> self , Func<A , IEnumerable<B>> f )
			=> self.AsEnumerable().Bind( f );

		public static Maybe<B> Lift<A, B>
			( this Maybe<A> self , Func<A , B> f )
			=> self.Match(
				() => None ,
				x => Just( f( x ) ) );

		public static Maybe<A> Flatten<A>
			( this Maybe<A> self  )
			=> self;


		public static Maybe<Unit> ForEach<A>
			( this Maybe<A> self , Action<A> act )
			=> Lift( self , act.ToFunc() );

		}
}


