using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Helpers;
using Microsoft.Win32.SafeHandles;

namespace AlohaLibrary.Infraestrutura
{
    public class BdContextoALH : IBdContextoALH, IDisposable
    {
        public OleDbConnection Conexion { get; private set; }
        public OleDbDataAdapter Adaptador { get; private set; }
        public BdContextoALH(string path)
        {
            try
            {
                //VFPOLEDB
                string cadenaConexion = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = {path};Extended Properties=dBASE IV;";
                Conexion = new OleDbConnection(cadenaConexion);
                Adaptador = new OleDbDataAdapter("", Conexion.ConnectionString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool _disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
                _safeHandle?.Dispose();
            }

            _disposed = true;
        }
    }
}
