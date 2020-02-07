using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonfileuploader
{
    public static class Strings
    {
        public static string DataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Anon Uploader\";

        public static string DataFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Anon Uploader\Data.txt";
    }
}
