using System;

namespace Coshx.IntelHexParser {
    public class IntelHexParserException: Exception {
        public enum Kind {
            INVALID_LINE,
            MISSING_START_CODE,
            UNSUPPORTED_RECORD_TYPE,
            INVALID_CHECKSUM
        }
        
        public Kind MyKind { get; internal set; }
        
        internal IntelHexParserException(Kind kind): base(kind.ToString()) {
            this.MyKind = kind;
        }

    }
}