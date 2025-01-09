using Avalonia;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Sy.Avalonia.Material.Media;

/// <summary>
/// A <see cref="Shape"/> converter for Avalonia's XAML parser.
/// </summary>
public class ShapeConverter : TypeConverter {
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
		return sourceType == typeof(string);
	}

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) {
		return value is string s ? Shape.Parse(s) : null;
	}
}

/// <summary>
/// Similar to <see cref="CornerRadius"/>, representing the radii of a rectangle's corners, but 
/// allowing circular corners by setting the value to <see cref="Circular"/>.
/// </summary>
[TypeConverter(typeof(ShapeConverter))]
public readonly struct Shape : IEquatable<Shape> {
	private const string CircularShortName = "C";
	private const string CircularLongName = "Circular";

	private readonly double _bottomLeft;
	private readonly double _bottomRight;
	private readonly double _topLeft;
	private readonly double _topRight;

	/// <summary>
	/// A corner value used to indicate a circular shape.
	/// </summary>
	public const double Circular = double.PositiveInfinity;

	/// <summary>
	/// A shape where all corners are circular.
	/// </summary>
	public static readonly Shape CircularShape = new(Circular);

	/// <summary>
	/// Creates a new <see cref="Shape"/> instance with the default radius of 0dps (rectangular).
	/// </summary>
	public Shape() : this(0.0) {
		//
	}

	/// <summary>
	/// Creates a new <see cref="Shape"/> instance with a uniform shape on all corners.
	/// </summary>
	/// <param name="uniformShape">
	/// The shape for all corners.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="uniformShape"/> is less than 0.0.
	/// </exception>
	public Shape(double uniformShape) {
		CheckValue(uniformShape);

		_topLeft = uniformShape;
		_topRight = uniformShape;
		_bottomLeft = uniformShape;
		_bottomRight = uniformShape;
	}

	/// <summary>
	/// Creates a new <see cref="Shape"/> instance.
	/// </summary>
	/// <param name="top">
	/// The shape for the top corners.
	/// </param>
	/// <param name="bottom">
	/// The shape for the bottom corners.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="top"/> or <paramref name="bottom"/> is less than 0.0.
	/// </exception>
	public Shape(double top, double bottom) {
		CheckValue(top);
		CheckValue(bottom);

		_topLeft = top;
		_topRight = top;
		_bottomLeft = bottom;
		_bottomRight = bottom;
	}

	/// <summary>
	/// Creates a new <see cref="Shape"/> instance.
	/// </summary>
	/// <param name="topLeft">
	/// The shape for the top corners.
	/// </param>
	/// <param name="topRight">
	/// The shape for the bottom corners.
	/// </param>
	/// <param name="bottomRight">
	/// The shape for the top corners.
	/// </param>
	/// <param name="bottomLeft">
	/// The shape for the top corners.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Any parameter is less than 0.0.
	/// </exception>
	public Shape(double topLeft, double topRight, double bottomRight, double bottomLeft) {
		CheckValue(topLeft);
		CheckValue(topRight);
		CheckValue(bottomRight);
		CheckValue(bottomLeft);

		_topLeft = topLeft;
		_topRight = topRight;
		_bottomRight = bottomRight;
		_bottomLeft = bottomLeft;
	}

	/// <summary>
	/// Parses a string into a <see cref="Shape"/>.
	/// </summary>
	/// <remarks>
	/// Text will be interpreted as doubles, except for <c>Circular</c> or <c>C</c>, which 
	/// indicates <see cref="Circular"/>.
	/// <para/>
	/// Text can be formatted as one of the following:
	/// <list type="bullet">
	/// <item>[uniformShape] (e.g. <c>6.9</c>)</item>
	/// <item>[top] [bottom] (e.g. <c>42.0 C</c>)</item>
	/// <item>[topLeft] [topRight] [bottomRight] [bottomLeft] (e.g. <c>6 9.4 2.0 C</c>)</item>
	/// </list>
	/// </remarks>
	/// <param name="s">
	/// The string to parse.
	/// </param>
	/// <returns>
	/// The parsed shape.
	/// </returns>
	/// <exception cref="FormatException">
	/// <paramref name="s"/> was an invalid format for a <see cref="Shape"/>.
	/// </exception>
	public static Shape Parse(string s) {
		ArgumentNullException.ThrowIfNull(s);
		if (string.IsNullOrWhiteSpace(s)) {
			throw new FormatException("Invalid Shape.");
		}

		// Split into arguments
		s = s.Trim();
		s = s.Replace("  ", " ");
		var args = s.Split(' ');

		try {
			if (args.Length == 1) {
				var uniformShape = ParseToken(args[0]);
				return new Shape(uniformShape);
			} else if (args.Length < 4) {
				var top = ParseToken(args[0]);
				var bottom = ParseToken(args[1]);
				return new Shape(top, bottom);
			} else {
				var topLeft = ParseToken(args[0]);
				var topRight = ParseToken(args[1]);
				var bottomRight = ParseToken(args[2]);
				var bottomLeft = ParseToken(args[3]);
				return new Shape(topLeft, topRight, bottomRight, bottomLeft);
			}
		} catch (FormatException) {
			throw;
		} catch (Exception ex) {
			throw new FormatException("Invalid Shape.", ex);
		}
	}

	/// <inheritdoc cref="ToCornerRadius(Size, Shape)"/>
	public CornerRadius ToCornerRadius(Size size) {
		return ToCornerRadius(size, CircularShape);
	}

	/// <summary>
	/// Converts this shape to a <see cref="CornerRadius"/> for a control with the specified size.
	/// </summary>
	/// <param name="size">
	/// The size of the control.
	/// </param>
	/// <param name="mask">
	/// The shape mask to apply.
	/// </param>
	/// <returns>
	/// The corner radii for the control.
	/// </returns>
	public CornerRadius ToCornerRadius(Size size, Shape mask) {
		var smallest = Math.Min(size.Width, size.Height);
		var half = smallest / 2.0;

		var topLeft = (_topLeft <= half) ? _topLeft : half;
		var topRight = (_topRight <= half) ? _topRight : half;
		var bottomRight = (_bottomRight <= half) ? _bottomRight : half;
		var bottomLeft = (_bottomLeft <= half) ? _bottomLeft : half;

		if (mask._topLeft != Circular) {
			if (mask._topLeft < topLeft) topLeft = mask._topLeft;
		}
		if (mask._topRight != Circular) {
			if (mask._topRight < topRight) topRight = mask._topRight;
		}
		if (mask._bottomRight != Circular) {
			if (mask._bottomRight < bottomRight) bottomRight = mask._bottomRight;
		}
		if (mask._bottomLeft != Circular) {
			if (mask._bottomLeft < bottomLeft) bottomLeft = mask._bottomLeft;
		}

		return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
	}

	public static bool operator ==(Shape left, Shape right) {
		return left.Equals(right);
	}

	public static bool operator !=(Shape left, Shape right) {
		return !(left == right);
	}

	public static implicit operator Shape(int value) {
		return new(value);
	}

	public bool Equals(Shape other) {
		if (other._topLeft != _topLeft) return false;
		if (other._topRight != _topRight) return false;
		if (other._bottomRight != _bottomRight) return false;
		if (other._bottomLeft != _bottomLeft) return false;
		return true;
	}

	public override bool Equals(object? obj) {
		if (obj is not Shape s) return false;
		return Equals(s);
	}

	public override int GetHashCode() {
		return HashCode.Combine(_topLeft, _topRight, _bottomRight, _bottomLeft);
	}

	private static void CheckValue(double value,
			[CallerArgumentExpression(nameof(value))] string? paramName = null) {
		if (value < 0.0) {
			var msg = "Value must be greater than or equal to 0.0.";
			throw new ArgumentOutOfRangeException(paramName, value, msg);
		}
	}

	private static double ParseToken(string s) {
		if (s.Equals(CircularLongName, StringComparison.OrdinalIgnoreCase)) {
			return Circular;
		}
		if (s.Equals(CircularShortName, StringComparison.OrdinalIgnoreCase)) {
			return Circular;
		}
		return double.Parse(s);
	}
}
