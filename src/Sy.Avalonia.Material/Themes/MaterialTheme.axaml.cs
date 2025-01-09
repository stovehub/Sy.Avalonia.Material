using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System.Diagnostics;

namespace Sy.Avalonia.Material.Themes;

public sealed class MaterialTheme : Styles, IResourceNode {
	private const string DefaultVariantKey = "Light";

	private readonly ResourceDictionary _defaultSchemeDark;
	private readonly ResourceDictionary _defaultSchemeLight;

	public MaterialTheme() : this(null) {
		//
	}

	public MaterialTheme(IServiceProvider? serviceProvider) {
		AvaloniaXamlLoader.Load(serviceProvider, this);

		_defaultSchemeDark = GetAndRemove<ResourceDictionary>("DefaultSchemeTokensDark");
		_defaultSchemeLight = GetAndRemove<ResourceDictionary>("DefaultSchemeTokensLight");
	}

	bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value) {
		var baseSuccess = TryGetResource(key, theme, out value);
		if (baseSuccess) return true;

		var variantKey = GetCurrentVariantKey();
		if (!IsSupportedVariantKey(variantKey)) return false;

		var resources = variantKey switch {
			"Dark" => _defaultSchemeDark,
			_ => _defaultSchemeLight,
		};
		return resources.TryGetValue(key, out value);
	}

	private T GetAndRemove<T>(string key) {
		var val = Resources[key] ?? throw new UnreachableException(key);
		if (val is not T t) throw new UnreachableException(key);
		Resources.Remove(key);
		return t;
	}

	private string GetCurrentVariantKey() {
		var actualVariantObj = Application.Current?.ActualThemeVariant.Key;
		if (actualVariantObj is string actualVariant) {
			return actualVariant;
		}
		return DefaultVariantKey;
	}

	private bool IsSupportedVariantKey(string variantKey) {
		if (variantKey == "Light") return true;
		if (variantKey == "Dark") return true;
		return false;
	}

}
