using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Factory.Logger
{
	public interface ILogger
	{
		ILogger Log();
	}

	public class TimeLogger : ILogger
	{
		public TimeLogger Log()
		{
			return new TimeLogger();
		}

		ILogger ILogger.Log()
		{
			throw new NotImplementedException();
		}
	}
}
