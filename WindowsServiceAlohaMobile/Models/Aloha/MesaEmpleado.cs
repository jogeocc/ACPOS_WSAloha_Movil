using Aloha.SDK.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class MesaEmpleado
    {
        public bool IsTable { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int IdMesa { get; set; }

        [JsonIgnore]
        public int Guests { get; set; }
        public List<Check> Checks { get; set; } = new List<Check>();
        public bool IsHold { get; set; }
        public string IsHoldVERSION2 { get; set; }
    }
    [JsonObject]
    public class Check
    {
        public int Id { get; set; }
        public int ChceckNumber { get; set; }
        public double Amount { get; set; }
        public double AmountDue { get; set; }
        public double Tax { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Comp> Comps { get; set; } = new List<Comp>();

        [JsonIgnore]
        public int Guests { get; set; }
        public double TotalCheck { get; set; }
        public int NumCheck { get; set; }
    }
    [JsonObject]
    public class Comp
    {
        public int Id { get; set; }
        public int IdComp { get; set; }
        public double AmountDiscount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
    [JsonObject]
    public class Promotion
    {
        public int Id { get; set; }
        public int IdPromo { get; set; }
        public string Name { get; set; }
        public double AmountDiscount { get; set; }
    }
    [JsonObject]
    public class Item
    {
        public int Id { get; set; }
        public int IdEntry { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string DisplayPrice { get; set; }
        public bool Ordered { get; set; }
        public int OrderMode { get; set; }
        public int NivelMod { get; set; }
        public string SpecialMessage { get; set; } = "";
        public List<Item> Mods { get; set; } = new List<Item>();
        public int ModCode { get; set; }
        public string Modstring { get; set; }
        public bool IsAnulado
        {
            get
            {
                return
                    ModCode == (int)ModCodes.MOD_PRINTED_DELETED
                    ||
                    ModCode == (int)ModCodes.MOD_DELETED;
            }
        }

        //CAMPOS DE LA FECHA 10/05/2023
        //AUTO HOLD ORDER MODE
        public string HoldTime { get; set; } = "";
        public int HoldOrderMode { get; set; }

        //CAMPOS DE LA FECHA 09/10/2023
        public int NumSilla { get; set; }

    }
    [JsonObject]
    public class Payment
    {
        public int IdPayment { get; set; }
        public int IdTender { get; set; }
        public double Amount { get; set; }
        public double Tip { get; set; }
        public string LabelPayment { get; set; }
    }
}
