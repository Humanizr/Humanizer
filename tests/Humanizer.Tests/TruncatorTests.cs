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

    // -- Additional tail-branch coverage tests (fn-9.12) --
    // Target uncovered lines/branches from artifacts/fn-9-local-coverage/uncovered.json:
    //
    // FixedLengthTruncator: lines 12-13 (null return), branches on 11,16,21,23,28
    //   - null truncationString with both Left/Right directions (line 21,23,28)
    //   - truncationString longer than length with both directions (line 21,23)
    //
    // FixedNumberOfCharactersTruncator: lines 12-13 (null), branches on 11,16,21,23,25,30,32,35,42,48,50,52,57,64,66,71
    //   - Non-alpha separators between alpha chars with both directions
    //   - Lines 62,77 (loop fallthrough) are structurally unreachable
    //
    // FixedNumberOfWordsTruncator: lines 12-13 (null), 72 (right fallthrough), 100 (left fallthrough)
    //   - Single-word input with length=0 triggers loop fallthrough (lines 72, 100)
    //
    // DynamicLengthAndPreserveWordsTruncator: lines 16-17 (null), 105-106 (prefix empty from right)
    //   - Effective length zero (delimiter fills length), no-space backtrack, whitespace prefix
    //   - Candidate too long from left, allowed content zero from left
    //
    // DynamicNumberOfCharactersAndPreserveWordsTruncator: lines 17-18 (null), 79-81, 113-114, 118-119, 123-124, 128-129
    //   - Delimiter longer than totalLength, no complete word fits, whitespace-only input
    //   - Prefix/suffix alpha count zero, delimiter >= totalLength but value fits

    [Theory(DisplayName = "22 - FixedLengthTruncator null truncation string branches")]
    [InlineData("Text longer than length", 5, null, "Text ", TruncateFrom.Right)]
    [InlineData("Text longer than length", 5, null, "ength", TruncateFrom.Left)]
    public void FixedLengthTruncatorNullTruncationString(string input, int length, string? truncationString, string expectedOutput, TruncateFrom from) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength, from));

    [Theory(DisplayName = "23 - FixedLengthTruncator truncation string longer than length")]
    [InlineData("Text longer than truncate length", 3, "very long truncation string", "Tex", TruncateFrom.Right)]
    [InlineData("Text longer than truncate length", 3, "very long truncation string", "gth", TruncateFrom.Left)]
    public void FixedLengthTruncatorTruncationStringLongerThanLength(string input, int length, string truncationString, string expectedOutput, TruncateFrom from) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength, from));

    [Theory(DisplayName = "24 - FixedNumberOfWordsTruncator word at end of string without trailing whitespace")]
    [InlineData("SingleWord", 0, "...", "...SingleWord", TruncateFrom.Left)]
    [InlineData("one two three", 1, "...", "...three", TruncateFrom.Left)]
    public void FixedNumberOfWordsTruncatorFromLeftSingleWordFallthrough(string input, int length, string? truncationString, string expectedOutput, TruncateFrom from) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords, from));

    [Theory(DisplayName = "25 - FixedNumberOfWordsTruncator single word fallthrough from right")]
    [InlineData("word", 0, "...", "word...")]
    public void FixedNumberOfWordsTruncatorSingleWordFallthroughFromRight(string input, int length, string? truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords));

    [Theory(DisplayName = "26 - FixedNumberOfCharactersTruncator additional branches")]
    [InlineData("a-b-c-d-e-f", 4, ".", "a-b-c.", TruncateFrom.Right)]
    [InlineData("a-b-c-d-e-f", 4, ".", ".d-e-f", TruncateFrom.Left)]
    [InlineData("a--b--c", 2, "", "a--b", TruncateFrom.Right)]
    [InlineData("a--b--c", 2, "", "b--c", TruncateFrom.Left)]
    public void FixedNumberOfCharactersTruncatorAdditionalBranches(string input, int length, string truncationString, string expectedOutput, TruncateFrom from) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfCharacters, from));

    [Theory(DisplayName = "27 - DynamicLengthAndPreserveWordsTruncator effective length zero from right")]
    [InlineData("Hello World", 3, "...", "...")]
    [InlineData("Hello World", 2, "..", "..")]
    [InlineData("Hello World", 1, ".", ".")]
    public void DynamicLengthAndPreserveWordsTruncatorEffectiveLengthZeroFromRight(string input, int length, string truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords));

    [Theory(DisplayName = "28 - DynamicLengthAndPreserveWordsTruncator allowed content zero from left")]
    [InlineData("Hello World", 3, "...", "...")]
    [InlineData("Hello World", 2, "..", "..")]
    [InlineData("Hello World", 1, ".", ".")]
    public void DynamicLengthAndPreserveWordsTruncatorAllowedContentZeroFromLeft(string input, int length, string truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "29 - DynamicLengthAndPreserveWordsTruncator no space found backtrack from right")]
    [InlineData("Verylongword end", 7, "...", "...")]
    public void DynamicLengthAndPreserveWordsTruncatorNoSpaceBacktrackFromRight(string input, int length, string truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords));

    [Theory(DisplayName = "30 - DynamicLengthAndPreserveWordsTruncator whitespace-only prefix from right")]
    [InlineData("  longword other", 5, "...", "...")]
    public void DynamicLengthAndPreserveWordsTruncatorWhitespaceOnlyPrefixFromRight(string input, int length, string truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords));

    [Theory(DisplayName = "31 - DynamicLengthAndPreserveWordsTruncator candidate too long from left")]
    [InlineData("Verylongword end", 7, "...", "...end")]
    public void DynamicLengthAndPreserveWordsTruncatorCandidateTooLongFromLeft(string input, int length, string truncationString, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.DynamicLengthAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "32 - DynamicNumberOfCharactersAndPreserveWordsTruncator delimiter longer than totalLength right")]
    [InlineData("abc def ghi", 2, "....", "")]
    public void DynCharPreserveWordsDelimiterLongerThanTotalRight(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "33 - DynamicNumberOfCharactersAndPreserveWordsTruncator delimiter longer than totalLength left")]
    [InlineData("abc def ghi", 2, "....", "")]
    public void DynCharPreserveWordsDelimiterLongerThanTotalLeft(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "34 - DynamicNumberOfCharactersAndPreserveWordsTruncator no complete word fits right")]
    [InlineData("Verylongword other", 5, "...", "...")]
    public void DynCharPreserveWordsNoCompleteWordFitsRight(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "35 - DynamicNumberOfCharactersAndPreserveWordsTruncator no complete word fits left")]
    [InlineData("other Verylongword", 5, "...", "...")]
    public void DynCharPreserveWordsNoCompleteWordFitsLeft(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords, TruncateFrom.Left));

    [Theory(DisplayName = "36 - DynamicNumberOfCharactersAndPreserveWordsTruncator whitespace input")]
    [InlineData("   ", 1, "...", "   ")]
    public void DynCharPreserveWordsWhitespaceOnlyInput(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "37 - DynamicNumberOfCharactersAndPreserveWordsTruncator delimiter >= totalLength with value fitting")]
    [InlineData("ab", 2, "....", "ab")]
    public void DynCharPreserveWordsDelimiterLongerThanTotalButValueFits(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "38 - DynamicNumberOfCharactersAndPreserveWordsTruncator prefix alphaLength zero right")]
    [InlineData("---  ---  word", 3, "...", "...")]
    public void DynCharPreserveWordsPrefixAlphaZeroRight(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords));

    [Theory(DisplayName = "39 - DynamicNumberOfCharactersAndPreserveWordsTruncator suffix alphaLength zero left")]
    [InlineData("word  ---  ---", 3, "...", "...")]
    public void DynCharPreserveWordsSuffixAlphaZeroLeft(string input, int length, string delimiter, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Truncate(length, delimiter, Truncator.DynamicNumberOfCharactersAndPreserveWords, TruncateFrom.Left));
}