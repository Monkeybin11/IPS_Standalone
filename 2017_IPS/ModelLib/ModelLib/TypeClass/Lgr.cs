using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    // Logger Monad
    public interface Lgr<MA,A> 
        where MA :  Maybe<A>
    {
        MA Value { get; set; }
        List<string> Logs { get; set; }
    }

    //public class Logger<MA,A>  : Lgr<MA,A>
    //{
    //
    //}
    //
    //
    //public static class LgerExtension
    //{
    //    public static ToLogger
    //}
    //
}
