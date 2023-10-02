using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Desktop
{
    [JsonObject]
    /// <summary>
    /// Modelo de peticion de Datos para liberacion de usuarios
    /// </summary>
    public class RequestUser
    {
        /// <summary>
        /// Gets or sets the identifier user.
        /// </summary>
        /// <value>
        /// Id del usuario que va a ser liberado.
        /// </value>
        public int IdUser { get; set; }
    }
}
