using Avalonia.Media;
using System.ComponentModel;
using System.Globalization;

namespace Sy.Avalonia.Material.Media;

/// <summary>
/// <see cref="Elevation"/> converter for Avalonia's XAML parser.
/// </summary>
public class ElevationConverter : TypeConverter {
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
		if (sourceType == typeof(string)) return true;
		if (sourceType == typeof(int)) return true;
		return false;
	}

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) {
		if (value is string s) {
			return Elevation.Parse(s);
		}
		if (value is int i) {
			return new Elevation(i);
		}
		return null;
	}
}

/// <summary>
/// A distance from the front of one material surface to the front of another.
/// </summary>
/// <remarks>
/// This value is measured along the z-axis in density-independent pixels (dps).
/// <para/>
/// This distance is typically measured using shadows.
/// </remarks>
/// <see href="https://m3.material.io/styles/elevation/applying-elevation">
/// Elevation — Material Design 3
/// </see>
[TypeConverter(typeof(ElevationConverter))]
public readonly struct Elevation : IEquatable<Elevation> {
	private readonly int _value;

	private readonly record struct PartialShadow(double X, double Y, double Blur, double Spread);

	private const byte UmbraOpacity = 51;
	private const byte PenumbraOpacity = 36;
	private const byte AmbientOpacity = 31;

	private static readonly Dictionary<int, PartialShadow> _umbraPresets = new() {
		{  0, new(0,  0,  0,  0) },
		{  1, new(0,  2,  1, -1) },
		{  2, new(0,  3,  1, -2) },
		{  3, new(0,  3,  3, -2) },
		{  4, new(0,  2,  4, -1) },
		{  5, new(0,  3,  5, -1) },
		{  6, new(0,  3,  5, -1) },
		{  7, new(0,  4,  5, -2) },
		{  8, new(0,  5,  5, -3) },
		{  9, new(0,  5,  6, -3) },
		{ 10, new(0,  6,  6, -3) },
		{ 11, new(0,  6,  7, -4) },
		{ 12, new(0,  7,  8, -4) },
		{ 13, new(0,  7,  8, -4) },
		{ 14, new(0,  7,  9, -4) },
		{ 15, new(0,  8,  9, -5) },
		{ 16, new(0,  8, 10, -5) },
		{ 17, new(0,  8, 11, -5) },
		{ 18, new(0,  9, 11, -5) },
		{ 19, new(0,  9, 12, -6) },
		{ 20, new(0, 10, 13, -6) },
		{ 21, new(0, 10, 13, -6) },
		{ 22, new(0, 10, 14, -6) },
		{ 23, new(0, 11, 14, -7) },
		{ 24, new(0, 11, 15, -7) },
	};

	private static readonly Dictionary<int, PartialShadow> _penumbraPresets = new() {
		{  0, new(0,  0,  0,  0) },
		{  1, new(0,  1,  1,  0) },
		{  2, new(0,  2,  2,  0) },
		{  3, new(0,  3,  4,  0) },
		{  4, new(0,  4,  5,  0) },
		{  5, new(0,  5,  8,  0) },
		{  6, new(0,  6, 10,  0) },
		{  7, new(0,  7, 10,  1) },
		{  8, new(0,  8, 10,  1) },
		{  9, new(0,  9, 12,  1) },
		{ 10, new(0, 10, 14,  1) },
		{ 11, new(0, 11, 15,  1) },
		{ 12, new(0, 12, 17,  2) },
		{ 13, new(0, 13, 19,  2) },
		{ 14, new(0, 14, 21,  2) },
		{ 15, new(0, 15, 22,  2) },
		{ 16, new(0, 16, 24,  2) },
		{ 17, new(0, 17, 26,  2) },
		{ 18, new(0, 18, 28,  2) },
		{ 19, new(0, 19, 29,  2) },
		{ 20, new(0, 20, 31,  3) },
		{ 21, new(0, 21, 33,  3) },
		{ 22, new(0, 22, 35,  3) },
		{ 23, new(0, 23, 36,  3) },
		{ 24, new(0, 24, 38,  3) },
	};

	private static readonly Dictionary<int, PartialShadow> _ambientPresets = new() {
		{  0, new(0,  0,  0,  0) },
		{  1, new(0,  1,  3,  0) },
		{  2, new(0,  1,  5,  0) },
		{  3, new(0,  1,  8,  0) },
		{  4, new(0,  1, 10,  0) },
		{  5, new(0,  1, 14,  0) },
		{  6, new(0,  1, 18,  0) },
		{  7, new(0,  2, 16,  1) },
		{  8, new(0,  3, 14,  2) },
		{  9, new(0,  3, 16,  2) },
		{ 10, new(0,  4, 18,  3) },
		{ 11, new(0,  4, 20,  3) },
		{ 12, new(0,  5, 22,  4) },
		{ 13, new(0,  5, 24,  4) },
		{ 14, new(0,  5, 26,  4) },
		{ 15, new(0,  6, 28,  5) },
		{ 16, new(0,  6, 30,  5) },
		{ 17, new(0,  6, 32,  5) },
		{ 18, new(0,  7, 34,  6) },
		{ 19, new(0,  7, 36,  6) },
		{ 20, new(0,  8, 38,  7) },
		{ 21, new(0,  8, 40,  7) },
		{ 22, new(0,  8, 42,  7) },
		{ 23, new(0,  9, 44,  8) },
		{ 24, new(0,  9, 46,  8) },
	};

	/// <summary>
	/// Creates a new <see cref="Elevation"/> instance with the default distance of 0dps.
	/// </summary>
	public Elevation() : this(0) {
		//
	}

	/// <summary>
	/// Creates a new <see cref="Elevation"/> instance with a specified value.
	/// </summary>
	/// <param name="value">
	/// The distance measured along the z-axis, in dps.
	/// </param>
	public Elevation(int value) {
		if (value < 0 || value > 24) {
			var msg = "Value must range between 0 and 24.";
			throw new ArgumentOutOfRangeException(nameof(value), value, msg);
		}
		_value = value;
	}

	public static bool operator ==(Elevation left, Elevation right) {
		return left.Equals(right);
	}

	public static bool operator !=(Elevation left, Elevation right) {
		return !(left == right);
	}

	public static implicit operator Elevation(int value) {
		return new(value);
	}

	public static Elevation Parse(string s) {
		try {
			var value = int.Parse(s);
			return new(value);
		} catch (FormatException) {
			throw;
		} catch (Exception e) {
			throw new FormatException(null, e);
		}
	}

	/// <summary>
	/// Creates a composite shadow that visualizes this elevation.
	/// </summary>
	/// <returns>
	/// A composite shadow that visualizes this elevation.
	/// </returns>
	public BoxShadows ToBoxShadows(Color color) {
		var umbraColor = Color.FromArgb(UmbraOpacity, color.R, color.G, color.B);
		var penumbraColor = Color.FromArgb(PenumbraOpacity, color.R, color.G, color.B);
		var ambientColor = Color.FromArgb(AmbientOpacity, color.R, color.G, color.B);

		var umbra = ToBoxShadow(_umbraPresets[_value], umbraColor);
		var penumbra = ToBoxShadow(_penumbraPresets[_value], penumbraColor);
		var ambient = ToBoxShadow(_ambientPresets[_value], ambientColor);

		var shadows = new BoxShadows(umbra, [penumbra, ambient]);
		return shadows;
	}

	public bool Equals(Elevation other) {
		return other._value == _value;
	}

	public override bool Equals(object? obj) {
		if (obj is not Elevation e) return false;
		return Equals(e);
	}

	public override int GetHashCode() {
		return _value.GetHashCode();
	}

	private static BoxShadow ToBoxShadow(PartialShadow partialShadow, Color color) {
		var shadow = new BoxShadow() {
			Blur = partialShadow.Blur,
			Color = color,
			OffsetX = partialShadow.X,
			OffsetY = partialShadow.Y,
			Spread = partialShadow.Spread,
		};
		return shadow;
	}
}