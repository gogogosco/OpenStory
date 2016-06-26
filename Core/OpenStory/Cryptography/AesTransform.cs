using System;
using System.Security.Cryptography;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a cryptographic transformer based on the AES algorithm.
    /// </summary>
    public sealed class AesTransform : CryptoTransformBase
    {
        private const int IvLength = 16;
        private const int BlockLength = 1460;

        private readonly ICryptoTransform aes;

        private static ICryptoTransform GetTransformer(byte[] key)
        {
            using (var cipher = new RijndaelManaged())
            {
                cipher.Padding = PaddingMode.None;
                cipher.Mode = CipherMode.ECB;
                cipher.Key = key;

                var transform = cipher.CreateEncryptor();
                return transform;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AesTransform"/> class.
        /// </summary>
        /// <remarks>
        /// The provided arrays are copied into the <see cref="AesTransform"/> instance to avoid mutation.
        /// </remarks>
        /// <param name="table">The shuffle transformation table.</param>
        /// <param name="vector">The initial value for the shuffle transformation.</param>
        /// <param name="key">The AES key.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided parameters is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if any of the provided parameters has an invalid number of elements.</exception>
        public AesTransform(byte[] table, byte[] vector, byte[] key)
            : base(table, vector)
        {
            Guard.NotNull(() => key, key);

            if (key.Length != 32)
            {
                throw new ArgumentException(CommonStrings.AesKeyMustBe32Bytes, nameof(key));
            }

            this.aes = GetTransformer(key);
        }

        /// <inheritdoc />
        public override void TransformArraySegment(byte[] data, byte[] vector, int segmentStart, int segmentEnd)
        {
            var xorBlock = new byte[IvLength];

            // First block is 4 elements shorter because of the header.
            const int FirstBlockLength = BlockLength - 4;

            int blockStart = segmentStart;
            int blockEnd = Math.Min(blockStart + FirstBlockLength, segmentEnd);

            this.TransformBlock(data, vector, blockStart, blockEnd, xorBlock);

            blockStart += FirstBlockLength;
            while (blockStart < segmentEnd)
            {
                blockEnd = Math.Min(blockStart + BlockLength, segmentEnd);

                this.TransformBlock(data, vector, blockStart, blockEnd, xorBlock);

                blockStart += BlockLength;
            }
        }

        /// <summary>
        /// Performs the AES transformation on a single block of the data.
        /// </summary>
        /// <remarks><para>
        /// The parameter <paramref name="xorBlock"/> is used only for performance
        /// considerations, to avoid instantiating a new array every time a transformation has
        /// to be done. It should not be shorter than 16 elements, and it's unnecessary for
        /// it to be longer. Its contents will be overwritten.
        /// </para></remarks>
        /// <param name="data">The array containing the block.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="blockStart">The start offset of the block.</param>
        /// <param name="blockEnd">The end offset of the block.</param>
        /// <param name="xorBlock">An array to use for the internal XOR operations.</param>
        private void TransformBlock(byte[] data, byte[] iv, int blockStart, int blockEnd, byte[] xorBlock)
        {
            FillXorBlock(iv, xorBlock);

            int xorBlockPosition = 0;
            for (int position = blockStart; position < blockEnd; position++)
            {
                if (xorBlockPosition == 0)
                {
                    xorBlock = this.aes.TransformFinalBlock(xorBlock, 0, IvLength);
                }

                data[position] ^= xorBlock[xorBlockPosition];
                xorBlockPosition++;
                if (xorBlockPosition == IvLength)
                {
                    xorBlockPosition = 0;
                }
            }
        }

        /// <summary>
        /// Fills a 16-element byte array with copies of the specified IV.
        /// </summary>
        /// <param name="iv">The IV to copy.</param>
        /// <param name="xorBlock">The block to use.</param>
        private static void FillXorBlock(byte[] iv, byte[] xorBlock)
        {
            for (int i = 0; i < IvLength; i += 4)
            {
                Buffer.BlockCopy(iv, 0, xorBlock, i, 4);
            }
        }
    }
}
