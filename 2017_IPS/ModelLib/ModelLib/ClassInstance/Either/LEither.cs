using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;

namespace ModelLib.ClassInstance
{
	public class LEither<R> : IEither<string , R>
	{
		public string Left { get; set; }
		public R Right { get; set; }
		public bool IsRight { get; set; }

		public LEither( R right )
		{
			Right = right;
			IsRight = true;
		}

		public LEither( R right , string left)
		{
			Right = right;
			Left = left;
			IsRight = true;
		}


		public LEither( string left , bool isleft = false )
		{
			Left = left;
			IsRight = false;
		}

		public LEither()
		{
			IsRight = false;
		}
	}

	public static class LEither_Property
	{
		public static LEither<R> ToLEither<R>(
		this R val )
		{
			return new LEither<R>( val );
		}

		public static LEither<R> ToLEither<R>(
		this R val ,
		string left)
		{
			return new LEither<R>( val , left );
		}


		// 1 Bind -> Left (실패시 Left State만 내보내고 로그는 비어있다. )
		// 2 Bind -> Left
		// 3 Bind(Log) -> Left with Log (Left가 들어왔으므로 이 로그 바인드는 로그를 포함해 출력하게 된다. )

		// this method is for time logging Either Type
		public static LEither<B> Bind<A, B>(
		this LEither<A> src ,
		Func<A , B> func ,
		string log )
		{
			if ( src.IsRight )
			{
				try { return func( src.Right ).ToLEither<B>(); }
				catch { }
			}
			var time = DateTime.Now.ToString("yyMMdd_HH mm ss");
			string fulllog = "Error( " + time + " ) : " + log;
			return new LEither<B>( fulllog );
		}



		// If We Dont need to log . use this 
		public static LEither<B> Bind<A, B>(
		this LEither<A> src ,
		Func<A , B> func )
		{
			if ( src.IsRight )
			{
				try { return func( src.Right ).ToLEither<B>(); }
				catch { }
			}
			return new LEither<B>();
		}

		
	}

}
