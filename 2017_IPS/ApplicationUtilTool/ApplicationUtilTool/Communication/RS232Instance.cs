using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.Communication
{
	// Class Instance for use RS232 
	public interface RS232Instance
	{
		bool Open();
		void Close();
		bool Send( string cmd );
		string Query( string cmd );
	}
}
