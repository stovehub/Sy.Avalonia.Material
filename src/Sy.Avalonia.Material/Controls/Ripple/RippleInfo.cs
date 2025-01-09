using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Media;

namespace Sy.Avalonia.Material.Controls;

internal sealed class RippleInfo {
	public required IImmutableBrush Brush { get; init; }

	public RoundedRect Clip { get; init; }

	public required Easing Easing { get; init; }

	public required TimeSpan FadeDuration { get; init; }
	
	public required double MaxRadius { get; init; }

	public required double Opacity { get; init; }

	public required Point Origin { get; init; }

	public required TimeSpan SpreadDuration { get; init; }

	public RippleInfo() {
		//
	}
}
