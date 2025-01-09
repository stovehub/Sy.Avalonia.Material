using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Sy.Avalonia.Material.Media;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties related to shadows representing the <see cref="Elevation"/> of 
/// Material controls.
/// </summary>
/// <remarks>
/// Shadows can be directly applied to the following types:
/// <list type="bullet">
/// <item><see cref="Border"/></item>
/// <item><see cref="ContentPresenter"/></item>
/// </list>
/// </remarks>
public sealed class ShadowHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="ColorProperty"/>.
	/// </summary>
	public const string ColorPropertyName = "Color";

	/// <summary>
	/// The property name of <see cref="ElevationProperty"/>.
	/// </summary>
	public const string ElevationPropertyName = "Elevation";

	/// <summary>
	/// Defines the <c>Color</c> property.
	/// </summary>
	public static readonly AttachedProperty<Color> ColorProperty =
		AvaloniaProperty.RegisterAttached<ShadowHelper, AvaloniaObject, Color>(
			ColorPropertyName);

	/// <summary>
	/// Defines the <c>Elevation</c> property.
	/// </summary>
	public static readonly AttachedProperty<Elevation> ElevationProperty =
		AvaloniaProperty.RegisterAttached<ShadowHelper, AvaloniaObject, Elevation>(
			ElevationPropertyName);

	static ShadowHelper() {
		ColorProperty.Changed.AddClassHandler<Border>(OnColorChanged);
		ColorProperty.Changed.AddClassHandler<ContentPresenter>(OnColorChanged);
		ElevationProperty.Changed.AddClassHandler<Border>(OnElevationChanged);
		ElevationProperty.Changed.AddClassHandler<ContentPresenter>(OnElevationChanged);
	}

	public ShadowHelper() {
		//
	}

	/// <summary>
	/// Gets the color of the shadow.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property value of.
	/// </param>
	/// <returns>
	/// The shadow color.
	/// </returns>
	public static Color GetColor(AvaloniaObject element) {
		return element.GetValue(ColorProperty);
	}

	/// <summary>
	/// Gets the elevation the shadow represents.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The elevation represented by the shadow.
	/// </returns>
	public static Elevation GetElevation(AvaloniaObject element) {
		return element.GetValue(ElevationProperty);
	}

	/// <summary>
	/// Sets the color of the shadow.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <param name="value">
	/// The shadow color.
	/// </param>
	public static void SetColor(AvaloniaObject element, Color value) {
		element.SetValue(ColorProperty, value);
	}

	/// <summary>
	/// Sets the elevation the shadow represents.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <param name="value">
	/// The elevation represented by the shadow.
	/// </param>
	public static void SetElevation(AvaloniaObject element, Elevation value) {
		element.SetValue(ElevationProperty, value);
	}

	private static void OnColorChanged(Border control, AvaloniaPropertyChangedEventArgs args) {
		var newValue = args.GetNewValue<Color>();
		var elevation = control.GetValue(ElevationProperty);

		var shadows = elevation.ToBoxShadows(newValue);
		control.BoxShadow = shadows;
	}

	private static void OnColorChanged(ContentPresenter control, AvaloniaPropertyChangedEventArgs args) {
		var newValue = args.GetNewValue<Color>();
		var elevation = control.GetValue(ElevationProperty);

		var shadows = elevation.ToBoxShadows(newValue);
		control.BoxShadow = shadows;
	}

	private static void OnElevationChanged(Border control, AvaloniaPropertyChangedEventArgs args) {
		var newValue = args.GetNewValue<Elevation>();
		var color = control.GetValue(ColorProperty);

		var shadows = newValue.ToBoxShadows(color);
		control.BoxShadow = shadows;
	}

	private static void OnElevationChanged(ContentPresenter control, AvaloniaPropertyChangedEventArgs args) {
		var newValue = args.GetNewValue<Elevation>();
		var color = control.GetValue(ColorProperty);

		var shadows = newValue.ToBoxShadows(color);
		control.BoxShadow = shadows;
	}
}
