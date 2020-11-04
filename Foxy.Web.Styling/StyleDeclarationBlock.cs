using Foxy.Web.Styling.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Foxy.Web.Styling
{
    /// <summary>
    /// Represents a group of style rules. The final result is obtainable with the ToString call.
    /// </summary>
    public class StyleDeclarationBlock
    {
        private readonly List<StyleDeclaration> _styles;
        private readonly ThreadsafeStyleBuilderCache _cache;

        internal StyleDeclarationBlock(ThreadsafeStyleBuilderCache cache)
        {
            _cache = cache;
            _styles = new List<StyleDeclaration>();
        }

        /// <summary>
        /// Gets the list of added style rules.
        /// </summary>
        public IReadOnlyList<StyleDeclaration> Styles => _styles;

        /// <summary>
        /// Adds a stlye rule to the end of the declaration block if the
        /// condition is true and the value is not null or empty.
        /// </summary>
        /// <param name="property">
        /// The property part of the style rule.
        /// </param>
        /// <param name="value">
        /// The value part of the style rule. Can be null or empty in which case
        /// the rule won't be added.
        /// </param>
        /// <param name="condition">
        /// A condition which determines if the style rule should be added.
        /// </param>
        /// <exception cref="ArgumentException">Property is null or empty.</exception>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(string property, string value, bool condition = true)
        {
            AddInner(property, value, condition);
            return this;
        }

        /// <summary>
        /// Adds a stlye rule to the end of the declaration block if the
        /// condition is true and the value function evaulates to not null or empty.
        /// </summary>
        /// <param name="property">
        /// The property part of the style rule.
        /// </param>
        /// <param name="value">
        /// A Function that calculate the value part of the style rule.
        /// If it returns null or empty then the style rule won't be added.
        /// </param>
        /// <param name="condition">
        /// A condition which determines if the style rule should be added.
        /// </param>
        /// <exception cref="ArgumentException">Property is null or empty.</exception>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(string property, Func<string> value, bool condition = true)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            AddInner(property, value(), condition);
            return this;
        }

        /// <summary>
        /// Adds a stlye rule to the end of the declaration block if the
        /// predicate evaulates to true and the value is not null or empty.
        /// </summary>
        /// <param name="property">
        /// The property part of the style rule.
        /// </param>
        /// <param name="value">
        /// The value part of the style rule. Can be null or empty in which case
        /// the rule won't be added.
        /// </param>
        /// <param name="predicate">
        /// A predicate which if evaulates to true then the style rule added othervise it skipped.
        /// </param>
        /// <exception cref="ArgumentException">Property is null or empty.</exception>
        /// <exception cref="ArgumentNullException">The value or the predicate is null.</exception>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(string property, string value, Func<bool> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            AddInner(property, value, predicate());
            return this;
        }

        /// <summary>
        /// Adds a stlye rule to the end of the declaration block if the
        /// predicate evaulates to true and the value function evaulates to not null or empty.
        /// </summary>
        /// <param name="property">
        /// The property part of the style rule.
        /// </param>
        /// <param name="value">
        /// A Function that calculate the value part of the style rule.
        /// If it returns null or empty then the style rule won't be added.
        /// </param>
        /// <param name="predicate">
        /// A predicate which if evaulates to true then the style rule added othervise it skipped.
        /// </param>
        /// <exception cref="ArgumentException">Property is null or empty.</exception>
        /// <exception cref="ArgumentNullException">The value or the predicate is null.</exception>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(string property, Func<string> value, Func<bool> predicate)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            AddInner(property, value(), predicate());
            return this;
        }

        /// <summary>
        /// Adds the styles from the given block to the end of this block.
        /// It can be null.
        /// </summary>
        /// <param name="declarationBlock">A style declaration block or null.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(StyleDeclarationBlock declarationBlock)
        {
            if (declarationBlock is null)
            {
                return this;
            }

            foreach (var item in declarationBlock._styles)
            {
                AddInner(item.Property, item.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds the contents of the style key to the end of this block.
        /// The value in the dictionary should be string or convertible to string.
        /// This method doesn't add anything if the attributes is null or there is
        /// no style key in it. The value is broken into style rules so it needs
        /// to be valid. ( Meaning ; separated list with property:value pairs.)
        /// </summary>
        /// <param name="attributes">A dictionary which may contain style or null.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(IReadOnlyDictionary<string, object> attributes)
        {
            if (attributes is null)
            {
                return this;
            }

            if (attributes.TryGetValue("style", out var style)
                && !(style is null))
            {
                var styleStr = style as string ?? style.ToString();
                if (styleStr.Length == 0)
                {
                    return this;
                }

                var stylePairs = styleStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var stylePairStr in stylePairs)
                {
                    var stylePair = stylePairStr.Split(new[] { ':' });
                    if (stylePair.Length != 2)
                    {
                        throw new ArgumentException($"Invalid style found in the attributes.style: '{styleStr}'");
                    }

                    AddInner(stylePair[0].Trim(), stylePair[1].Trim());
                }
            }

            return this;
        }

        /// <summary>
        /// Adds the properties to the end of this block. The property names used
        /// as style property. They converted to kebab-case format and underscores
        /// are replaced with hyphen. The property values can be any type that
        /// can be converted to string or null. The conversion method is cached
        /// which is bound to the style builder.
        /// </summary>
        /// <remarks>
        /// Examples:
        /// <code>Stlye.Add(new { BorderWidth = "1px", _webkitTransition = "ease" }) -> border-width:1px;-webkit-transition:ease</code>
        /// </remarks>
        /// <param name="values">An anonymous object where the properties used as stlye rules.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock Add(object values)
        {
            if (values is null)
            {
                return this;
            }

            var type = values.GetType();
            var extractor = _cache.GetOrAdd(type, CreateExtractor);
            extractor(values, AddInner);

            return this;
        }

        /// <summary>
        /// Adds multiple object to the end of the declaration block. The objects
        /// can be any types that the Add methods can process.
        /// </summary>
        /// <param name="values">an array of style declaration.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public StyleDeclarationBlock AddMultiple(params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                return this;
            }

            foreach (var item in values)
            {
                if (item == null)
                {
                    continue;
                }

                if (item is ValueTuple<string, string> type1)
                {
                    AddInner(type1.Item1, type1.Item2);
                }
                else if (item is ValueTuple<string, Func<string>> type2)
                {
                    AddInner(type2.Item1, type2.Item2());
                }
                else if (item is ValueTuple<string, string, bool> type3)
                {
                    AddInner(type3.Item1, type3.Item2, type3.Item3);
                }
                else if (item is ValueTuple<string, Func<string>, bool> type4)
                {
                    AddInner(type4.Item1, type4.Item2(), type4.Item3);
                }
                else if (item is ValueTuple<string, string, Func<bool>> type5)
                {
                    AddInner(type5.Item1, type5.Item2, type5.Item3());
                }
                else if (item is ValueTuple<string, Func<string>, Func<bool>> type6)
                {
                    AddInner(type6.Item1, type6.Item2(), type6.Item3());
                }
                else if (item is StyleDeclarationBlock styleBuilder)
                {
                    Add(styleBuilder);
                }
                else if (item is IReadOnlyDictionary<string, object> attributes)
                {
                    Add(attributes);
                }
                else
                {
                    Add(item);
                }
            }

            return this;
        }

        /// <summary>
        /// Determines whether a property is in the style declaration block.
        /// </summary>
        /// <param name="propertyName">The property to locate in the rules. It can be null.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public bool HasStyle(string propertyName)
        {
            return _styles.Any(e => e.Property == propertyName);
        }

        /// <summary>
        /// Finds and returns with the value of the property. If not found then it returns null.
        /// </summary>
        /// <param name="propertyName">A property name to find.</param>
        /// <returns>Returns the value of the style declaration or null.</returns>
        public string GetPropertyValue(string propertyName)
        {
            return _styles.Find(e => e.Property == propertyName).Value;
        }

        /// <summary>
        /// Returns with the assembled style definition.
        /// </summary>
        /// <returns>
        /// A string where the style definitions joined with semicolons.
        /// </returns>
        public override string ToString()
        {
            return string.Join(";", _styles);
        }

        private void AddInner(string property, string value, bool condition = true)
        {
            ValidateProperty(property);
            if (condition && !string.IsNullOrEmpty(value))
            {
                _styles.Add(new StyleDeclaration(property, value));
            }
        }

        private ProcessStyleDelegate CreateExtractor(Type type)
        {
            var lines = new List<Expression>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var valuesParam = Expression.Parameter(typeof(object));
            var addMethod = Expression.Parameter(typeof(AddStyleDelegate));
            var valuesVar = Expression.Variable(type);
            var castedValuesParam = Expression.Convert(valuesParam, type);
            var valuesVarAssigment = Expression.Assign(valuesVar, castedValuesParam);
            var trueConstant = Expression.Constant(true);
            var nullConstant = Expression.Constant(null, typeof(object));
            var toStringMethod = typeof(object).GetMethod(nameof(object.ToString));
            lines.Add(valuesVarAssigment);
            foreach (var property in properties)
            {
                var valueGetter = (Expression)Expression.Property(valuesVar, property);
                var notNull = Expression.ReferenceNotEqual(Expression.Convert(valueGetter, typeof(object)), nullConstant);
                var stringValue = Expression.Call(valueGetter, toStringMethod);
                var className = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen(property);
                var styleNameConstant = Expression.Constant(className);
                var invocation = Expression.Invoke(addMethod, styleNameConstant, stringValue, trueConstant);
                var conditionalAdd = Expression.IfThen(notNull, invocation);
                lines.Add(conditionalAdd);
            }

            var body = Expression.Block(new ParameterExpression[] { valuesVar }, lines);
            var method = Expression.Lambda<ProcessStyleDelegate>(body, valuesParam, addMethod);
            return method.Compile();
        }

        private void ValidateProperty(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException($"'{nameof(property)}' cannot be null or empty", nameof(property));
            }
        }
    }
}
