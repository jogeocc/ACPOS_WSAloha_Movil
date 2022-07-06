using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Helpers
{
    public static class DirectoriosAloha
    {
        public static string GetAlohaFolder()
        {
            string alohaFolder = Environment.GetEnvironmentVariable("IBERDIR");

            if (!Directory.Exists(alohaFolder))
            {
                alohaFolder = Environment.GetEnvironmentVariable("LOCALDIR");

                if (!Directory.Exists(alohaFolder))
                {
                    throw new DirectoryNotFoundException("No se puede encontrar directorio de Aloha.");
                }
            }

            return alohaFolder;
        }

        public static string GetAlohaBinFolder()
        {
            return Path.Combine(GetAlohaFolder(), "BIN");
        }

        public static string GetAlohaTmpFolder()
        {
            return Path.Combine(GetAlohaFolder(), "TMP");
        }

        public static string GetAlohaDataFolder()
        {
            return Path.Combine(GetAlohaFolder(), "DATA");
        }

        public static string GetAlohaDateFolder(string fecha)
        {
            return Path.Combine(GetAlohaFolder(), fecha);
        }
    }
}
