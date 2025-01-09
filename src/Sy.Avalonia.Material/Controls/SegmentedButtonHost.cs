using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Styling;
using System.Collections.Specialized;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// A Material Segmented Button bar.
/// </summary>
public class SegmentedButtonHost : UniformGrid {
	/// <summary>
	/// The <see cref="Style"/> class name applied to a button between two other buttons.
	/// </summary>
	public const string BodyStyleClassName = "body";

	/// <summary>
	/// The <see cref="Style"/> class name applied to the first button in the sequence.
	/// </summary>
	public const string FirstStyleClassName = "first";

	/// <summary>
	/// The <see cref="Style"/> class name applied to buttons layed out horizontally.
	/// </summary>
	public const string HorizontalStyleClassName = "horizontal";

	/// <summary>
	/// The <see cref="Style"/> class name applied to the last button in the sequence.
	/// </summary>
	public const string LastStyleClassName = "last";

	/// <summary>
	/// The <see cref="Style"/> class name applied to buttons layed out vertically.
	/// </summary>
	public const string VerticalStyleClassName = "vertical";

	/// <summary>
	/// Defines the <see cref="Columns"/> property.
	/// </summary>
	public static new readonly DirectProperty<SegmentedButtonHost, int> ColumnsProperty =
		AvaloniaProperty.RegisterDirect<SegmentedButtonHost, int>(
			nameof(Columns), e => e.Columns);

	/// <summary>
	/// Defines the <see cref="ChildControlTheme"/> property.
	/// </summary>
	public static readonly StyledProperty<ControlTheme?> ChildControlThemeProperty =
		AvaloniaProperty.Register<SegmentedButtonHost, ControlTheme?>(
			nameof(ChildControlTheme));

	/// <summary>
	/// Defines the <see cref="FirstColumn"/> property.
	/// </summary>
	public static new readonly DirectProperty<SegmentedButtonHost, int> FirstColumnProperty =
		AvaloniaProperty.RegisterDirect<SegmentedButtonHost, int>(
			nameof(FirstColumn), e => e.FirstColumn);

	/// <summary>
	/// Defines the <see cref="Orientation"/> property.
	/// </summary>
	public static readonly StyledProperty<Orientation> OrientationProperty =
		AvaloniaProperty.Register<SegmentedButtonHost, Orientation>(nameof(Orientation));

	/// <summary>
	/// Defines the <see cref="Rows"/> property.
	/// </summary>
	public static new readonly DirectProperty<SegmentedButtonHost, int> RowsProperty =
		AvaloniaProperty.RegisterDirect<SegmentedButtonHost, int>(nameof(Rows), e => e.Rows);

	/// <summary>
	/// Gets or sets the control theme applied to child elements.
	/// </summary>
	/// <remarks>
	/// If set to <see langword="null" />, the control templates will be left unchanged.
	/// </remarks>
	public ControlTheme? ChildControlTheme {
		get => GetValue(ChildControlThemeProperty);
		set => SetValue(ChildControlThemeProperty, value);
	}

	/// <summary>
	/// Gets the column count.
	/// </summary>
	/// <remarks>
	/// If the value is <c>0</c>, the column count will be calculated automatically.
	/// </remarks>
	public new int Columns {
		get => base.Columns;
		private set {
			base.Columns = value;
		}
	}

	/// <summary>
	/// Gets, for the first row, the column where the items will start.
	/// </summary>
	public new int FirstColumn {
		get => base.FirstColumn;
		private set {
			base.FirstColumn = value;
		}
	}

	/// <summary>
	/// Gets or sets the orientation in which child controls will be layed out.
	/// </summary>
	public Orientation Orientation {
		get => GetValue(OrientationProperty);
		set => SetValue(OrientationProperty, value);
	}

	/// <summary>
	/// Gets the row count.
	/// </summary>
	/// <remarks>
	/// If the value is <c>0</c>, the row count will be calculated automatically.
	/// </remarks>
	public new int Rows {
		get => base.Rows;
		private set {
			base.Rows = value;
		}
	}

	/// <summary>
	/// Creates a new <see cref="SegmentedButtonHost"/> instance.
	/// </summary>
	public SegmentedButtonHost() {
		//
	}

	protected override void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		base.ChildrenChanged(sender, e);
		ApplyChildControlThemes();
		UpdateChildStyleClasses();
	}

	protected override void OnInitialized() {
		base.OnInitialized();

		UpdateGridLayout();

		ApplyChildControlThemes();
		UpdateChildStyleClasses();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == ChildControlThemeProperty) {
			ApplyChildControlThemes();
			return;
		}

		if (change.Property == OrientationProperty) {
			UpdateGridLayout();
			UpdateChildStyleClasses();
			return;
		}
	}

	private void ApplyChildControlThemes() {
		foreach (var control in Children) {
			control.Theme = ChildControlTheme;
		}
	}

	private void UpdateChildStyleClasses() {
		if (Children.Count == 0) return;

		var isHorizontal = Orientation == Orientation.Horizontal;
		for (int i = 0; i < Children.Count; i++) {
			var child = Children[i];

			child.Classes.Remove(HorizontalStyleClassName);
			child.Classes.Remove(VerticalStyleClassName);
			child.Classes.Remove(FirstStyleClassName);
			child.Classes.Remove(BodyStyleClassName);
			child.Classes.Remove(LastStyleClassName);

			if (i == 0 && Children.Count == 1) {
				continue;
			}

			if (isHorizontal) child.Classes.Add(HorizontalStyleClassName);
			if (!isHorizontal) child.Classes.Add(VerticalStyleClassName);

			if (i == 0) {
				child.Classes.Add(FirstStyleClassName);
				continue;
			}
			if (i == Children.Count - 1) {
				child.Classes.Add(LastStyleClassName);
				continue;
			}
			child.Classes.Add(BodyStyleClassName);
		}
	}

	private void UpdateGridLayout() {
		var rows = Orientation == Orientation.Horizontal ? 1 : 0;
		var columns = Orientation == Orientation.Vertical ? 1 : 0;

		Rows = rows;
		Columns = columns;
	}
}
