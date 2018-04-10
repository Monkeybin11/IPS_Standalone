using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSResult
	{
		public List<SpotData> SpotDataList;
		public List<double> WaveLen;

		public IPSResult( List<double> wavelen )
		{
			WaveLen = wavelen;
			SpotDataList = new List<SpotData>();
		}

		

	}

	public class SpotData
	{
		// For Genrelize  
		//public CrtnCrd CrtnPos;
		public PlrCrd	PlrPos;
		public CrtnCrd CrtPos { get { return PlrPos.ToCartesian() as CrtnCrd; } }
		public double	Thickness;
		public double[] IntenList;
		public double[] Reflectivity;

		public SpotData( PlrCrd pos , double thckness , double [ ] intens , double[] reflectivityes )
		{
			PlrPos		= pos;
			Thickness	= thckness;
			IntenList	= intens;
			Reflectivity = reflectivityes;
		}
	}

	public class IPSResult_ForGrid
	{
		public string X { get; set; }
		public string Y { get; set; }
		public string Thickness { get; set; }
	}


	public class DisplayData
	{
		public double X;
		public double Y;
		public double Theta;
		public double Rho;
		public double Thickness;
	}

	public static class DataExt
	{
		public static IPSResult_ForGrid ToGridResult(
			this SpotData self )
		{
			var cartesian = self.PlrPos.ToCartesian();
			return new IPSResult_ForGrid()
			{
				X = cartesian.X.ToString("N5") ,
				Y = cartesian.Y.ToString("N5") ,
				Thickness = self.Thickness.ToString( "N4" )
			};
		}
	}
}
