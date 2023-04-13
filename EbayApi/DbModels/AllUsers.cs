using System.ComponentModel.DataAnnotations;

namespace EbayApi.DbModels
{
    public class AllUsers
    {
        [Key]
        public int UID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }

    }
}
