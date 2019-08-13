using System;
using System.Collections.Generic;
using System.Linq;

namespace Coshx.IntelHexParser {
    public class Serializer {
        private const int START_CODE_OFFSET = 0;
        private const int START_CODE_LENGTH = 1;
        private const int BYTE_COUNT_OFFSET = START_CODE_OFFSET + START_CODE_LENGTH;
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
        
        private bool IsChecksumValid(String line, Record currentRecord) {
            byte current = Convert.ToByte(line.Substring(DATA_OFFSET + currentRecord.DataLength * 2, CHECKSUM_LENGTH), 16);
            
            return currentRecord.Checksum == current;
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

        public String Serialize(byte[] source) {
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public Dictionary<ushort, byte[]> Deserialize(IEnumerable<string> lines) {

            var records = new List<Record>();

            foreach (var line in lines)
            {
                if (line.Length < MinimumLineLength)
                    throw new IntelHexParserException(IntelHexParserException.Kind.INVALID_LINE);
                if (line[0] != ':')
                    throw new IntelHexParserException(IntelHexParserException.Kind.MISSING_START_CODE);
                if (!IsChecksumValid(line))
                    throw new IntelHexParserException(IntelHexParserException.Kind.INVALID_CHECKSUM);

                var dataLength = Convert.ToInt16(line.Substring(BYTE_COUNT_OFFSET, BYTE_COUNT_LENGTH), 16);
                var address = Convert.ToUInt16(line.Substring(ADDRESS_OFFSET, ADDRESS_LENGTH), 16);
                var type = Convert.ToInt16(line.Substring(RECORD_TYPE_OFFSET, RECORD_TYPE_LENGTH), 16);
                var data = new byte[dataLength];
                for (int i = 0; i < dataLength; i++)
                {
                    data[i] = Convert.ToByte(line.Substring(DATA_OFFSET + 2 * i, 2), 16);
                }

                if (type != 0 && type != 1 && type != 4 && type != 5)
                    throw new IntelHexParserException(IntelHexParserException.Kind.UNSUPPORTED_RECORD_TYPE);

                records.Add(new Record()
                {
                    DataLength = dataLength,
                    Address = address,
                    Type = type,
                    Data = data
                });
            }

            var sizes = new Dictionary<ushort, int>();
            ushort page = 0;
            foreach (var record in records)
            {
                if(record.Type == 5)
                {
                    continue;
                }
                else if (record.Type == 4)
                {
                    page = (ushort)((record.Data[1] << 8) + record.Data[0]);
                    continue;
                }
                else if (record.Type == 1)
                    break;

                if (!sizes.ContainsKey(page))
                {
                    sizes.Add(page, record.Address + record.DataLength);
                }
                else if (sizes[page] < record.Address + record.DataLength)
                    sizes[page] = record.Address + record.DataLength;
            }

            var results = new Dictionary<ushort, byte[]>();
            foreach(var size in sizes)
            {
                results.Add(size.Key, new byte[size.Value]);
            }

            page = 0;
            foreach (var record in records)
            {
                if (record.Type == 5)
                {
                    continue;
                }
                else if (record.Type == 4)
                {
                    page = (ushort)((record.Data[1] << 8) + record.Data[0]);
                    continue;
                }
                else if (record.Type == 1)
                    break;

                record.Data.CopyTo(results[page], record.Address);
            }

            return results;
        }
    }
}