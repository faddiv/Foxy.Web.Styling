using System;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Implementation of the <see cref="ICssBuilder"/>. Use this class throught the interface.
    /// </summary>
    public class CssBuilder : ICssBuilder
    {
        private readonly CssBuilderOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssBuilder"/> class.
        /// </summary>
        /// <param name="options">An options object which modifies class name generation and other things.</param>
        public CssBuilder(CssBuilderOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public CssClassList this[params object[] arguments]
        {
            get
            {
                return Create().AddMultiple(arguments);
            }
        }

        /// <inheritdoc />
        public CssClassList this[string cssClass, params (string, Func<bool>)[] tuple]
        {
            get
            {
                return Create().Add(cssClass).Add(tuple);
            }
        }

        /// <inheritdoc />
        public CssClassList Create(CssBuilderOptions options = null)
        {
            return new CssClassList(options ?? _options);
        }
    }
}
