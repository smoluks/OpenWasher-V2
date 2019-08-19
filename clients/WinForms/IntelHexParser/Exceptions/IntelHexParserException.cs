using System;

namespace Coshx.IntelHexParser {

    /// <summary>
    /// Parcing exception
    /// </summary>
    public class IntelHexParserException: Exception {

        /// <summary>
        /// Exception cause enum
        /// </summary>
        public enum Kind {
            /// <summary>
            /// Bad line size
            /// </summary>
            INVALID_LINE,
            /// <summary>
            /// Not found ':'
            /// </summary>
            MISSING_START_CODE,
            /// <summary>
            /// Unknown type
            /// </summary>
            UNSUPPORTED_RECORD_TYPE,
            /// <summary>
            /// Bad checksum
            /// </summary>
            INVALID_CHECKSUM
        }

        /// <summary>
        /// Exception cause
        /// </summary>
        public Kind MyKind { get; internal set; }
        
        internal IntelHexParserException(Kind kind): base(kind.ToString()) {
            this.MyKind = kind;
        }

    }
}