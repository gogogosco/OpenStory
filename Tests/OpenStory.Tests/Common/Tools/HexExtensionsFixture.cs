﻿using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.HexExtensions")]
    [TestFixture]
    public sealed class HexExtensionsFixture
    {
        #region ToByte

        [Test]
        public void ToByte_Should_Throw_On_Null_Input()
        {
            ((string)null)
                .Invoking(s => s.ToByte())
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ToByte_Should_Throw_On_Odd_Input_Length()
        {
            @"12345"
                .Invoking(s => s.ToByte())
                .ShouldThrow<ArgumentException>()
                .WithMessagePrefix(CommonStrings.StringLengthMustBeEven);
        }

        [Test]
        public void ToByte_Should_Throw_On_Invalid_Digit_Characters(
            [Values(@"1LOLCATZ", @"11LMAOCATZ")]string invalidHex)
        {
            invalidHex
                .Invoking(s => s.ToByte())
                .ShouldThrow<ArgumentException>()
                .WithMessagePrefix(CommonStrings.StringMustContainOnlyHexDigits);
        }

        [Test]
        public void ToByte_Should_Parse_Null_Bytes()
        {
            var expected = new byte[] { 0x01, 0x00, 0x10, 0x00 };

            var actual = @"01001000".ToByte();

            actual.Should().ContainInOrder(expected);
            actual.Should().HaveSameCount(expected);
        }

        #endregion

        #region ToHex

        [Test]
        public void ToHex_Should_Throw_On_Null_Array()
        {
            ((byte[])null)
                .Invoking(a => a.ToHex())
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ToHex_Should_Hyphenate_Bytes()
        {
            var bytes = new byte[] { 0x12, 0x34 };

            bytes.ToHex(true).Should().Be("12-34");
        }

        [Test]
        public void ToHex_Should_Stringify_Zero_Bytes()
        {
            var bytes = new byte[] { 0x00, 0x12, 0x00, 0x34 };

            bytes.ToHex().Should().Be("00120034");
        }

        #endregion
    }
}
