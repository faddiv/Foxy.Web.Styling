using Foxy.Web.Styling.Internals;
using System;
using System.Reflection;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Provides programmatic configuration for the css builder.
    /// </summary>
    /// <remarks>
    /// Also contains the caches for anonymous type to class names and enum to class name conversions
    /// since the convertes only called before caching but not after and this could lead inconsistent results.
    /// </remarks>
    public class CssBuilderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CssBuilderOptions"/> class.
        /// </summary>
        public CssBuilderOptions()
        {
        }

        /// <summary>
        /// Gets or sets the name converter for the property to name conversion which used for anonymous types.
        /// </summary>
        public Func<PropertyInfo, string> PropertyToClassNameConverter { get; set; } = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;

        /// <summary>
        /// Gets or sets the name converter for the enum to name conversion which used for enum types.
        /// </summary>
        public Func<Enum, string> EnumToClassNameConverter { get; set; } = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;

        /// <summary>
        /// Gets or sets a value indicating whether the class names should be unique in the class list.
        /// If true then a class added only once and if condition is false and the class is in the list then the class removed.
        /// </summary>
        public bool Deduplicate { get; set; } = false;
    }
}
