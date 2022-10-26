using Aloha.SDK.Common;
using AlohaWebServiceMobile.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class MesaEmpleado
    {
        public bool IsTable { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public List<Check> Checks { get; set; } = new List<Check>();
    }
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
    }

    public class Comp
    {
        public int Id { get; set; }
        public int IdComp { get; set; }
        public double AmountDiscount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }

    public class Promotion
    {
        public int Id { get; set; }
        public int IdPromo { get; set; }
        public string Name { get; set; }
        public double AmountDiscount { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public int IdEntry { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string DisplayPrice { get; set; }
        public bool Ordered { get; set; }
        public int Ordered1 { get; set; }
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
    }
    public class Payment
    {
        public int IdPayment { get; set; }
        public int IdTender { get; set; }
        public double Amount { get; set; }
        public double Tip { get; set; }
    }
}
