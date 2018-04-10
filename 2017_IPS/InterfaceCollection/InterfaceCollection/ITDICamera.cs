using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceCollection
{
    public enum TdiMode { Tdi , Area}
    public enum DirectionMode { Forward , Backward}
    public interface ITDICameraAPI<TbufType,TtransInfo,TconnectInfoType>
    {
		// output = this 
		bool Connect( TconnectInfoType connect );
		void Disconnect();
		void Grab();
		void Freeze();
        TbufType GetFullBuffer();
		void RegistBuffGetEvt();
        TtransInfo GetBufferHW();
		void LineRate(double value);
		void ExposureMode( double value );
		void Direction( DirectionMode direction );
		void TDIMode( TdiMode mode);
    }
}
