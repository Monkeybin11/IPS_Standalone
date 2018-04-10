using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;
using ModelLib.Data;
using System.Diagnostics;

namespace ModelLib.ClassInstance
{
	public class TEither : IEither<string , IStgCtrl>
	{
		public int TimeOutSec;
		public string Left { get; set; }
		public IStgCtrl Right { get; set; }
		public bool IsRight { get; set; }

		public TEither()
		{
			IsRight = false;
		}

		public TEither( string log )
		{
			IsRight = false;
			Left = log;
		}

		public TEither( IStgCtrl sr , int tim = 20000 )
		{
			Right = sr;
			TimeOutSec = tim;
			if ( Right != null ) IsRight = true;
		}

		public TEither( IStgCtrl sr , bool pass , int tim = 20000)
		{
			Right = sr;
			IsRight = pass;
			TimeOutSec = tim;
			if ( Right != null ) IsRight = true;
		}
	}

	public static class SEither_Ext
	{
		public static TEither ToTEither(
			this IStgCtrl src  )
		=> new TEither( src );

		public static TEither ToTEither(
			this IStgCtrl src ,
			int time )
		=> new TEither( src , time );
		
		public static TEither Bind(
			this TEither src ,
			Func<IStgCtrl , TEither> func )
		{
			if ( src.IsRight )
			{
				int passtime = 0;
				Stopwatch stw = new Stopwatch();
				func( src.Right );
				while ( passtime < src.TimeOutSec )
				{
					stw.Start();
					if ( src.Right.Query( src.Right.Status )
						 == src.Right.StatusOK ) return src; // Right
					else stw.Stop();
					passtime = ( int )(stw.ElapsedMilliseconds/1000);
				}
			}
			return new TEither(); // Left No Log
		}

		public static TEither Bind(
			this TEither src ,
			Func<IStgCtrl , TEither> func ,
			string log)
		{
			//var timenow = DateTime.Now.ToString("HH-mm-ss");
			string nextlog = "[Function Error] : " + log;

			if ( src.IsRight )
			{
				int passtime = 0;
				Stopwatch stw = new Stopwatch();
				func( src.Right );
				while ( passtime < src.TimeOutSec )
				{
					stw.Start();
					if ( src.Right.Query( src.Right.Status ) 
						 == src.Right.StatusOK ) return src; // Function Success -> Right
					else stw.Stop();
					passtime = ( int )( stw.ElapsedMilliseconds / 1000 );
				}
				return new TEither( nextlog ); // Function Fail
			}
			return new TEither( new StringBuilder()
										.Append(src.Left)
										.Append(Environment.NewLine)
										.Append( nextlog )
										.ToString()); // Pass Left
		}

		public static LEither<A> ToLEither<A>(
			this TEither self ,
			A value)
		{
			return self.IsRight
					? new LEither<A>( value , self.Left )
					: new LEither<A>( self.Left , false ); 
		}

	}

}
