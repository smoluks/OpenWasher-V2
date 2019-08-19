using IntelHexParser.Enums;
using System;
using System.Collections.Generic;

namespace Coshx.IntelHexParser
{
    /// <summary>
    /// Intel HEX to binary and binary to Intel Hex converter
    /// </summary>
    public class Serializer {
        private const int START_CODE_LENGTH = 1;
        private const int BYTE_COUNT_OFFSET = START_CODE_LENGTH;
        private const int BYTE_COUNT_LENGTH = 2;
        private const int ADDRESS_OFFSET = BYTE_COUNT_OFFSET + BYTE_COUNT_LENGTH;
        private const int ADDRESS_LENGTH = 4;
        private const int RECORD_TYPE_OFFSET = ADDRESS_OFFSET + ADDRESS_LENGTH;
        private const int RECORD_TYPE_LENGTH = 2;
        private const int DATA_OFFSET = RECORD_TYPE_OFFSET + RECORD_TYPE_LENGTH;
        private const int CHECKSUM_LENGTH = 2; 
        
        private int MinimumLineLength {
            get {
                return START_CODE_LENGTH + BYTE_COUNT_LENGTH + ADDRESS_LENGTH + RECORD_TYPE_LENGTH + CHECKSUM_LENGTH;
            }
        }
        
        private bool IsChecksumValid(string line)
        {
            byte checksum = 0;
            for (int i = 1; i < line.Length - 2; i += 2)
            {
                checksum += Convert.ToByte(line.Substring(i, 2), 16);
            }
            checksum = (byte)(~checksum + 1);

            return checksum == Convert.ToByte(line.Substring(line.Length - 2, 2), 16);
        }

        /*public String Serialize(byte[] source) {
            int i = 0, currentRecordIndex;
            Record[] records;
            String outcome;
            Record currentRecord;
            
            if (source.Length == 0) {
                return "";
            }

            records = new Record[source.Length / 255 + 2];
            currentRecord = null;
            currentRecordIndex = 0;
            
            for (i = 0; i < source.Length; i++) {
                if (i % 255 == 0) {
                    if (currentRecord != null) {
                        records[currentRecordIndex++] = currentRecord;
                    }
                    currentRecord = new Record();
                    currentRecord.Type = 0;
                    currentRecord.Data = new byte[255];
                    currentRecord.Address = (ushort) ((i / 255) * (256 + 12));
                    currentRecord.DataLength = 0;
                }
                
                currentRecord.Data[i % 255] = source[i];
                currentRecord.DataLength++;
            }
            
            if (currentRecord.DataLength > 0 && i % 255 != 0) {
                records[currentRecordIndex] = currentRecord;
            }

            var lastRecord = new Record();
            lastRecord.Type = 1;
            lastRecord.Address = 0;
            lastRecord.DataLength = 0;
            records[records.Length - 1] = lastRecord;
            
            outcome = "";
            i = 0;
            foreach (Record r in records) {
                outcome += r.ToString();
                if (i < records.Length - 1) {
                    outcome += Environment.NewLine;
                }
                i++;
            }
            
            return outcome;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public Dictionary<ushort, byte[]> Deserialize(IEnumerable<string> lines) {

            var records = new List<Record>();

            foreach (var line in lines)
            {
                //check line
                if (line.Length < MinimumLineLength)
                    throw new IntelHexParserException(IntelHexParserException.Kind.INVALID_LINE);
                if (line[0] != ':')
                    throw new IntelHexParserException(IntelHexParserException.Kind.MISSING_START_CODE);
                if (!IsChecksumValid(line))
                    throw new IntelHexParserException(IntelHexParserException.Kind.INVALID_CHECKSUM);

                //parce line and check
                var record = new Record();

                var typeNumber = Convert.ToInt16(line.Substring(RECORD_TYPE_OFFSET, RECORD_TYPE_LENGTH), 16);
                if (!Enum.IsDefined(typeof(LineType), typeNumber))
                {
                    throw new IntelHexParserException(IntelHexParserException.Kind.UNSUPPORTED_RECORD_TYPE);
                }
                record.Type = (LineType)typeNumber;

                record.DataLength = Convert.ToInt16(line.Substring(BYTE_COUNT_OFFSET, BYTE_COUNT_LENGTH), 16);
                record.Address = Convert.ToUInt16(line.Substring(ADDRESS_OFFSET, ADDRESS_LENGTH), 16);                

                var data = new byte[record.DataLength];
                for (int i = 0; i < record.DataLength; i++)
                {
                    data[i] = Convert.ToByte(line.Substring(DATA_OFFSET + 2 * i, 2), 16);
                }                 

                records.Add(record);
            }

            //calculate page sizes
            var sizes = new Dictionary<ushort, int>();
            ushort page = 0;
            foreach (var record in records)
            {
                if (record.Type ==  LineType.EndOfFile)
                   break;
                else switch (record.Type)
                {
                    case LineType.Data:
                        if (!sizes.ContainsKey(page))
                        {
                            sizes.Add(page, record.Address + record.DataLength);
                        }
                        else if (sizes[page] < record.Address + record.DataLength)
                        {
                            sizes[page] = record.Address + record.DataLength;
                        }
                        break;                    
                    case LineType.ExtendedLinearAddress:
                        page = (ushort)((record.Data[1] << 8) + record.Data[0]);
                        break;
                    case LineType.StartLinearAddress:
                        break;
                }                
            }

            //merge data
            var results = new Dictionary<ushort, byte[]>();
            foreach(var size in sizes)
            {
                results.Add(size.Key, new byte[size.Value]);
            }

            page = 0;
            foreach (var record in records)
            {
                if (record.Type == LineType.EndOfFile)
                    break;
                else switch (record.Type)
                    {
                        case LineType.Data:
                            record.Data.CopyTo(results[page], record.Address);
                            break;
                        case LineType.ExtendedLinearAddress:
                            page = (ushort)((record.Data[1] << 8) + record.Data[0]);
                            break;
                        case LineType.StartLinearAddress:
                            break;
                    }
            }

            return results;
        }
    }
}