using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sy.Avalonia.Material.Media;

namespace Sy.Avalonia.Material.Controls;

/// <summary>
/// Provides attached properties pertaining to an item's <see cref="Shape"/>.
/// </summary>
/// <remarks>
/// Shapes can be directly applied to the following types:
/// <list type="bullet">
/// <item><see cref="Border"/></item>
/// <item><see cref="ContentPresenter"/></item>
/// <item><see cref="TemplatedControl"/></item>
/// </list>
/// <see cref="ShapeMaskProperty">ShapeMask</see> can be used to further restrict shaping.  The 
/// mask only lessens the corner radius of <see cref="ShapeProperty">Shape</see>.
/// <para/>
/// Example:
/// <para/>
/// <code>
/// Corner     TL       TR       BR       BL
///  Shape     12       12    Circular    24
///   Mask  Circular    24       12       12
/// ------  -------- -------- -------- --------
/// Output     12       12       12       12
/// </code>
/// </remarks>
public class ShapeHelper : AvaloniaObject {
	//private static readonly HashSet<Layoutable> _hookedItems = [];

	/// <summary>
	/// The property name of <see cref="ShapeProperty"/>.
	/// </summary>
	public const string ShapePropertyName = "Shape";

	/// <summary>
	/// The property name of <see cref="ShapeMaskProperty"/>.
	/// </summary>
	public const string ShapeMaskPropertyName = "ShapeMask";

	/// <summary>
	/// Defines the <c>Shape</c>> property.
	/// </summary>
	public static readonly AttachedProperty<Shape> ShapeProperty =
		AvaloniaProperty.RegisterAttached<ShapeHelper, AvaloniaObject, Shape>(
			ShapePropertyName);

	/// <summary>
	/// Defines the <c>ShapeMask</c> property.
	/// </summary>
	public static readonly AttachedProperty<Shape> ShapeMaskProperty =
		AvaloniaProperty.RegisterAttached<ShapeHelper, AvaloniaObject, Shape>(
			ShapeMaskPropertyName, new(Shape.Circular));

	private static readonly AttachedProperty<bool> IsHookedProperty =
		AvaloniaProperty.RegisterAttached<ShapeHelper, AvaloniaObject, bool>(
			"IsHooked");

	static ShapeHelper() {
		ShapeProperty.Changed.AddClassHandler<Border>(OnShapeChanged);
		ShapeProperty.Changed.AddClassHandler<ContentPresenter>(OnShapeChanged);
		ShapeProperty.Changed.AddClassHandler<TemplatedControl>(OnShapeChanged);
	}

	public ShapeHelper() {
		//
	}

	/// <summary>
	/// Gets the <see cref="Shape"> of the element.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property value of.
	/// </param>
	/// <returns>
	/// The element shape.
	/// </returns>
	public static Shape GetShape(AvaloniaObject element) {
		return element.GetValue(ShapeProperty);
	}

	/// <summary>
	/// Gets the mask applied to the <see cref="Shape"> of an element.
	/// </summary>
	/// <param name="element">
	/// The element to get the attached property value of.
	/// </param>
	/// <returns>
	/// The mask applied to the element shape
	/// </returns>
	public static Shape GetShapeMask(AvaloniaObject element) {
		return element.GetValue(ShapeMaskProperty);
	}

	/// <summary>
	/// Sets the <see cref="Shape"> of an element.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The element shape
	/// </param>
	public static void SetShape(AvaloniaObject element, Shape value) {
		element.SetValue(ShapeProperty, value);
	}

	/// <summary>
	/// Sets the mask applied to the <see cref="Shape"> of an element.
	/// </summary>
	/// <param name="element">
	/// The element to set the attached property for.
	/// </param>
	/// <param name="value">
	/// The mask applied to the element shape.
	/// </param>
	public static void SetShapeMask(AvaloniaObject element, Shape value) {
		element.SetValue(ShapeMaskProperty, value);
	}

	private static void Control_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e) {
		if (sender is not Control control) return;
		Unhook(control);
	}

	private static void Control_SizeChanged(object? sender, RoutedEventArgs args) {
		TryUpdateControl(sender);
	}

	private static void OnShapeChanged(Border control, AvaloniaPropertyChangedEventArgs args) {
		TryHook(control);
		SetShape(control);
	}

	private static void OnShapeChanged(ContentPresenter control, AvaloniaPropertyChangedEventArgs args) {
		TryHook(control);
		SetShape(control);
	}

	private static void OnShapeChanged(TemplatedControl control, AvaloniaPropertyChangedEventArgs args) {
		TryHook(control);
		SetShape(control);
	}

	private static void SetShape(Border control) {
		var size = control.Bounds.Size;
		var shape = GetShape(control);
		var mask = GetShapeMask(control);

		var corners = shape.ToCornerRadius(size, mask);
		control.CornerRadius = corners;
	}

	private static void SetShape(ContentPresenter control) {
		var size = control.Bounds.Size;
		var shape = GetShape(control);
		var mask = GetShapeMask(control);

		var corners = shape.ToCornerRadius(size, mask);
		control.CornerRadius = corners;
	}

	private static void SetShape(TemplatedControl control) {
		var size = control.Bounds.Size;
		var shape = GetShape(control);
		var mask = GetShapeMask(control);

		var corners = shape.ToCornerRadius(size, mask);
		control.CornerRadius = corners;
	}

	private static void TryHook(Control control) {
		var isHooked = control.GetValue(IsHookedProperty);
		if (isHooked) return;

		control.SetValue(IsHookedProperty, true);

		control.DetachedFromVisualTree += Control_DetachedFromVisualTree;
		control.AddHandler(Control.SizeChangedEvent, Control_SizeChanged, handledEventsToo: true);
	}

	private static void TryUpdateControl(object? control) {
		if (control is Border border) {
			SetShape(border);
			return;
		}
		if (control is ContentPresenter contentPresenter) {
			SetShape(contentPresenter);
			return;
		}
		if (control is TemplatedControl templatedControl) {
			SetShape(templatedControl);
			return;
		}
	}

	private static void Unhook(Control control) {
		var isHooked = control.GetValue(IsHookedProperty);
		if (!isHooked) return;

		control.SetValue(IsHookedProperty, false);

		control.DetachedFromVisualTree -= Control_DetachedFromVisualTree;
		control.RemoveHandler(Control.SizeChangedEvent, Control_SizeChanged);
	}
}
