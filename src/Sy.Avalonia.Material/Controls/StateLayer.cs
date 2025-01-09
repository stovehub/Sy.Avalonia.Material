using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Sy.Avalonia.Material.Utils;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// The state layer, including ripple feedback, for Material controls.
/// </summary>
/// <remarks>
/// This class supports "Remote Control" via the <see cref="RconSourceProperty">RconSource</see> 
/// property.  Enabling this feature captures routed events from the specified source, and 
/// disables routed events from this control.
/// </remarks>
public class StateLayer : Ripple {
	private bool _isRconEnabled;
	private bool _isRippleActive;

	/// <summary>
	/// The property name for <see cref="RconIgnoreHandledProperty"/>.
	/// </summary>
	public const string RconIgnoreHandledPropertyName = "RconIgnoreHandled";

	/// <summary>
	/// The property name for <see cref="RconSourceProperty"/>.
	/// </summary>
	public const string RconSourcePropertyName = "RconSource";

	/// <summary>
	/// The property name for <see cref="RconTargetNameProperty"/>.
	/// </summary>
	public const string RconTargetNamePropertyName = "RconTargetName";

	/// <summary>
	/// Defines the <see cref="DragOpacity"/> property.
	/// </summary>
	public static readonly StyledProperty<double> DragOpacityProperty =
		AvaloniaProperty.Register<StateLayer, double>(nameof(DragOpacity), 0.16);

	/// <summary>
	/// Defines the <see cref="FocusOpacity"/> property.
	/// </summary>
	public static readonly StyledProperty<double> FocusOpacityProperty =
		AvaloniaProperty.Register<StateLayer, double>(nameof(FocusOpacity), 0.1);

	/// <summary>
	/// Defines the <see cref="HoverOpacity"/> property.
	/// </summary>
	public static readonly StyledProperty<double> HoverOpacityProperty =
		AvaloniaProperty.Register<StateLayer, double>(nameof(HoverOpacity), 0.08);

	/// <summary>
	/// Defines the <see cref="IsDraggedState"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsDraggedStateProperty =
		AvaloniaProperty.Register<StateLayer, bool>(nameof(IsDraggedState));

	/// <summary>
	/// Defines the <see cref="IsFocusedState"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsFocusedStateProperty =
		AvaloniaProperty.Register<StateLayer, bool>(nameof(IsFocusedState));

	/// <summary>
	/// Defines the <see cref="IsHoveredState"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsHoveredStateProperty =
		AvaloniaProperty.Register<StateLayer, bool>(nameof(IsHoveredState));

	/// <summary>
	/// Defines the <see cref="IsRconEnabled"/> property.
	/// </summary>
	public static readonly DirectProperty<StateLayer, bool> IsRconEnabledProperty =
		AvaloniaProperty.RegisterDirect<StateLayer, bool>(
			nameof(IsRconEnabled), o => o.IsRconEnabled);

	/// <summary>
	/// Defines the <see cref="IsPressedState"/> property.
	/// </summary>
	public static readonly StyledProperty<bool> IsPressedStateProperty =
		AvaloniaProperty.Register<StateLayer, bool>(nameof(IsPressedState));

	/// <summary>
	/// Defines the <c>RconIgnoreHandled</c> property.
	/// </summary>
	public static readonly AttachedProperty<bool> RconIgnoreHandledProperty =
		AvaloniaProperty.RegisterAttached<StateLayer, InputElement, bool>(
			RconIgnoreHandledPropertyName, inherits: true);

	/// <summary>
	/// Defines the <c>RconSource</c> property.
	/// </summary>
	public static readonly AttachedProperty<InputElement?> RconSourceProperty =
		AvaloniaProperty.RegisterAttached<StateLayer, InputElement, InputElement?>(
			RconSourcePropertyName, inherits: true);

	/// <summary>
	/// Defines the <c>RconTargetName</c> property.
	/// </summary>
	public static readonly AttachedProperty<string?> RconTargetNameProperty =
		AvaloniaProperty.RegisterAttached<StateLayer, InputElement, string?>(
			RconTargetNamePropertyName, inherits: true);

	/// <summary>
	/// Defines the <see cref="StateBrush"/> property.
	/// </summary>
	public static readonly StyledProperty<IBrush> StateBrushProperty =
		AvaloniaProperty.Register<StateLayer, IBrush>(
			nameof(StateBrush), Brushes.Transparent);

	/// <summary>
	/// Defines the <see cref="StateOpacity"/> property.
	/// </summary>
	public static readonly StyledProperty<double> StateOpacityProperty =
		AvaloniaProperty.Register<StateLayer, double>(
			nameof(StateOpacity));

	/// <summary>
	/// Gets or sets the opacity added to <see cref="StateOpacity"/> when 
	/// <see cref="IsDraggedState"/> is <see langword="true"/>.
	/// </summary>
	public double DragOpacity {
		get => GetValue(DragOpacityProperty);
		set => SetValue(DragOpacityProperty, value);
	}

	/// <summary>
	/// Gets or sets the opacity added to <see cref="StateOpacity"/> when 
	/// <see cref="IsFocusedState"/> is <see langword="true"/>.
	/// </summary>
	public double FocusOpacity {
		get => GetValue(FocusOpacityProperty);
		set => SetValue(FocusOpacityProperty, value);
	}

	/// <summary>
	/// Gets or sets the opacity added to <see cref="StateOpacity"/> when 
	/// <see cref="IsHoveredState"/> is <see langword="true"/>.
	/// </summary>
	public double HoverOpacity {
		get => GetValue(HoverOpacityProperty);
		set => SetValue(HoverOpacityProperty, value);
	}

	/// <summary>
	/// Gets or sets whether the <c>dragged</c> state is active.
	/// </summary>
	public bool IsDraggedState {
		get => GetValue(IsDraggedStateProperty);
		set => SetValue(IsDraggedStateProperty, value);
	}

	/// <summary>
	/// Gets or sets whether the <c>focused</c>> state is active.
	/// </summary>
	public bool IsFocusedState {
		get => GetValue(IsFocusedStateProperty);
		set => SetValue(IsFocusedStateProperty, value);
	}

	/// <summary>
	///  Gets or sets whether the <c>hovered</c>> state is active.
	/// </summary>
	public bool IsHoveredState {
		get => GetValue(IsHoveredStateProperty);
		set => SetValue(IsHoveredStateProperty, value);
	}

	/// <summary>
	///  Gets or sets whether the <c>pressed</c>> state is active.
	/// </summary>
	/// <remarks>
	/// If <see cref="Ripple.IsFeedbackEnabled"/> is <see langword="true"/>, this value is ignored.
	/// </remarks>
	public bool IsPressedState {
		get => GetValue(IsPressedStateProperty);
		set => SetValue(IsPressedStateProperty, value);
	}

	/// <summary>
	/// Gets whether remote control is currently enabled.
	/// </summary>
	public bool IsRconEnabled {
		get => _isRconEnabled;
		private set => SetAndRaise(IsRconEnabledProperty, ref _isRconEnabled, value);
	}

	/// <summary>
	/// Gets or sets the brush applied to the state layer.
	/// </summary>
	public IBrush StateBrush {
		get => GetValue(StateBrushProperty);
		set => SetValue(StateBrushProperty, value);
	}

	/// <summary>
	/// Gets the cumulative state opacity.
	/// </summary>
	/// <remarks>
	/// <b>NOTE</b>: While the backing <see cref="StyledProperty{TValue}"/> is modifiable, this 
	/// property is not meant to be modified manually.
	/// </remarks>
	public double StateOpacity {
		get => GetValue(StateOpacityProperty);
		private set => SetValue(StateOpacityProperty, value);
	}

	/// <summary>
	/// Creates a new <see cref="StateLayer"/> instance.
	/// </summary>
	public StateLayer() {
		//
	}

	/// <summary>
	/// Gets whether remote control routed events are ignored if handled.
	/// </summary>
	/// <param name="element">
	/// The element to get the value for.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if handled events are ignored; otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GetRconIgnoreHandled(AvaloniaObject element) {
		return element.GetValue(RconIgnoreHandledProperty);
	}

	/// <summary>
	/// Gets the source of remotely sourced routed events.
	/// </summary>
	/// <param name="element">
	/// The element to get the value for.
	/// </param>
	/// <returns>
	/// The source control.
	/// </returns>
	public static InputElement? GetRconSource(AvaloniaObject element) {
		return element.GetValue(RconSourceProperty);
	}

	/// <summary>
	/// Gets the name of the targeted <see cref="StateLayer"/>; or <see langword="null"/> if all 
	/// <see cref="StateLayer"/> children are targeted.
	/// </summary>
	/// <param name="element">
	/// The element to get the value for.
	/// </param>
	/// <returns>
	/// The target name.
	/// </returns>
	public static string? GetRconTargetName(AvaloniaObject element) {
		return element.GetValue(RconTargetNameProperty);
	}

	/// <summary>
	/// Sets whether remote control routed events are ignored if handled.
	/// </summary>
	/// <param name="element">
	/// The element to get the value for.
	/// </param>
	/// <param name="value">
	/// <see langword="true"/> if handled events should be ignored; otherwise, 
	/// <see langword="false"/>.
	/// </param>
	public static void SetRconIgnoreHandled(AvaloniaObject element, bool value) {
		element.SetValue(RconIgnoreHandledProperty, value);
	}

	/// <summary>
	/// Sets the source of remotely sourced routed events.
	/// </summary>
	/// <param name="element">
	/// The element to set the value of.
	/// </param>
	/// <param name="value">
	/// The source control.
	/// </param>
	public static void SetRconSource(AvaloniaObject element, InputElement? value) {
		element.SetValue(RconSourceProperty, value);
	}

	/// <summary>
	/// Sets the name of the targeted <see cref="StateLayer"/>
	/// </summary>
	/// <param name="element">
	/// The element to set the value for.
	/// </param>
	/// <param name="value">
	/// The target name; or <see langword="null"/> if all <see cref="StateLayer"/> children are 
	/// targeted.
	/// </param>
	public static void SetRconTargetName(AvaloniaObject element, string? value) {
		element.SetValue(RconTargetNameProperty, value);
	}

	protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
		base.OnApplyTemplate(e);

		CheckRconSource(null);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == FeedbackOpacityProperty) {
			if (IsFeedbackEnabled) return;
			UpdateOpacity();
			return;
		}

		if (change.Property == FocusOpacityProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == HoverOpacityProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == IsDraggedStateProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == IsFeedbackEnabledProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == IsFocusedStateProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == IsHoveredStateProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == IsPressedStateProperty) {
			UpdateOpacity();
			return;
		}

		if (change.Property == NameProperty) {
			CheckRconSource(null);
			return;
		}

		if (change.Property == RconSourceProperty) {
			var oldSource = change.GetOldValue<InputElement?>();
			CheckRconSource(oldSource);
			return;
		}

		if (change.Property == RconTargetNameProperty) {
			CheckRconSource(null);
			return;
		}
	}

	private void CheckRconSource(InputElement? oldSource) {
		var source = GetRconSource(this);
		oldSource ??= source;
		if (oldSource is null) return;

		DisableRconFor(oldSource);
		if (source is null) return; // impossible, but intellisense disagrees

		var targetName = GetRconTargetName(this);
		if (targetName is null) {
			EnableRconFor(source);
			return;
		}

		if (TemplatedParent is StyledElement parent) {
			var isLocalScope = ReferenceEquals(source, TemplatedParent);
			if (isLocalScope && targetName != Name) return;
			if (!isLocalScope && targetName != parent.Name) return;
		} else {
			if (targetName != Name) return;
		}

		EnableRconFor(source);
	}

	private void DisableRconFor(InputElement control) {
		control.RemoveHandler(LostFocusEvent, Rcon_LostFocus);
		control.RemoveHandler(PointerCaptureLostEvent, Rcon_PointerCapturedLost);
		control.RemoveHandler(PointerPressedEvent, Rcon_PointerPressed);
		control.RemoveHandler(PointerReleasedEvent, Rcon_PointerReleased);

		StopRipple();
		IsLocalRoutedEventsEnabled = true;
		IsRconEnabled = false;
	}

	private void EnableRconFor(InputElement control) {
		IsRconEnabled = true;
		IsLocalRoutedEventsEnabled = false;

		control.AddHandler(LostFocusEvent, Rcon_LostFocus, 
			handledEventsToo: true);
		control.AddHandler(PointerCaptureLostEvent, Rcon_PointerCapturedLost,
			handledEventsToo: true);
		control.AddHandler(PointerPressedEvent, Rcon_PointerPressed,
			handledEventsToo: true);
		control.AddHandler(PointerReleasedEvent, Rcon_PointerReleased,
			handledEventsToo: true);
	}

	private void Rcon_LostFocus(object? sender, RoutedEventArgs e) {
		if (!_isRippleActive) return;
		StopRipple();
		_isRippleActive = false;
	}

	private void Rcon_PointerCapturedLost(object? sender, RoutedEventArgs e) {
		if (!_isRippleActive) return;
		StopRipple();
		_isRippleActive = false;
	}

	private void Rcon_PointerPressed(object? sender, PointerPressedEventArgs e) {
		if (!IsEffectivelyEnabled) return;
		if (_isRippleActive) return;

		var ignoreHandled = GetRconIgnoreHandled(this);
		if (ignoreHandled) {
			var thisRoot = VisualUtils.GetTemplateRoot(this);
			var sourceRoot = VisualUtils.GetTemplateRoot(e.Source);
			var wasHandledLocally = ReferenceEquals(thisRoot, sourceRoot);
			if (!wasHandledLocally && e.Handled) return;
		}

		var pointer = e.GetCurrentPoint(this);
		if (!pointer.Properties.IsLeftButtonPressed) return;

		var origin = e.GetPosition(this);
		_isRippleActive = StartRipple(origin);
	}

	private void Rcon_PointerReleased(object? sender, RoutedEventArgs e) {
		if (!_isRippleActive) return;
		StopRipple();
		_isRippleActive = false;
	}

	private void UpdateOpacity() {
		if (IsDraggedState) {
			StateOpacity = DragOpacity;
			return;
		}

		var opacity = 0d;
		if (IsPressedState && !IsFeedbackEnabled) {
			opacity += FeedbackOpacity;
		}
		if (IsFocusedState) {
			opacity += FocusOpacity;
		}
		if (IsHoveredState) {
			opacity += HoverOpacity;
		}

		StateOpacity = opacity;
	}
}
