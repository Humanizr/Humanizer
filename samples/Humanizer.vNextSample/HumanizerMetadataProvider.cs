using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Humanizer;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;

namespace Humanizer.vNextSample
{
    public class HumanizerMetadataProvider : IDisplayMetadataProvider
    {
        private static bool IsTransformRequired(string propertyName, DisplayMetadata modelMetadata, IReadOnlyList<object> propertyAttributes)
        {
            if (!string.IsNullOrEmpty(modelMetadata.SimpleDisplayProperty))
                return false;

            if (propertyAttributes.OfType<DisplayNameAttribute>().Any())
                return false;

            if (propertyAttributes.OfType<DisplayAttribute>().Any())
                return false;

            if (string.IsNullOrEmpty(propertyName))
                return false;
            
            return true;
        }

        /// <summary>
        /// Gets the values for properties of <see cref="T:Microsoft.AspNet.Mvc.ModelBinding.Metadata.DisplayMetadata"/>. 
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNet.Mvc.ModelBinding.Metadata.DisplayMetadataProviderContext"/>.</param>
        public void GetDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var propertyAttributes = context.Attributes;
            var modelMetadata = context.DisplayMetadata;
            var propertyName = context.Key.Name;

            if (IsTransformRequired(propertyName, modelMetadata, propertyAttributes))
                modelMetadata.DisplayName = propertyName.Humanize().Transform(To.TitleCase);
        }
    }
}