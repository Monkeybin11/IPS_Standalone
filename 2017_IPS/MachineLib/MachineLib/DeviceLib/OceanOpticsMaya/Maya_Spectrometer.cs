using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;
using ModelLib.ClassInstance;
using OmniDriver;
using SpeedyCoding;
using System.Windows.Forms;

namespace MachineLib.DeviceLib
{
	public class Maya_Spectrometer : IMaya_Spectrometer
	{
		int Index;
		object key = new object();
		public NETWrapper Sptr; // Spectrometer
		public List<double> Datas;
		public List<double> WaveLen;

		public Maya_Spectrometer()
		{
			Sptr = new NETWrapper();
			Datas = new List<double>();
			WaveLen = new List<double>();
			Sptr.setCorrectForElectricalDark( Index , 1 );
		}

	

		public bool Connect()
		=> Sptr.openAllSpectrometers() > 0 ? true : false;

		public double [ ] GetSpectrum()
		{
			//lock ( key )
			Sptr.setCorrectForElectricalDark( Index , 1 );
			return Sptr.getSpectrum( Index );
		}

		public double [ ] GetWaveLen(  )
		=> Sptr.getWavelengths( Index );

		public string LastException()
		=> Sptr.getLastException();

		public string SerialNum()
		=> Sptr.getSerialNumber( Index );


		public IMaya_Spectrometer Timeout( int millisec )
		=> this.Act( x => Sptr.setTimeout( Index , millisec ) );

		public IMaya_Spectrometer IntegrationTime( int microsec )
		=> this.Act( x => Sptr.setIntegrationTime( Index , microsec ) );

		public IMaya_Spectrometer BoxCar( int width )
		=> this.Act( x => Sptr.setBoxcarWidth( Index , width ) );

		public IMaya_Spectrometer ScanAvg( int count )
		=> this.Act( x => Sptr.setScansToAverage( Index , count ) );

		public IMaya_Spectrometer RemoveDark()
		=> this.Act( x => Sptr.setCorrectForElectricalDark(Index , 1) );
	}



}
