public class ComparingTests
{
    [Theory]
    [InlineData(13, 23, -1)]
    [InlineData(23, 23, 0)]
    [InlineData(45, 23, 1)]
    public void CompareStrongTyped(double value, double valueToCompareWith, int expectedResult)
    {
        var valueSize = new ByteSize(value);
        var otherSize = new ByteSize(valueToCompareWith);
        var result = valueSize.CompareTo(otherSize);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(13, 23, -1)]
    [InlineData(23, 23, 0)]
    [InlineData(45, 23, 1)]
    public void CompareUntyped(double value, double valueToCompareWith, int expectedResult)
    {
        var valueSize = new ByteSize(value);
        object otherSize = new ByteSize(valueToCompareWith);
        var result = valueSize.CompareTo(otherSize);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(new[] { "1GB", "3KB", "5MB" }, new[] { "3KB", "5MB", "1GB" })]
    [InlineData(new[] { "1MB", "3KB", "5MB" }, new[] { "3KB", "1MB", "5MB" })]
    public void SortList(IEnumerable<string> values, IEnumerable<string> expected)
    {
        var list = values.Select(ByteSize.Parse).ToList();
        list.Sort();

        Assert.Equal(expected.Select(ByteSize.Parse), list);
    }

    [Fact]
    public void EqualsWithNull()
    {
        var size = ByteSize.FromBytes(1);
        Assert.False(size.Equals((object?)null));
    }

    [Fact]
    public void EqualsWithNonByteSizeObject()
    {
        var size = ByteSize.FromBytes(1);
        Assert.False(size.Equals("not a ByteSize"));
    }

    [Fact]
    public void EqualsWithSameByteSize()
    {
        var size1 = ByteSize.FromBytes(1);
        object size2 = ByteSize.FromBytes(1);
        Assert.True(size1.Equals(size2));
    }

    [Fact]
    public void EqualsWithDifferentByteSize()
    {
        var size1 = ByteSize.FromBytes(1);
        object size2 = ByteSize.FromBytes(2);
        Assert.False(size1.Equals(size2));
    }

    [Fact]
    public void GetHashCodeReturnsConsistentValue()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(1);
        Assert.Equal(size1.GetHashCode(), size2.GetHashCode());
    }

    [Fact]
    public void CompareToNullReturnsPositive()
    {
        var size = ByteSize.FromBytes(1);
        Assert.Equal(1, size.CompareTo(null));
    }

    [Fact]
    public void CompareToNonByteSizeThrows()
    {
        var size = ByteSize.FromBytes(1);
        Assert.Throws<ArgumentException>(() => size.CompareTo("not a ByteSize"));
    }

    [Fact]
    public void EqualityOperator()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(1);
        Assert.True(size1 == size2);
    }

    [Fact]
    public void InequalityOperator()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(2);
        Assert.True(size1 != size2);
    }

    [Fact]
    public void LessThanOperator()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(2);
        Assert.True(size1 < size2);
        Assert.False(size2 < size1);
    }

    [Fact]
    public void LessThanOrEqualOperator()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(2);
        var size3 = ByteSize.FromBytes(1);
        Assert.True(size1 <= size2);
        Assert.True(size1 <= size3);
        Assert.False(size2 <= size1);
    }

    [Fact]
    public void GreaterThanOperator()
    {
        var size1 = ByteSize.FromBytes(2);
        var size2 = ByteSize.FromBytes(1);
        Assert.True(size1 > size2);
        Assert.False(size2 > size1);
    }

    [Fact]
    public void GreaterThanOrEqualOperator()
    {
        var size1 = ByteSize.FromBytes(2);
        var size2 = ByteSize.FromBytes(1);
        var size3 = ByteSize.FromBytes(2);
        Assert.True(size1 >= size2);
        Assert.True(size1 >= size3);
        Assert.False(size2 >= size1);
    }
}