﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests
{
    public class DehumanizeToEnumTests
    {
        [Fact]
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.CustomDescription.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void ThrowsForNonEnums()
        {
            Assert.Throws<ArgumentException>(() => EnumTestsResources.CustomDescription.DehumanizeTo<DummyStructWithEnumInterfaces>());
        }

        [Fact]
        public void ThrowsForEnumNoMatch()
        {
            Assert.Throws<KeyNotFoundException>(() => EnumTestsResources.CustomDescription.DehumanizeTo<DummyEnum>());
        }

        [Fact]
        public void CanHumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void CanApplyTitleCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumUnderTest.MemberWithoutDescriptionAttribute,
                EnumTestsResources.MemberWithoutDescriptionAttributeTitle.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void CanApplyLowerCaseCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumUnderTest.MemberWithoutDescriptionAttribute,
                EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void AllCapitalMembersAreReturnedAsIs()
        {
            Assert.Equal(
                EnumUnderTest.ALLCAPITALS,
                EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo<EnumUnderTest>());
        }

        struct DummyStructWithEnumInterfaces : IComparable, IFormattable, IConvertible
        {
            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                throw new NotImplementedException();
            }

            public TypeCode GetTypeCode()
            {
                throw new NotImplementedException();
            }

            public bool ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public char ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public sbyte ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public byte ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public short ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ushort ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public int ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public uint ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public long ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ulong ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public float ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public double ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public decimal ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public DateTime ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public string ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public object ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }
        }

        enum DummyEnum
        {
            First,
            Second
        }
    }

    

}
