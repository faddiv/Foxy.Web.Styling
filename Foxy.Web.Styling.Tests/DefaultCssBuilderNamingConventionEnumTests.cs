using FluentAssertions;
using System;
using Xunit;

namespace Foxy.Web.Styling
{
    public class DefaultCssBuilderNamingConventionEnumTests
    {
        public CssBuilderOptions options = new CssBuilderOptions();

        public Dummy PascalCase = Dummy.PascalCase;

        public Dummy PascalCase_WithUnderScore = Dummy.PascalCase_WithUnderScore;

        [Fact]
        public void DefaultValue_is_correct()
        {
            options.EnumToClassNameConverter.Should().Be(new Func<Enum, string>(CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen));
        }

        [Fact]
        public void None_doesnt_convert()
        {
            options.EnumToClassNameConverter = CssBuilderNamingConventions.None;

            var result = options.EnumToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("PascalCase_WithUnderScore");
        }

        [Fact]
        public void UnderscoreToHyphen_converts_underscore()
        {
            options.EnumToClassNameConverter = CssBuilderNamingConventions.UnderscoreToHyphen;

            var result = options.EnumToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("PascalCase-WithUnderScore");
        }

        [Fact]
        public void KebabCase_converts_to_kebab_case()
        {
            options.EnumToClassNameConverter = CssBuilderNamingConventions.KebabCase;

            var result = options.EnumToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("pascal-case_-with-under-score");
        }

        [Fact]
        public void KebabCaseWithUnderscoreToHyphen_converts_to_kebab_case_and_undersore_to_hyphen()
        {
            options.EnumToClassNameConverter = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;

            var result = options.EnumToClassNameConverter(PascalCase_WithUnderScore);

            result.Should().Be("pascal-case--with-under-score");
        }

        public enum Dummy
        {
            PascalCase,
            PascalCase_WithUnderScore
        }
    }
}
