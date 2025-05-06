using Aloha.SDK.Common;
using LasaFOHLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraccionInfoAloha
{
    public class AlohaConnection
    {

        private SdkFunctions _sdkFunctions = new SdkFunctions();

        private IIberFuncs23 xFunction;
        private IIberDepot depot;
        private SdkFunctions SdkFunctions;


        public object MonitoreoCuenta(int IdTable)
        {
            int isBloqueada = 0;
            long IdTerm = 0;
            try
            {
                IIberFuncs23 xfuncs23 = AlohaSdkFactory.GetIberFuncs23Instance();
                bool IsIberTS = xfuncs23.IsTableService(); // FUNCION PARA SABER SI ES TABLE SERVICE o QUICK SERVICE
                Console.WriteLine($"IBER ES: {(IsIberTS ? "TABLE" : "QS")}");
                if (IsIberTS)
                {
                    try
                    {

                        IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance(); //INSTANCIAMOS EL DEPOT PARA LA BUSQUEDA DEL ELEMENTO

                        //AL SER UNA TABLA DE ACCESO RAPIDO PODEMOS USAR LA FUNCION "FindObjectFromId", IMPORTANTE LEER EN LA DOCUMENTACION EN LA PAG 32, PARA SABER QUE TABLAS SE ACCEDEN DE MANERA RAPIDA

                        IberObject table = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, IdTable).First(); //RECUPERAMOS LA MESA A TRAVES DE SU ID UNICO
                        
                        //ESTE NOS RETORNA EL OBJETO cheque,QUE CONTIENE LA INFORMACION DE SOLO ESE CHEQUE

                        //PARA SABER LA INFORMACION DE QUE PODEMOS EXTRAER LEER LA PAGINA 246-247  


                        isBloqueada = table.GetBoolVal("LOCKED");
                        IdTerm = table.GetLongVal("LOCKTERMINAL");
                        
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("error al rastrear info", ex);
                    }

                }
                else
                {
                    Console.WriteLine($"IBER ES: {(IsIberTS ? "TABLE" : "QS")}");

                    //check = IberQs(requestCheck);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Identificando servicio", ex);
            }

            var demo = new { 
            
                IdTable = IdTable,
                IsLock = isBloqueada==1, 
                IdTermLock = IdTerm

            };


            return demo;

        }



    }
}
