using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;


namespace IPSDataHandler
{
	using ThicknessAndComposition_Inspector_IPS_Core;
	public static partial class Handler
	{
		public static WaveLength WaveLength( double val )
			=> new WaveLength( val );

		public static Reflectivity Reflectivity( double val )
			=> new Reflectivity( val );

		public static Intensity Intensity( double val )
			=> new Intensity( val );

		public static Thickness Thickness( double val )
			=> new Thickness( val );

		public static Thickness Thickness( Maybe<double> val )
			=> new Thickness( val );

		public static mCrtCrd mCrtCrd( Maybe<double> x , Maybe<double> y )
			=> new mCrtCrd( x , y );
		//Helper
		public static Maybe<double> ParseToDouble( string s )
		{
			double result;
			double.TryParse( s , out result );
			
			return double.TryParse( s , out result )
			   ? Just( result ) : None;
		}
	}
}

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using ModelLib.Data;
	public class ScanPosStr
	{
		public readonly Maybe<string> Pos;

		public ScanPosStr()
		{
			Pos = None;
		}

		public ScanPosStr( string val )
		{
			if ( IsValid( val ) ) Pos = val;
			else Pos = None;
		}

		static bool IsValid( string val ) // Type Constrain
		{
			double _;
			return  val.Split(' ')
					   .Select( x => double.TryParse( x , out _ ))
					   .Aggregate( (f,s) => f && s);
		}

		public static implicit operator string( ScanPosStr self )
			=> self.Pos.Value;
	}

	public class ScanPos
	{
		readonly Maybe<double[]> Pos;
		public ScanPos()
		{
			Pos = None;
		}

		public double X { get { return Pos.Value [ 0 ]; } }
		public double Y { get { return Pos.Value [ 1 ]; } }
		public ScanPos( Maybe<double[]> val )
		{
			if ( IsValid( val ) ) Pos = val;
			else Pos = None;
		}

		static bool IsValid( Maybe<double [ ]> val ) // Type Constrain
		=> val.isJust ? true : false;
		
	}

	public class WaveLength
	{
		public readonly Maybe<double> Value;
		
		public WaveLength()
		{
			Value = None;
		}

		public WaveLength( double val )
		{
			if ( IsValid( val ) ) Value = val;
			else Value = None;
		}

		static bool IsValid( double val ) // Type Constrain
			=> val > 0;

		public static implicit operator Maybe<double>( WaveLength self )
			=> self.Value;
		public static implicit operator WaveLength( Maybe<double> x )
			=> x.isJust ? new WaveLength( x.Value ) : new WaveLength();

		public static implicit operator double( WaveLength self )
			=> self.Value.Value;

	}

	public class Reflectivity
	{
		public readonly Maybe<double> Value;

		public Reflectivity()
		{
			Value = None;
		}

		public Reflectivity( double val )
		{
			if ( IsValid( val ) ) Value = Just( val );
			else Value = None;
		}

		static bool IsValid( double val ) // Type Constrain
			=> val < 100 || val > 0;


		public static implicit operator Maybe<double>( Reflectivity self )
			=> self.Value;
		public static implicit operator Reflectivity( Maybe<double> x )
			=> x.isJust ? new Reflectivity( x.Value ) : new Reflectivity();

		public static implicit operator double( Reflectivity self )
			=> self.Value.Value;

	}

	public class Intensity 
	{
		public readonly Maybe<double> Value;
		public Intensity()
		{
			Value = None;
		}

		public Intensity( double val )
		{
			Value = Just(val);
		}

		public static implicit operator Maybe<double>( Intensity self )
			=> self.Value;
		public static implicit operator Intensity( Maybe<double> x )
		=> x.isJust ? new Intensity( x.Value ) : new Intensity();
		public static implicit operator double( Intensity self )
		=> self.Value.Value;



	}

	public class Thickness
	{
		public readonly Maybe<double> Value;

		public Thickness()
		{
			Value = None;
		}

		public Thickness( double val )
		{
			Value = Just(val);
		}

		public Thickness( Maybe<double> val )
		{
			Value =  val;
		}

		public static implicit operator Maybe<double>( Thickness self )
			=> self.Value;

		public static implicit operator Thickness( double x )
			=> new Thickness( x );
		public static implicit operator double( Thickness self )
			=> self.Value.Value;
	}


	public class mCrtCrd
	{
		public readonly Maybe<CrtnCrd> Pos;
		   
  		public mCrtCrd()
		{
			Pos = None;
		}

		public mCrtCrd( Maybe<double> x , Maybe<double> y )
		{
			Pos = x.Match(
				() => None ,
				xx => y.Match(
					() => None ,
					yy => Just(new CrtnCrd(xx , yy))) );
		}

		public B Match<B>( Func<B> Nothing , Func<CrtnCrd , B> Just )
			=> Pos.isJust ? Just( Pos.Value ) : Nothing();

	}
}
