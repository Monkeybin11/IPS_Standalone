using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using ModelLib.Monad;
using System.Runtime.InteropServices;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public partial class DalsaTDICam : IDalsaTDICam
    {
        public bool Connect( string connect )
        {
			// Load Config , Create Object
			this.Initialize();
			ConnectSerialPort( connect );
			return true;
		}

        public void Disconnect( )
        {
            try
            {
                if ( Xfer != null )
                {
                    Xfer.Destroy();
                    Xfer.Dispose();
                }

                if ( AcqDevice != null )
                {
                    AcqDevice.Destroy();
                    AcqDevice.Dispose();
                }

                if ( Acquisition != null )
                {
                    Acquisition.Destroy();
                    Acquisition.Dispose();
                }

                if ( Buffers != null )
                {
                    Buffers.Destroy();
                    Buffers.Dispose();
                }
                if ( ServerLocation != null ) ServerLocation.Dispose();
            }
            catch ( Exception )
            {
            }
        }

        public void Direction( DirectionMode direction )
        {
            SerialCom.SetCamParm( CommandList.scd, ( double )( direction == DirectionMode.Forward ? 0:1 ) );
        }

        public void ExposureMode( double value )
        {
            SerialCom.SetCamParm( CommandList.sem , value );
           
        }

        public void Freeze()
        {
			if ( Xfer.Grabbing ) Xfer.Freeze();
        }

        public int[] GetBufferHW()
        {
            try
            {
                return new int [ Buffers.Width * Buffers.Height ];
            }
            catch ( Exception )
            {
                return new int[0];
            }
        }

        public byte[] GetFullBuffer()
        {
            try
            {
                byte[] output = new byte[Buffers.Width*Buffers.Height];
                GCHandle outputAddr = GCHandle.Alloc( output, GCHandleType.Pinned); // output 의 주소 만듬
                IntPtr pointer = outputAddr.AddrOfPinnedObject(); // 
                Buffers.ReadRect( 0 , 0 , Buffers.Width , Buffers.Height , pointer );
                Marshal.Copy( pointer , output , 0 , output.Length );
                outputAddr.Free();
                return output;
            }
            catch ( Exception )
            {
                return default(byte[]);
            }
        }

        public void Grab()
        {
			if ( !Xfer.Grabbing ) Xfer.Grab();
        }
 
        public void LineRate( double value )
        {
			SerialCom.SetCamParm( CommandList.ssf , value );
        }

        public void RegistBuffGetEvt()
        {
            // sjw 모륵겠다. 
        }

        public void TDIMode( TdiMode mode )
        {
			SerialCom.SetCamParm( CommandList.tdi , mode == TdiMode.Tdi ? 1 : 0 );
        }

	
	}
}
