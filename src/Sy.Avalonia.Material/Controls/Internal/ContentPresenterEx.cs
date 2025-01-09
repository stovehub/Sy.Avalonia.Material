using Avalonia;
using Avalonia.Controls.Presenters;
using Sy.Avalonia.Material.Media;

namespace Sy.Avalonia.Material.Controls;

internal sealed class ContentPresenterEx : ContentPresenter {
	private bool _isChanging;
	private string? _originalContent;

	public static readonly StyledProperty<Casing> ContentCharacterCasingProperty =
		AvaloniaProperty.Register<ContentPresenterEx, Casing>(
			nameof(ContentCharacterCasing), Casing.Normal);

	public Casing ContentCharacterCasing {
		get => GetValue(ContentCharacterCasingProperty);
		set => SetValue(ContentCharacterCasingProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == ContentCharacterCasingProperty) {
			UpdateContent();
			return;
		}

		if (_isChanging) return;
		if (change.Property != ContentProperty) return;
		if (Content is not string stringContent) {
			_originalContent = null;
			return;
		}

		_originalContent = stringContent;
		UpdateContent();
	}

	private void UpdateContent() {
		if (_originalContent == null) return;

		var casing = ContentCharacterCasing;
		if (casing == Casing.Normal) return;

		try {
			_isChanging = true;
			if (casing == Casing.Upper) {
				var newContent = _originalContent.ToUpper();
				Content = newContent;
				return;
			}
			if (casing == Casing.Lower) {
				var newContent = _originalContent.ToLower();
				Content = newContent;
				return;
			}
		} finally {
			_isChanging = false;
		}
	}
}
