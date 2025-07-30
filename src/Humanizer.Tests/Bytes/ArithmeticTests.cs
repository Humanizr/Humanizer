public class ArithmeticTests
{
    [Fact]
    public void Add()
    {
        var size1 = ByteSize.FromBytes(1);
        var result = size1.Add(size1);

        Assert.Equal(2, result.Bytes);
        Assert.Equal(16, result.Bits);
    }

    [Fact]
    public void AddBits()
    {
        var size = ByteSize
            .FromBytes(1)
            .AddBits(8);

        Assert.Equal(2, size.Bytes);
        Assert.Equal(16, size.Bits);
    }

    [Fact]
    public void AddBytes()
    {
        var size = ByteSize
            .FromBytes(1)
            .AddBytes(1);

        Assert.Equal(2, size.Bytes);
        Assert.Equal(16, size.Bits);
    }

    [Fact]
    public void AddKilobytes()
    {
        var size = ByteSize
            .FromKilobytes(2)
            .AddKilobytes(2);

        Assert.Equal(4 * 1024 * 8, size.Bits);
        Assert.Equal(4 * 1024, size.Bytes);
        Assert.Equal(4, size.Kilobytes);
    }

    [Fact]
    public void AddMegabytes()
    {
        var size = ByteSize
            .FromMegabytes(2)
            .AddMegabytes(2);

        Assert.Equal(4 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4 * 1024 * 1024, size.Bytes);
        Assert.Equal(4 * 1024, size.Kilobytes);
        Assert.Equal(4, size.Megabytes);
    }

    [Fact]
    public void AddGigabytes()
    {
        var size = ByteSize
            .FromGigabytes(2)
            .AddGigabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024, size.Megabytes);
        Assert.Equal(4d, size.Gigabytes);
    }

    [Fact]
    public void AddTerabytes()
    {
        var size = ByteSize
            .FromTerabytes(2)
            .AddTerabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024, size.Gigabytes);
        Assert.Equal(4d, size.Terabytes);
    }

    [Fact]
    public void AddPetabytes()
    {
        var size = ByteSize
            .FromPetabytes(2)
            .AddPetabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024, size.Terabytes);
        Assert.Equal(4d, size.Petabytes);
    }

    [Fact]
    public void AddExabytes()
    {
        var size = ByteSize
            .FromExabytes(2)
            .AddExabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024 * 1024, size.Terabytes);
        Assert.Equal(4d * 1024, size.Petabytes);
        Assert.Equal(4d, size.Exabytes);
    }


    [Fact]
    public void AddZettabytes()
    {
        var size = ByteSize
            .FromZettabytes(2)
            .AddZettabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Terabytes);
        Assert.Equal(4d * 1024 * 1024, size.Petabytes);
        Assert.Equal(4d * 1024, size.Exabytes);
        Assert.Equal(4d, size.Zettabytes);
    }


    [Fact]
    public void AddYottabytes()
    {
        var size = ByteSize
            .FromYottabytes(2)
            .AddYottabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Terabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Petabytes);
        Assert.Equal(4d * 1024 * 1024, size.Exabytes);
        Assert.Equal(4d * 1024, size.Zettabytes);
        Assert.Equal(4d, size.Yottabytes);
    }


    [Fact]
    public void AddRonnabytes()
    {
        var size = ByteSize
            .FromRonnabytes(2)
            .AddRonnabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Terabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Petabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Exabytes);
        Assert.Equal(4d * 1024 * 1024, size.Zettabytes);
        Assert.Equal(4d * 1024, size.Yottabytes);
        Assert.Equal(4d, size.Ronnabytes);
    }


    [Fact]
    public void AddQuettabytes()
    {
        var size = ByteSize
            .FromQuettabytes(2)
            .AddQuettabytes(2);

        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Bytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Kilobytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Megabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Gigabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, size.Terabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 1024, size.Petabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Exabytes);
        Assert.Equal(4d * 1024 * 1024 * 1024, size.Zettabytes);
        Assert.Equal(4d * 1024 * 1024, size.Yottabytes);
        Assert.Equal(4d * 1024, size.Ronnabytes);
        Assert.Equal(4d, size.Quettabytes);

    }

    [Fact]
    public void Subtract()
    {
        var size = ByteSize
            .FromBytes(4)
            .Subtract(ByteSize.FromBytes(2));

        Assert.Equal(16, size.Bits);
        Assert.Equal(2, size.Bytes);
    }

    [Fact]
    public void IncrementOperator()
    {
        var size = ByteSize.FromBytes(2);
        size++;

        Assert.Equal(24, size.Bits);
        Assert.Equal(3, size.Bytes);
    }

    [Fact]
    public void NegativeOperator()
    {
        var size = ByteSize.FromBytes(2);

        size = -size;

        Assert.Equal(-16, size.Bits);
        Assert.Equal(-2, size.Bytes);
    }

    [Fact]
    public void DecrementOperator()
    {
        var size = ByteSize.FromBytes(2);
        size--;

        Assert.Equal(8, size.Bits);
        Assert.Equal(1, size.Bytes);
    }

    [Fact]
    public void PlusOperator()
    {
        var size1 = ByteSize.FromBytes(1);
        var size2 = ByteSize.FromBytes(1);

        var result = size1 + size2;

        Assert.Equal(2, result.Bytes);
    }

    [Fact]
    public void MinusOperator()
    {
        var size1 = ByteSize.FromBytes(2);
        var size2 = ByteSize.FromBytes(1);

        var result = size1 - size2;

        Assert.Equal(1, result.Bytes);
    }
}
