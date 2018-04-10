using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool.FileIO;
using SpeedyCoding;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSConfig 
	{
		public IPSConfig()
		{
			
		}
		
		// -- Configs config --
		public string BaseDirPath { get; set; } 
		public string StartupConfigName { get; set; } 

		// -- HW Config Hidden--
		// Spetrometer
		public int Boxcar { get; set; }
		public int Scan2Avg { get; set; }
		public int IntegrationTime { get; set; }
		public int SpectrumWaitTime { get { return IntegrationTime * Scan2Avg + 20; } }

		// Stage
		public int Port { get; set; }
		public int XStgSpeed { get; set; }
		public int RStgSpeed { get; set; }

		public double EdgeEnd { get; set; }


	}

	public static class IPSConfigExt
	{
		public static IPSConfig SaveConfig(
			this IPSConfig src ,
			string dirpaht,
			string name)
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
