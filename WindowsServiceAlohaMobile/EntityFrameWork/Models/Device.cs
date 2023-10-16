using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Models
{
    [JsonObject]
    public class Device
    {
        [Key]
        public int ID { get; set; }


        [MaxLength(100)]
        public string Id_Device { get; set; }

        public string Device_name { get; set; }

    }
}
