using Avalonia;
using Avalonia.Controls;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties pertaining to Text.
/// </summary>
/// <remarks>
/// The <c>BaselineOffset</c> property may become obsolete in the future, as 
/// <see cref="TextBlock.BaselineOffset"/> may be implemented / fixed in later versions of 
/// Avalonia.  As of v11, this is not the case.
/// </remarks>
public class TextHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="BaselineOffsetProperty"/>.
	/// </summary>
	public const string BaselineOffsetPropertyName = "BaselineOffset";

	/// <summary>
	/// Defines the <c>BaselineOffset</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> BaselineOffsetProperty =
		AvaloniaProperty.RegisterAttached<TextHelper, Control, double>(
			BaselineOffsetPropertyName, inherits:true);

	static TextHelper() {
		BaselineOffsetProperty.Changed.AddClassHandler<TextBlock>(OnBaselineOffsetChanged);
	}

	public TextHelper() {
		//
	}

	/// <summary>
	/// Gets the inheritable baseline offset of the element.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property value of.
	/// </param>
	/// <returns>
	/// The baseline offset.
	/// </returns>
	public static double GetBaselineOffset(AvaloniaObject element) {
		return element.GetValue(BaselineOffsetProperty);
	}

	/// <summary>
	/// Sets the inheritable baseline offset of the element.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The baseline offset.
	/// </param>
	public static void SetBaselineOffset(AvaloniaObject element, double value) {
		element.SetValue(BaselineOffsetProperty, value);
	}

	private static void OnBaselineOffsetChanged(TextBlock textBlock, AvaloniaPropertyChangedEventArgs args) {
		var newValue = args.GetNewValue<double>();
		var padding = new Thickness(0, -newValue, 0, newValue);
		textBlock.Padding = padding;
	}
}
