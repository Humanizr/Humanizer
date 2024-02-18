namespace Humanizer.Tests;

public class EnumHumanizeWithCustomDescriptionPropertyNamesTests
{
    [ModuleInitializer]
    public static void Initializer() =>
        Configurator.EnumDescriptionPropertyLocator = (enumType, property) =>
        {
            if (enumType == typeof(TargetEnum))
            {
                return property.Name == "Info";
            }

            return property.Name == "Description";
        };

    [Fact]
    public void HonorsCustomPropertyAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithCustomPropertyAttribute, TargetEnum.MemberWithCustomPropertyAttribute.Humanize());

    [Fact]
    public void CanHumanizeMembersWithoutDescriptionAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, TargetEnum.MemberWithoutDescriptionAttribute.Humanize());
}


public enum TargetEnum
{
    [CustomProperty(EnumTestsResources.MemberWithCustomPropertyAttribute)]
    MemberWithCustomPropertyAttribute,
    MemberWithoutDescriptionAttribute,
}