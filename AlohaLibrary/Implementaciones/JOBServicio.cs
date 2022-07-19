using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class JOBServicio : ServicioBaseALH<JOB>, IJOB
    {
        public JOBServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<JOB> GetAll()
        {
            List<JOB> Jobs = new List<JOB>();

            return Jobs;
        }
    }
}
