using AlohaWebServiceMobile.Models.Transacciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Utils
{
    public class FuncionesArchivo
    {
        private string NombreArchivo = ".\\Trans.log";
        private string RespaldoArchivo = ".\\Mirror.log";


        public void AddTrans(User user)
        {

            try
            {
                //App.bdInterna.users.Add(user);
                string json = JsonConvert.SerializeObject(App.bdInterna.users);
                WriteTrans(json);

            }
            catch (Exception ex)
            {
                App.logger.Error($"Error guardando transacciones", ex);
            }
        }

        public void DeleteTrans(User user)
        {
            try
            {
                //App.bdInterna.users.Remove(user);
                string json = JsonConvert.SerializeObject(App.bdInterna.users);
                WriteTrans(json);
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al eliminar usuario saliendo de sesion", ex);
            }
        }

        public MyOwnList<User> ReadTrans()
        {
            MyOwnList<User> Users = new MyOwnList<User>();
            try
            {
                string json = "";
                if (!File.Exists(NombreArchivo)) return Users;
                using (BinaryReader binaryReader =
                    new BinaryReader(File.Open(NombreArchivo, FileMode.Open)))
                {
                    json = binaryReader.ReadString();

                }
                if (!string.IsNullOrEmpty(json))
                {
                    App.logger.Info($"NO EXISTEN USUARIOS ACTUALES EN SISTEMA");
                    Users = JsonConvert.DeserializeObject<MyOwnList<User>>(json);
                }


            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al recuperar usuarios en sesion", ex);
            }


            return Users;
        }



        public void WriteTrans(string json)
        {
            if (File.Exists(NombreArchivo))
            {
                if (File.Exists(RespaldoArchivo))
                {
                    File.Delete(RespaldoArchivo);
                }
                File.Copy(NombreArchivo, RespaldoArchivo);
                File.Delete(NombreArchivo);
            }
            using (BinaryWriter binWriter =
                new BinaryWriter(File.Open(NombreArchivo, FileMode.Create)))
            {
                binWriter.Write(json);
            }
        }
    }
}
