using System.Text;

namespace Hallo.UnitTest.Builders
{
    public class DataBytesBuilder : ObjectBuilder<byte[]>
    {
        private string _text;
        private Encoding _encoding;

        public DataBytesBuilder()
        {
            _text = "DataBytesBuilder";
            _encoding = Encoding.UTF8;
        }

        public DataBytesBuilder WithText(string value)
        {
            _text = value;
            return this;
        }

        public DataBytesBuilder WithEncoding(Encoding value)
        {
            _encoding = value;
            return this;
        }

        public override byte[] Build()
        {
            return _encoding.GetBytes(_text);
        }
    }
}