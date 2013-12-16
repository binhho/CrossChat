using System;
using System.Collections.Generic;
using System.Diagnostics;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Infrastructure.Protocol;
using Abo.Server.Infrastructure.Serialization.Implementations;
using Abo.Utils.Extensions;
using NUnit.Framework;
using ProtoBuf;

namespace Abo.Server.Infrastructure.Tests.Serialization
{
    [TestFixture]
    public class ProtobufTests
    {
        [Test]
        public void TestInheritance()
        {
            var serializer = new ProtobufDtoSerializer();
            BaseClass bc = new SubClass2 {BaseInt = 1, Sub1Int = 2, Sub2Int = 3};

            var bcBytes = serializer.Serialize(bc);
            var deserialized = serializer.Deserialize<BaseClass>(bcBytes);

            Assert.IsInstanceOf(typeof (SubClass2), deserialized);
            Assert.AreEqual(6, deserialized.BaseInt + ((SubClass1)deserialized).Sub1Int + ((SubClass2)deserialized).Sub2Int);
        }

        private readonly Random _random = new Random();
        
        private string GenerateString(int minLen, int maxLen)
        {
            string chars = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars += chars.ToLower();

            int length = _random.Next(minLen, maxLen + 1);
            var strChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                strChars[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(strChars);
        }

        [Test]
        public void SizeTests()
        {
            var serializers = new IDtoSerializer[] 
                {
                    new ProtobufDtoSerializer(),
                    new ZippedProtobufDtoSerializer(),
                    new XmlDtoSerializer(),
                    new JsonDtoSerializer(),

                    new ProtobufDtoSerializer(),
                    new ZippedProtobufDtoSerializer(),
                    new XmlDtoSerializer(),
                    new JsonDtoSerializer(),
                };


            var players = new List<UserDto>();
            var messages = new List<PublicMessageDto>();


            for (int i = 0; i < 30; i++)
            {
                var message = new PublicMessageDto()
                {
                    AuthorName = GenerateString(5, 25),
                    Body = GenerateString(5, 120),
                    Role = (UserRoleEnum)_random.Next(0, 3),
                    Timestamp = DateTime.Now.AddHours(_random.Next(0, 1000))
                };

                messages.Add(message);
            }
            for (int i = 0; i < 450; i++)
            {
                var dto = new UserDto
                {
                    Age = _random.Next(13, 40),
                    Country = GenerateString(5, 15),
                    GamesCount = _random.Next(1, 5000),
                    Id = i,
                    IsDevoiced = _random.Next(0, 2) == 1,
                    Name = GenerateString(5, 25),
                    PhotoId = _random.Next(1, 15000),
                    Role = (UserRoleEnum)_random.Next(0, 3),
                    Sex = _random.Next(0, 2) == 1,
                    VictoriesCount = _random.Next(1, 2000),
                    Xp = _random.Next(1, 15000)
                };
                players.Add(dto);
            }

            string result = "";
            foreach (var serializer in serializers)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var playerBytes = serializer.Serialize(players);
                var msgBytes = serializer.Serialize(messages);
                sw.Stop();
                result += string.Format("{0}: {1} in {2} ms;\r\n", serializer.GetType().Name, StringExtensions.ToShortSizeInBytesString(playerBytes.Length /* 450*/ + msgBytes.Length /** 40*/), sw.ElapsedMilliseconds);
            }
        }
    }

    [ProtoContract]
    [ProtoInclude(500, typeof (SubClass1))]
    public class BaseClass
    {
        [ProtoMember(1)]
        public int BaseInt { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(1000, typeof(SubClass2))]
    public class SubClass1 : BaseClass
    {
        [ProtoMember(2)]
        public int Sub1Int { get; set; }
    }

    [ProtoContract]
    public class SubClass2 : SubClass1
    {
        [ProtoMember(3)]
        public int Sub2Int { get; set; }
    }
}