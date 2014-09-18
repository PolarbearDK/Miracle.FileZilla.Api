using System;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    internal class Error: IBinarySerializable
    {
        public string ErrorMessage { get; set; }

        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            reader.Verify((byte)1);
            ErrorMessage = reader.ReadText();
        }

        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            throw new NotImplementedException();
        }
    }
}
