
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device.device
{
    public interface ISpectrometer : IDevice
    {
        int BoxCar { get; set; }
        int IntegrationTime { get; set; }
        int SamplingCount { get; set; }
        
        double[] LastWavelength { get; set; }
        double[] LastIntensity { get; set; }
        double MaxCount { get; set; }

        bool SetIntegrationTime(int integrationTime);
        bool SetSamplingCount(int samplingCount);
        bool SetBoxCar(int boxcarCount);
        bool SetRemoveDark();
        bool Measure();
        bool Measure(out double[] Wavlength, out double[] Intensity);

        bool SetIntegrationTime(int deviceIndex, int integrationTime);
        bool SetSamplingCount(int deviceIndex, int samplingCount);
        bool SetBoxCar(int deviceIndex, int boxcarCount);
        bool SetRemoveDark(int deviceIndex);
        bool Measure(int deviceIndex);
        bool Measure(int deviceIndex, out double[] Wavlength, out double[] Intensity);
    }
}
