using Avalonia.Controls;
using MaterialColorUtilities.Schemes;

namespace Sy.Avalonia.Material.Utils;

public static class ResourceDictionaryExt
{
    public static ResourceDictionary CreateResourceDictionary(ResourceDictionary oldResource, Scheme<uint> scheme)
    {
        Scheme<string> schemeString = scheme.Convert(x => "#" + x.ToString("X")[2..]);
        oldResource["md.sys.color.primary"] = schemeString.Primary.ParseColor();
        oldResource["md.sys.color.on-primary"] = schemeString.OnPrimary.ParseColor();
        oldResource["md.sys.color.primary-container"] = schemeString.PrimaryContainer.ParseColor();
        oldResource["md.sys.color.on-primary-container"] = schemeString.OnPrimaryContainer.ParseColor();
        oldResource["md.sys.color.secondary"] = schemeString.Secondary.ParseColor();
        oldResource["md.sys.color.on-secondary"] = schemeString.OnSecondary.ParseColor();
        oldResource["md.sys.color.secondary-container"] = schemeString.SecondaryContainer.ParseColor();
        oldResource["md.sys.color.on-secondary-container"] = schemeString.OnSecondaryContainer.ParseColor();
        oldResource["md.sys.color.tertiary"] = schemeString.Tertiary.ParseColor();
        oldResource["md.sys.color.on-tertiary"] = schemeString.OnTertiary.ParseColor();
        oldResource["md.sys.color.tertiary-container"] = schemeString.TertiaryContainer.ParseColor();
        oldResource["md.sys.color.on-tertiary-container"] = schemeString.OnTertiaryContainer.ParseColor();
        oldResource["md.sys.color.error"] = schemeString.OnError.ParseColor();
        oldResource["md.sys.color.on-error"] = schemeString.OnError.ParseColor();
        oldResource["md.sys.color.error-container"] = schemeString.ErrorContainer.ParseColor();
        oldResource["md.sys.color.on-error-container"] = schemeString.OnErrorContainer.ParseColor();
        oldResource["md.sys.color.background"] = schemeString.Background.ParseColor();
        oldResource["md.sys.color.on-background"] = schemeString.OnBackground.ParseColor();
        oldResource["md.sys.color.surface"] = schemeString.Surface.ParseColor();
        oldResource["md.sys.color.on-surface"] = schemeString.OnSurface.ParseColor();
        oldResource["md.sys.color.surface-variant"] = schemeString.SurfaceVariant.ParseColor();
        oldResource["md.sys.color.on-surface-variant"] = schemeString.OnSurfaceVariant.ParseColor();
        oldResource["md.sys.color.outline"] = schemeString.Outline.ParseColor();
        oldResource["md.sys.color.outline-variant"] = schemeString.OutlineVariant.ParseColor();
        oldResource["md.sys.color.shadow"] = schemeString.Shadow.ParseColor();
        oldResource["md.sys.color.inverse-surface"] = schemeString.Surface.ParseColor();
        oldResource["md.sys.color.inverse-on-surface"] = schemeString.OnSurface.ParseColor();
        oldResource["md.sys.color.inverse-primary"] = schemeString.InversePrimary.ParseColor();
        oldResource["md.sys.color.surface-dim"] = schemeString.SurfaceDim.ParseColor();
        oldResource["md.sys.color.surface-bright"] = schemeString.SurfaceBright.ParseColor();
        oldResource["md.sys.color.surface-container-low"] = schemeString.SurfaceContainerLow.ParseColor();
        oldResource["md.sys.color.surface-container-lowest"] = schemeString.SurfaceContainerLowest.ParseColor();
        oldResource["md.sys.color.surface-container"] = schemeString.SurfaceContainer.ParseColor();
        oldResource["md.sys.color.surface-container-high"] = schemeString.SurfaceContainerHigh.ParseColor();
        oldResource["md.sys.color.surface-container-highest"] = schemeString.SurfaceContainerHighest.ParseColor();

        var newDict = new ResourceDictionary();
        foreach (var kvp in oldResource)
        {
            newDict.Add(kvp.Key, kvp.Value);
        }

        return newDict;
    }
}