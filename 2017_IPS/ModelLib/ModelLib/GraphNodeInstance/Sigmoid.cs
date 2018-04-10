using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.GraphNodeInstance
{
	public class Sigmoid
	{
		public double Output;
		public double Forward( double x )
		{
			Output = 1 / ( 1 + Math.Exp( -x ) );
			return Output;
		}

	}
}
