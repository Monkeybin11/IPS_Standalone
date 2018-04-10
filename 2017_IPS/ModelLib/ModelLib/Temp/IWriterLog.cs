using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface IWriterLog<T>
    {
        T Log { get; set; }
        T Combine(T logA , T logB);
    }
}
