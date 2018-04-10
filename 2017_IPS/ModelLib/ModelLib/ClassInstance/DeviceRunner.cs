using ModelLib.TypeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.ClassInstance
{
	public class DeviceRunner<D> : DeviceRun<D> where D : Device
	{
		public List<bool> BatchRun( List<D> deviceList )
		{
			return deviceList.Select( x => Task.Run( () => x.Run() ) )
							 .Select( x => x.Result )
							 .ToList();
		}
	}
}
