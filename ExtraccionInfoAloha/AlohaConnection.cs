using Aloha.SDK.Common;
using LasaFOHLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

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





        public object RecuperarChequeCerrado(int EmployeeId, int TableId, int CheckId)
        {
            dynamic check = new ExpandoObject();
            try
            {
                check.Id = CheckId;
                check.Items = new List<dynamic>();
                check.Payments = new List<dynamic>();
                check.Promotions = new List<dynamic>();
                check.Comps = new List<dynamic>();

                IIberFuncs23 xfuncs23 = AlohaSdkFactory.GetIberFuncs23Instance();
                 xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                bool IsIberTS = xfuncs23.IsTableService(); // FUNCION PARA SABER SI ES TABLE SERVICE o QUICK SERVICE
                SdkFunctions = new SdkFunctions();
                Console.WriteLine($"IBER ES: {(IsIberTS ? "TABLE" : "QS")}");

                IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();

                IberEnum enumEmpleado = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, EmployeeId);
                IberObject empleado = enumEmpleado.First();

                if (empleado == null)
                    throw new Exception("Empleado no encontrado con ID: " + EmployeeId);

                IberEnum chequesCerradosEnum = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, CheckId);
                IberObject ChequeCerrado = chequesCerradosEnum.First();

                while (ChequeCerrado != null && ChequeCerrado.GetLongVal("ID") != CheckId)
                    ChequeCerrado = chequesCerradosEnum.Next();

                if (ChequeCerrado == null)
                    throw new Exception("Cheque cerrado no encontrado con ID: " + CheckId);

                IberEnum MesasCerradasEnum = empleado.GetEnum((int)COMEnums.INTERNAL_EMP_CLOSED_TABLES);
                IberObject MesaCerrada = MesasCerradasEnum.First();

                while (MesaCerrada != null && MesaCerrada.GetLongVal("ID") != TableId)
                    MesaCerrada = MesasCerradasEnum.Next();

                if (MesaCerrada == null)
                    throw new Exception("Mesa cerrada no encontrada con ID: " + TableId);

             
                xFunction.GetCheckTotal(CheckId, out double subTotal, out double tax);
                check.Amount = subTotal;
                check.Tax = tax;
                check.IdRev = ChequeCerrado.GetLongVal("REV_ID");
                check.NumMesa = MesaCerrada.GetLongVal("TABLEDEF_ID") + "";
                check.NumSeats = 0;

                try
                {
                    IberEnum ItemsEmpleado = ChequeCerrado.GetEnum((int)COMEnums.INTERNAL_CHECKS_ENTRIES);
                    IberObject ItemAbierto = ItemsEmpleado.First();
                    int IdPadre = 0;

                    while (ItemAbierto != null)
                    {
                        dynamic item = new ExpandoObject();
                        item.Id = ItemAbierto.GetLongVal("DATA");
                        item.IdEntry = ItemAbierto.GetLongVal("ID");
                        item.Name = ItemAbierto.GetStringVal("DISP_NAME");
                        item.Price = ItemAbierto.GetDoubleVal("PRICE");
                        item.DisplayPrice = ItemAbierto.GetStringVal("DISP_PRICE").Trim();
                        item.ModCode = ItemAbierto.GetLongVal("MOD_CODE");
                        item.NivelMod = ItemAbierto.GetLongVal("LEVEL");
                        int IsMessage = ItemAbierto.GetLongVal("TYPE");
                        item.Ordered = ItemAbierto.GetBoolVal("SELECTED") > 0;
                        item.OrderMode = ItemAbierto.GetLongVal("MODE");
                        item.Modstring = ItemAbierto.GetStringVal("MOD_STRING");
                        item.NumSilla = ItemAbierto.GetLongVal("SEAT");
                        item.Mods = new List<dynamic>();

                        if (item.ModCode == 8 || item.ModCode == 12) {
                            continue;
                        }


                        if (IsMessage == 0)
                        {
                            if (item.NivelMod == 0)
                            {
                                IdPadre = item.IdEntry;
                                check.Items.Add(item);
                            }
                            else
                            {
                                dynamic padre = null;
                                foreach (var i in check.Items)
                                {
                                    if (i.IdEntry == IdPadre)
                                    {
                                        padre = i;
                                        break;
                                    }
                                }
                                if (padre != null)
                                {
                                    padre.Mods.Add(item);
                                }
                            }
                        }
                        else
                        {
                            dynamic padre = null;
                            foreach (var i in check.Items)
                            {
                                if (i.IdEntry == IdPadre)
                                {
                                    padre = i;
                                    break;
                                }
                            }
                            if (padre != null)
                            {
                                padre.SpecialMessage = item.Name;
                            }
                        }

                        ItemAbierto = ItemsEmpleado.Next();
                    }
                }
                catch (Exception exItems)
                {
                    Console.WriteLine($"[ERROR] al recuperar items del cheque {CheckId}: {exItems.Message}");
                    Console.WriteLine($"[TRACE] {exItems.StackTrace}");
                }

                double AmountPayed = 0;
                foreach (var p in check.Payments)
                {
                    AmountPayed += (double)p.Amount;
                }

                check.AmountDue = ChequeCerrado.GetDoubleVal("COMPLETETOTAL") - AmountPayed;
                check.Guests = ChequeCerrado.GetLongVal("GUESTS");
                check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                check.NumCheck = ChequeCerrado.GetLongVal("NUMBER") + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] al recuperar la cuenta cerrada {CheckId}: {ex.Message}");
                Console.WriteLine($"[TRACE] {ex.StackTrace}");
                return null;
            }

            return check;
        }

    }
}
