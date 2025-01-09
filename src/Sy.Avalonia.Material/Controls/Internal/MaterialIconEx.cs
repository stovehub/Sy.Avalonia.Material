using Avalonia;
using Material.Icons;
using Material.Icons.Avalonia;

namespace Sy.Avalonia.Material.Controls;

internal sealed class MaterialIconEx : MaterialIcon {
	public static readonly StyledProperty<MaterialIconKind> CheckedIconProperty =
		AvaloniaProperty.Register<MaterialIconEx, MaterialIconKind>(
			nameof(CheckedIcon));

	public static readonly StyledProperty<MaterialIconKind> IndeterminateIconProperty =
		AvaloniaProperty.Register<MaterialIconEx, MaterialIconKind>(
			nameof(IndeterminateIcon));

	public static readonly StyledProperty<bool?> IsCheckedProperty =
		AvaloniaProperty.Register<MaterialIconEx, bool?>(nameof(IsChecked));

	public static readonly StyledProperty<bool> IsCheckedVisibleProperty =
		AvaloniaProperty.Register<MaterialIconEx, bool>(nameof(IsCheckedVisible), true);

	public static readonly StyledProperty<bool> IsIndeterminateVisibleProperty =
		AvaloniaProperty.Register<MaterialIconEx, bool>(nameof(IsIndeterminateVisible), true);

	public static readonly StyledProperty<bool> IsUncheckedVisibleProperty =
		AvaloniaProperty.Register<MaterialIconEx, bool>(nameof(IsUncheckedVisible), true);

	public static new readonly DirectProperty<MaterialIconEx, bool> IsVisibleProperty =
		AvaloniaProperty.RegisterDirect<MaterialIconEx, bool>(nameof(IsVisible), e => e.IsVisible);

	public static new readonly DirectProperty<MaterialIconEx, MaterialIconKind> KindProperty =
		AvaloniaProperty.RegisterDirect<MaterialIconEx, MaterialIconKind>(
			nameof(Kind), e => e.Kind);

	public static readonly StyledProperty<MaterialIconKind> UncheckedIconProperty =
		AvaloniaProperty.Register<MaterialIconEx, MaterialIconKind>(
			nameof(UncheckedIcon));

	public MaterialIconKind CheckedIcon {
		get => GetValue(CheckedIconProperty);
		set => SetValue(CheckedIconProperty, value);
	}

	public MaterialIconKind IndeterminateIcon {
		get => GetValue(IndeterminateIconProperty);
		set => SetValue(IndeterminateIconProperty, value);
	}

	public bool? IsChecked {
		get => GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public bool IsCheckedVisible {
		get => GetValue(IsCheckedVisibleProperty);
		set => SetValue(IsCheckedVisibleProperty, value);
	}

	public bool IsIndeterminateVisible {
		get => GetValue(IsIndeterminateVisibleProperty);
		set => SetValue(IsIndeterminateVisibleProperty, value);
	}

	public bool IsUncheckedVisible {
		get => GetValue(IsUncheckedVisibleProperty);
		set => SetValue(IsUncheckedVisibleProperty, value);
	}

	public new bool IsVisible {
		get => base.IsVisible;
		private set {
			base.IsVisible = value;
		}
	}

	public new MaterialIconKind Kind {
		get => base.Kind;
		private set {
			base.Kind = value;
		}
	}

	protected override Type StyleKeyOverride => typeof(MaterialIcon);

	public MaterialIconKind UncheckedIcon {
		get => GetValue(UncheckedIconProperty);
		set => SetValue(UncheckedIconProperty, value);
	}

	public MaterialIconEx() {
		UpdateIcon();
		UpdateIsVisible();
	}

	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
		base.OnAttachedToVisualTree(e);
		UpdateIcon();
		UpdateIsVisible();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == IsCheckedProperty) {
			UpdateIcon();
			UpdateIsVisible();
			return;
		}

		if (change.Property == CheckedIconProperty) {
			MaybeUpdateIcon(true, false);
			return;
		}

		if (change.Property == IndeterminateIconProperty) {
			MaybeUpdateIcon(false, true);
			return;
		}

		if (change.Property == UncheckedIconProperty) {
			MaybeUpdateIcon(false, false);
			return;
		}

		if (change.Property == IsCheckedVisibleProperty) {
			MaybeUpdateIsVisible(true, false);
			return;
		}

		if (change.Property == IsIndeterminateVisibleProperty) {
			MaybeUpdateIsVisible(false, true);
			return;
		}

		if (change.Property == IsUncheckedVisibleProperty) {
			MaybeUpdateIsVisible(false, false);
			return;
		}
	}

	private void MaybeUpdateIcon(bool forChecked, bool forIndeterminate) {
		var isIndeterminate = IsChecked == null;
		if (forIndeterminate == isIndeterminate) {
			Kind = IndeterminateIcon;
			return;
		}

		var isChecked = IsChecked.HasValue && IsChecked == true;
		if (forChecked == isChecked) {
			Kind = CheckedIcon;
			return;
		}

		var isUnchecked = !isChecked;
		var forUnchecked = !(forChecked || forIndeterminate);
		if (forUnchecked == isUnchecked) {
			Kind = UncheckedIcon;
		}
	}

	private void MaybeUpdateIsVisible(bool forChecked, bool forIndeterminate) {
		var isIndeterminate = IsChecked == null;
		if (forIndeterminate == isIndeterminate) {
			IsVisible = IsIndeterminateVisible;
			return;
		}

		var isChecked = IsChecked.HasValue && IsChecked == true;
		if (forChecked == isChecked) {
			IsVisible = IsCheckedVisible;
			return;
		}

		var isUnchecked = !isChecked;
		var forUnchecked = !(forChecked || forIndeterminate);
		if (forUnchecked == isUnchecked) {
			IsVisible = IsUncheckedVisible;
		}
	}

	private void UpdateIcon() {
		var isIndeterminate = IsChecked == null;
		if (isIndeterminate) {
			Kind = IndeterminateIcon;
			return;
		}

		var isChecked = IsChecked.HasValue && IsChecked == true;
		if (isChecked) {
			Kind = CheckedIcon;
			return;
		}

		Kind = UncheckedIcon;
	}

	private void UpdateIsVisible() {
		var isIndeterminate = IsChecked == null;
		if (isIndeterminate) {
			IsVisible = IsIndeterminateVisible;
			return;
		}

		var isChecked = IsChecked.HasValue && IsChecked == true;
		if (isChecked) {
			IsVisible = IsCheckedVisible;
			return;
		}

		IsVisible = IsUncheckedVisible;
	}
}
