using Foxy.Web.Styling.Internals;

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
