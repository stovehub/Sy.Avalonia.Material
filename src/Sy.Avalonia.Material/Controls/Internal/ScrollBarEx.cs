using Avalonia;
using Avalonia.Controls.Primitives;
using System.Reflection;

namespace Sy.Avalonia.Material.Controls;

internal sealed class ScrollBarEx : ScrollBar {
	private static readonly MethodInfo? s_collapseAfterDelayMethod;
	private static readonly MethodInfo? s_expandMethod;

	protected override Type StyleKeyOverride => typeof(ScrollBar);

	static ScrollBarEx() {
		var type = typeof(ScrollBar);
		var privateMethodFlags = BindingFlags.NonPublic | BindingFlags.Instance;

		s_expandMethod = type.GetMethod("Expand", privateMethodFlags);
		s_collapseAfterDelayMethod = type.GetMethod("CollapseAfterDelay", privateMethodFlags);
	}

	public ScrollBarEx() {
		//
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == ValueProperty) {
			Expand();
			if (!AllowAutoHide) return;
			CollapseAfterDelay();
			return;
		}
	}

	private void CollapseAfterDelay() {
		s_collapseAfterDelayMethod?.Invoke(this, null);
	}

	private void Expand() {
		s_expandMethod?.Invoke(this, null);
	}
}
