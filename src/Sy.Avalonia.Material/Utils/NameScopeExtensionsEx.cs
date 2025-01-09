using Avalonia.Controls;

namespace Sy.Avalonia.Material.Utils;

internal static class NameScopeExtensionsEx {
	/// <summary>
	/// Finds a required named element in an <see cref="INameScope"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The element type.
	/// </typeparam>
	/// <param name="nameScope">
	/// THe name scope.
	/// </param>
	/// <param name="name">
	/// The name.
	/// </param>
	/// <returns>
	/// The named element.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// Any argument is null.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// <typeparamref name="T"/> does not match the control named <paramref name="name"/>.
	/// <para/>-or-<para/>
	/// A control named <paramref name="name"/> could not be found.
	/// </exception>
	public static T FindRequired<T>(this INameScope nameScope, string name) where T : class {
		var result = nameScope.Find<T>(name);
		if (result is not null) return result;

		var type = typeof(T).Name;
		var msg = $"A control named '{name}' of type '{type}' must be included in the template.";
		throw new InvalidOperationException(msg);
	}

}
