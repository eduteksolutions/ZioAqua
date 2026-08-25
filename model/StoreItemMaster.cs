using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
   
          public class tblStoreItemMast
    {
        [Key]
        public int ICodeNum { get; set; }

        public string ICodeStr { get; set; }

        public string IName { get; set; }

        public string IShortName { get; set; }

        public int IGrpCd { get; set; }

        public int IUom { get; set; }

        public int ISaleTax { get; set; }

        public decimal IPrate { get; set; }

        public decimal IMrp { get; set; }

        public decimal IStock { get; set; }

        public int IMargin { get; set; }

        public int Openingstock { get; set; }

        public string Openingstatus { get; set; }

        public string? LoginName { get; set; }

        public DateTime LUserDt { get; set; }

        public int BusinessId { get; set; }
    }
}

