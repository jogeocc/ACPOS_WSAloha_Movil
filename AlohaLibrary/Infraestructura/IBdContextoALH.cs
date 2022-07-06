using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Infraestrutura
{
    public interface IBdContextoALH
    {
        OleDbConnection Conexion { get; }
        OleDbDataAdapter Adaptador { get; }
    }
}
