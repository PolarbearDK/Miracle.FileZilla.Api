using System;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    internal class Error: IBinarySerializable
    {
        public string ErrorMessage { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            reader.Verify((byte)1);
            ErrorMessage = reader.ReadText();
        }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
