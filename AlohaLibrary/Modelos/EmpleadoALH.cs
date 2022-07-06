namespace AlohaLibrary.Modelos
{
    public class EmpleadoALH
    {
        public int ID { get; set; }
        //public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        //public int SEC_NUM { get; set; }
        //public int SSN { get; set; }
        //public string SSNTEXT { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string LASTNAME { get; set; }
        public string NICKNAME { get; set; }
        public string NombreCompleto
        {
            get
            {
                string valor = "";
                if (!string.IsNullOrEmpty(FIRSTNAME))
                {
                    valor += $"{FIRSTNAME} ";
                }

                if (!string.IsNullOrEmpty(MIDDLENAME))
                {
                    valor += $"{MIDDLENAME} ";
                }

                if (!string.IsNullOrEmpty(LASTNAME))
                {
                    valor += LASTNAME;
                }

                return valor.Trim();
            }
        }
    }
}
