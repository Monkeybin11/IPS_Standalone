using ModelLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineLib.DeviceLib
{
	public class SgmaStg_XR_Virtual : ISgmaStg_XR , IStgCtrl
	{
		public string Home { get { return "H:"; } set { } } 
		public string GoAbs	{ get{return "A:";} set { } }
		public string GoRel { get { return "M:"; } set { } }
		public string SetSpeed { get{return "D:";} set { } }
		public string Status	{ get{return "!:";} set { } }
		public string StatusOK { get{return "R"; } set { } }
		public string Pos		{ get{return "Q:";} set { } }
		public string Go		{ get{return "G:";} set { } }
		public string Stop { get { return "L:"; } set { } }

		public bool Open()
		{
			return true;
		}

		public string Query( string cmd )
		{
			return StatusOK;
		}

		public bool Send( string cmd )
		{
			Console.WriteLine( "Send : " + cmd );
			Thread.Sleep( 1000 );
			//return cmd == "H:1" ? false : true;
			return true;
		}

		public bool WaitReady( int timeoutSec )
		{
			return true;
		}

		public bool SendAndReady( string cmd , int timeoutSec = 0 )
		{
			Console.WriteLine( "Send : " + cmd );
			Thread.Sleep( 100 );
			//return cmd == "H:1" ? false : true;
			return true;
		}

		public void Close()
		{ }
	}
}
