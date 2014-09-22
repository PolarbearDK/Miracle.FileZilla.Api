using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Miracle.FileZilla.Api
{
    internal static class BinaryExtensions
    {
        public static ushort ReadBigEndianInt16(this BinaryReader reader)
        {
            return (ushort)(reader.ReadByte() << 8 | reader.ReadByte());
        }

        public static void WriteBigEndianInt16(this BinaryWriter writer, ushort value)
        {
            writer.Write((byte)(value >> 8));
            writer.Write((byte)(value & 0xFF));
        }

        public static int ReadBigEndianInt24(this BinaryReader reader)
        {
            var b1 = reader.ReadByte();
            var b2 = reader.ReadByte();
            var b3 = reader.ReadByte();
            return (int)(
                b1 << 16 |
                b2 << 8 |
                b3);
        }

        public static void WriteBigEndianInt24(this BinaryWriter writer, int value)
        {
            writer.Write((byte)((value >> 16) & 0xFF));
            writer.Write((byte)((value >> 8) & 0xFF));
            writer.Write((byte)(value & 0xFF));
        }

        public static uint ReadBigEndianInt32(this BinaryReader reader)
        {
            var b1 = reader.ReadByte();
            var b2 = reader.ReadByte();
            var b3 = reader.ReadByte();
            var b4 = reader.ReadByte();
            return (uint) (
                b1 << 24 |
                b2 << 16 |
                b3 << 8 |
                b4);
        }

        public static void WriteBigEndianInt32(this BinaryWriter writer, uint value)
        {
            writer.Write((byte)(value >> 24));
            writer.Write((byte)((value >> 16) & 0xFF));
            writer.Write((byte)((value >> 8) & 0xFF));
            writer.Write((byte)(value & 0xFF));
        }

        public static T[] ReadArray<T>(this byte[] data, int protocolVersion) where T : IBinarySerializable, new()
        {
            var list = new List<T>();
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                while (reader.BaseStream.Position != data.Length)
                {
                    list.Add(reader.Read<T>(protocolVersion));
                }
            }
            return list.ToArray();

        }

        public static T Read<T>(this byte[] data, int protocolVersion) where T : IBinarySerializable, new()
        {
            return data.Read(reader =>
            {
                var item = new T();
                item.Deserialize(reader, protocolVersion);
                return item;
            });
        }

        public static T Read<T>(this byte[] data, Func<BinaryReader,T> func)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var item = func(reader);
                if (reader.BaseStream.Position != data.Length)
                    throw new ProtocolException("Serialization error. More data available while deserializing type " + typeof(T).Name);
                return item;
            }
        }

        public static T Read<T>(this BinaryReader reader, int protocolVersion) where T : IBinarySerializable, new()
        {
            var item = new T();
            item.Deserialize(reader, protocolVersion);
            return item;
        }

        public static void Write<T>(this BinaryWriter writer, T item, int protocolVersion) where T : IBinarySerializable
        {
            if(item.GetType() != typeof(T))
                throw new ApiException(string.Format("Attempt to serialize type {0} as type {1}.", item.GetType(), typeof(T)));

            item.Serialize(writer, protocolVersion);
        }

        public static List<T> ReadList<T>(this BinaryReader reader, int protocolVersion) where T : IBinarySerializable, new()
        {
            int length = reader.ReadBigEndianInt16();
            var list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(reader.Read<T>(protocolVersion));
            }
            return list;
        }

        public static T[] ReadArray<T>(this BinaryReader reader, Func<BinaryReader, T> elementReader)
        {
            int length = reader.ReadBigEndianInt16();
            var list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(elementReader(reader));
            }
            return list.ToArray();
        }

        public static void WriteList<T>(this BinaryWriter writer, IList<T> list, int protocolVersion) where T : IBinarySerializable
        {
            if(list.Count > ushort.MaxValue)
                throw new ApiException(string.Format("Lists of more than {0} elements is not supported by the FileZilla API.", ushort.MaxValue));

            writer.WriteBigEndianInt16((ushort)list.Count);
            foreach (var item in list)
            {
                writer.Write(item, protocolVersion);
            }
        }

        public static string ReadText(this BinaryReader reader)
        {
            var howMany = reader.ReadBigEndianInt16();
            if (howMany <= 0) return null;
            var data = reader.ReadBytes(howMany);
            return Encoding.UTF8.GetString(data);
        }

        public static string ReadRemainingAsText(this BinaryReader reader)
        {
            byte[] textBytes = reader.ReadBytes((int) (reader.BaseStream.Length-reader.BaseStream.Position));
            return Encoding.UTF8.GetString(textBytes);
        }

        public static string ReadText24(this BinaryReader reader)
        {
            var howMany = reader.ReadBigEndianInt24();
            if (howMany <= 0) return null;
            var data = reader.ReadBytes(howMany);
            return Encoding.UTF8.GetString(data);
        }

        public static void WriteText24(this BinaryWriter writer, string text)
        {
            if (string.IsNullOrEmpty(text))
                writer.WriteBigEndianInt24(0);
            else
            {
                var data = Encoding.UTF8.GetBytes(text);
                writer.WriteBigEndianInt24((ushort) data.Length);
                writer.Write(data);
            }
        }

        public static void WriteText(this BinaryWriter writer, string text)
        {
            if (string.IsNullOrEmpty(text))
                writer.WriteBigEndianInt16(0);
            else
            {
                var data = Encoding.UTF8.GetBytes(text);
                writer.WriteBigEndianInt16((ushort) data.Length);
                writer.Write(data);
            }
        }

        public static List<string> ReadTextList(this BinaryReader reader)
        {
            var howMany = reader.ReadBigEndianInt16();
            var list = new List<string>();
            for (int i = 0; i < howMany; i++)
            {
                list.Add(reader.ReadText());
            }
            return list;
        }

        public static void WriteTextList(this BinaryWriter writer, IList<string> list)
        {
            writer.WriteBigEndianInt16((ushort)list.Count);
            foreach (var item in list)
            {
                writer.WriteText(item);
            }
        }

        public static DateTime? ReadDate(this BinaryReader reader)
        {
            var year = reader.ReadBigEndianInt16();
            var month = reader.ReadByte();
            var day = reader.ReadByte();

            if (year == 0 && month == 0 && day == 0)
                return null;

            return new DateTime(year, month, day);
        }

        public static void WriteDate(this BinaryWriter writer, DateTime? value)
        {
            if (value.HasValue)
            {
                writer.WriteBigEndianInt16((ushort)value.Value.Year);
                writer.Write((byte)value.Value.Month);
                writer.Write((byte)value.Value.Day);
            }
            else
            {
                writer.Write((int)0);
            }
        }

        public static TimeSpan? ReadTime(this BinaryReader reader)
        {
            var hour = reader.ReadByte();
            var minute = reader.ReadByte();
            var second = reader.ReadByte();

            if (hour == 0 && minute == 0 && second == 0)
                return null;

            return new TimeSpan(hour, minute, second);
        }

        public static void WriteTime(this BinaryWriter writer, TimeSpan? value)
        {
            if (value.HasValue)
            {
                writer.Write((byte)value.Value.Hours);
                writer.Write((byte)value.Value.Minutes);
                writer.Write((byte)value.Value.Seconds);
            }
            else
            {
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
            }
        }

        public static T ReadLength<T>(this BinaryReader reader, int length, Func<BinaryReader, T> action)
        {
            var pos = reader.BaseStream.Position;
            T item = length > 0 ? action(reader) : default(T);
            if (reader.BaseStream.Position - pos != length)
                throw new ProtocolException(string.Format("Serialization error. Length expected {0} actual {1}", reader.BaseStream.Position - pos, length));
            return item;
        }

        public static void WriteLength(this BinaryWriter writer, Action<BinaryWriter> action)
        {
            // Remember where length element is in stream
            var lengthPosition = writer.BaseStream.Position;
            writer.Write((int)0);

            // Serialize data
            action(writer);

            // Calculate length, reposition stream to position of length element and overwrite in buffer
            var endPosition = writer.BaseStream.Position;
            var length = (int)(endPosition - lengthPosition - sizeof(int));
            writer.BaseStream.Position = lengthPosition;
            writer.Write(length);

            // And move back to end of stream
            writer.BaseStream.Position = endPosition;
        }

        public static void Verify(this BinaryReader reader, string s)
        {
            Verify(reader, Encoding.ASCII.GetBytes(s));
        }

        public static void Verify(this BinaryReader reader, byte[] data)
        {
            foreach (var expectedByte in data)
            {
                Verify(reader, expectedByte);
            }
        }

        public static void Verify(this BinaryReader reader, byte expected)
        {
            var actual = reader.ReadByte();
            if (actual != expected)
                throw ProtocolException.Create(reader, expected, actual);
        }

        public static void Verify(this BinaryReader reader, MessageOrigin expectedMessageOrigin, MessageType expectedMessageType)
        {
            var expected = (byte)(((int)expectedMessageOrigin) | ((byte)expectedMessageType << 2));

            var actual = reader.ReadByte();
            if (actual != expected)
            {
                var actualMessageOrigin = (MessageOrigin) (actual & 0x3);
                var actualMessageType = (MessageType) (actual >> 2);
                throw new ProtocolException(string.Format("Expected message type/id {0}/{1} actual {2}/{3} at offset {4:x}({4})",
                    expectedMessageOrigin, expectedMessageType, 
                    actualMessageOrigin, actualMessageType,
                    reader.BaseStream.Position - 1));
            }
        }

        public static void Verify(this BinaryReader reader, ushort expected)
        {
            var actual = reader.ReadUInt16();
            if (actual != expected)
                throw ProtocolException.Create(reader, expected, actual);
        }

        public static void Verify(this BinaryReader reader, uint expected)
        {
            var actual = reader.ReadUInt32();
            if (actual != expected)
                throw ProtocolException.Create(reader, expected, actual);
        }
    }
}