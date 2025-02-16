using Avalonia.Media;

namespace Sy.Avalonia.Material.Utils;

public static class StringUtils
{
    /// <summary>
    /// Hex string representing color, ex. #0000FF for blue.
    /// </summary>
    /// <param name="argb">ARGB representation of a color.</param>
    public static string HexFromArgb(this uint argb) => "#" + argb.ToString("X8").Substring(2);
}

public static class ColorExt
{
    public static Color ParseColor(this string hex) => Color.Parse(hex);
}