using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;

namespace ModelLib.ClassInstance
{
	public class Either<L, R> : IEither<L , R>
	{
		public L Left { get; set; }
		public R Right { get; set; }
		public bool IsRight { get; set; }

		public Either( R right )
		{
			Right = right;
			IsRight = true;
		}

		public Either( L left , bool isleft = false )
		{
			Left = left;
			IsRight = false;
		}
	}

	public static class Either_Ext
	{
		public static Either<L , R> ToEither<L, R>(
		this R val )
		{
			return new Either<L , R>( val );
		}

		public static Either<L , B> Bind<A, B, L>(
		this Either<L , A> src ,
		Func<A , B> func ,
		L left )
		{
			if ( src.IsRight ) return func( src.Right ).ToEither<L , B>();
			else
			{
				return new Either<L , B>( left );
			}
		}
	}



}
