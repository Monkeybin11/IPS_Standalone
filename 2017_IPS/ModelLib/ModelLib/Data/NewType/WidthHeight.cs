using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Data.NewType
{
	public struct Width
	{
		public readonly double W;
		public Width( double w )
		{
			W = w;
		}

		public static implicit operator Width( double val )
			=> new Width( val );

		public static implicit operator double( Width w )
			=> w.W;

	}

	public struct Height
	{
		public readonly double H;
		public Height( double h )
		{
			H = h;
		}

		public static implicit operator Height(double val)
			=> new Height(val);

		public static	implicit operator double(Height h)
			=> h.H;

	}	

}
