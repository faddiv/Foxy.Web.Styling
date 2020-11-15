using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Foxy.Web.Styling
{
    public class CssClassListExcludeDuplicationTests
    {
        private CssBuilder cssBuilder = new CssBuilder(new CssBuilderOptions
        {
            Deduplicate = true
        });

        private CssClassList CreateCssDefinition(CssBuilderOptions options = null)
        {
            return cssBuilder.Create(options);
        }

        [Fact]
        public void Add_duplicates_by_default()
        {
            var result = CreateCssDefinition(new CssBuilderOptions())
                .Add("c1")
                .Add("c1")
                .ToString();

            result.Should().Be("c1 c1");
        }

        [Fact]
        public void AddMultiple_adds_a_class_only_single_time()
        {
            var result = CreateCssDefinition()
                .AddMultiple("c1", ("c1", true), new { c1 = true })
                .ToString();

            result.Should().Be("c1");
        }

        [Fact]
        public void Add_adds_a_class_only_single_time()
        {
            var result = CreateCssDefinition()
                .Add("c1")
                .Add(("c1", true))
                .Add(new { c1 = true })
                .ToString();

            result.Should().Be("c1");
        }

        [Fact]
        public void Add_adds_a_class_only_the_first_time()
        {
            var result = CreateCssDefinition()
                .Add("c1")
                .Add("c2")
                .Add("c1")
                .ToString();

            result.Should().Be("c1 c2");
        }

        [Fact]
        public void Add_considers_multi_css_in_string_with_condition()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add("c2 c3", true)
                .ToString();

            result.Should().Be("c1 c2 c3");
        }

        [Fact]
        public void Add_considers_multi_css_in_string_with_predicate()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add("c2 c3", () => true)
                .ToString();

            result.Should().Be("c1 c2 c3");
        }

        [Fact]
        public void Add_considers_multi_css_in_tuple_with_condition()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(("c2 c3", true), ("c3 c4", true))
                .ToString();

            result.Should().Be("c1 c2 c3 c4");
        }

        [Fact]
        public void Add_considers_multi_css_in_enumerable()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(new[] { "c2 c3", "c3 c4" })
                .ToString();

            result.Should().Be("c1 c2 c3 c4");
        }

        [Fact]
        public void Add_considers_multi_css_in_CssBuilder()
        {
            var other = CreateCssDefinition().Add("c2 c3");
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(other)
                .ToString();

            result.Should().Be("c1 c2 c3");
        }

        [Fact]
        public void Add_considers_multi_css_in_tuple_with_predicate()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(("c2 c3", () => true), ("c3 c4", () => true))
                .ToString();

            result.Should().Be("c1 c2 c3 c4");
        }

        [Fact]
        public void Add_considers_multi_css_in_Dictionary()
        {
            var dic = new Dictionary<string, object>
            {
                {"class", "c2 c3" }
            };
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(dic)
                .ToString();

            result.Should().Be("c1 c2 c3");
        }

        [Fact]
        public void Add_considers_multi_css_in_AnonymousType()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2")
                .Add(new { c2 = true, c3 = true })
                .ToString();

            result.Should().Be("c1 c2 c3");
        }

        [Fact]
        public void Add_with_condition_false_removes_added_class()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2 c3 c4")
                .Add(new { c1 = false })
                .Add("c2", false)
                .Add(("c3", false))
                .ToString();

            result.Should().Be("c4");
        }

        [Fact]
        public void Add_with_condition_false_can_remove_multiple_class()
        {
            var result = CreateCssDefinition()
                .Add("c1 c2 c3")
                .Add("c1 c2", false)
                .ToString();

            result.Should().Be("c3");
        }
    }
}
