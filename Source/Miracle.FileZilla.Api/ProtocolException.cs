using System;
using System.IO;
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

        /// <summary>
        /// Creates a new Exception.
        /// </summary>
        protected ProtocolException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Create expected/actual exception.
        /// </summary>
        /// <typeparam name="T">Generic type of expected/actual</typeparam>
        /// <param name="reader">binary reader to get position from</param>
        /// <param name="expected">expected value</param>
        /// <param name="actual">actual value</param>
        /// <returns></returns>
        public static ProtocolException Create<T>(BinaryReader reader, T expected, T actual)
        {
            var message = string.Format("Expected {0} {1} actual {2} at offset 0x{3:x}({3})", 
                typeof(T).Name,
                expected, 
                actual,
                reader.BaseStream.Position - SizeOf<T>());
            return new ProtocolException(message);
        }

        /// <summary>
        /// Get size of generic type in bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static int SizeOf<T>()
        {
            var dm = new DynamicMethod("$", typeof (int), Type.EmptyTypes);
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, typeof (T));
            il.Emit(OpCodes.Ret);
            var func = (Func<int>) dm.CreateDelegate(typeof (Func<int>));
            return func();
        }
    }
}