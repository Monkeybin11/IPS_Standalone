using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;

namespace ModelLib.Data
{
	public interface Polar : Crd2D , IFormattable
	{
		double Theta { get; set; }
		double Rho { get; set; }
		
		

	}
	public class PlrCrd : Polar
	{
		public double Theta { get; set; }
		public double Rho { get; set; }
		
		
		
		

		public override string ToString()
		{
			var strRho = Rho.ToString();
			var strTheta = Theta.ToString();
			return  strTheta + "," + strRho  ;
		}

		public string ToString( string format , IFormatProvider formatProvider )
		{
			var strRho = Rho.ToString();
			var strTheta = Theta.ToString();
			return strTheta + "," + strRho;
		}

		public PlrCrd(  )
		{
			
		}

		public PlrCrd( double theta , double rho  )
		{
			Theta = theta;
			Rho = rho;
			
			
		}
	}
	public class PlrUnit : Polar
	{
		public double Theta { get { return default( double ); } set { } }
		public double Rho { get { return default( double ); } set { } }

		public string ToString( string format , IFormatProvider formatProvider )
		{
			var strTheta = Theta.ToString();
			var strRho = Rho.ToString();
			return strRho + "," + strTheta;
		}
	}


	public interface Cartesian : Crd2D
	{
		double X { get; set; }
		double Y { get; set; }
	}

	public class CrtnCrd : Cartesian
	{
		public double X { get; set; }
		public double Y { get; set; }

		public CrtnCrd()
		{ }

		public CrtnCrd( double x , double y )
		{
			X = x;
			Y = y;
		}
	}

	public class CrtnUnit : Cartesian // TODO : Not Complete yet
	{
		public double X { get { return default( double ); } set { } }
		public double Y { get { return default( double ); } set { } }
	}

	public static class Crd2D_Property
	{
		public static Cartesian ToCartesian<A>(
		this A src )
		where A : Crd2D
		{
			var polar = src as PlrCrd;
			var crtn = src as CrtnCrd;

			if ( crtn != null ) return crtn;
			else if ( polar == null ) return new CrtnUnit();
			return new CrtnCrd(
							   polar.Rho * Math.Cos( polar.Theta * Math.PI / 180.0 ) , // x
							   polar.Rho * Math.Sin( polar.Theta * Math.PI / 180.0 ) // y
							   );
		}

		public static Polar ToPolar<A>(
		this A src )
		where A : Crd2D
		{
			var polar = src as PlrCrd;
			var crtn = src as CrtnCrd;

			if ( polar != null ) return polar;
			else if ( crtn == null ) return new PlrUnit();

			var theta = Math.Atan2( crtn.Y , crtn.X ) * 180 / Math.PI;

			return new PlrCrd(
							   theta < 0 ? 360 + theta : theta ,
							   Math.Sqrt( crtn.X * crtn.X + crtn.Y * crtn.Y )  );
		}


		public static A Add<A, B>(
		this A v1 ,
		A v2 )
		where A : class, Crd2D
		where B : class, Crd2D
		{
			// v1 , v2 가 crtn 인지 plr 인지 
			var vec1c = (v1 as CrtnCrd);
			var vec1p = (v1 as PlrCrd);
			var vec2c = (v2 as CrtnCrd);
			var vec2p = (v2 as PlrCrd);

			Cartesian c1 = vec1c != null ? vec1c :
						   vec1p != null ? vec1p.ToCartesian() :
						   new CrtnUnit();

			Cartesian c2 = vec2c != null ? vec1c :
						   vec2p != null ? vec1p.ToCartesian() :
						   new CrtnUnit();

			var cr1 = c1 as CrtnCrd;
			var cr2 = c2 as CrtnCrd;

			return new CrtnCrd( cr1.X + cr2.X , cr1.Y + cr2.Y ) as A;
		}


		#region matcher helper

		public static bool ToCondition<T>(
		this T src ) =>
			src == null ? false : true;

		#endregion
	}
}
