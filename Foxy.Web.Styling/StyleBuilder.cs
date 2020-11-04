using Foxy.Web.Styling.Internals;
using System;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Implementation of the <see cref="IStyleBuilder"/>. Use this class throught the interface.
    /// </summary>
    public class StyleBuilder : IStyleBuilder
    {
        private readonly ThreadsafeStyleBuilderCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBuilder"/> class.
        /// </summary>
        public StyleBuilder()
        {
            _cache = new ThreadsafeStyleBuilderCache();
        }

        /// <inheritdoc />
        public StyleDeclarationBlock this[params object[] arguments]
        {
            get
            {
                return Create().AddMultiple(arguments);
            }
        }

        /// <inheritdoc />
        public StyleDeclarationBlock this[params (string, string, Func<bool>)[] arguments]
        {
            get
            {
                var style = Create();
                foreach (var item in arguments)
                {
                    style.Add(item.Item1, item.Item2, item.Item3);
                }

                return style;
            }
        }

        /// <inheritdoc />
        public StyleDeclarationBlock this[params (string, Func<string>, bool)[] arguments]
        {
            get
            {
                var style = Create();
                foreach (var item in arguments)
                {
                    style.Add(item.Item1, item.Item2, item.Item3);
                }

                return style;
            }
        }

        /// <inheritdoc />
        public StyleDeclarationBlock this[params (string, Func<string>, Func<bool>)[] arguments]
        {
            get
            {
                var style = Create();
                foreach (var item in arguments)
                {
                    style.Add(item.Item1, item.Item2, item.Item3);
                }

                return style;
            }
        }

        /// <inheritdoc />
        public StyleDeclarationBlock Create()
        {
            return new StyleDeclarationBlock(_cache);
        }

        /// <summary>
        /// Clears thy style cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.ClearCache();
        }
    }
}
