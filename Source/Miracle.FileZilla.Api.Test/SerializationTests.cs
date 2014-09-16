using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using Miracle.FileZilla.Api.Elements;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Miracle.FileZilla.Api.Test
{
    [TestFixture]
    public class SerializationTests
    {
        private void TestSerializationAndDeserialization<T>(Action<Fixture> customizeAction) where T : IBinarySerializable, new()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomDateSequenceGenerator());
            fixture.Customizations.Add(new RandomTimeSpanSequenceGenerator());
            customizeAction(fixture);

            var source = fixture.Create<T>();
            var target = new T();

            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memoryStream, Encoding.UTF8))
                {
                    source.Serialize(writer);
                    data = memoryStream.ToArray();
                }
            }
            
            HexDump(data);

            using (var memoryStream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    target.Deserialize(reader);
                }
            }

            var compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(source, target);

            //If different, write out the differences
            if (!result.AreEqual)
                Assert.Fail(result.DifferencesString);
        }

        private void TestSerializationAndDeserialization<T>() where T : IBinarySerializable, new()
        {
            TestSerializationAndDeserialization<T>(x => { });
        }


        [Test]
        public void PermissionTest()
        {
            TestSerializationAndDeserialization<Permission>();
        }

        [Test]
        public void AccountSettingsTest()
        {
            TestSerializationAndDeserialization<AccountSettings>(
                // Do not create Tristate.Default values 
                fixture => fixture.Customizations.Add(new RandomTriStateSequenceGenerator(TriState.No, TriState.Yes)));
        }

        [Test]
        public void GroupTest()
        {
            TestSerializationAndDeserialization<Group>(
                // Do not create Tristate.Default values
                fixture => fixture.Customizations.Add(new RandomTriStateSequenceGenerator(TriState.No, TriState.Yes)));
        }

        [Test]
        public void LimitTest()
        {
            TestSerializationAndDeserialization<Limit>();
        }

        [Test]
        public void SpeedLimitTest()
        {
            TestSerializationAndDeserialization<SpeedLimit>();
        }

        [Test]
        public void UserTest()
        {
            TestSerializationAndDeserialization<User>();
        }

        private void HexDump(byte[] data)
        {
            var hex = new StringBuilder(48);
            var ascii = new StringBuilder(16);
            int offset = 0;
            const int rowSize = 16;

            foreach (var b in data)
            {
                hex.AppendFormat("{0:X2} ", b);
                ascii.Append(b > 31 ? (char)b : '.');

                if (ascii.Length == rowSize)
                {
                    Debug.Print("{0:X4} : {1}{2} {0}", offset, hex, ascii);
                    hex.Clear();
                    ascii.Clear();
                    offset += rowSize;
                }
            }

            if (ascii.Length != 0)
            {
                while (hex.Length < 48) hex.Append(' ');
                while (ascii.Length < 16) ascii.Append(' ');
                Debug.Print("{0:X4} : {1}{2} {0}", offset, hex, ascii);
            }
        }
    }
}
