using System;
using System.Reflection;

namespace Blazorify.Utilities.Styling
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
        private readonly ThreadsafeCssBuilderCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssBuilderOptions"/> class.
        /// </summary>
        public CssBuilderOptions()
        {
            _cache = new ThreadsafeCssBuilderCache();
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
        /// Gets or sets a value indicating whether the class names should be checked before adding to the list.
        /// If true then if a css class added then it isn't added again.
        /// </summary>
        public bool ExcludeDuplication { get; set; } = false;

        /// <summary>
        /// Clears the connected cache. Use this if you change the settings.
        /// </summary>
        public void ClearCache()
        {
            _cache.ClearCache();
        }

        internal ThreadsafeCssBuilderCache GetCache()
        {
            return _cache;
        }
    }
}
