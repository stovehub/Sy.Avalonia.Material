using Avalonia;
using Avalonia.Controls.Primitives;
using Material.Icons;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties related to Material <see cref="ToggleButton">ToggleButtons</see>.
/// </summary>
/// <remarks>
/// These properties are used in conjunction with the attached properties offered by 
/// <see cref="ButtonHelper"/>.
/// </remarks>
public sealed class ToggleButtonHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="CheckedIconProperty"/>.
	/// </summary>
	public const string CheckedIconPropertyName = "CheckedIcon";

	/// <summary>
	/// The property name of <see cref="IndeterminateIconProperty"/>.
	/// </summary>
	public const string IndeterminateIconPropertyName = "IndeterminateIcon";

	/// <summary>
	/// The property name of <see cref="IsCheckedIconVisibleProperty"/>.
	/// </summary>
	public const string IsCheckedIconVisiblePropertyName = "IsCheckedIconVisible";

	/// <summary>
	/// The property name of <see cref="IsIndeterminateIconVisibleProperty"/>.
	/// </summary>
	public const string IsIndeterminateIconVisiblePropertyName = "IsIndeterminateIconVisible";

	/// <summary>
	/// Defines the <c>CheckedIcon</c> property.
	/// </summary>
	public static readonly AttachedProperty<MaterialIconKind> CheckedIconProperty =
		AvaloniaProperty.RegisterAttached<ToggleButtonHelper, ToggleButton, MaterialIconKind>(
			CheckedIconPropertyName, MaterialIconKind.Check);

	/// <summary>
	/// Defines the <c>IndeterminateIcon</c> property.
	/// </summary>
	public static readonly AttachedProperty<MaterialIconKind> IndeterminateIconProperty =
		AvaloniaProperty.RegisterAttached<ToggleButtonHelper, ToggleButton, MaterialIconKind>(
			IndeterminateIconPropertyName, MaterialIconKind.QuestionMark);

	/// <summary>
	/// Defines the <c>IsCheckedIconVisible</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> IsCheckedIconVisibleProperty =
		AvaloniaProperty.RegisterAttached<ToggleButtonHelper, ToggleButton, bool>(
			IsCheckedIconVisiblePropertyName, false);

	/// <summary>
	/// Defines the <c>IsIndeterminateIconVisible</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> IsIndeterminateIconVisibleProperty =
		AvaloniaProperty.RegisterAttached<ToggleButtonHelper, ToggleButton, bool>(
			IsIndeterminateIconVisiblePropertyName, false);

	public ToggleButtonHelper() {
		//
	}

	/// <summary>
	/// Gets the icon displayed in the button when in the <c>checked</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The displayed icon.
	/// </returns>
	public static MaterialIconKind GetCheckedIcon(AvaloniaObject element) {
		return element.GetValue(CheckedIconProperty);
	}

	/// <summary>
	/// Gets the icon displayed in the button when in the <c>indeterminate</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The displayed icon.
	/// </returns>
	public static MaterialIconKind GetIndeterminateIcon(AvaloniaObject element) {
		return element.GetValue(IndeterminateIconProperty);
	}

	/// <summary>
	/// Gets whether the icon is visible when in the <c>checked</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the icon is visible; otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GetIsCheckedIconVisible(AvaloniaObject element) {
		return element.GetValue(IsCheckedIconVisibleProperty);
	}

	/// <summary>
	/// Gets whether the icon is visible when in the <c>indeterminate</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the icon is visible; otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GetIsIndeterminateIconVisible(AvaloniaObject element) {
		return element.GetValue(IsIndeterminateIconVisibleProperty);
	}

	/// <summary>
	/// Sets the icon to display in the button when in the <c>checked</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The icon to display.
	/// </param>
	public static void SetCheckedIcon(AvaloniaObject element, MaterialIconKind value) {
		element.SetValue(CheckedIconProperty, value);
	}

	/// <summary>
	/// Sets the icon to display in the button when in the <c>indeterminate</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The icon to display.
	/// </param>
	public static void SetIndeterminateIcon(AvaloniaObject element, MaterialIconKind value) {
		element.SetValue(IndeterminateIconProperty, value);
	}

	/// <summary>
	/// Sets whether the icon is visible when in the <c>checked</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// <see langword="true"/> if the icon should be visible; otherwise, <see langword="false"/>.
	/// </param>
	public static void SetIsCheckedIconVisible(AvaloniaObject element, bool value) {
		element.SetValue(IsCheckedIconVisibleProperty, value);
	}

	/// <summary>
	/// Sets whether the icon is visible when in the <c>indeterminate</c> state.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// <see langword="true"/> if the icon should be visible; otherwise, <see langword="false"/>.
	/// </param>
	public static void SetIsIndeterminateIconVisible(AvaloniaObject element, bool value) {
		element.SetValue(IsIndeterminateIconVisibleProperty, value);
	}
}
