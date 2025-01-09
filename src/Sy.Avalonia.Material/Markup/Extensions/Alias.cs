using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections;

namespace Sy.Avalonia.Material.Markup.Extensions;

/// <summary>
/// An alias for a resource in a <see cref="ResourceDictionary"/>.
/// </summary>
public sealed class Alias : MarkupExtension {
	/// <summary>
	/// The key of the referenced item.
	/// </summary>
	public string? ResourceKey { get; set; }

	public Alias() {
		
	}

	public override object ProvideValue(IServiceProvider serviceProvider) {
		return _ProvideLocalValue(serviceProvider) ?? ProvideApplicationValue()!;
	}

	private object? _ProvideLocalValue(IServiceProvider serviceProvider) {
		var roProviderObj = serviceProvider.GetService(typeof(IRootObjectProvider));
		if (roProviderObj is not IDictionary roProviderDict) return null;
		return roProviderDict.Contains(ResourceKey!) ? roProviderDict[ResourceKey!] : null;
	}

	private object? ProvideApplicationValue() {
		return Application.Current?.FindResource(ResourceKey!);
	}
}
