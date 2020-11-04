using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Blazorify.Utilities.Styling
{
    public class DefaultCssBuilderNamingConventionPropertyTests
    {
        public CssBuilderOptions options = new CssBuilderOptions();

        public PropertyInfo PascalCase_WithUnderScore = typeof(Dummy).GetProperty(nameof(Dummy.PascalCase_WithUnderScore));

        [Fact]
        public void DefaultValue_is_correct()
        {
            options.PropertyToClassNameConverter.Should().Be(new Func<PropertyInfo, string>(CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen));
        }

        [Fact]
        public void None_doesnt_convert()
        {
            options.PropertyToClassNameConverter = CssBuilderNamingConventions.None;

            var result = options.PropertyToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("PascalCase_WithUnderScore");
        }

        [Fact]
        public void UnderscoreToHyphen_converts_underscore()
        {
            options.PropertyToClassNameConverter = CssBuilderNamingConventions.UnderscoreToHyphen;

            var result = options.PropertyToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("PascalCase-WithUnderScore");
        }

        [Fact]
        public void KebabCase_converts_to_kebab_case()
        {
            options.PropertyToClassNameConverter = CssBuilderNamingConventions.KebabCase;

            var result = options.PropertyToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("pascal-case_-with-under-score");
        }

        [Fact]
        public void KebabCaseWithUnderscoreToHyphen_converts_to_kebab_case_and_undersore_to_hyphen()
        {
            options.PropertyToClassNameConverter = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;

            var result = options.PropertyToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("pascal-case--with-under-score");
        }

        private class Dummy
        {
            public bool PascalCase_WithUnderScore { get; set; }

        }
    }
}
