using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using ModelLib.Monad;
using SpeedyCoding;
using System.Runtime.CompilerServices;
using System.Diagnostics;


namespace MachineLib.DeviceLib.DalsaTDICamera
{
	public class DalsaTDICam_Dummy : IDalsaTDICam
	{
		public string ConfigFile
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool Connect( string connect )
		{
			throw new NotImplementedException();
		}

		public void Direction( DirectionMode direction )
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}

		public void ExposureMode( double value )
		{
			throw new NotImplementedException();
		}

		public void Freeze()
		{
			throw new NotImplementedException();
		}

		public int [ ] GetBufferHW()
		{
			throw new NotImplementedException();
		}

		public byte [ ] GetFullBuffer()
		{
			throw new NotImplementedException();
		}

		public void Grab()
		{
			throw new NotImplementedException();
		}

		public void LineRate( double value )
		{
			throw new NotImplementedException();
		}

		public void RegistBuffGetEvt()
		{
			throw new NotImplementedException();
		}

		public void TDIMode( TdiMode mode )
		{
			throw new NotImplementedException();
		}

		bool ITDICameraAPI<byte [ ] , int [ ] , string>.Connect( string connect )
		{
			throw new NotImplementedException();
		}
	}
}
