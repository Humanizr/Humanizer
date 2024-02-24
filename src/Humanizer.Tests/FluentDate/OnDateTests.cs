﻿#if NET6_0_OR_GREATER

public class OnDateTests
{
    [Fact]
    public void OnJanuaryThe23rd() =>
        Assert.Equal(new(DateTime.Now.Year, 1, 23), OnDate.January.The23rd);

    [Fact]
    public void OnDecemberThe4th() =>
        Assert.Equal(new(DateTime.Now.Year, 12, 4), OnDate.December.The4th);

    [Fact]
    public void OnFebruaryThe() =>
        Assert.Equal(new(DateTime.Now.Year, 2, 11), OnDate.February.The(11));
}

#endif
