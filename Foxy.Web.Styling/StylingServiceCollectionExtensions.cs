using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Extends The <see cref="IServiceCollection"/> with AddCssBuilder and AddStyleBuilder.
    /// </summary>
    public static class StylingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the css builder services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">
        /// The <see cref="IServiceCollection"/> to add services to.
        /// </param>
        /// <param name="action">
        /// An <see cref="Action{CssBuilderOptions}"/>  to configure the provided <see cref="CssBuilderOptions"/>.
        /// </param>
        public static void AddCssBuilder(
            this IServiceCollection serviceCollection,
            Action<CssBuilderOptions> action = null)
        {
            serviceCollection.TryAddSingleton<ICssBuilder, CssBuilder>();
            serviceCollection.AddSingleton(p =>
            {
                var options = new CssBuilderOptions();
                action?.Invoke(options);
                return options;
            });
        }

        /// <summary>
        /// Adds the style builder services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">
        /// The <see cref="IServiceCollection"/> to add services to.
        /// </param>
        public static void AddStyleBuilder(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IStyleBuilder, StyleBuilder>();
        }
    }
}
