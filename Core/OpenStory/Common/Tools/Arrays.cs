﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Array helpers!
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Clones a byte array.
        /// </summary>
        /// <param name="array">The array to clone.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="array"/> is <c>null</c>.
        /// </exception>
        /// <returns>the new array.</returns>
        public static byte[] FastClone(this byte[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int length = array.Length;
            var newArray = new byte[length];
            Buffer.BlockCopy(array, 0, newArray, 0, length);
            return newArray;
        }

        /// <summary>
        /// Extracts a segment from a given array.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="start">The start of the segment.</param>
        /// <param name="length">The length of the segment.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="start"/> is negative or outside the range of the array, 
        /// or if <paramref name="length"/> is negative or the segment ends outside the array's bounds.</exception>
        /// <returns>a copy of the segment.</returns>
        public static byte[] CopySegment(this byte[] array, int start, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (start < 0 || array.Length <= start)
            {
                throw new ArgumentOutOfRangeException("start");
            }
            if (length < 0 || array.Length < start + length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            var segment = new byte[length];
            Buffer.BlockCopy(array, start, segment, 0, length);
            return segment;
        }
    }
}
