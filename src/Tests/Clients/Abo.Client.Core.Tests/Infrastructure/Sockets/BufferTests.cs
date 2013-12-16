using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abo.Server.Infrastructure.Protocol;
using NUnit.Framework;
using Abo.Utils;

namespace Abo.Client.Core.Tests.Infrastructure.Sockets
{
    [TestFixture]
    public class BufferTests
    {
        [Test(Description="")]
        public void ParsingTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var commandParser = new CommandParser();
            var buffer = new CommandBuffer(commandParser);

            var resultCommands = new List<Command>();
            buffer.CommandAssembled += resultCommands.Add;
            
            int maxBytes = 1024 * 4;

            var allBytes = new List<byte>();

            for (int i = 1; i < maxBytes; i++)
            {
                allBytes.AddRange(commandParser.ToBytes(CommandNames.Data, GetBytes(i)));
            }

            var chunks = allBytes.Chunk(256).ToList();
            for (int i = 0; i < chunks.Count; i++)
            {
                buffer.AppendBytes(chunks[i].ToArray());
            }

            Assert.AreEqual(maxBytes - 1, resultCommands.Count);
            Assert.AreEqual(1, resultCommands[0].Data.Length);
            Assert.AreEqual(3, resultCommands[2].Data.Length);
            Assert.AreEqual(maxBytes - 1, resultCommands[maxBytes - 2].Data.Length);

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
        }

        private static Random _random = new Random();

        [Test]
        public void ParsingShouldIgnoreCorruptedCommands()
        {
        }

        private byte[] GetBytes(int count)
        {
            return Enumerable.Repeat((byte) 1, count).ToArray();
        }
    }
}
