using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hallo.Util;
using NUnit.Framework;


namespace SippermanServer.UnitTests
{
    [TestFixture]
    public class BufferReaderTests
    {
        private BufferReader _reader;
        private Byte[] _buffer;
        private string _word;

        [Test]
        public void ReadHannes_ConsumeH_ExpectIndexToBeOne()
        {
            CreateBufferReader("hannes");
            _reader.Consume('h');
            _reader.Index.Should().Be(1);
        }

        [Test]
        public void ReadHannes_ConsumeH_ExpectCurrentToA()
        {
            CreateBufferReader("hannes");
            _reader.Consume('h');
            _reader.Current.Should().Be('a');
        }

        [Test]
        public void ReadHannes_ConsumeH_Expect_Current_ToBeA()
        {
            CreateBufferReader("hannes");
            _reader.Consume('h');
            _reader.Current.Should().Be('a');
        }

        [Test]
        public void ReadHannes_ConsumeL_ExpectTheCurrentRemainsH()
        {
            CreateBufferReader("hannes");
            _reader.Consume('l');
            _reader.Current.Should().Be('h');
        }

        [Test]
        public void ReadHannes_ConsumeHAN_ExpectTheCurrentToBeN()
        {
            CreateBufferReader("hannes");
            _reader.Consume('h', 'a', 'n');
            _reader.Current.Should().Be('n');
        }

        [Test]
        public void ReadHannes_Peek_ExpectThePeekedCharIsAAndCurrentRemainesH()
        {
            //demonstrates that 'Peek' property gets the next char without changing the Current
            CreateBufferReader("Hannes");
            var peeked = _reader.Peek;
            _reader.Current.Should().Be('H');
            peeked.Should().Be('a');
            _reader.Current.Should().Be('H');
        }

        [Test]
        public void ReadH_ContainsH_ExpectItContainsH()
        {
            CreateBufferReader("H");
            var contains = _reader.Contains('H').Should().BeTrue();
        }

        [Test]
        public void ReadH_ContainsL_ExpectItDontContainsH()
        {
            CreateBufferReader("H");
            var contains = _reader.Contains('L').Should().BeFalse();
        }

        [Test]
        public void ReadHannes_L_ContainsL_ExpectItContainsL()
        {
            CreateBufferReader("Hannes L");
            var contains = _reader.Contains('L').Should().BeTrue();
        }

        private void CreateBufferReader(string word)
        {
             _word = word;
            _buffer = EncodingUtil.GetUtf8Bytes(_word);
            _reader = new BufferReader(_buffer, Encoding.UTF8);
        }
    }
}
