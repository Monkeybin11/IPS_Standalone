using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLib.DeviceLib
{
	public class Maya_Spectrometer_Virtual : IMaya_Spectrometer
	{
		public IMaya_Spectrometer BoxCar( int width )
		{
			return this;
		}

		public bool Connect()
		{
			return true;
		}

		public double [ ] GetSpectrum()
		{
			Random rnd = new Random();
			var res = Enumerable.Range(0,1068).Select( x => (double)rnd.Next(1000,3000)).ToArray<double>();
			res [ 0 ] = 123;
			res [ 1 ] = 456;
			res [ 2 ] = 789;
			return res;
		}

		public double [ ] GetWaveLen()
		{
			double w = (1120 - 200) / 1068.0;
			var res = Enumerable.Range( 0 , 1068 ).Select( x => x * w + 200 ).ToArray<double>();
			res [ 0 ] = 123;
			res [ 1 ] = 456;
			res [ 2 ] = 789;
			return res;
		}

		public IMaya_Spectrometer IntegrationTime( int time )
		{
			return this;
		}

		public string LastException()
		{
			return "Last Exception";
		}

		public IMaya_Spectrometer RemoveDark()
		{
			return this;
		}

		public IMaya_Spectrometer ScanAvg( int count )
		{
			return this;
		}

		public string SerialNum()
		{
			return "Dummy";
		}

		public IMaya_Spectrometer Timeout( int millisec )
		{
			return this;
		}
	}
}
