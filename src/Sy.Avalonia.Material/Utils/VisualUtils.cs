using Avalonia;

namespace Sy.Avalonia.Material.Utils;

internal static class VisualUtils {
	/// <summary>
	/// Travels up the visual tree to find the template root.
	/// </summary>
	/// <param name="element">
	/// The element to get the template root of.
	/// </param>
	/// <returns>
	/// The template root.
	/// </returns>
	public static AvaloniaObject? GetTemplateRoot(AvaloniaObject? element) {
		if (element is not StyledElement currentStyled) return element;
		if (currentStyled.TemplatedParent is null) return currentStyled;

		while(currentStyled.TemplatedParent is StyledElement nextStyled) {
			currentStyled = nextStyled;
		}

		return currentStyled;
	}

	/// <inheritdoc cref="GetTemplateRoot(AvaloniaObject?)"/>
	public static AvaloniaObject? GetTemplateRoot(object? element) {
		if (element is not AvaloniaObject avalonia) return null;
		return GetTemplateRoot(avalonia);
	}

	/// <summary>
	/// Travels up the visual tree searching for an ancestor within the template.
	/// </summary>
	/// <param name="root">
	/// The element under test.
	/// </param>
	/// <param name="search">
	/// The element being searched up the visual tree for.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="root"/> is a template descendant of, or is, 
	/// <paramref name="search"/>; otherwise, <see langword="false" />.
	/// </returns>
	public static bool IsTemplateDescendantOrSelf(AvaloniaObject? root, AvaloniaObject search) {
		if (ReferenceEquals(root, search)) return true;
		if (root is not StyledElement currentStyled) return false;

		while(currentStyled.TemplatedParent is StyledElement nextStyled) {
			if (ReferenceEquals(nextStyled, search)) return true;
		}

		return false;
	}

	/// <inheritdoc cref="IsTemplateDescendantOrSelf(AvaloniaObject?, AvaloniaObject)"/>
	public static bool IsTemplateDescendantOrSelf(object? test, object? search) {
		if (test is not AvaloniaObject testElement) return false;
		if (search is not AvaloniaObject searchElement) return false;
		return IsTemplateDescendantOrSelf(testElement, searchElement);
	}
}
