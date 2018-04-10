using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.AmplifiedType
{
	public static class PartialApplication
	{
		//For Function
		public static Func<T1 , R> Compose<T1, T2, R>( this Func<T2 , R> g , Func<T1 , T2> f )
			=> x => g( f( x ) );

		public static Func<T2 , R> Apply<T1, T2, R>( this Func<T1 , T2 , R> func , T1 t1 )
			=> t2 => func( t1 , t2 );

		public static Func<T2 , T3 , R> Apply<T1, T2, T3, R>( this Func<T1 , T2 , T3 , R> func , T1 t1 )
			=> ( t2 , t3 ) => func( t1 , t2 , t3 );

		public static Func<T2 , T3 , T4 , R> Apply<T1, T2, T3 , T4 , R>( this Func<T1 , T2 , T3 , T4 , R> func , T1 t1 )
			=> ( t2 , t3  , t4) => func( t1 , t2 , t3 , t4 );

		public static Func<T2 , T3 , T4 , T5 , R> Apply<T1, T2, T3, T4, T5, R>( this Func<T1 , T2 , T3 , T4 , T5 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 ) => func( t1 , t2 , t3 , t4 , t5 );

		public static Func<T2 , T3 , T4 , T5 , T6 , R> Apply<T1, T2, T3, T4, T5, T6, R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 ) => func( t1 , t2 , t3 , t4 , t5 , t6 );

		public static Func<T2 , T3 , T4 , T5 , T6 , T7 , R> Apply<T1, T2, T3, T4, T5, T6, T7, R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , T7 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 );


		public static Func<T2 , T3 , T4 , T5 , T6 , T7 , T8 , R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 );

		public static Func<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 );

		public static Func<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 );

		public static Func<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , T11, R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10 , T11 , R>( this Func<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , T11 , R> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 , t11 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 , t11 );



		public static Func<I1 , I2 , R> Map<I1, I2, T, R>( this Func<I1 , I2 , T> @this , Func<T , R> func )
		   => ( i1 , i2 ) => func( @this( i1 , i2 ) );


		//For Action

		public static Action<T2 > Apply<T1, T2>( this Action<T1 , T2 > func , T1 t1 )
			=> t2 => func( t1 , t2 );

		public static Action<T2 , T3 > Apply<T1, T2, T3>( this Action<T1 , T2 , T3 > func , T1 t1 )
			=> ( t2 , t3 ) => func( t1 , t2 , t3 );

		public static Action<T2 , T3 , T4 > Apply<T1, T2, T3, T4>( this Action<T1 , T2 , T3 , T4 > func , T1 t1 )
			=> ( t2 , t3 , t4 ) => func( t1 , t2 , t3 , t4 );

		public static Action<T2 , T3 , T4 , T5 > Apply<T1, T2, T3, T4, T5>( this Action<T1 , T2 , T3 , T4 , T5 > func , T1 t1 )
			=> ( t2 , t3 , t4 , t5 ) => func( t1 , t2 , t3 , t4 , t5 );

		public static Action<T2 , T3 , T4 , T5 , T6> Apply<T1, T2, T3, T4, T5 , T6>( this Action<T1 , T2 , T3 , T4 , T5 , T6> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 ) => func( t1 , t2 , t3 , t4 , t5 , t6);

		public static Action<T2 , T3 , T4 , T5 , T6 , T7> Apply<T1, T2, T3, T4, T5, T6, T7>( this Action<T1 , T2 , T3 , T4 , T5 , T6 , T7> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7);


		public static Action<T2 , T3 , T4 , T5 , T6 , T7 , T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>( this Action<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 );

		public static Action<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>( this Action<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9);

		public static Action<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>( this Action<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10> func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 ,t10 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9  ,t10);

		public static Action<T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , T11 > Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11 >( this Action<T1 , T2 , T3 , T4 , T5 , T6 , T7 , T8 , T9 , T10 , T11 > func , T1 t1 )
		=> ( t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 , t11 ) => func( t1 , t2 , t3 , t4 , t5 , t6 , t7 , t8 , t9 , t10 , t11 );



		public static Action<I1 , I2 > Map<I1, I2, T>( this Func<I1 , I2 , T> @this , Action<T > func )
		   => ( i1 , i2 ) => func( @this( i1 , i2 ) );




	}
}
