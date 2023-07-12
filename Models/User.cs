using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{

    public enum Role
    {
        ADMIN,
        USER
    }

    public class User
    {
        [Key]
        public int id { get; set; }

        [DataType(DataType.Text)]
        public string firstName { get; set; }

        [DataType(DataType.Text)]
        public string lastName { get; set; }
        public bool gender { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }
        public Role? role { get; set; } = Role.USER;

        [DataType(DataType.DateTime)]
        public DateTime dateOfBirth { get; set; }

        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }
    }
}
