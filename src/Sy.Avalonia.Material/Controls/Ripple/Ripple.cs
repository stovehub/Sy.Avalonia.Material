using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Threading;

namespace Sy.Avalonia.Material.Controls;

/*
 * Based on that of the Material.Avalonia project, with improvements.
 * https://github.com/AvaloniaCommunity/Material.Avalonia/blob/master/Material.Ripple/RippleEffect.cs
 */

/// <summary>
/// A control that creates the Material Ripple feedback effect.
/// </summary>
public class Ripple : TemplatedControl {
	private static readonly Easing s_easing = new CircularEaseOut();
	private static readonly TimeSpan s_spreadDuration = TimeSpan.FromMilliseconds(2000);
	private static readonly TimeSpan s_fadeDuration = TimeSpan.FromMilliseconds(600);

	private bool _isCanceled;
	private bool _isLocalRoutedEventsEnabled = true;
	private CompositionContainerVisual? _container;
	private CompositionCustomVisual? _current;

	/// <summary>
	/// Defines the <see cref="FeedbackBrush"/> property.
	/// </summary>
	public static readonly StyledProperty<IBrush> FeedbackBrushProperty =
		AvaloniaProperty.Register<Ripple, IBrush>(
			nameof(FeedbackBrush), Brushes.Transparent);

	/// <summary>
	/// Defines the <see cref="FeedbackOpacity"/> property.
	/// </summary>
	public static readonly StyledProperty<double> FeedbackOpacityProperty =
		AvaloniaProperty.Register<Ripple, double>(
			nameof(FeedbackOpacity), 0.1);

	/// <summary>
	/// Defines the <see cref="FeedbackOrigin"/> property.
	/// </summary>
	public static readonly StyledProperty<FeedbackOrigin> FeedbackOriginProperty =
		AvaloniaProperty.Register<Ripple, FeedbackOrigin>(
			nameof(FeedbackOrigin), FeedbackOrigin.Cursor);

	/// <summary>
	/// Defines the <see cref="IsFeedbackAnimated"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsFeedbackEnabledProperty =
		AvaloniaProperty.Register<Ripple, bool>(
			nameof(IsFeedbackEnabled), true);

	/// <summary>
	/// Defines the <see cref="IsCapturingOutsideCornerRadius"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsCapturingOutsideCornerRadiusProperty =
		AvaloniaProperty.Register<Ripple, bool>(
			nameof(IsCapturingOutsideCornerRadius), false);

	/// <summary>
	/// Gets or sets the brush applied to the feedback effect.
	/// </summary>
	public IBrush FeedbackBrush {
		get => GetValue(FeedbackBrushProperty);
		set => SetValue(FeedbackBrushProperty, value);
	}

	/// <summary>
	/// Gets or sets the opacity of the feedback effect.
	/// </summary>
	public double FeedbackOpacity {
		get => GetValue(FeedbackOpacityProperty);
		set => SetValue(FeedbackOpacityProperty, value);
	}

	/// <summary>
	/// Gets or sets the origin point for the ripple feedback.
	/// </summary>
	public FeedbackOrigin FeedbackOrigin {
		get => GetValue(FeedbackOriginProperty);
		set => SetValue(FeedbackOriginProperty, value);
	}

	/// <summary>
	/// Gets or sets whether the feedback opacity is animated (ripple).
	/// </summary>
	public bool IsFeedbackEnabled {
		get => GetValue(IsFeedbackEnabledProperty);
		set => SetValue(IsFeedbackEnabledProperty, value);
	}

	/// <summary>
	/// Gets or sets whether routed events are captured if outside of the corner radius, but still 
	/// within the actual bounds of this control.
	/// </summary>
	public bool IsCapturingOutsideCornerRadius {
		get => GetValue(IsCapturingOutsideCornerRadiusProperty);
		set => SetValue(IsCapturingOutsideCornerRadiusProperty, value);
	}

	/// <summary>
	/// Gets or sets whether local routed events are enabled for ripple feedback.
	/// </summary>
	protected bool IsLocalRoutedEventsEnabled {
		get => _isLocalRoutedEventsEnabled;
		set => _isLocalRoutedEventsEnabled = value;
	}

	static Ripple() {
		BackgroundProperty.OverrideDefaultValue<Ripple>(Brushes.Transparent);
	}

	/// <summary>
	/// Creates a new <see cref="Ripple"/> instance.
	/// </summary>
	public Ripple() {
		//
	}

	/// <summary>
	/// Starts a new ripple at the specified point.
	/// </summary>
	/// <param name="point">
	/// The origin point.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if a new ripple was started as a result of this call; otherwise, 
	/// <see langword="false"/>.
	/// </returns>
	public virtual bool StartRipple(Point point) {
		if (!IsFeedbackEnabled) return false;

		return AddRipple(point);
	}

	/// <summary>
	/// Stops the current ripple.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if a ripple was stopped as a result of this call; otherwise, 
	/// <see langword="false"/>.
	/// </returns>
	public virtual bool StopRipple() {
		return RemoveCurrentRipple();
	}

	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
		base.OnAttachedToVisualTree(e);

		var @this = ElementComposition.GetElementVisual(this)!;
		_container = @this.Compositor.CreateContainerVisual();
		UpdateContainerSize();
		ElementComposition.SetElementChildVisual(this, _container);
	}

	protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
		base.OnDetachedFromVisualTree(e);

		_container = null;
		ElementComposition.SetElementChildVisual(this, null);
	}

	protected override void OnLostFocus(RoutedEventArgs e) {
		base.OnLostFocus(e);

		if (!IsLocalRoutedEventsEnabled) return;

		_isCanceled = true;
		RemoveCurrentRipple();
	}

	protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e) {
		base.OnPointerCaptureLost(e);

		if (!IsLocalRoutedEventsEnabled) return;

		_isCanceled = true;
		RemoveCurrentRipple();
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e) {
		base.OnPointerPressed(e);

		if (!IsFeedbackEnabled) return;
		if (!IsLocalRoutedEventsEnabled) return;

		var pointer = e.GetCurrentPoint(this);
		if (!pointer.Properties.IsLeftButtonPressed) return;

		var origin = e.GetPosition(this);
		AddRipple(origin);
	}

	protected override void OnPointerReleased(PointerReleasedEventArgs e) {
		base.OnPointerReleased(e);

		if (!IsLocalRoutedEventsEnabled) return;

		_isCanceled = true;
		RemoveCurrentRipple();
	}

	protected override void OnSizeChanged(SizeChangedEventArgs e) {
		base.OnSizeChanged(e);

		UpdateContainerSize();
	}

	private bool AddRipple(Point origin) {
		if (_container == null) return false;
		if (_current is not null) return false;

		_isCanceled = false;

		// Adjust feedback origin, if necessary
		var isCenterOrigin = FeedbackOrigin == FeedbackOrigin.Center;
		if (isCenterOrigin) origin = Bounds.Center;

		// Max radius; distance from origin to furthest corner
		var vToTopLeft = new Vector(-origin.X, -origin.Y);
		var vToTopRight = new Vector(Bounds.Width, 0) + vToTopLeft;
		var vToBottomRight = new Vector(Bounds.Width, Bounds.Height) + vToTopLeft;
		var vToBottomLeft = new Vector(0, Bounds.Height) + vToTopLeft;

		var maxRadius = Math.Max(vToTopLeft.Length, vToTopRight.Length);
		maxRadius = Math.Max(maxRadius, vToBottomRight.Length);
		maxRadius = Math.Max(maxRadius, vToBottomLeft.Length);
		maxRadius *= 1.33;

		// Get standard properties
		var brush = FeedbackBrush.ToImmutable();
		var clipRect = new Rect(0, 0, Bounds.Width, Bounds.Height);
		var clip = new RoundedRect(clipRect, CornerRadius);

		var info = new RippleInfo() {
			Brush = brush,
			Clip = clip,
			Easing = s_easing,
			FadeDuration = s_fadeDuration,
			MaxRadius = maxRadius,
			Opacity = FeedbackOpacity,
			Origin = origin,
			SpreadDuration = s_spreadDuration,
		};
		var handler = new RippleHandler(info);

		var @this = ElementComposition.GetElementVisual(this)!;
		var visual = @this.Compositor.CreateCustomVisual(handler);
		visual.Size = new Vector(Bounds.Width, Bounds.Height);

		_container.Children.Add(visual);
		_current = visual;

		visual.SendHandlerMessage(RippleHandler.StartSpreadMessage);
		
		if (_isCanceled) RemoveCurrentRipple();
		return true;
	}

	private bool RemoveCurrentRipple() {
		if (_current == null) return false;

		var last = _current;
		_current = null;

		last.SendHandlerMessage(RippleHandler.StartFadeMessage);
		if (_container is null) return false;

		var container = _container;
		DispatcherTimer.RunOnce(
			() => container.Children.Remove(last),
			s_spreadDuration,
			DispatcherPriority.Render);

		return true;
	}

	private void UpdateContainerSize() {
		_isCanceled = true;
		if (_container is null) return;

		var width = Bounds.Width;
		var height = Bounds.Height;
		var size = new Vector(width, height);

		_container.Size = size;
		foreach (var child in _container.Children) {
			child.Size = size;
		}
	}
}
