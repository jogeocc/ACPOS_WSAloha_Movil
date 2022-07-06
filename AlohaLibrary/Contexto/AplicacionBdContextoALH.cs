using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Helpers;
using AlohaLibrary.Infraestrutura;

namespace AlohaLibrary.Contexto
{
    public class AplicacionBdContextoALH : BdContextoALH
    {
        public AplicacionBdContextoALH(string path) : base(path)
        {
        }

        public AplicacionBdContextoALH() : base(DirectoriosAloha.GetAlohaDataFolder())
        {

        }
    }
}
