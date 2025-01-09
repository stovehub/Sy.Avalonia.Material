using Avalonia;
using Avalonia.Controls;
using Sy.Avalonia.Material.Media;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties related to controls that derive from <see cref="ContentControl"/>.
/// </summary>
public sealed class ControlHelper : AvaloniaObject {
	/// <summary>
	/// The property name of <see cref="ContentCharacterCasingProperty"/>
	/// </summary>
	public const string ContentCharacterCasingPropertyName = "ContentCharacterCasing";

	/// <summary>
	/// The property name of <see cref="IsMaterialAnimationEnabledProperty"/>.
	/// </summary>
	public const string IsMaterialAnimationEnabledPropertyName = "IsMaterialAnimationsEnabled";

	/// <summary>
	/// Defines the <c>ContentCharacterCasing</c> property.
	/// </summary>
	public static readonly AttachedProperty<Casing> ContentCharacterCasingProperty =
		AvaloniaProperty.RegisterAttached<ControlHelper, AvaloniaObject, Casing>(
			ContentCharacterCasingPropertyName, Casing.Normal);

	/// <summary>
	/// Defines the <c>IsMaterialAnimationsEnabled</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> IsMaterialAnimationsEnabledProperty =
		AvaloniaProperty.RegisterAttached<ControlHelper, AvaloniaObject, bool>(
			IsMaterialAnimationEnabledPropertyName, defaultValue: true, inherits: true);

	public ControlHelper() {
		//
	}

	/// <summary>
	/// Gets the character casing for the control's content.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// The content character casing.
	/// </returns>
	public static Casing GetContentCharacterCasing(AvaloniaObject element) {
		return element.GetValue(ContentCharacterCasingProperty);
	}

	/// <summary>
	/// Gets whether Material theme-based control animations are enebaled.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property of.
	/// </param>
	/// <returns>
	/// Whether animations are enabled.
	/// </returns>
	public static bool GetIsMaterialAnimationsEnabled(AvaloniaObject element) {
		return element.GetValue(IsMaterialAnimationsEnabledProperty);
	}

	/// <summary>
	/// Sets the character casing of the control's content.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <param name="value">
	/// The content character casing.
	/// </param>
	public static void SetContentCharacterCasing(AvaloniaObject element, Casing value) {
		element.SetValue(ContentCharacterCasingProperty, value);
	}

	/// <summary>
	/// Sets whether Material theme-based control animations are enabled.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property of.
	/// </param>
	/// <param name="value">
	/// Whether animations should be enabled.
	/// </param>
	public static void SetIsMaterialAnimationsEnabled(AvaloniaObject element, bool value) {
		element.SetValue(IsMaterialAnimationsEnabledProperty, value);
	}
}
