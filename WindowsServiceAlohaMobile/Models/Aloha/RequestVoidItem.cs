using System.Collections.Generic;


namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class RequestVoidItem
    {
        public int IdTerm { get; set; }
        public int IdEmpleado { get; set; }
        public int IdCheck { get; set; }
        public List<ItemAnulado> ItemAnulados { get; set; } = new List<ItemAnulado>();
        public int IdVoidReason { get; set; }
    }
    public class ItemAnulado
    {
        public int IdEntry { get; set; }
    }
}
