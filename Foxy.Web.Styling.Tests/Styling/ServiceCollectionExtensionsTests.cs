using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blazorify.Utilities.Styling
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCssBuilder_registers_CssBuilder()
        {
            ServiceCollection coll = new ServiceCollection();
            coll.AddCssBuilder();

            var builderDescription = coll.Should().Contain(sd => sd.ServiceType == typeof(ICssBuilder)).Which;
            builderDescription.ImplementationType.Should().Be<CssBuilder>();
            builderDescription.Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddCssBuilder_registers_CssBuilderOptions()
        {
            ServiceCollection coll = new ServiceCollection();
            coll.AddCssBuilder();

            var builderDescription = coll.Should().Contain(sd => sd.ServiceType == typeof(CssBuilderOptions)).Which;
            builderDescription.Lifetime.Should().Be(ServiceLifetime.Singleton);
            builderDescription.ImplementationFactory.Should().NotBeNull();
        }

        [Fact]
        public void AddCssBuilder_uses_options_Action()
        {
            ServiceCollection coll = new ServiceCollection();
            var called = false;
            coll.AddCssBuilder((o) => called = true);

            var serviceProvider = coll.BuildServiceProvider();
            var builder = serviceProvider.GetService<ICssBuilder>();
            builder.Should().NotBeNull();
            called.Should().BeTrue();
        }

        [Fact]
        public void AddStyleBuilder_registers_StyleBuilder()
        {
            ServiceCollection coll = new ServiceCollection();
            coll.AddStyleBuilder();

            var builderDescription = coll.Should().Contain(sd => sd.ServiceType == typeof(IStyleBuilder)).Which;
            builderDescription.ImplementationType.Should().Be<StyleBuilder>();
            builderDescription.Lifetime.Should().Be(ServiceLifetime.Singleton);
        }
    }
}
