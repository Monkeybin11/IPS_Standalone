using InterfaceCollection;
using ModelLib.Monad;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public interface IDalsaTDICam : ITDICameraAPI<byte [ ] , int [ ] , string>
    {
        string ConfigFile { get; }

       bool Connect( string connect );
       void Direction( DirectionMode direction );
       void Disconnect();
       void ExposureMode( double value );
		void Freeze();
        int [ ] GetBufferHW();
        byte [ ] GetFullBuffer();
       void Grab();
       void LineRate( double value );
       void RegistBuffGetEvt();
		void TDIMode( TdiMode mode );
    }
}