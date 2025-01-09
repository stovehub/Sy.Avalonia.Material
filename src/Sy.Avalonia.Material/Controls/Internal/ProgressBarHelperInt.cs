using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace Sy.Avalonia.Material.Controls;

internal class ProgressBarHelperInt : AvaloniaObject {
	public static readonly AttachedProperty<double> CircularIndicatorStartAngleProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			"CircularIndicatorStartAngle");

	public static readonly AttachedProperty<double> CircularIndicatorSweepProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			"CircularIndicatorSweep");

	public static readonly AttachedProperty<double> CircularSizeProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			"CircularSize");

	public static readonly AttachedProperty<double> CircularTrackStartAngleProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			"CircularTrackStartAngle");

	public static readonly AttachedProperty<double> CircularTrackSweepProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, double>(
			"CircularTrackSweep");

	public static readonly AttachedProperty<Thickness> DeterminateContainerPaddingProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, Thickness>(
			"DeterminateContainerPadding");

	public static readonly AttachedProperty<Thickness> DeterminateIndicatorMarginProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, Thickness>(
			"DeterminateIndicatorMargin");

	public static readonly AttachedProperty<Thickness> IndeterminateContainerPaddingProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, Thickness>(
			"IndeterminateContainerPadding");

	public static readonly AttachedProperty<Thickness> IndeterminateTrackMarginProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, Thickness>(
			"IndeterminateTrackMargin");

	public static readonly AttachedProperty<bool> IsCircularProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, bool>(
			"IsCircular");

	public static readonly AttachedProperty<bool> IsHookedProperty =
		AvaloniaProperty.RegisterAttached<ProgressBarHelperInt, ProgressBar, bool>(
			"IsHooked");

	static ProgressBarHelperInt() {
		IsCircularProperty.Changed.AddClassHandler<ProgressBar>(OnPropertyChanged);
		IsHookedProperty.Changed.AddClassHandler<ProgressBar>(OnPropertyChanged);
		ProgressBarHelper.CircularStrokeLineCapProperty.Changed.AddClassHandler<ProgressBar>(OnPropertyChanged);
		ProgressBarHelper.SpacingProperty.Changed.AddClassHandler<ProgressBar>(OnPropertyChanged);
	}

	public ProgressBarHelperInt() {
		//
	}

	public static double GetCircularIndicatorStartAngle(AvaloniaObject element) {
		return element.GetValue(CircularIndicatorStartAngleProperty);
	}

	public static double GetCircularIndicatorSweep(AvaloniaObject element) {
		return element.GetValue(CircularIndicatorSweepProperty);
	}

	public static double GetCircularSize(AvaloniaObject element) {
		return element.GetValue(CircularSizeProperty);
	}

	public static double GetCircularTrackStartAngle(AvaloniaObject element) {
		return element.GetValue(CircularTrackStartAngleProperty);
	}

	public static double GetCircularTrackSweep(AvaloniaObject element) {
		return element.GetValue(CircularTrackSweepProperty);
	}

	public static Thickness GetDeterminateContainerPadding(AvaloniaObject element) {
		return element.GetValue(DeterminateContainerPaddingProperty);
	}

	public static Thickness GetDeterminateIndicatorMargin(AvaloniaObject element) {
		return element.GetValue(DeterminateIndicatorMarginProperty);
	}

	public static Thickness GetIndeterminateContainerPadding(AvaloniaObject element) {
		return element.GetValue(IndeterminateContainerPaddingProperty);
	}

	public static bool GetIsCircular(AvaloniaObject element) {
		return element.GetValue(IsCircularProperty);
	}

	public static void SetIsCircular(AvaloniaObject element, bool value) {
		element.SetValue(IsCircularProperty, value);
	}

	public static void SetIsHooked(AvaloniaObject element, bool value) {
		element.SetValue(IsHookedProperty, value);
	}

	private static void CalculateBarsIfCircular(ProgressBar control) {
		var isCircular = control.GetValue(IsCircularProperty);
		if (!isCircular) return;

		var diameter = control.GetValue(CircularSizeProperty);
		if (double.IsNaN(diameter) || diameter == 0) return;

		var radius = diameter / 2;
		var thickness = control.GetValue(ProgressBarHelper.CircularStrokeThicknessProperty);

		var capType = control.GetValue(ProgressBarHelper.CircularStrokeLineCapProperty);
		var capLinear = capType == PenLineCap.Flat ? 0 : thickness / 2;
		var spacingLinear = control.GetValue(ProgressBarHelper.SpacingProperty);

		var capAngle = capLinear / radius * 180 / Math.PI;
		var spacingAngle = spacingLinear / radius * 180 / Math.PI;
		
		var progress = (control.Value - control.Minimum) / (control.Maximum - control.Minimum);
		var isEdgeCase = progress <= 0 || progress >= 1;

		var startAngle = (!isEdgeCase) ? (spacingAngle * 0.5) : 0;
		var totalSweep = (!isEdgeCase) ? 360 - spacingAngle : 360;

		// Active Indicator Start
		var iStartRaw = startAngle;
		var iStartAdj = !isEdgeCase ? capAngle : 0;
		var iStart = iStartRaw + iStartAdj;
		
		// Active Indicator Sweep
		var iSweepRaw = totalSweep * progress;
		var iSweepAdj = !isEdgeCase ? -capAngle : 0;
		var iSweep = iSweepRaw + iSweepAdj;
		if (!isEdgeCase && iSweep < 1) iSweep = 1;

		// Track Start
		var tStartRaw = iStart + iSweep;
		var tStartAdj = !isEdgeCase ? spacingAngle + capAngle * 2 : 0;
		var tStart = tStartRaw + tStartAdj;

		// Track Sweep
		var tSweepRaw = totalSweep - tStart;
		var tSweepAdj = !isEdgeCase ? 0 : 0;
		var tSweep = tSweepRaw + tSweepAdj;
		if (!isEdgeCase && tSweep < 0) tSweep = 0;

		// Set values
		control.SetValue(CircularIndicatorStartAngleProperty, iStart);
		control.SetValue(CircularIndicatorSweepProperty, iSweep);
		control.SetValue(CircularTrackStartAngleProperty, tStart);
		control.SetValue(CircularTrackSweepProperty, tSweep);
	}

	private static void CalculateSizeIfCircular(ProgressBar control) {
		var isCircular = control.GetValue(IsCircularProperty);
		if (!isCircular) return;

		var width = control.Bounds.Width;
		var height = control.Bounds.Height;
		var size = Math.Min(width, height);

		control.SetValue(CircularSizeProperty, size);
	}

	private static void CalculatePlacementsIfLinear(ProgressBar control) {
		var isCircular = control.GetValue(IsCircularProperty);
		if (isCircular) return;

		var isHorizontal = control.Orientation == Orientation.Horizontal;
		var spacing = control.GetValue(ProgressBarHelper.SpacingProperty);

		// Determinate Container
		var dPadding = isHorizontal ? new Thickness(-spacing, 0) : new(0, -spacing);

		// Determinate Indicator
		var dMargin = isHorizontal ? new Thickness(0, 0, spacing, 0) : new(0, spacing, 0, 0);

		// Indeterminate Container
		var iPaddingPart = spacing * -0.5;
		var iPadding = isHorizontal ? new Thickness(iPaddingPart, 0) : new(0, iPaddingPart);

		// Indeterminate Tracks and Indicators
		var iMarginPart = spacing * 0.5;
		var iMargin = isHorizontal ? new Thickness(iMarginPart, 0) : new(0, iMarginPart);

		// Set values
		control.SetValue(DeterminateContainerPaddingProperty, dPadding);
		control.SetValue(DeterminateIndicatorMarginProperty, dMargin);
		control.SetValue(IndeterminateContainerPaddingProperty, iPadding);
		control.SetValue(IndeterminateTrackMarginProperty, iMargin);
	}

	private static void Control_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs args) {
		if (sender is not ProgressBar control) return;

		control.SetValue(IsHookedProperty, false);
	}

	private static void Control_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs args) {
		if (sender is not ProgressBar control) return;

		if (args.Property == ProgressBar.OrientationProperty) {
			CalculatePlacementsIfLinear(control);
			return;
		}

		if (args.Property == RangeBase.ValueProperty) {
			CalculateBarsIfCircular(control);
			return;
		}
	}

	private static void Control_SizeChanged(object? sender, RoutedEventArgs args) {
		if (sender is not ProgressBar control) return;

		CalculateSizeIfCircular(control);
		CalculateBarsIfCircular(control);
	}

	private static void Hook(ProgressBar control) {
		control.DetachedFromVisualTree += Control_DetachedFromVisualTree;
		control.PropertyChanged += Control_PropertyChanged;
		control.AddHandler(Control.SizeChangedEvent, Control_SizeChanged, handledEventsToo: true);

		CalculatePlacementsIfLinear(control);
		CalculateSizeIfCircular(control);
		CalculateBarsIfCircular(control);
	}

	private static void OnPropertyChanged(ProgressBar control, AvaloniaPropertyChangedEventArgs args) {
		if (args.Property == IsCircularProperty) {
			CalculatePlacementsIfLinear(control);
			CalculateSizeIfCircular(control);
			CalculateBarsIfCircular(control);
			return;
		}

		if (args.Property == IsHookedProperty) {
			(var wasHooked, var isHooked) = args.GetOldAndNewValue<bool>();
			if (wasHooked) Unhook(control);
			if (isHooked) Hook(control);
			return;
		}

		if (args.Property == ProgressBarHelper.CircularStrokeLineCapProperty) {
			CalculateBarsIfCircular(control);
			return;
		}

		if (args.Property == ProgressBarHelper.SpacingProperty) {
			CalculatePlacementsIfLinear(control);
			CalculateBarsIfCircular(control);
			return;
		}
	}

	private static void Unhook(ProgressBar control) {
		control.DetachedFromVisualTree -= Control_DetachedFromVisualTree;
		control.PropertyChanged -= Control_PropertyChanged;
		control.RemoveHandler(Control.SizeChangedEvent, Control_SizeChanged);
	}
}
