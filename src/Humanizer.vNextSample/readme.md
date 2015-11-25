This sample project shows how [HumanizerMetadataProvider](https://github.com/mrchief/Humanizer/blob/master/src/Humanizer.vNextSample/HumanizerMetadataProvider.cs) can be integrated in your ASP.NET vNext project.

#### What's new with vNext?

vNext changes things slightly from [previous versions](https://github.com/MehdiK/Humanizer#mix-this-into-your-framework-to-simplify-your-life) of ASP.NET MVC. [Quoting](https://github.com/aspnet/Mvc/issues/2522#issuecomment-100298332) ASP.NET team's Ryan Nowak,
>  the new extensibility model is based on adding your own provider to a list, not based on subclassing or replacing a provider. Providers run in the order you put them in (in options) and each one can see and modify the results of the previous providers.

Basically this means implementing your own `IDisplayMetadataProvider` and adding it to options in during startup. Doing this allows the default `DataAnnotationsMetadataProvider` to do its thing and then runs the `HumanizerMetadataProvider` to do its magic. The order of runs depends on the order of adding them to options.

Without further ado, Here's the code:

```csharp
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

    public void GetDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var propertyAttributes = context.Attributes;
        var modelMetadata = context.DisplayMetadata;
        var propertyName = context.Key.Name;

        if (IsTransformRequired(propertyName, modelMetadata, propertyAttributes))
            modelMetadata.DisplayName = propertyName.Humanize().Transform(To.TitleCase);
    }
}
```

And adding it to options during startup can be accomplished as (normally in `Startup.cs`):

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ... other stuff

    // Add MVC services to the services container.
    services.AddMvc()
        .Configure<MvcOptions>(options => options.ModelMetadataDetailsProviders.Add(new HumanizerMetadataProvider())); ;

    // ... other stuff
}
```

This adds `HumanizerMetadataProvider` to the vNext `ModelMetadataDetailsProviders`.   


