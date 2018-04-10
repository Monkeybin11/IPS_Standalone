using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
    public interface Functor<A>
    {
		A FMap( A a , Func<A,A> func);
    }
}
