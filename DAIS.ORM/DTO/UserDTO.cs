using DAIS.ORM.Framework;
using System.Text;

namespace DAIS.ORM.DTO
{
    [TableName("user")]
    public class UserDTO
    {
        [Column("id", ColumnType.PrimaryKey)]
        public long Id { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("password")]
        public byte[] Password { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("show_email")]
        public bool ShowEmail { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("deleted", DeleteIndicator = true)]
        public bool IsDeleted { get; set; }

        public string PasswordString
        {
            get { return Encoding.ASCII.GetString(Password); }
            set { Password = Encoding.ASCII.GetBytes(value); }
        }
    }
}
