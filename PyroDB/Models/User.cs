using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace PyroDB.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string EncryptedPassword { get; set; }
        [NotMapped]
        public string Password { get => ""; set => SetPassword(value); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Salt { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public States State { get; set; }

        public void SetPassword(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            Salt = Guid.NewGuid().ToString();
            var saltedPassword = password + Salt;
            EncryptedPassword = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)).Base64Encode();
        }

        public bool CheckPassword(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            var saltedPassword = password + Salt;
            string testPassword = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)).Base64Encode();
            return testPassword == EncryptedPassword;
        }



        [Flags]
        public enum States
        {

            Unverified  = 1,
            Active      = 2,
            Blocked     = 4,
            
        }
    }
}
