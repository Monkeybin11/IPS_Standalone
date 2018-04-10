using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceCollection
{
    public interface ISerialCommunication<Tinstance>
    {
        Tinstance Connection( Tinstance instance );
        Tinstance Send( Tinstance instance , string text );
        Tinstance Close( Tinstance instance );
    }
}
