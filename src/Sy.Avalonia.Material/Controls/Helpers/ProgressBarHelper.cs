using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties pertaining to a <see cref="ProgressBar"/>.
/// </summary>
public class ProgressBarHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="CircularStrokeLineCapProperty"/>.
	/// </summary>
	public const string CircularStrokeLineCapPropertyName = "CircularStrokeLineCap";

	/// <summary>
	/// The property name of <see cref="CircularStrokeThicknessProperty"/>.
	/// </summary>
	public const string CircularStrokeThicknessPropertyName = "CircularStrokeLineCap";

	/// <summary>
	/// The property name of <see cref="IsStopIndicatorVisibleProperty"/>.
	/// </summary>
	public const string IsStopIndicatorVisiblePropertyName = "IsStopIndicatorVisible";

	/// <summary>
	/// The property name of <see cref="SpacingProperty"/>.
	/// </summary>
	public const string SpacingPropertyName = "Spacing";

	/// <summary>
	/// Defines the <c>CircularStrokeLineCap</c> property.
	/// </summary>
	public static readonly AttachedProperty<PenLineCap> CircularStrokeLineCapProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, PenLineCap>(
			CircularStrokeLineCapPropertyName, PenLineCap.Round);

	/// <summary>
	/// Defines the <c>CircularStrokeThickness</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> CircularStrokeThicknessProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			CircularStrokeThicknessPropertyName, defaultValue: 4);

	/// <summary>
	/// Defines the <c>IsStopIndicatorVisible</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> IsStopIndicatorVisibleProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelper, ProgressBar, bool>(
			IsStopIndicatorVisiblePropertyName, defaultValue: true);

	/// <summary>
	/// Defines the <c>Spacing</c> property.
	/// </summary>
	public static readonly AttachedProperty<double> SpacingProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelper, ProgressBar, double>(
			SpacingPropertyName, defaultValue: 4d, validate: ValidateSpacing);

	public ProgressBarHelper() {
		//
	}

	/// <summary>
	/// Gets the stroke line cap applied to circular progress bars.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The stroke line cap.
	/// </returns>
	public static PenLineCap GetCircularStrokeLineCap(AvaloniaObject element) {
		return element.GetValue(CircularStrokeLineCapProperty);
	}

	/// <summary>
	/// Gets the stroke thickness applied to circular progress bars.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The stroke thickness.
	/// </returns>
	public static double GetCircularStrokeThickness(AvaloniaObject element) {
		return element.GetValue(CircularStrokeThicknessProperty);
	}

	/// <summary>
	/// Gets whether the stop indicator is visible.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the stop indicator is visible; otherwise, 
	/// <see langword="false"/>.
	/// </returns>
	public static bool GetIsStopIndicatorVisible(AvaloniaObject element) {
		return element.GetValue(IsStopIndicatorVisibleProperty);
	}

	/// <summary>
	/// Gets the spacing between the indicator and track.
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
	/// Sets the stroke line cap applied to circular progress bars.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <returns>
	/// The stroke line cap.
	/// </returns>
	public static void SetCircularStrokeLineCap(AvaloniaObject element, PenLineCap value) {
		element.SetValue(CircularStrokeLineCapProperty, value);
	}

	/// <summary>
	/// Sets the stroke thickness applied to circular progress bars.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <returns>
	/// The stroke thickness.
	/// </returns>
	public static void SetCircularStrokeThickness(AvaloniaObject element, double value) {
		element.SetValue(CircularStrokeThicknessProperty, value);
	}

	/// <summary>
	/// Sets whether the stop indicator is visible.
	/// </summary>
	/// <param name="element">
	/// The element to Set the attached property of.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the stop indicator should be visible; otherwise, 
	/// <see langword="false"/>.
	/// </returns>
	public static void SetIsStopIndicatorVisible(AvaloniaObject element, bool value) {
		element.SetValue(IsStopIndicatorVisibleProperty, value);
	}

	/// <summary>
	/// Sets the spacing between the indicator and track.
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
		if (value > 4d) return false;
		return true;
	}
}
