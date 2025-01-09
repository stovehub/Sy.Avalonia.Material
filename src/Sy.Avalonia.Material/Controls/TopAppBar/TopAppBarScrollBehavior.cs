namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Enumerates how a <see cref="TopAppBar"/> responds when the user scrolls the inner content of a 
/// <see cref="Scaffold"/>.
/// </summary>
public enum TopAppBarScrollBehavior {
	/// <summary>
	/// Similar to <see cref="ExitUntilCollapsed"/>, except the app bar will expand when the user 
	/// pulls the scaffold's inner content down, regardless of the position.
	/// </summary>
	/// <remarks>
	/// The app bar will only expand to a compact state, unless the inner content has been pulled 
	/// all the way down.
	/// </remarks>
	EnterAlways,

	/// <summary>
	/// When the user pulls up the scaffold's inner content, the top app bar collapses.
	/// </summary>
	/// <remarks>
	/// The app bar will only expand when the user pulls the scaffold's inner content all the way 
	/// back down.
	/// </remarks>
	ExitUntilCollapsed,

	/// <summary>
	/// The app bar remains in place, but may partially collapse to a compact state when the user 
	/// pulls the scaffold's inner content up.
	/// </summary>
	Pinned,
}
