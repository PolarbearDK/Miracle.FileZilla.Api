using System;
using System.IO;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Miracle.FileZilla.Api.Test
{
    [TestFixture]
    public class SerializationTests
    {
        public const int ProtocolVersion = 0x00010F00;

        private void TestSerializationAndDeserialization<T>(Action<Fixture> customizeAction) where T : IBinarySerializable, new()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomDateSequenceGenerator());
            fixture.Customizations.Add(new RandomTimeSpanSequenceGenerator());
            customizeAction(fixture);

            var source = fixture.Create<T>();
            var target = new T();

            var memoryStream = new MemoryStream();
            using (var writer = new BinaryWriter(memoryStream, Encoding.UTF8))
            {
                source.Serialize(writer, ProtocolVersion);
            }
            byte[] data = memoryStream.ToArray();
            
            Hex.Dump(Console.Out, data);

            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                target.Deserialize(reader, ProtocolVersion);
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
        public void SharedFolderTest()
        {
            TestSerializationAndDeserialization<SharedFolder>();
        }

        [Test]
        public void SettingsTest()
        {
            TestSerializationAndDeserialization<Settings>(
                // Do not create Tristate.Default values 
                fixture => fixture.Customizations.Add(new RandomOptionGenerator()));
        }

        [Test]
        public void SpeedLimitTest()
        {
            TestSerializationAndDeserialization<SpeedLimit>();
        }

        [Test]
        public void SpeedLimitRuleTest()
        {
            TestSerializationAndDeserialization<SpeedLimitRule>();
        }

        [Test]
        public void UserTest()
        {
            TestSerializationAndDeserialization<User>();
        }
    }
}
