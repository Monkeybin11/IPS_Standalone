using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniDriver;

namespace SpectormeterTest
{
	class Program
	{
		static void Main( string [ ] args )
		{
			int numberOfSpectrometers;
			int spectrometerIndex;

			OmniDriver.NETWrapper wrapper = new OmniDriver.NETWrapper();
			numberOfSpectrometers = wrapper.openAllSpectrometers();

			if ( numberOfSpectrometers < 1 ) return; 
			
			spectrometerIndex = 0;

			var res1 = wrapper.getSpectrum( spectrometerIndex ) ;
			var res2 = wrapper.getWavelengths( spectrometerIndex ).Max() ;



		}
	}
}
