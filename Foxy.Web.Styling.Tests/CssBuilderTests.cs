using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Foxy.Web.Styling
{
    public class CssBuilderTests
    {
        private static ICssBuilder CreateCssBuilder()
        {
            ServiceCollection coll = new ServiceCollection();
            coll.AddCssBuilder();
            var provider = coll.BuildServiceProvider();
            var css = provider.GetService<ICssBuilder>();
            return css;
        }

        [Fact]
        public void CssBuilder_Create_creates_new_CssDefinition()
        {
            ICssBuilder css = CreateCssBuilder();

            var result = css.Create().ToString();

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void CssBuilder_Create_with_options_param_uses_it()
        {
            ICssBuilder css = CreateCssBuilder();

            var result = css.Create(new CssBuilderOptions
            {
                PropertyToClassNameConverter = (e) => "ok"
            }).Add(new { something = true }).ToString();

            result.Should().Be("ok");
        }

        [Fact]
        public void CssBuilder_indexer_processes_the_input_params()
        {
            var css = CreateCssBuilder();

            var result = css["c1", ("c2", true)].ToString();

            result.Should().Be("c1 c2");
        }

        [Fact]
        public void CssBuilder_indexer_skips_null()
        {
            var css = CreateCssBuilder();

            var result = css[(string)null].ToString();

            result.Should().Be("");
        }

        [Fact]
        public void CssBuilder_indexer_skips_empty()
        {
            var css = CreateCssBuilder();

            var result = css[""].ToString();

            result.Should().Be("");
        }

        [Fact]
        public void CssBuilder_indexer2_processes_the_input_params_with_function()
        {
            var css = CreateCssBuilder();

            var result = css["c1", ("c2", () => true)].ToString();

            result.Should().Be("c1 c2");
        }

        [Fact]
        public void CssBuilder_indexer2_skips_null()
        {
            var css = CreateCssBuilder();

            var result = css[null, ("c2", () => true)].ToString();

            result.Should().Be("c2");
        }
    }
}
