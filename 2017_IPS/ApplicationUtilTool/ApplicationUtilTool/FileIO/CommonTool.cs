using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
    public class CommonTool
    {
        public void ChangeFileExtension(string path, string extension)
        {
            Path.ChangeExtension( path , extension );
        }

    }
}
