using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
	public interface Device // D : Device Type Class Instance
	{
		bool Run();
	}

	public interface DeviceRun<D> where D : Device
	{
		List<bool> BatchRun( List<D> deviceList );
	}
}
