using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Material.Icons;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties related to Material <see cref="Button">Buttons</see>.
/// </summary>
/// <remarks>
/// When used for a <see cref="ToggleButton"/>, these values assume the bytton is in the 
/// <c>unchecked</c> state.  Other state icons can be set using the attached properties offered by 
/// <see cref="ToggleButtonHelper"/>.
/// </remarks>
public sealed class ButtonHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="IconProperty"/>.
	/// </summary>
	public const string IconPropertyName = "Icon";

	/// <summary>
	/// The property name of <see cref="IsIconVisibleProperty"/>.
	/// </summary>
	public const string IsIconVisiblePropertyName = "IsIconVisible";

	/// <summary>
	/// The <see cref="Style"/> class name applied when <c>IsIconVisible</c> is 
	/// <see langword="true"/>.
	/// </summary>
	/// <remarks>
	/// <b>NOTE</b>: This style class is only enabled for the <see cref="Button"/> type, excluding 
	/// types derived from it.
	/// </remarks>
	public const string HasIconStyleClassName = "hasicon";

	/// <summary>
	/// Defines the <c>Icon</c> property.
	/// </summary>
	public static readonly AttachedProperty<MaterialIconKind> IconProperty =
		AvaloniaProperty.RegisterAttached<ButtonHelper, Button, MaterialIconKind>(
			IconPropertyName, MaterialIconKind.Add);

	/// <summary>
	/// Defines the <c>IsIconVisible</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> IsIconVisibleProperty =
		AvaloniaProperty.RegisterAttached<ButtonHelper, Button, bool>(
			IsIconVisiblePropertyName, false);

	static ButtonHelper() {
		IsIconVisibleProperty.Changed.AddClassHandler<Button>(OnIsIconVisibleChanged);
	}

	public ButtonHelper() {
		//
	}

	/// <summary>
	/// Gets the icon displayed in the button.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The displayed icon.
	/// </returns>
	public static MaterialIconKind GetIcon(AvaloniaObject element) {
		return element.GetValue(IconProperty);
	}

	/// <summary>
	/// Gets whether the icon is visible.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the icon is visible; otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GetIsIconVisible(AvaloniaObject element) {
		return element.GetValue(IsIconVisibleProperty);
	}

	/// <summary>
	/// Sets the icon to display in the button.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The icon to display.
	/// </param>
	public static void SetIcon(AvaloniaObject element, MaterialIconKind value) {
		element.SetValue(IconProperty, value);
	}

	/// <summary>
	/// Sets whether the icon is visible.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// <see langword="true"/> if the icon should be visible; otherwise, <see langword="false"/>.
	/// </param>
	public static void SetIsIconVisible(AvaloniaObject element, bool value) {
		element.SetValue(IsIconVisibleProperty, value);
	}

	private static void OnIsIconVisibleChanged(Button button, AvaloniaPropertyChangedEventArgs args) {
		UpdateClasses(button);
	}

	private static void UpdateClasses(Button button) {
		if (button.GetType() != typeof(Button)) return;

		button.Classes.Remove(HasIconStyleClassName);

		var isIconVisible = GetIsIconVisible(button);
		if (!isIconVisible) return;

		button.Classes.Add(HasIconStyleClassName);
	}
}
