﻿namespace Humanizer;

class HungarianOrdinalizer : DefaultOrdinalizer
{
    // In hungarian language ordinal numbers marked with a dot "." at the end
    public override string Convert(int number, string numberString) => numberString + ".";
}