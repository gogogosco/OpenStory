﻿using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common.Game
{
    [Category("OpenStory.Common.Game.PointS")]
    [TestFixture]
    public sealed class PointSFixture
    {
        [Test]
        public void MaxComponents_Should_Return_Correct_PointS()
        {
            var a = new PointS(10, 20);
            var b = new PointS(-20, 30);

            var c = PointS.MaxComponents(a, b);

            c.Should().HaveComponents(10, 30);
        }

        [Test]
        public void MinComponents_Should_Return_Correct_PointS()
        {
            var a = new PointS(-20, 30);
            var b = new PointS(20, -40);

            var c = PointS.MinComponents(a, b);

            c.Should().HaveComponents(-20, -40);
        }

        [Test]
        public void Binary_Plus_Operator_Should_Return_Correct_PointS()
        {
            var a = new PointS(-20, 20);
            var b = new PointS(20, -20);

            var c = a + b;

            c.Should().HaveComponents(0, 0);
        }

        [Test]
        public void Unary_Minus_Operator_Should_Return_Correct_PointS()
        {
            var a = new PointS(20, -20);

            var b = -a;

            b.Should().HaveComponents(-20, 20);
        }

        [Test]
        public void Unary_Minus_Operator_Should_Throw_On_MinValue()
        {
            var point = new PointS(short.MinValue, short.MinValue);

            point
                .Invoking(a => (-a).Whatever())
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Equality_Operator_Should_Return_True_For_Same_PointS()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(1, 2);

            (point1 == point2).Should().BeTrue();
        }

        [Test]
        public void Equality_Operator_Should_Return_False_For_Different_PointS()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(3, 4);

            (point1 == point2).Should().BeFalse();
        }

        [Test]
        public void Inequality_Operator_Should_Return_True_For_Different_PointS()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(2, 1);

            (point1 != point2).Should().BeTrue();
        }

        [Test]
        public void Inequality_Operator_Should_Return_False_For_Same_PointS()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(1, 2);

            (point1 != point2).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_Return_True_For_Same_PointS()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(1, 2);
            
            Equals(point1, point2).Should().BeTrue();
        }

        [Test]
        public void Equals_Should_Return_False_For_Different_PointS()
        {
            Equals(new PointS(1, 2), new PointS(2, 3)).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_Return_True_For_Same_PointS_As_Object()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(1, 2);

            point1.Equals((object)point2).Should().BeTrue();
        }

        [Test]
        public void Equals_Should_Return_False_For_Different_PointS_As_Object()
        {
            var point1 = new PointS(1, 2);
            var point2 = new PointS(2, 3);

            point1.Equals((object)point2).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_Return_False_For_New_Object()
        {
            var point1 = new PointS(1, 2);

            point1.Equals(new object()).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_Return_False_For_Null_Object()
        {
            var point1 = new PointS(1, 2);

            point1.Equals(null).Should().BeFalse();
        }

        [Test]
        public void GetHashCode_Should_Return_Same_Hash_For_Same_PointS()
        {
            var hash1 = new PointS(1, 2).GetHashCode();
            var hash2 = new PointS(1, 2).GetHashCode();
            hash1.Should().Be(hash2);
        }
    }
}
