using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System.Diagnostics;
using Avalonia.Media;
using MaterialColorUtilities.Palettes;
using MaterialColorUtilities.Schemes;
using MaterialColorUtilities.Utils;
using Sy.Avalonia.Material.Utils;

namespace Sy.Avalonia.Material.Themes;

public partial class MaterialTheme : Styles, IResourceNode
{
    private readonly Application _app;

    private const string DefaultVariantKey = "Light";

    private readonly ResourceDictionary _defaultSchemeDark;
    private readonly ResourceDictionary _defaultSchemeLight;

    public Action<Scheme<uint>>? OnColorThemeChanged { get; set; }
    public Action<ThemeVariant>? OnBaseThemeChanged { get; set; }

    
    public MaterialTheme()
    {
        AvaloniaXamlLoader.Load(this);
        _app = Application.Current!;
        _app.ActualThemeVariantChanged += (_, e) => OnBaseThemeChanged?.Invoke(_app.ActualThemeVariant);
        _defaultSchemeDark = GetAndRemove<ResourceDictionary>("DefaultSchemeTokensDark");
        _defaultSchemeLight = GetAndRemove<ResourceDictionary>("DefaultSchemeTokensLight");
    }

    public static MaterialTheme GetInstance() => GetInstance(Application.Current!);

    public static MaterialTheme GetInstance(Application app)
    {
        var theme = app.Styles.FirstOrDefault(style => style is MaterialTheme);
        if (theme is not MaterialTheme mTheme)
            throw new InvalidOperationException("aa");

        return mTheme;
    }
    /*
    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        var baseSuccess = TryGetResource(key, theme, out value);
        if (baseSuccess) return true;

        var variantKey = GetCurrentVariantKey();
        if (!IsSupportedVariantKey(variantKey)) return false;

        var resources = variantKey switch
        {
            "Dark" => _defaultSchemeDark,
            _ => _defaultSchemeLight,
        };
        return resources.TryGetValue(key, out value);
    }
    */
    public void ChangeTheme(Color seed)
    {
        var hey = ColorUtils.ArgbFromRgb(seed.R, seed.G, seed.B);
        ApplySchemes(hey);
    }

    public void SwitchBaseTheme()
    {
        if (Application.Current is null) return;
        var newBase = Application.Current.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
        Application.Current.RequestedThemeVariant = newBase;
    }

    private void ApplySchemes(uint seed)
    {
        CorePalette corePalette = CorePalette.Of(seed);
        Scheme<uint> lightScheme = new LightSchemeMapper().Map(corePalette);
        Scheme<uint> darkScheme = new DarkSchemeMapper().Map(corePalette);

        var newLightRd = ResourceDictionaryExt.CreateResourceDictionary(_defaultSchemeLight, lightScheme);
        var newDarkRd = ResourceDictionaryExt.CreateResourceDictionary(_defaultSchemeDark, darkScheme);

        _app.Resources.ThemeDictionaries[ThemeVariant.Light] = newLightRd;
        _app.Resources.ThemeDictionaries[ThemeVariant.Dark] = newDarkRd;
    }


    private T GetAndRemove<T>(string key)
    {
        var val = Resources[key] ?? throw new UnreachableException(key);
        if (val is not T t) throw new UnreachableException(key);
        Resources.Remove(key);
        return t;
    }

    private string GetCurrentVariantKey()
    {
        var actualVariantObj = Application.Current?.ActualThemeVariant.Key;
        if (actualVariantObj is string actualVariant)
        {
            return actualVariant;
        }

        return DefaultVariantKey;
    }

    private bool IsSupportedVariantKey(string variantKey)
    {
        return variantKey switch
        {
            "Light" or "Dark" => true,
            _ => false
        };
    }
}