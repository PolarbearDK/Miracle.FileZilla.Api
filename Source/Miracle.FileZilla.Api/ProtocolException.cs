using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Exception thrown when a problem is detected in the FileZillaApi data protocol
    /// </summary>
    [Serializable]
    public class ProtocolException : ApiException
    {
        /// <summary>
        /// Creates a new Exception.
        /// </summary>
        public ProtocolException()
        {
        }

        /// <summary>
        /// Creates a new Exception.
        /// </summary>
        public ProtocolException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new Exception.
        /// </summary>
        public ProtocolException(string message, Exception inner)
            : base(message, inner)
        {
        }

#if NETFULL
		/// <summary>
		/// Creates a new Exception.
		/// </summary>
		protected ProtocolException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif

        internal static ProtocolException Create(MessageType expectedMessageType, FileZillaMessage[] actualFileZillaMessages)
        {
            var message = string.Format("Expected message {0} actual {1}",
                expectedMessageType,
                string.Join(",", actualFileZillaMessages.Select(x => x.MessageOrigin.ToString() + "/" + x.MessageType.ToString()))
            );
            return new ProtocolException(message);
        }

		/// <summary>
		/// Create expected/actual exception.
		/// </summary>
		/// <typeparam name="T">Generic type of expected/actual</typeparam>
		/// <param name="expected">expected value</param>
		/// <param name="actual">actual value</param>
		/// <param name="offset">Byte offset of reader</param>
		/// <returns></returns>
		public static ProtocolException Create<T>(T expected, T actual, long offset)
        {
            var message = string.Format("Expected {0} {1} actual {2} at offset 0x{3:x}({3})", 
                typeof(T).Name,
                expected, 
                actual,
                offset);
            return new ProtocolException(message);
        }
    }
}