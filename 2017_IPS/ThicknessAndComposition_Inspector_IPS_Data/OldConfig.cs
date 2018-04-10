using ApplicationUtilTool.FileIO;
using SpeedyCoding;
using ModelLib.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSConfigold
	{
		public IPSConfigold()
		{

		}

		public void SetPosition()
		{
			var fnc = new Func<List<PlrCrd>>(()=> {
				if ( RhoCount == 0 ) return null;
				var first = new List<PlrCrd>();
				var counter = RhoFirst == 0
								? 1.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) )
								: 0.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) );

				var second = Enumerable.Range( counter, (int)RhoCount - counter) // if start rho is 0, next rho is 0 + rhostep and count is count -1 ( 1 of -1 is start counter)
								.SelectMany( f => Enumerable.Range( 0 , (int)ThetaCount) ,
											(f,s) => new PlrCrd(
															ThetaFirst + s*ThetaStep ,
															RhoFirst + f*RhoStep ))
								.ToList();

				return first.Act( x => x.AddRange( second ) ).OrderBy( x => x.Theta).ThenBy( x => x.Rho).ToList();

			} );

			var fixpos = new Func<List<PlrCrd>>(()=> {
				if ( RhoCount == 0 ) return null;
				var first = new List<PlrCrd>();
				var counter = RhoFirst == 0
								? 1.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) )
								: 0.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) );

				var second = Enumerable.Range( counter, (int)RhoCount - counter) // if start rho is 0, next rho is 0 + rhostep and count is count -1 ( 1 of -1 is start counter)
								.SelectMany( f => Enumerable.Range( 0 , (int)ThetaCount) ,
											(f,s) => new PlrCrd(
															ThetaFirst + s*ThetaStep ,
															RhoFirst + f*RhoStep ))
								.ToList();

				return first.Act( x => x.AddRange( second ) ).OrderBy( x => x.Theta).ThenBy( x => x.Rho).ToList();

			} );
			//ScanSpot = fnc();
			ScanSpot = fixpos();
		}


		// -- Configs config --
		public string BaseDirPath { get; set; }
		public string StartupConfigName { get; set; }

		// -- scan config --
		public int SampleDiameter { get; set; }
		public List<PlrCrd> ScanSpot
		{
			get; set;
		}

		public double ThetaFirst { get; set; }
		public double ThetaStep { get; set; }
		public double ThetaCount
		{
			get
			{
				var count1 = 360 / ThetaStep;
				return Enumerable.Range( 0 , ( int )count1 )
					.Select( x => ThetaFirst + ThetaStep * x )
					.Where( x => x <= 360 )
					.Select( x => 1 )
					.Aggregate( ( f , s ) => f + 1 );
			}
		}
		public double RhoFirst { get; set; }
		public double RhoStep { get; set; }
		public double RhoCount { get; set; }

		// -- HW Config Hidden--
		// Spetrometer
		public int Boxcar { get; set; }
		public int Scan2Avg { get; set; }
		public int IntegrationTime { get; set; }
		public int SpectrumWaitTime { get; set; }

		// Stage
		public int Port { get; set; }
		public int XStgSpeed { get; set; }
		public int RStgSpeed { get; set; }
	}

	public static class IPSConfigExtOld
	{
		public static IPSConfig SaveConfig(
			this IPSConfig src ,
			string dirpaht ,
			string name )
		{
			XmlTool.WriteXmlClass( src , dirpaht , name );
			return src;
		}

		#region sub
		public static int CalcAngCount( int counter , double input , double step , double limit )
			=> input >= limit
				? counter
				: CalcAngCount( counter++ , ( input + 1 ) * step , step , limit );

		#endregion
	}

}
