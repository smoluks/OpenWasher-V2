using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntelHexParserTest")]

namespace Coshx.IntelHexParser {

    internal class Record {
        internal int Type { get; set; }
        internal int DataLength { get; set; }
        internal UInt16 Address { get; set; }
        internal byte[] Data { get; set; }
        
        internal byte Checksum {
            get {
                byte checksum;
                
                checksum = (byte) DataLength;
                checksum += (byte) Type;
                checksum += (byte) Address;
                checksum += (byte) ((Address & 0xFF00) >> 8);
                
                for (int i = 0; i < DataLength; i++) {
                    checksum += Data[i];
                }
                
                checksum = (byte) (~checksum + 1);
                
                return checksum;
            }
        }
        
        public override String ToString() {
            String outcome;

            // store in little endian, show in big endian
            byte[] addressBytes = BitConverter.GetBytes(Address);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(addressBytes);

            outcome = String.Format("{0}{1:X2}{2:X2}{3:X2}{4:X2}", ':', DataLength, addressBytes[0], addressBytes[1], Type);
            
            for (int i = 0; i < DataLength; i++) {
                outcome += String.Format("{0:X2}",  Data[i]);
            }
            
            outcome += String.Format("{0:X2}",  Checksum);
            
            return outcome;
        }
    } 
}