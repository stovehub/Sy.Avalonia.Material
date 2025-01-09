namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Defines the behavior types of a <see cref="Card"/>.
/// </summary>
public enum CardBehavior {
	/// <summary>
	/// The card acts as a simple container, and does not respond to user interaction.
	/// </summary>
	Static = 0,

	/// <summary>
	/// The card responds to user interaction and processes inputs, similar to a <see cref="Button"/>.
	/// </summary>
	Actionable = 1,

	/// <summary>
	/// Extending from <see cref="Actionable"/>, the card can also be dragged.
	/// </summary>
	Draggable = 2,
}
