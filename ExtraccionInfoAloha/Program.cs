using System;
using Newtonsoft.Json;
using Aloha.SDK.Common;
using LasaFOHLib;

namespace ExtraccionInfoAloha
{
    class Program
    {
        static void Main(string[] args)
        {
            AlohaConnection alohaConnection = new AlohaConnection();

            Console.WriteLine("=== CONSULTA DE INFORMACIÓN DE MESA ===");

            // Solicitar ID de mesa para monitoreo
            Console.Write("Ingrese el ID de la mesa: ");
            int idMesa;
            while (!int.TryParse(Console.ReadLine(), out idMesa))
            {
                Console.Write("ID inválido. Ingrese nuevamente el ID de la mesa: ");
            }

            var infoMesa = alohaConnection.MonitoreoCuenta(idMesa);
            Console.WriteLine("Resultado de MonitoreoCuenta:");
            Console.WriteLine(JsonConvert.SerializeObject(infoMesa, Formatting.Indented));

            Console.WriteLine("\n=== CONSULTA DE CHEQUE CERRADO ===");

            // Solicitar ID de empleado
            Console.Write("Ingrese el ID del empleado: ");
            int idEmpleado;
            while (!int.TryParse(Console.ReadLine(), out idEmpleado))
            {
                Console.Write("ID inválido. Ingrese nuevamente el ID del empleado: ");
            }

            // Solicitar ID de la mesa (de nuevo, ya que puede ser diferente)
            Console.Write("Ingrese nuevamente el ID de la mesa (para la consulta de cheque cerrado): ");
            while (!int.TryParse(Console.ReadLine(), out idMesa))
            {
                Console.Write("ID inválido. Ingrese nuevamente el ID de la mesa: ");
            }

            // Solicitar ID del cheque
            Console.Write("Ingrese el ID del cheque: ");
            int idCheque;
            while (!int.TryParse(Console.ReadLine(), out idCheque))
            {
                Console.Write("ID inválido. Ingrese nuevamente el ID del cheque: ");
            }

            var chequeCerrado = alohaConnection.RecuperarChequeCerrado(idEmpleado, idMesa, idCheque);
            Console.WriteLine("Resultado de RecuperarChequeCerrado:");
            Console.WriteLine(JsonConvert.SerializeObject(chequeCerrado, Formatting.Indented));

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
