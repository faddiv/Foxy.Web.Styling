using System;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Defines a class that provides the mechanisms to build a style declaration block.
    /// </summary>
    public interface IStyleBuilder
    {
        /// <summary>
        /// Starts a StyleDefinition and adds all the arguments as stlye. The result is produced by .ToString().
        /// </summary>
        /// <param name="arguments">List of values that cen be converted to styles.</param>
        /// <returns>A StyleDefinition instance that contains the processed arguments, and can be used in the style attribute directly.</returns>
        /// <remarks>
        /// Example:
        /// <code>&lt;div style="@Styles[("width", "100px"),("height", "200px", true), new { BorderWidth = "1px" }]"&gt;...&lt;/div&gt;</code>
        /// </remarks>
        StyleDeclarationBlock this[params object[] arguments] { get; }

        /// <summary>
        /// Starts a StyleDefinition and adds all the arguments as stlye. The result is produced by .ToString().
        /// </summary>
        /// <param name="arguments">List of values that cen be converted to styles.</param>
        /// <returns>A StyleDefinition instance that contains the processed arguments, and can be used in the style attribute directly.</returns>
        StyleDeclarationBlock this[params (string, string, Func<bool>)[] arguments] { get; }

        /// <summary>
        /// Starts a StyleDefinition and adds all the arguments as stlye. The result is produced by .ToString().
        /// </summary>
        /// <param name="arguments">List of values that cen be converted to styles.</param>
        /// <returns>A StyleDefinition instance that contains the processed arguments, and can be used in the style attribute directly.</returns>
        StyleDeclarationBlock this[params (string, Func<string>, bool)[] arguments] { get; }

        /// <summary>
        /// Starts a StyleDefinition and adds all the arguments as stlye. The result is produced by .ToString().
        /// </summary>
        /// <param name="arguments">List of values that cen be converted to styles.</param>
        /// <returns>A StyleDefinition instance that contains the processed arguments, and can be used in the style attribute directly.</returns>
        StyleDeclarationBlock this[params (string, Func<string>, Func<bool>)[] arguments] { get; }

        /// <summary>
        /// Starts an empty StyleDefinition. The result is produced by .ToString().
        /// </summary>
        /// <returns>An empty StyleDefinition.</returns>
        StyleDeclarationBlock Create();
    }
}
