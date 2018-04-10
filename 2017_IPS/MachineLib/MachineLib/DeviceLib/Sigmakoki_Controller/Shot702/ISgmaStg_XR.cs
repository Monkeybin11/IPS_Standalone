namespace MachineLib.DeviceLib
{
	public interface ISgmaStg_XR
	{
		string Go { get; set; }
		string GoAbs { get; set; }
		string GoRel { get; set; }
		string Home { get; set; }
		string Pos { get; set; }
		string SetSpeed { get; set; }
		string Status { get; set; }
		string StatusOK { get; set; }
		string Stop { get; set; }

		bool Open();
		string Query( string cmd );
		bool Send( string cmd );
		bool WaitReady( int timeoutSec );
		bool SendAndReady( string cmd , int timeoutSec = 0 );

		void Close();
	}
}