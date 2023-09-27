using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Models
{
    public class Mapeo_pagos
    {
        public int ID { get; set; } //Incrementable y PK

        public int id_pago_aloha { get; set; }

        public string clv_mapeo_pago { get; set; }

        public string nombre_pago_aloha { get; set; }

        public bool status { get; set; }
    }
}
