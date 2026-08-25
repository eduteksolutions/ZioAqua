using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
  
        public class tblStoreItemGrpMast
    {
        [Key]
        public int IGrpCd { get; set; }

            public string IGrpName { get; set; }

            public int IRackCd { get; set; }

            public string IGrpDescr { get; set; }

            public string LoginName { get; set; }

            public DateTime LUserDt { get; set; }

            public int BusinessId { get; set; }
        
    }
}
