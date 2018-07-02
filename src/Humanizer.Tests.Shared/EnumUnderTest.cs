using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Humanizer.Tests
{
    public enum EnumUnderTest
    {
#if !NETFX_CORE
        [Description(EnumTestsResources.MemberWithDescriptionAttribute)]
        MemberWithDescriptionAttribute,
        [DescriptionSubclass(EnumTestsResources.MemberWithDescriptionAttributeSubclass)]
#endif
        MemberWithDescriptionAttributeSubclass,
        [CustomDescription(EnumTestsResources.MemberWithCustomDescriptionAttribute)]
        MemberWithCustomDescriptionAttribute,
        [ImposterDescription(42)]
        MemberWithImposterDescriptionAttribute,
        [CustomProperty(EnumTestsResources.MemberWithCustomPropertyAttribute)]
        MemberWithCustomPropertyAttribute,
        MemberWithoutDescriptionAttribute,
        ALLCAPITALS,
        [Display(Description = EnumTestsResources.MemberWithDisplayAttribute)]
        MemberWithDisplayAttribute,
        [Display(Description = "MemberWithLocalizedDisplayAttribute", ResourceType = typeof(EnumTestsResources))]
        MemberWithLocalizedDisplayAttribute,
        [Display(Name = EnumTestsResources.MemberWithDisplayAttributeWithoutDescription)]
        MemberWithDisplayAttributeWithoutDescription
    }

    public class EnumTestsResources
    {
        public const string MemberWithDescriptionAttribute = "Some Description";
        public const string MemberWithDescriptionAttributeSubclass = "Description in Description subclass";
        public const string MemberWithCustomDescriptionAttribute = "Description in custom Description attribute";
        public const string MemberWithImposterDescriptionAttribute = "Member with imposter description attribute";
        public const string MemberWithCustomPropertyAttribute = "Description in custom property attribute";
        public const string MemberWithoutDescriptionAttributeSentence = "Member without description attribute";
        public const string MemberWithoutDescriptionAttributeTitle = "Member Without Description Attribute";
        public const string MemberWithoutDescriptionAttributeLowerCase = "member without description attribute";
        public const string MemberWithDisplayAttribute = "Description from Display attribute";
        public const string MemberWithDisplayAttributeWithoutDescription = "Displayattribute without description";
        public static string MemberWithLocalizedDisplayAttribute { get { return "Localized description from Display attribute"; } }
    }

    public class ImposterDescriptionAttribute : Attribute
    {
        public int Description { get; set; }

        public ImposterDescriptionAttribute(int description)
        {
            Description = description;
        }
    }

    public class CustomDescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public CustomDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

#if !NETFX_CORE
    public class DescriptionSubclassAttribute : DescriptionAttribute
    {
        public DescriptionSubclassAttribute(string description) : base(description)
        {
        }

        public override string Description
        {
            get { return "Overridden " + base.Description; }
        }
    }
#endif

    public class CustomPropertyAttribute : Attribute
    {
        public string Info { get; set; }

        public CustomPropertyAttribute(string info)
        {
            Info = info;
        }
    }
}
