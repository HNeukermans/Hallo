using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Hannes.Net.Sip;
using System.IO;

namespace Hannes.Net.Parsers
{
    

    public class SipMessageParser : ISipMessageParser
    {
        
        public SipMessageParser()
        { }

        public SipMessage ParseSipMessage(byte[] dataBytes, string messageContent)
        { 
            //Contract.Requires<ArgumentNullException>(dataBytes != null);
            byte[] bodyContent;

            SeparateBodyFromMessage(dataBytes, out messageContent, out bodyContent);

            List<string> lines = ParseMessageLines(messageContent);

            if (lines.Count == 0) throw new SipParseException(ExceptionMessage.InvalidFormat);

            SipMessage sipMessage = ProcessFirstLine(lines[0]);

            lines.RemoveAt(0);

            ProcessHeaderLines(lines, sipMessage);

            //if(sipMessage.ContentLength.Value > 0)
            //{
                
            //}
                                    
            return sipMessage;
        }

        private void SeparateBodyFromMessage(byte[] dataBytes, out string messageContent, out byte[] bodyContent)
        {
            messageContent = null;
            bodyContent = null;
            int i = 0;
            try
            {
                while (!(dataBytes[i] == '\r' &&
                            dataBytes[i + 1] == '\n' &&
                                dataBytes[i+2] == '\r' &&
                                    dataBytes[i + 3] == '\n')) i++;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new SipParseException(ExceptionMessage.InvalidFormat);
            }

            messageContent = Encoding.UTF8.GetString(dataBytes, 0, i);

            var bodyLenght = dataBytes.Length - (i+4);

            if ( bodyLenght > 0)
            {
                 bodyContent = new byte[bodyLenght];
                Array.Copy(dataBytes, i + 4, bodyContent, 0, bodyLenght);
            }                     
        }

        internal List<string> ParseFirstLineAndHeaders(byte[] dataBytes, out int c)
        {
            var lines = new List<string>(); 

            string line = "";

            c = 0;

            do
            {
                //bool isFoldedLine = false;

                //if (lines.Count > 0)
                //{
                //    if (dataBytes[c] == '\t' || dataBytes[c] == ' ') isFoldedLine = true;
                //}                 
                    
                int j = FindIndexLastNoCrOrLfChar(c, dataBytes);
                                    
                int lenght = (j - c) + 1;
                                    
                line = Encoding.UTF8.GetString(dataBytes, c, lenght);

                    //if (!isFoldedLine)
                    //{
                        lines.Add(line);
                    //}
                    //else
                    //{
                    //    string unfolded = lines[lines.Count - 1] + line;
                    //    lines[lines.Count - 1] = unfolded;
                    //}

                    c = FindIndexFirstCharAfterCrLf(j + 1, dataBytes);

                    if (c == dataBytes.Length) break;
            }
            while (line != string.Empty);

            return lines;
        }

        private void ProcessHeaderLines(List<string> headerLines, SipMessage sipMessage)
        {
            //var factory = new SipParserFactory();
            //var headers = new List<SipHeaderBase>();
            //foreach (var headerLine in headerLines) 
            //{
            //    var parser = factory.CreateHeaderParser(ParseHeaderName(headerLine));
            //    SipHeaderBase sipHeader = parser.Parse(new Util.StringReader(headerLine));
            //    headers.Add(sipHeader);
            //}
           // sipMessage.SetHeaders(headers); 
        }

        private string ParseHeaderName(string headerLine)
        {
            var sr = new Util.StringReader(headerLine);
            var name = sr.QuotedReadToDelimiter(':');
            if(name == null) throw new SipParseException(ExceptionMessage.InvalidHeaderName);
            return name.Trim();
        }

        private List<string> ParseMessageLines(string messageContent)
        {
            List<string> lines = new List<string>();

            using (StringReader r = new StringReader(messageContent))
            {
                while (r.Peek() > -1)
                {
                    string newLine = r.ReadLine();
                    if (newLine == string.Empty) break;

                    if (lines.Count > 0 && (newLine[0] == ' ' || newLine[0] == '\t'))
                    {
                        //the header line is folded. => Unfold.
                        lines[lines.Count - 1] = lines[lines.Count - 1] + " " + newLine.Substring(1);
                    }

                    lines.Add(newLine);
                }
            }

            return lines;
        }

        private List<string> ParseMessageLines()
        {
            throw new NotImplementedException();
        }

        private SipMessage ProcessFirstLine(string firstLine)
        {
            if (firstLine.EndsWith(SipConstants.SipTwoZeroString))
            {
                var message = new SipRequest();                    
                SipRequestLine requestLine = new SipRequestLineParser().Parse(firstLine);
                message.RequestLine = requestLine;
                return message;
            }
            else if (firstLine.StartsWith(SipConstants.SipTwoZeroString))
            {
                var message = new SipResponse();
                var statusLine = new SipStatusLineParser().Parse(firstLine);
                message.StatusLine = statusLine;
                return message;
            }

            throw new SipParseException(ExceptionMessage.InvalidFirstLineFormat);
        }

        private int FindIndexFirstCharAfterCrLf(int startIndex, byte[] dataBytes)
        {
            int i = startIndex;

            while (i < dataBytes.Length && (dataBytes[i] == '\r' || dataBytes[i] == '\n')) i++;
                           
            return i++;                
        }

        private int ReverseFindFirstNonControlChar(int startIndex, byte[] dataBytes)
        {
            int i = startIndex;
            while (dataBytes[i] < 0x20) i--;
            return i;
        }

        private int FindIndexFirstNonControlChar(int startIndex, byte[] dataBytes)
        {
            int i = startIndex;

            // Squeeze out any leading control character.
            while (dataBytes[i] < 0x20) i++;

            return i;
        }

        private int FindIndexLastNoCrOrLfChar(int startIndex, byte[] dataBytes)
        {
            int i = startIndex;
                           
            while (dataBytes[i] != '\r' && dataBytes[i] != '\n') i++;

            return i-1;            
        }
    }
}
