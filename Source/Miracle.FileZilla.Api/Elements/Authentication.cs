using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Used during connect to FileZilla server.
    /// </summary>
    internal class Authentication : IBinarySerializable
    {
        byte[] _nonce1;
        byte[] _nonce2;

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        public void Deserialize(BinaryReader reader)
        {
            reader.Verify((byte)0);
            reader.ReadLongLength(r2 =>
            {
                _nonce1 = r2.ReadArray(r3 => r3.ReadByte());
                _nonce2 = r2.ReadArray(r3 => r3.ReadByte());
                return true;
            });
        }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        internal byte[] HashPassword(string password)
        {
            var cryptoServiceProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] dataToHash = _nonce1.Concat(Encoding.UTF8.GetBytes(password)).Concat(_nonce2).ToArray();
            var hash = cryptoServiceProvider.ComputeHash(dataToHash);
            return hash;
        }
    }
}