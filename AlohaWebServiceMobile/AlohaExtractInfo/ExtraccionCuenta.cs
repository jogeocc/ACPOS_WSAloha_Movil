using Aloha.SDK.Common;
using AlohaWebServiceMobile.Models.Aloha;
using AlohaWebServiceMobile.Models.SAP;
using LasaFOHLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketGenerateAloha.Models;

namespace AlohaWebServiceMobile.AlohaExtractInfo
{
    public class ExtraccionCuenta
    {
        public List<DetallePago> MonitoreoCuenta(int IdCheck)
        {
            List<DetallePago> ListaPagosSocios = new List<DetallePago>();
            try
            {
                IIberFuncs23 xfuncs23 = AlohaSdkFactory.GetIberFuncs23Instance();
                bool IsIberTS = xfuncs23.IsTableService();
                App.logger.Info($"IBER ES: {(IsIberTS ? "TABLE" : "QS")}");
                if (IsIberTS)
                {
                    try
                    {

                        IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
                        IberObject cheque = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, IdCheck).First();
                        double pagoTotal = cheque.GetDoubleVal("COMPLETETOTAL");
                        IberEnum pagos = cheque.GetEnum((int)COMEnums.INTERNAL_CHECKS_PAYMENTS);
                        IberObject pago = pagos.First();
                        for (int k = 0; k < pagos.Count; k += 1)
                        {

                            DetallePago detallePago = new DetallePago();
                            detallePago.CardCode = pago.GetStringVal("IDENT");
                            detallePago.Amount = pago.GetDoubleVal("AMOUNT");
                            pagoTotal -= detallePago.Amount;
                            if (pagoTotal <= 0)
                            {
                                detallePago.Amount += pagoTotal;
                            }
                            ListaPagosSocios.Add(detallePago);
                            if (k != pagos.Count - 1) pago = pagos.Next();
                        }
                    }
                    catch (Exception ex)
                    {
                        App.logger.Error("error al rastrear info", ex);
                    }

                }
                else
                {
                    App.logger.Info($"IBER ES: {(IsIberTS ? "TABLE" : "QS")}");

                    //check = IberQs(requestCheck);
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error Identificando servicio", ex);
            }

            return ListaPagosSocios;
        }

    }
}
