using Foxy.Web.Styling.Internals;
using System;
using System.Collections.Concurrent;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Implementation of the <see cref="ICssBuilder"/>. Use this class throught the interface.
    /// </summary>
    public class CssBuilder : ICssBuilder
    {
        private static readonly ConcurrentDictionary<CssBuilderOptions, ThreadsafeCssBuilderCache> _caches
            = new ConcurrentDictionary<CssBuilderOptions, ThreadsafeCssBuilderCache>();

        private readonly ThreadsafeCssBuilderCache _defaultCache;

        private readonly CssBuilderOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssBuilder"/> class.
        /// </summary>
        /// <param name="options">An options object which modifies class name generation and other things.</param>
        /// <exception cref="ArgumentNullException">options is null.</exception>
        public CssBuilder(CssBuilderOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _defaultCache = _caches.GetOrAdd(_options, CreateCacheForOptions);
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
        public CssClassList Create(CssBuilderOptions options = null)
        {
            var cache = options != null
                ? _caches.GetOrAdd(options, CreateCacheForOptions)
                : _defaultCache;
            return new CssClassList(options ?? _options, cache);
        }

        private static ThreadsafeCssBuilderCache CreateCacheForOptions(CssBuilderOptions arg)
        {
            return new ThreadsafeCssBuilderCache();
        }

    }
}
