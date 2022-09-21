using AlohaWebServiceMobile.Models.Transacciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Utils
{

    public class FuncionesArchivo
    {
        private string NombreArchivo = "Trans.log";
        private string RespaldoArchivo = "Mirror.log";


        public void WriteTrans(User user)
        {
            string json = JsonConvert.SerializeObject("");
        }
        public void DeleteTrans(User user)
        {
            
        }
    }
}
