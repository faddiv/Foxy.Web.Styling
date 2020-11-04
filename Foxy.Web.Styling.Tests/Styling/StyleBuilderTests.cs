using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Blazorify.Utilities.Styling
{
    public class StyleBuilderTests
    {
        private const string Width = "width";
        private const string Border = "border";
        private const string Height = "height";
        private const string Value1 = "100px";
        private const string Value2 = "200px";
        private const string Value3 = "1px";
        private const string Result = "width:100px;height:200px";

        private IStyleBuilder CreateStyleBuilder()
        {
            return new StyleBuilder();
        }

        [Fact]
        public void indexer_Should_apply_values()
        {
            var style = CreateStyleBuilder();
            var result = style[(Width, Value1, true), (Border, Value3, false), (Height, Value2, true)].ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void indexer2_Should_apply_values()
        {
            var style = CreateStyleBuilder();
            var result = style[
                (Width, Value1, () => true),
                (Border, Value3, () => false),
                (Height, Value2, () => true)].ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void indexer3_Should_apply_values()
        {
            var style = CreateStyleBuilder();
            var result = style[
                (Width, () => Value1, true),
                (Border, () => Value3, false),
                (Height, () => Value2, true)].ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void indexer4_Should_apply_values()
        {
            var style = CreateStyleBuilder();
            var result = style[
                (Width, () => Value1, () => true),
                (Border, () => Value3, () => false),
                (Height, () => Value2, () => true)].ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Create_creates_empty_style()
        {
            var style = CreateStyleBuilder();
            var result = style.Create().ToString();

            result.Should().BeNullOrEmpty();
        }
    }
}
