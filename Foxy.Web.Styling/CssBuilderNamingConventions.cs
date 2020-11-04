using System;
using System.Reflection;
using System.Text;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// A collection of functions which can be used to configure the <see cref="CssBuilderOptions"/>.
    /// </summary>
    public static class CssBuilderNamingConventions
    {
        private const char Hyphen = '-';
        private const char Underscore = '_';

        /// <summary>
        /// Returns with the property name without any conversion.
        /// </summary>
        /// <param name="info">Property to convert.</param>
        /// <returns>The name of the property.</returns>
        public static string None(PropertyInfo info)
        {
            return info.Name;
        }

        /// <summary>
        /// Returns with the enum name without any conversion.
        /// </summary>
        /// <param name="value">Enum value to convert.</param>
        /// <returns>The name of the enum value.</returns>
        public static string None(Enum value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns with a css class name which converted from the property name by
        /// replacing underscores(_) with hyphen(-).
        /// </summary>
        /// <param name="info">Property to convert.</param>
        /// <returns>The name of the property where the underscores(_) converted to hyphen(-).</returns>
        public static string UnderscoreToHyphen(PropertyInfo info)
        {
            return info.Name.Replace(Underscore, Hyphen);
        }

        /// <summary>
        /// Returns with a css class name which converted from the enum name by
        /// replacing underscores(_) with hyphen(-).
        /// </summary>
        /// <param name="value">Enum value to convert.</param>
        /// <returns>The name of the enum where the underscores(_) converted to hyphen(-).</returns>
        public static string UnderscoreToHyphen(Enum value)
        {
            return value.ToString().Replace(Underscore, Hyphen);
        }

        /// <summary>
        /// Returns with a css class name which converted from the property name by using kebab case syntax.
        /// </summary>
        /// <remarks>
        /// In kebab case every uppercase letter is converted to lowercase and with the exception of
        /// the first letter a hyphen(-) is put before it (example: CssClassName -> css-class-name).
        /// </remarks>
        /// <param name="info">Property to convert.</param>
        /// <returns>The name of the property converted to kebab case.</returns>
        public static string KebabCase(PropertyInfo info)
        {
            return KebabCase(info.Name, false);
        }

        /// <summary>
        /// Returns with a css class name which converted from the enum name by using kebab case syntax.
        /// </summary>
        /// <remarks>
        /// In kebab case every uppercase letter is converted to lowercase and with the exception of
        /// the first letter a hyphen(-) is put before it (example: CssClassName -> css-class-name).
        /// </remarks>
        /// <param name="value">Enum value to convert.</param>
        /// <returns>The name of the enum converted to kebab case.</returns>
        public static string KebabCase(Enum value)
        {
            return KebabCase(value.ToString(), false);
        }

        /// <summary>
        /// Returns with a css class name which converted from the property name by using kebab case syntax
        /// and the underscores(_) replaced with hyphen(-).
        /// </summary>
        /// <remarks>
        /// In kebab case every uppercase letter is converted to lowercase and with the exception of
        /// the first letter a hyphen(-) is put before it (example: CssClassName -> css-class-name).
        /// </remarks>
        /// <param name="info">Property to convert.</param>
        /// <returns>
        /// The name of the property converted to kebab case the underscores(_) replaced with hyphen(-).
        /// </returns>
        public static string KebabCaseWithUnderscoreToHyphen(PropertyInfo info)
        {
            return KebabCase(info.Name, true);
        }

        /// <summary>
        /// Returns with a css class name which converted from the enum name by using kebab case syntax
        /// and the underscores(_) replaced with hyphen(-).
        /// </summary>
        /// <remarks>
        /// In kebab case every uppercase letter is converted to lowercase and with the exception of
        /// the first letter a hyphen(-) is put before it (example: CssClassName -> css-class-name).
        /// </remarks>
        /// <param name="value">Enum value to convert.</param>
        /// <returns>
        /// The name of the enum converted to kebab case the underscores(_) replaced with hyphen(-).
        /// </returns>
        public static string KebabCaseWithUnderscoreToHyphen(Enum value)
        {
            return KebabCase(value.ToString(), true);
        }

        private static string KebabCase(string name, bool underscoreToHyphen)
        {
            var builder = new StringBuilder(name.Length * 2);
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (underscoreToHyphen && ch == Underscore)
                {
                    builder.Append(Hyphen);
                }
                else if (char.IsUpper(ch))
                {
                    if (i > 0)
                    {
                        builder.Append(Hyphen);
                    }

                    builder.Append(char.ToLowerInvariant(ch));
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }
    }
}
