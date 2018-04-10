using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
	public interface IEither<L,R>
	{
		L Left { get; set; }
		R Right { get; set; }
		bool IsRight { get; set; }

	}
}
