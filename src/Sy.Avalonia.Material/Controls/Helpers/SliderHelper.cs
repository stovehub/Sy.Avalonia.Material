using Avalonia;
using Avalonia.Controls;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties pertaining to a <see cref="Slider"/>.
/// </summary>
public class SliderHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="HandleLengthProperty"/>.
	/// </summary>
	public const string HandleLengthPropertyName = "HandleLength";

	/// <summary>
	/// The property name of <see cref="HandleThicknessProperty"/>.
	/// </summary>
	public const string HandleThicknessPropertyName = "HandleThickness";

	/// <summary>
	/// The property name of <see cref="SpacingProperty"/>.
	/// </summary>
	public const string SpacingPropertyName = "Spacing";

	/// <summary>
	/// The property name of <see cref="TrackThicknessProperty"/>.
	/// </summary>
	public const string TrackThicknessPropertyName = "TrackThickness";

	/// <summary>
	/// Defines the <c>HandleLength</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> HandleLengthProperty =
		AvaloniaProperty.RegisterAttached<SliderHelper, Slider, double>(
			HandleLengthPropertyName, defaultValue: 44);

	/// <summary>
	/// Defines the <c>HandleThickness</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> HandleThicknessProperty =
		AvaloniaProperty.RegisterAttached<SliderHelper, Slider, double>(
			HandleThicknessPropertyName, defaultValue: 4);

	/// <summary>
	/// Defines the <c>Spacing</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> SpacingProperty =
		AvaloniaProperty.RegisterAttached<SliderHelper, Slider, double>(
			SpacingPropertyName, defaultValue: 6d, validate: ValidateSpacing);

	/// <summary>
	/// Defines the <c>TrackThickness</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> TrackThicknessProperty =
		AvaloniaProperty.RegisterAttached<SliderHelper, Slider, double>(
			TrackThicknessPropertyName, defaultValue: 16);

	public SliderHelper() {
		//
	}

	public static double GetHandleLength(AvaloniaObject element) {
		return element.GetValue(HandleLengthProperty);
	}

	public static double GetHandleThickness(AvaloniaObject element) {
		return element.GetValue(HandleThicknessProperty);
	}

	/// <summary>
	/// Gets the spacing between the handle and track.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The spacing.
	/// </returns>
	public static double GetSpacing(AvaloniaObject element) {
		return element.GetValue(SpacingProperty);
	}

	/// <summary>
	/// Sets the spacing between the handle and track.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <param name="value">
	/// The spacing.
	/// </param>
	public static void SetSpacing(AvaloniaObject element, double value) {
		element.SetValue(SpacingProperty, value);
	}

	private static bool ValidateSpacing(double value) {
		if (value < 0d) return false;
		if (value > 6d) return false;
		return true;
	}
}
