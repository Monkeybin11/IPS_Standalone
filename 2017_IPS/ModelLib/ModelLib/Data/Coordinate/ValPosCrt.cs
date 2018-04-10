using System;
using System.Collections.Generic;


namespace ModelLib.AmplifiedType
{
	using ModelLib.Data;
	public static partial class Handler
	{
		public static ValPosCrt ValPosCrt( X x , Y y , double value = 0 )
			=> new ValPosCrt( x , y , value );
	}
}

namespace ModelLib.Data
{
	using static System.Math;
	public class ValPosCrt
	{
		public readonly X X;
		public readonly Y Y;
		public readonly double Value;

		public ValPosCrt( X x , Y y , double val )
		{
			X = x;
			Y = y;
			Value = val;
		}

		public static bool operator <( ValPosCrt a , ValPosCrt b )
			=> ( Pow( a.X , 2 ) + Pow( a.Y , 2 ) ) < ( Pow( b.X , 2 ) + Pow( b.Y , 2 ) );

		public static bool operator >( ValPosCrt a , ValPosCrt b )
			=> ( Pow( a.X , 2 ) + Pow( a.Y , 2 ) ) > ( Pow( b.X , 2 ) + Pow( b.Y , 2 ) );

		public static implicit operator Tuple<X , Y , double>( ValPosCrt valpos )
			=> Tuple.Create( valpos.X , valpos.Y , valpos.Value );

		public static implicit operator ValPosCrt( Tuple<X , Y , double> data )
			=> new ValPosCrt( data.Item1 , data.Item2 , data.Item3 );

		//public static implicit operator ValPos( ValPosCrt p)
		//	=> 


		public static ValPosCrt operator +( ValPosCrt a , double b )
			=> new ValPosCrt( a.X , a.Y , a.Value + b );

		public static ValPosCrt operator -( ValPosCrt a , double b )
			=> new ValPosCrt( a.X , a.Y , a.Value - b );

		public static ValPosCrt operator *( ValPosCrt a , double b )
			=> new ValPosCrt( a.X , a.Y , a.Value * b );

		public static ValPosCrt operator /( ValPosCrt a , double b )
			=> new ValPosCrt( a.X , a.Y , b != 0 ? a.Value / b : double.MaxValue );
	}


	public static partial class ValPosExt
	{
		public static ValPosCrt AddPos( this ValPosCrt self , ValPosCrt target )
			=> new ValPosCrt( self.X + target.X , self.Y + target.Y , self.Value );

		public static ValPosCrt SubPos( this ValPosCrt self , ValPosCrt target )
			=> new ValPosCrt( self.X - target.X , self.Y - target.Y , self.Value );
	}

	#region Helper unit

	public struct X
	{
		public readonly double Value;

		public X(double x)
		{
			Value = x;
		}

		public static implicit operator double(X x)
			=> x.Value;

		public static implicit operator X(double x)
			=> new X(x);
	}

	public struct Y
	{
		public readonly double Value;

		public Y( double y )
		{
			Value = y;
		}

		public static implicit operator double( Y y )
			=> y.Value;

		public static implicit operator Y( double y )
			=> new Y( y );
	}

	#endregion	

}
