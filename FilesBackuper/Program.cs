using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesBackuper
{
    class Program
    {
        static void Main(string[] args)
        {

            Utils.readIni();

            Utils.directoryCopy(Params.sourcePath, Params.destinationPath);
        }
    }
}
