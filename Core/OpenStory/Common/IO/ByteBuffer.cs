﻿namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a buffer of bytes... what did you expect?
    /// </summary>
    public sealed class ByteBuffer
    {
        private readonly byte[] _bytes;

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        public int Length => _bytes.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteBuffer"/> class.
        /// </summary>
        public ByteBuffer(byte[] bytes)
        {
            Guard.NotNull(() => bytes, bytes);

            _bytes = bytes.FastClone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Write(IPacketBuilder builder)
        {
            Guard.NotNull(() => builder, builder);

            builder.WriteBytes(_bytes);
        }
    }
}
