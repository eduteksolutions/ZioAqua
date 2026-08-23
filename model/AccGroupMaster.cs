using System.ComponentModel.DataAnnotations;

namespace zioAqua.model
{
    public class AccGroupMaster
    {
        [Key]
        public int Code { get; set; }

        public string AccGroupName { get; set; }

        public string MasterType { get; set; }

        public int BusinessId { get; set; }

        public string LoginName { get; set; }

        public DateTime LUserDt { get; set; }
    }
}
