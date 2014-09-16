using System.IO;
using System.Text;

namespace Miracle.FileZilla.Api.Elements
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
        public string Password {get; set; }

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
        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            UserName = reader.ReadText();
            Password = reader.ReadText();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.WriteText(UserName);
            writer.WriteText(Password);
        }

        /// <summary>
        /// Compute a correctly formatted hashed password for use in User.Password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            var ret = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                ret.AppendFormat("{0:x2}", data[i]);
            return ret.ToString();
        }
    }
}