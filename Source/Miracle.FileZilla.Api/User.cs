using System;
using System.IO;
using System.Text;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class representing a FileZilla user
    /// </summary>
    public class User : Group
    {
        /// <summary>
        /// Name of user
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Hashed password of user. Use User.HashPassword to set value from cleartext
        /// </summary>
        public string Password {get; private set; }
        /// <summary>
        /// Salt used to hash password.
        /// </summary>
        public string Salt { get; private set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public User()
        {
            Enabled = TriState.Default;
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public override void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            base.Deserialize(reader, protocolVersion, index);
            UserName = reader.ReadText();
            Password = reader.ReadText();
            if (protocolVersion >= ProtocolVersions.Sha512)
            {
                Salt = reader.ReadText();
            }
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public override void Serialize(BinaryWriter writer, int protocolVersion, int index)
        {
            base.Serialize(writer, protocolVersion, index);
            writer.WriteText(UserName);
            writer.WriteText(Password);
            if (protocolVersion >= ProtocolVersions.Sha512)
            {
                writer.WriteText(Salt);
            }
        }

        /// <summary>
        /// Assign a new password to the user. If protocolVersion requires it, also generate a salt.
        /// </summary>
        /// <param name="password">New passwordinary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="length">Optional parameter to limit the length of the generated password</param>
        public void AssignPassword(string password, int protocolVersion, int? length = null)
        {
            if (protocolVersion < ProtocolVersions.Sha512)
            {
                var pass = HashPasswordMd5(password);
                if (length.HasValue)
                {
                    Password = pass.Substring(0, length.Value);
                }
                Password = pass;
            }
            else
            {
                Salt = GenerateSalt();
                var pass = HashPasswordSha512(password, Salt);
                if (length.HasValue)
                {
                    Password = pass.Substring(0, length.Value);
                }
                Password = HashPasswordSha512(password, Salt);
            }
        }

        /// <summary>
        /// Compute a correctly formatted hashed password for use in User.Password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashPasswordSha512(string password, string salt)
        {
            var x = new System.Security.Cryptography.SHA512CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(password+salt);
            data = x.ComputeHash(data);
            var ret = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                ret.AppendFormat("{0:X2}", data[i]);
            return ret.ToString();
        }

        /// <summary>
        /// Compute a correctly formatted hashed password for use in User.Password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPasswordMd5(string password)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            var ret = new StringBuilder(32);
            for (int i = 0; i < data.Length; i++)
                ret.AppendFormat("{0:x2}", data[i]);
            return ret.ToString();
        }

        private const string ValidSaltChars = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        private const int SaltSize = 64;

        private static string GenerateSalt() 
        {
            var random = new Random();
            var salt = new StringBuilder(SaltSize); 

            for (var i = 0; i < SaltSize; i++)
            {
                salt.Append(ValidSaltChars[random.Next(ValidSaltChars.Length)]);
            }
            return salt.ToString();
        }
    }
}