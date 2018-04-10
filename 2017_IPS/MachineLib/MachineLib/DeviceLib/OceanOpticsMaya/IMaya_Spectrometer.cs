namespace MachineLib.DeviceLib
{
	public interface IMaya_Spectrometer
	{
		IMaya_Spectrometer BoxCar( int width );
		bool Connect();
		double [ ] GetSpectrum();
		double [ ] GetWaveLen();
		IMaya_Spectrometer IntegrationTime( int time );
		string LastException();
		IMaya_Spectrometer RemoveDark();
		IMaya_Spectrometer ScanAvg( int count );
		string SerialNum();
		IMaya_Spectrometer Timeout( int millisec );
	}
}