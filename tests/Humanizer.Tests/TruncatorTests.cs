public class TruncatorTests
{
    [Theory(DisplayName = "01 - Truncate")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text longer than truncate length", 10, "Text long…")]
    [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
    public void Truncate(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length));

    [Theory(DisplayName = "02.1 - TruncateWithFixedLengthTruncator")]
    [InlineData("short text", 20, null, "short text")]
    [InlineData("short text", 20, "trunc", "short text")]
    [InlineData("short text", 20, "very long truncation string", "short text")]
    [InlineData("Text longer than truncate length", 10, "very long truncation string", "Text longe")]
    [InlineData("Text longer than truncate length", 10, "trunc", "Text trunc")]
    public void TruncateWithCustomTruncationString(string? input, int length, string? trunactionString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, trunactionString));

    [Theory(DisplayName = "02.2 - TruncateWithFixedLengthTruncator")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text longer than truncate length", 10, "Text long…")]
    [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
    public void TruncateWithFixedLengthTruncator(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedLength));

    [Theory(DisplayName = "03 - TruncateWithFixedNumberOfCharactersTruncator")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more characters than truncate length", 10, "Text with m…")]
    [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
    public void TruncateWithFixedNumberOfCharactersTruncator(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfCharacters));

    [Theory(DisplayName = "04 - TruncateWithFixedNumberOfCharactersTruncator")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more words than truncate length", 4, "Text with more words…")]
    [InlineData("Text with number of words equal to truncate length", 9, "Text with number of words equal to truncate length")]
    [InlineData("Text with less words than truncate length", 8, "Text with less words than truncate length")]
    [InlineData("Words are\nsplit\rby\twhitespace", 4, "Words are\nsplit\rby…")]
    public void TruncateWithFixedNumberOfWordsTruncator(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfWords));

    [Theory(DisplayName = "05 - TruncateWithDynamicLengthAndPreserveWordsTruncator")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text longer than truncate length", 10, "Text…")]
    [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "…")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 1, "…")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 2, "A…")]
    public void TruncateWithDynamicLengthAndPreserveWordsTruncator(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.DynamicLengthAndPreserveWords));

    [Theory(DisplayName = "06 - TruncateWithDynamicNumberOfCharactersAndPreserveWordsTruncator")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more characters than truncate length", 10, "Text with…")]
    [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "…")]
    [InlineData("A Text with delimiter length less than truncate length and the first word fit", 2, "A…")]
    [InlineData("A Text with delimiter length equal to truncate length and the first word fit", 1, "…")]
    public void TruncateWithDynamicNumberOfCharactersAndPreserveWordsTruncator(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "07 - TruncateWithTruncationString")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text longer than truncate length", 10, "...", "Text lo...")]
    [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
    public void TruncateWithTruncationString(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString));

    [Theory(DisplayName = "08 - TruncateWithTruncationStringAndFixedLengthTruncator")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text longer than truncate length", 10, "...", "Text lo...")]
    [InlineData("Text with different truncation string", 10, "---", "Text wi---")]
    [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
    public void TruncateWithTruncationStringAndFixedLengthTruncator(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength));

    [Theory(DisplayName = "09 - TruncateWithTruncationStringAndFixedNumberOfCharactersTruncator")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more characters than truncate length", 10, "...", "Text wit...")]
    [InlineData("Text with different truncation string", 10, "---", "Text wit---")]
    [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
    [InlineData("Text     with additional spaces and null truncate string", 10, null, "Text     with ad")]
    [InlineData("Text     with additional spaces and empty string as truncate string", 10, "", "Text     with ad")]
    public void TruncateWithTruncationStringAndFixedNumberOfCharactersTruncator(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfCharacters));

    [Theory(DisplayName = "10 - TruncateWithTruncationStringAndFixedNumberOfWordsTruncator")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more words than truncate length", 4, "...", "Text with more words...")]
    [InlineData("Text with different truncation string", 4, "---", "Text with different truncation---")]
    [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
    [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
    [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "Words are\nsplit\rby...")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null truncation string truncates")]
    public void TruncateWithTruncationStringAndFixedNumberOfWordsTruncator(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords));

    [Theory(DisplayName = "11 - TruncateWithTruncationStringAndDynamicLengthAndPreserveWordsTruncator")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text longer than truncate length", 10, "...", "Text...")]
    [InlineData("Text with different truncation string", 10, "---", "Text---")]
    [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
    [InlineData("Text with delimiter length greater than truncate length truncates nothingness without truncation string", 2, "", "")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "...", "...")]
    [InlineData("Text with delimiter length less than truncate length and the last word fit", 4, "...", "...")]
    [InlineData("Text with delimiter length less than truncate length and the last word fit", 5, "...", "...")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 3, null, "")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
    public void TruncateWithTruncationStringAndDynamicLengthAndPreserveWordsTruncator(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords));

    [Theory(DisplayName = "12 - TruncateWithTruncationStringAndDynamicNumberOfCharactersAndPreserveWordsTruncator")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more characters than truncate length", 10, "...", "Text...")]
    [InlineData("Text with different truncation string", 10, "---", "Text---")]
    [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "")]
    [InlineData("Text     with additional spaces and null truncate string", 10, null, "Text     with")]
    [InlineData("Text     with additional spaces and empty string as truncate string", 10, "", "Text     with")]
    [InlineData("Text with delimiter length greater than truncate length truncates nothingness without truncation string", 2, "", "")]
    [InlineData("Text delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "...", "...")]
    [InlineData("Text with delimiter length less than truncate length and the last word fit", 4, "...", "...")]
    [InlineData("Text with delimiter length less than truncate length and the last word fit", 5, "...", "...")]
    [InlineData("Text with delimiter length less than truncate length and the last word fit", 7, "...", "Text...")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 2, null, "")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 3, null, "")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 5, null, "Null")]
    public void TruncateWithTruncationStringAndDynamicNumberOfCharactersAndPreserveWordsTruncator(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "13 - TruncateWithFixedLengthTruncatorTruncateFromLeft")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text longer than truncate length", 10, "…te length")]
    [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
    public void TruncateWithFixedLengthTruncatorTruncateFromLeft(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedLength, TruncateFrom.Left));

    [Theory(DisplayName = "14 - TruncateWithFixedNumberOfCharactersTruncatorTruncateFromLeft")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more characters than truncate length", 10, "…ate length")]
    [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
    [InlineData("Text with strange characters ^$(*^ and more ^$**)%  ", 10, "…rs ^$(*^ and more ^$**)%  ")]
    public void TruncateWithFixedNumberOfCharactersTruncatorTruncateFromLeft(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfCharacters, TruncateFrom.Left));

    [Theory(DisplayName = "15 - TruncateWithFixedNumberOfWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more words than truncate length", 4, "…words than truncate length")]
    [InlineData("Text with number of words equal to truncate length", 9, "Text with number of words equal to truncate length")]
    [InlineData("Text with less words than truncate length", 8, "Text with less words than truncate length")]
    [InlineData("Words are\nsplit\rby\twhitespace", 4, "…are\nsplit\rby\twhitespace")]
    [InlineData("Text with whitespace at the end  ", 4, "…whitespace at the end")]
    public void TruncateWithFixedNumberOfWordsTruncatorTruncateFromLeft(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfWords, TruncateFrom.Left));

    [Theory(DisplayName = "16 - TruncateWithDynamicLengthAndPreserveWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text longer than truncate length", 10, "…length")]
    [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "…")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 5, "…fit")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 4, "…fit")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 3, "…")]
    public void TruncateWithDynamicLengthAndPreserveWordsTruncatorTruncateFromLeft(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.DynamicLengthAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "17 - TruncateWithDynamicNumberOfCharactersAndPreserveWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, null)]
    [InlineData("", 10, "")]
    [InlineData("a", 1, "a")]
    [InlineData("Text with more characters than truncate length", 10, "…length")]
    [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
    [InlineData("Text with strange characters ^$(*^ and more ^$**)%  ", 10, "…^$(*^ and more ^$**)%  ")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "…")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 4, "…fit")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 3, "…")]
    public void TruncateWithDynamicNumberOfCharactersAndPreserveWordsTruncatorTruncateFromLeft(string? input, int length, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, Truncator.DynamicNumberOfCharactersAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "18 - TruncateWithTruncationStringAndFixedLengthTruncatorTruncateFromLeft")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text longer than truncate length", 10, "...", "... length")]
    [InlineData("Text with different truncation string", 10, "---", "--- string")]
    [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "ng")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "ring")]
    public void TruncateWithTruncationStringAndFixedLengthTruncatorTruncateFromLeft(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength, TruncateFrom.Left));

    [Theory(DisplayName = "19 - TruncateWithTruncationStringAndFixedNumberOfCharactersTruncatorTruncateFromLeft")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more characters than truncate length", 10, "...", "...e length")]
    [InlineData("Text with different truncation string", 10, "---", "---n string")]
    [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates to fixed number of characters without truncation string", 2, "...", "ng")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "ring")]
    public void TruncateWithTruncationStringAndFixedNumberOfCharactersTruncatorTruncateFromLeft(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfCharacters, TruncateFrom.Left));

    [Theory(DisplayName = "20 - TruncateWithTruncationStringAndFixedNumberOfWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more words than truncate length", 4, "...", "...words than truncate length")]
    [InlineData("Text with different truncation string", 4, "---", "---with different truncation string")]
    [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
    [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
    [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "...are\nsplit\rby\twhitespace")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "length without truncation string")]
    [InlineData("Text with whitespace at the end  ", 4, "...", "...whitespace at the end")]
    public void TruncateWithTruncationStringAndFixedNumberOfWordsTruncatorTruncateFromLeft(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords, TruncateFrom.Left));

    [Theory(DisplayName = "20 - TruncateWithTruncationStringAndDynamicLengthAndPreserveWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text longer than truncate length", 10, "...", "...length")]
    [InlineData("Text with different truncation string", 10, "---", "---string")]
    [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
    [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates nothingness without truncation string", 2, "", "")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 5, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 6, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 7, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 9, "...", "...alone")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 4, "...", "...")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 3, "...", "...")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 6, null, "string")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 7, null, "string")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 10, null, "string")]
    public void TruncateWithTruncationStringAndDynamicLengthAndPreserveWordsTruncatorTruncateFromLeft(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "21 - TruncateWithTruncationStringAndDynamicNumberOfCharactersAndPreserveWordsTruncatorTruncateFromLeft")]
    [InlineData(null, 10, "...", null)]
    [InlineData("", 10, "...", "")]
    [InlineData("a", 1, "...", "a")]
    [InlineData("Text with more characters than truncate length", 10, "...", "...length")]
    [InlineData("Text with different truncation string", 10, "---", "---string")]
    [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
    [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
    [InlineData("Text with delimiter length greater than truncate length truncates nothingness without truncation string", 2, "", "")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 2, "...", "")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 4, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 5, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 6, "...", "...")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 8, "...", "...alone")]
    [InlineData("Textual with delimiter length less than truncate length and starting word longer than truncate length to truncation string alone", 9, "...", "...alone")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 2, "...", "")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 4, ".....", "fit")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 5, ".....", ".....")]
    [InlineData("A Text with delimiter length less than truncate length and the last word fit", 6, ".....", ".....")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 6, null, "string")]
    [InlineData("Null truncation string truncates to truncate length without truncation string", 7, null, "string")]
    public void TruncateWithTruncationStringAndDynamicNumberOfCharactersAndPreserveWordsTruncatorTruncateFromLeft(string? input, int length, string? truncationString, string? expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicNumberOfCharactersAndPreserveWords, TruncateFrom.Left));
}