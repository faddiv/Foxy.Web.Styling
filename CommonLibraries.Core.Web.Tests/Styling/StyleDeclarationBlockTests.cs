using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blazorify.Utilities.Styling
{
    public class StyleDeclarationBlockTests
    {
        private const string Width = "width";
        private const string Border = "border";
        private const string Height = "height";
        private const string Value1 = "100px";
        private const string Value2 = "200px";
        private const string Value3 = "1px";
        private const string Result = "width:100px;height:200px";

        private StyleBuilder styleBuilder = new StyleBuilder();
        private StyleDeclarationBlock CreateStyleDefinition()
        {
            return styleBuilder.Create();
        }

        [Fact]
        public void Add_string_string_bool_adds_stlyes_if_condition_is_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(Width, Value1, true)
                .Add(Border, Value3, false)
                .Add(Height, Value2, true)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_string_string_Func_bool_adds_stlyes_if_condition_is_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(Width, Value1, () => true)
                .Add(Border, Value3, () => false)
                .Add(Height, Value2, () => true)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_string_Func_string_bool_adds_stlyes_if_condition_is_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(Width, () => Value1, true)
                .Add(Border, () => Value3, false)
                .Add(Height, () => Value2, true)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_string_Func_string_Func_bool_adds_stlyes_if_condition_is_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(Width, () => Value1, () => true)
                .Add(Border, () => Value3, () => false)
                .Add(Height, () => Value2, () => true)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_StyleBuilder_adds_stlyes()
        {
            var builder = CreateStyleDefinition().Add(Width, Value1);
            var builderOther = CreateStyleDefinition()
                .Add(Height, Value2);

            var result = builder.Add(builderOther)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_Dictionary_adds_style_key()
        {
            var builder = CreateStyleDefinition().Add(Width, Value1);
            var attributes = new Dictionary<string, object>
            {
                {"style", $" {Height} : {Value2} " }
            };

            var result = builder.Add(attributes)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Add_object_adds_properties_as_styles()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(new
            {
                TextAlign = "center",
                zIndex = 100,
                BackgroundColor = "black"
            }).ToString();

            result.Should().Be("text-align:center;z-index:100;background-color:black");
        }

        [Fact]
        public void Add_object_renders_prefixes_correctly()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(new
            {
                _webkitTransition = "all 4s ease",
                _mozTransition = "all 4s ease",
                _msTransition = "all 4s ease",
                _oTransition = "all 4s ease",
            }).ToString();

            result.Should().Be("-webkit-transition:all 4s ease;-moz-transition:all 4s ease;-ms-transition:all 4s ease;-o-transition:all 4s ease");
        }

        [Fact]
        public void Add_object_handles_null()
        {
            var builder = CreateStyleDefinition();

            var result = builder.Add(new
            {
                width = (string)null
            }).ToString();

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void AddMultiple_adds_string_string()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, Value1),
                (Height, Value2))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_string_Func_string()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, new Func<string>(() => Value1)),
                (Height, new Func<string>(() => Value2)))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_string_string_bool_if_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, Value1, true),
                (Border, Value3, false),
                (Height, Value2, true))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_string_string_Func_bool_if_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, Value1, new Func<bool>(() => true)),
                (Border, Value3, new Func<bool>(() => false)),
                (Height, Value2, new Func<bool>(() => true)))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_string_Func_string_bool_if_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, new Func<string>(() => Value1), true),
                (Border, new Func<string>(() => Value3), false),
                (Height, new Func<string>(() => Value2), true))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_string_Func_string_Func_bool_if_true()
        {
            var builder = CreateStyleDefinition();

            var result = builder.AddMultiple(
                (Width, new Func<string>(() => Value1), new Func<bool>(() => true)),
                (Border, new Func<string>(() => Value3), new Func<bool>(() => false)),
                (Height, new Func<string>(() => Value2), new Func<bool>(() => true)))
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_StyleBuilder()
        {
            var builder = CreateStyleDefinition().Add(Width, Value1);
            var builderOther = CreateStyleDefinition()
                .Add(Height, Value2);

            var result = builder.AddMultiple(builderOther)
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void AddMultiple_adds_Dictionary()
        {
            var builder = CreateStyleDefinition().Add(Width, Value1);
            var attributes = new Dictionary<string, object>
            {
                {"style", $"{Height}: {Value2}" }
            };

            var result = builder.AddMultiple(attributes)
                .ToString();

            result.Should().Be(Result);
        }

        [Theory]
        [InlineData(Width, true)]
        [InlineData(Height, false)]
        public void HasStyle_returns_true_if_class_found(string propertyName, bool found)
        {
            var style = CreateStyleDefinition().Add(Width, Value1);

            style.HasStyle(propertyName).Should().Be(found);
        }

        [Theory]
        [InlineData(Width, Value1)]
        [InlineData(Height, null)]
        public void GetPropertyValue_returns_value_if_found(string propertyName, string value)
        {
            var style = CreateStyleDefinition().Add(Width, Value1);

            style.GetPropertyValue(propertyName).Should().Be(value);
        }

        [Fact]
        public void AddMultiple_adds_Object()
        {
            var builder = CreateStyleDefinition();


            var result = builder.AddMultiple(new
            {
                Width = Value1,
                Height = Value2
            })
                .ToString();

            result.Should().Be(Result);
        }

        [Fact]
        public void Styles_contains_added_stlyes()
        {
            var builder = CreateStyleDefinition()
                .Add(Width, Value1)
                .Add(Height, Value2)
                .Add(Border, Value3);

            builder.Styles.Should().HaveCount(3);
        }
    }
}
