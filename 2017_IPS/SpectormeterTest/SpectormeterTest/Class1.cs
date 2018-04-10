using OmniDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectormeterTest
{
	public class BanSpectrometer 
	{
		public string DeviceName { get; set; }
		public string LastErrorMsg { get; set; }

		public int IntegrationTime { get; set; }
		public int BoxCar { get; set; }
		public int SamplingCount { get; set; }
		public int DeviceIndex { get; set; }
		public int NumberOfSpectrometers { get; set; }
		public int NumberOfVirtualSpectrometers { get; set; }
		public bool IsConnected { get; set; }

		public double [ ] LastIntensity { get; set; }
		public double [ ] LastWavelength { get; set; }
		public double [ ] [ ] WaveLength { get; set; }
		public double [ ] [ ] Intensity { get; set; }
		public double [ ] [ ] VirtualWaveLength { get; set; }
		public double [ ] [ ] VirtualIntensity { get; set; }
		public double MaxCount { get; set; }

		//public Dictionary<int, double[]> IntensityDic = new Dictionary<int, double[]>();

		protected CCoWrapper OceanOpticsWrapper;

		public int GetSerialNumberToIndex( string serialNumber )
		{
			for ( int i = 0 ; i < NumberOfSpectrometers ; i++ )
			{
				if ( serialNumber == OceanOpticsWrapper.getSerialNumber( i ) )
					return i;
			}
			return -1;
		}

		public string GetSerialNumber( int deviceIndex )
		{
			return OceanOpticsWrapper.getSerialNumber( deviceIndex );
		}

		public int CheckIndex( string spectrometerSerial )
		{
			try
			{
				int spectrometerCount = OceanOpticsWrapper.openAllSpectrometers();
				for ( int i = 0 ; i < spectrometerCount ; ++i )
				{
					if ( OceanOpticsWrapper.getSerialNumber( i ) == spectrometerSerial )
						return i;
				}
				return -1;
			}
			catch ( Exception except )
			{
				return 0;
			}
		}

		public BanSpectrometer( int numberOfVirtualSpectrometers = 0 )
		{
			this.NumberOfVirtualSpectrometers = numberOfVirtualSpectrometers;
			DeviceIndex = 0;
			MaxCount = 65000;
			OceanOpticsWrapper = new CCoWrapper();
		}

		public BanSpectrometer( CCoWrapper loadWrapper , int numberOfVirtualSpectrometers = 0 )
		{
			this.NumberOfVirtualSpectrometers = numberOfVirtualSpectrometers;
			DeviceIndex = 0;
			MaxCount = 65000;
			OceanOpticsWrapper = new CCoWrapper();
			OceanOpticsWrapper = loadWrapper;
		}

		public bool Connect( string connectArgs = null )
		{
			try
			{
				NumberOfSpectrometers = OceanOpticsWrapper.openAllSpectrometers();
				Console.WriteLine( "연결된 스펙트로미터 : " + NumberOfSpectrometers.ToString() );

				if ( NumberOfVirtualSpectrometers > 0 )
				{
					VirtualWaveLength = new double [ NumberOfVirtualSpectrometers ] [ ];
					VirtualIntensity = new double [ NumberOfVirtualSpectrometers ] [ ];
					Console.WriteLine( "생성된 가상 스펙트로미터 : " + NumberOfVirtualSpectrometers.ToString() );
				}

				if ( NumberOfSpectrometers == 0 )
				{
					IsConnected = false;
					return false;
				}
				WaveLength = new double [ NumberOfSpectrometers ] [ ];
				Intensity = new double [ NumberOfSpectrometers ] [ ];

				IsConnected = true;
				return IsConnected;
			}
			catch ( Exception except )
			{
				LastErrorMsg = "Connect Error " + except.ToString();
				IsConnected = false;
				return IsConnected;
			}
		}

		public bool Release()
		{
			try
			{
				OceanOpticsWrapper.closeAllSpectrometers();
				IsConnected = false;
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		/// <summary>
		/// virtual device 전용 dummy 생성함수 입니다.
		/// </summary>
		/// <param name="dummyCount"></param>
		/// <param name="startWave"></param>
		/// <param name="endWave"></param>
		public void Measure( int dummyCount , int startWave , int endWave , int index )
		{
			Random rand = new Random();

			double[] wavelength = new double[dummyCount];
			double[] intensity = new double[dummyCount];

			int temp;

			for ( int i = 0 ; i < dummyCount / 3 ; i++ )    // i range : 2000
			{
				wavelength [ i ] = startWave + ( ( double )( endWave - startWave ) ) / dummyCount * i;
				intensity [ i ] = rand.Next( ( int )( i * 1.5 ) , ( int )( i * 2 ) );
			}

			for ( int i = dummyCount * 2 / 6 ; i < dummyCount * 3 / 6 ; i++ )
			{
				wavelength [ i ] = startWave + ( ( double )( endWave - startWave ) ) / dummyCount * i;
				intensity [ i ] = rand.Next( ( int )( i * 8.5 ) , i * 9 );
			}

			temp = dummyCount * 3 / 6;
			for ( int i = dummyCount * 3 / 6 ; i < dummyCount * 4 / 6 ; i++ )
			{
				wavelength [ i ] = startWave + ( ( double )( endWave - startWave ) ) / dummyCount * i;
				intensity [ i ] = rand.Next( ( int )( temp * 8.5 ) , temp * 9 );
				temp--;
			}

			for ( int i = dummyCount * 2 / 3 ; i < dummyCount ; i++ )
			{
				wavelength [ i ] = startWave + ( ( double )( endWave - startWave ) ) / dummyCount * i;
				intensity [ i ] = rand.Next( ( int )( temp * 1.5 ) , ( int )( temp * 2 ) );
				temp--;
			}
			LastWavelength = ( double [ ] )wavelength.Clone();
			LastIntensity = ( double [ ] )intensity;

			VirtualWaveLength [ index ] = ( double [ ] )wavelength;
			VirtualIntensity [ index ] = ( double [ ] )intensity;
		}

		public bool Measure()
		{
			try
			{
				LastWavelength = OceanOpticsWrapper.getWavelengths( DeviceIndex );
				LastIntensity = OceanOpticsWrapper.getSpectrum( DeviceIndex );

				WaveLength [ DeviceIndex ] = ( double [ ] )LastWavelength.Clone();
				Intensity [ DeviceIndex ] = ( double [ ] )LastIntensity.Clone();

				if ( LastIntensity.Max() > MaxCount )
				{
				}
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		/// <summary>
		/// 실제 사용되는 측정 메소드
		/// </summary>
		/// <param name="deviceIndex"></param>
		/// <returns></returns>
		public bool Measure( int deviceIndex )
		{
			try
			{
				LastWavelength = OceanOpticsWrapper.getWavelengths( deviceIndex );
				LastIntensity = OceanOpticsWrapper.getSpectrum( deviceIndex );

				WaveLength [ deviceIndex ] = ( double [ ] )LastWavelength.Clone();
				Intensity [ deviceIndex ] = ( double [ ] )LastIntensity.Clone();

				if ( LastIntensity.Max() > MaxCount )
				{
				}
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		public bool Measure( out double [ ] outWavelength , out double [ ] outIntensity )
		{
			try
			{
				LastWavelength = OceanOpticsWrapper.getWavelengths( DeviceIndex );
				LastIntensity = OceanOpticsWrapper.getSpectrum( DeviceIndex );

				WaveLength [ DeviceIndex ] = ( double [ ] )LastWavelength.Clone();
				Intensity [ DeviceIndex ] = ( double [ ] )LastIntensity.Clone();

				outWavelength = ( double [ ] )LastWavelength.Clone();
				outIntensity = ( double [ ] )LastIntensity.Clone();
				return true;
			}
			catch ( Exception except )
			{
				outWavelength = null;
				outIntensity = null;
				return false;
			}
		}

		public bool Measure( int deviceIndex , out double [ ] outWavelength , out double [ ] outIntensity )
		{
			try
			{
				LastWavelength = OceanOpticsWrapper.getWavelengths( deviceIndex );
				LastIntensity = OceanOpticsWrapper.getSpectrum( deviceIndex );

				WaveLength [ deviceIndex ] = ( double [ ] )LastWavelength.Clone();
				Intensity [ deviceIndex ] = ( double [ ] )LastIntensity.Clone();

				outWavelength = ( double [ ] )LastWavelength.Clone();
				outIntensity = ( double [ ] )LastIntensity.Clone();
				return true;
			}
			catch ( Exception except )
			{
				outWavelength = null;
				outIntensity = null;
				return false;
			}
		}


		public bool SetBoxCar( int boxcarCount )
		{
			try
			{
				this.BoxCar = boxcarCount;

				OceanOpticsWrapper.setBoxcarWidth( DeviceIndex , boxcarCount );
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		public bool SetBoxCar( int deviceIndex , int boxcarCount )
		{
			try
			{
				this.BoxCar = boxcarCount;

				OceanOpticsWrapper.setBoxcarWidth( deviceIndex , boxcarCount );
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		public bool SetIntegrationTime( int integrationTime )
		{
			try
			{
				this.IntegrationTime = integrationTime;

				OceanOpticsWrapper.setIntegrationTime( DeviceIndex , IntegrationTime * 1000 );    //ms 단위
				OceanOpticsWrapper.setCorrectForDetectorNonlinearity( DeviceIndex , 1 );
				return true;
			}
			catch ( Exception except )
			{
				LastErrorMsg = except.ToString();
				return false;
			}
		}

		public bool SetIntegrationTime( int deviceIndex , int integrationTime )
		{
			try
			{
				this.IntegrationTime = integrationTime;

				OceanOpticsWrapper.setIntegrationTime( deviceIndex , IntegrationTime * 1000 );    //ms 단위
				OceanOpticsWrapper.setCorrectForDetectorNonlinearity( deviceIndex , 1 );
				return true;
			}
			catch ( Exception except )
			{
				LastErrorMsg = except.ToString();
				return false;
			}
		}

		public bool SetRemoveDark()
		{
			try
			{
				OceanOpticsWrapper.setCorrectForElectricalDark( DeviceIndex , 1 );
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		public bool SetRemoveDark( int deviceIndex )
		{
			try
			{
				OceanOpticsWrapper.setCorrectForElectricalDark( deviceIndex , 1 );
				return true;
			}
			catch ( Exception except )
			{
				return false;
			}
		}

		public bool SetSamplingCount( int samplingCount )
		{
			try
			{
				this.SamplingCount = samplingCount;

				OceanOpticsWrapper.setScansToAverage( DeviceIndex , samplingCount );
				return true;
			}
			catch ( Exception except )
			{
				LastErrorMsg = except.ToString();
				return false;
			}
		}

		public bool SetSamplingCount( int deviceIndex , int samplingCount )
		{
			try
			{
				this.SamplingCount = samplingCount;

				OceanOpticsWrapper.setScansToAverage( deviceIndex , samplingCount );
				return true;
			}
			catch ( Exception except )
			{
				LastErrorMsg = except.ToString();
				return false;
			}
		}
	}
}
