using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// A versatile container that contains content and actions about a single subject.
/// </summary>
/// <see href="https://m3.material.io/components/cards/overview">
/// Cards — Material Design 3
/// </see>
[PseudoClasses(PseudoClassDragged)]
public class Card : Button {
	//private bool _isDragged;

	/*
	 * NOTE: Drag functionality is currently a work in progress.
	 */
	private const string PseudoClassDragged = "dragged";

	/// <summary>
	/// Defines the <see cref="Behavior"/> property.
	/// </summary>
	public static readonly StyledProperty<CardBehavior> BehaviorProperty =
		AvaloniaProperty.Register<Card, CardBehavior>(nameof(Behavior));

	/// <summary>
	/// Defines the <see cref="ClipToBounds"/> property.
	/// </summary>
	public static new readonly StyledProperty<bool> ClipToBoundsProperty =
		AvaloniaProperty.Register<Card, bool>(nameof(ClipToBounds));

	/// <summary>
	/// Gets or sets how the card responds to user interaction.
	/// </summary>
	public CardBehavior Behavior {
		get => GetValue(BehaviorProperty);
		set => SetValue(BehaviorProperty, value);
	}

	/// <inheritdoc cref="Visual.ClipToBounds"/>
	public new bool ClipToBounds {
		get => GetValue(ClipToBoundsProperty);
		set => SetValue(ClipToBoundsProperty, value);
	}

	protected override bool IsEnabledCore {
		get {
			if (base.IsEnabledCore) return true;

			var isStatic = Behavior == CardBehavior.Static;
			if (isStatic) return true;

			return false; ;
		}
	}

	static Card() {
		Visual.ClipToBoundsProperty.OverrideDefaultValue<Card>(false);
	}

	/// <summary>
	/// Creates a new <see cref="Card"/> instance.
	/// </summary>
	public Card() {
		//
	}

	protected override void OnClick() {
		if (Behavior == CardBehavior.Static) return;

		base.OnClick();
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e) {
		base.OnPointerPressed(e);

		if (Behavior == CardBehavior.Static) {
			e.Handled = false;
		}
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == BehaviorProperty) {
			UpdateIsEffectivelyEnabled();
			return;
		}
	}
}
