using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Transacciones
{
    public class BdInterna
    {
        public MyOwnList<User> users { get; set; } = new MyOwnList<User>();
    }

    public class User
    {
        public int IdEmpleado { get; set; }
        public string UserName { get; set; }
    }


    public class MyOwnList<T> : List<T>
    {
        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1" />.
        /// Realiza la accion y almacena el movimiento en el archivo trans
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
        public new void Add(T item)
        {
            base.Add(item);
            App.funcionesArchivo.AddTrans(item as User);
        }

        /// <summary>
        /// Realiza la accion y almacena el movimiento en el archivo trans
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Remove(T item)
        {
            base.Remove(item);
            App.funcionesArchivo.DeleteTrans(item as User);

        }
        public new void Clear()
        {
            base.Clear();
            App.funcionesArchivo.WriteTrans("");
        }
    }
}
