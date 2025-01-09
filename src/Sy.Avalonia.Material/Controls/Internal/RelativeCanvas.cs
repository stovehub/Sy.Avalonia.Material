using Avalonia;
using Avalonia.Controls;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// A <see cref="Canvas"/> whose alignment properties are relative to the width of the canvas.
/// </summary>
/// <remarks>
/// Values range from <c>0.0</c> to <c>1.0</c>.
/// </remarks>
public sealed class RelativeCanvas : Canvas {
	/// <summary>
	/// Defines the <see cref="IsPositionsRelativeToOrigin"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsPositionsRelativeToOriginProperty =
		AvaloniaProperty.Register<RelativeCanvas, bool>(nameof(IsPositionsRelativeToOrigin));

	/// <summary>
	/// Gets or sets whether the canvas positions are relative to their respective edges 
	/// (<see langword="false"/>), or to the origin (<see langword="true"/>).
	/// </summary>
	/// <remarks>
	/// Default: <see langword="false"/>
	/// </remarks>
	public bool IsPositionsRelativeToOrigin {
		get => GetValue(IsPositionsRelativeToOriginProperty);
		set => SetValue(IsPositionsRelativeToOriginProperty, value);
	}

	/// <summary>
	/// Creates a new <see cref="RelativeCanvas"/> instance.
	/// </summary>
	public RelativeCanvas() {
		//
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == IsPositionsRelativeToOriginProperty) {
			InvalidateArrange();
			return;
		}
	}

	protected override void ArrangeChild(Control child, Size finalSize) {
		double x = 0.0;
		double y = 0.0;

		var left = GetLeft(child);
		var leftScaled = left * finalSize.Width;
		var right = IsPositionsRelativeToOrigin ? 1 - GetRight(child) : GetRight(child);
		var rightScaled = right * finalSize.Width;
		double childWidth;

		if (!double.IsNaN(leftScaled) && !double.IsNaN(rightScaled)) {
			x = leftScaled;
			childWidth = finalSize.Width - rightScaled - leftScaled;
		} else if (!double.IsNaN(leftScaled)) {
			x = leftScaled;
			childWidth = child.DesiredSize.Width;
		} else if (!double.IsNaN(rightScaled)) {
			x = finalSize.Width - child.DesiredSize.Width - rightScaled;
			childWidth = child.DesiredSize.Width;
		} else {
			childWidth = child.DesiredSize.Width;
		}

		var top = GetTop(child);
		var bottom = IsPositionsRelativeToOrigin ? 1 - GetBottom(child) : GetBottom(child);
		var topScaled = top * finalSize.Height;
		var bottomScaled = bottom * finalSize.Height;
		double childHeight;

		if (!double.IsNaN(topScaled) && !double.IsNaN(bottomScaled)) {
			y = topScaled;
			childHeight = finalSize.Height - bottomScaled - topScaled;
		} else if (!double.IsNaN(topScaled)) {
			y = topScaled;
			childHeight = child.DesiredSize.Height;
		} else if (!double.IsNaN(bottomScaled)) {
			y = finalSize.Height - child.DesiredSize.Height - bottomScaled;
			childHeight = child.DesiredSize.Height;
		} else {
			childHeight = child.DesiredSize.Height;
		}

		if (childWidth < 0) childWidth = 0;
		if (childHeight < 0) childHeight = 0;

		var childPosition = new Point(x, y);
		var childSize = new Size(childWidth, childHeight);
		child.Arrange(new Rect(childPosition, childSize));
	}
}
