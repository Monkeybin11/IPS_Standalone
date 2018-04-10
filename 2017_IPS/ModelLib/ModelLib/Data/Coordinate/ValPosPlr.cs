using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModelLib.AmplifiedType
{
	using ModelLib.Data;
	public static partial class Handler
	{
		public static ValPosPlr ValPosPlr( Theta t , Rho r , double value = 0 )
			=> new ValPosPlr( t , r , value );
	}
}

namespace ModelLib.Data
{
	using static System.Math;

	public class ValPosPlr
	{
		public readonly Theta T;
		public readonly Rho R;
		public readonly double Value;

		public ValPosPlr( Theta t , Rho r , double val )
		{
			T = t;
			R = r;
			Value = val;
		}

		public static bool operator <( ValPosPlr a , ValPosPlr b )
			=> a.R < b.R;

		public static bool operator >( ValPosPlr a , ValPosPlr b )
			=> a.R > b.R;

		public static implicit operator Tuple<Theta , Rho , double>( ValPosPlr valpos )
			=> Tuple.Create( valpos.T , valpos.R , valpos.Value );

		public static explicit operator ValPosPlr( Tuple<Theta , Rho , double> data )
			=> new ValPosPlr( data.Item1 , data.Item2 , data.Item3 );

		public static ValPosPlr operator +( ValPosPlr a , double b )
			=> new ValPosPlr( a.T , a.R , a.Value + b );

		public static ValPosPlr operator -( ValPosPlr a , double b )
			=> new ValPosPlr( a.T , a.R , a.Value - b );

		public static ValPosPlr operator *( ValPosPlr a , double b )
			=> new ValPosPlr( a.T , a.R , a.Value * b );

		public static ValPosPlr operator /( ValPosPlr a , double b )
			=> new ValPosPlr( a.T , a.R , b != 0 ? a.Value / b : double.MaxValue );
	}

	public static partial class ValPosExt
	{
		public static ValPosPlr AddPos( this ValPosPlr self , ValPosPlr target )
			=> new ValPosPlr( self.T + target.T , self.R + target.R , self.Value );

		public static ValPosPlr SubPos( this ValPosPlr self , ValPosPlr target )
			=> new ValPosPlr( self.T - target.T , self.R - target.R , self.Value );
	}


	#region Helper unit

	public struct Theta
	{
		public readonly double Value;

		public Theta( double t )
		{
			Value = t;
		}

		public static implicit operator double( Theta t )
			=> t.Value;

		public static implicit operator Theta( double t )
			=> new Theta( t );

	}

	public struct Rho
	{
		public readonly double Value;

		public Rho( double r )
		{
			Value = r;
		}

		public static implicit operator double( Rho r )
			=> r.Value;

		public static implicit operator Rho( double r )
			=> new Rho( r );
	}

	#endregion



}
