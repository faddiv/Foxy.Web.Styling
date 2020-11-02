using Blazorify.Utilities.Styling.Internals;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazorify.Utilities.Styling
{
    /// <summary>
    /// Represents a list of css classes. The final result is obtainable with the ToString call.
    /// </summary>
    public class CssClassList
    {
        private const string Separator = " ";
        private static readonly char[] _separatorArray = new[] { ' ' };

        private readonly List<string> _cssClasses;
        private readonly CssBuilderOptions _options;
        private readonly ThreadsafeCssBuilderCache _cache;

        internal CssClassList(CssBuilderOptions options)
        {
            _cssClasses = new List<string>();
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (_options.EnumToClassNameConverter == null)
            {
                throw new ArgumentException("Options.EnumToClassNameConverter can't be null.");
            }

            if (_options.PropertyToClassNameConverter == null)
            {
                throw new ArgumentException("Options.PropertyToClassNameConverter can't be null.");
            }

            _cache = options.GetCache();
        }

        /// <summary>
        /// Gets the list of added class names.
        /// </summary>
        public IReadOnlyList<string> CssClasses => _cssClasses;

        /// <summary>
        /// Adds multiple class definition which can be string, enum, (string, bool),
        /// (string, Func&gt;bool&lt;), IEnumerable&gt;string&lt;, another CssDefinition,
        /// IReadOnlyDictionary&gt;string, object&lt; with an optional class key.
        /// </summary>
        /// <param name="values">The list of class definitions.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList AddMultiple(params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                return this;
            }

            foreach (var value in values)
            {
                if (value is string strValue)
                {
                    AddInner(strValue);
                }
                else if (value is Enum enumValue)
                {
                    Add(enumValue);
                }
                else if (value is ValueTuple<string, bool> tupleWithCondition)
                {
                    AddInner(tupleWithCondition.Item1, tupleWithCondition.Item2);
                }
                else if (value is ValueTuple<string, Func<bool>> tupleWithPredicate)
                {
                    AddInner(tupleWithPredicate.Item1, tupleWithPredicate.Item2());
                }
                else if (value is IEnumerable<string> cssList)
                {
                    Add(cssList);
                }
                else if (value is CssClassList other)
                {
                    Add(other);
                }
                else if (value is IReadOnlyDictionary<string, object> attributes)
                {
                    Add(attributes);
                }
                else
                {
                    Add(value);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds the css class to the list if the second parameter is true.
        /// If the css class is null or empty it is skipped.
        /// </summary>
        /// <param name="cssClass">A css class.</param>
        /// <param name="condition">If true the class is added to the list.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(string cssClass, bool condition = true)
        {
            AddInner(cssClass, condition);
            return this;
        }

        /// <summary>
        /// Adds the css class to the list if the second parameter evaulates to true.
        /// If the css class is null or empty it is skipped.
        /// </summary>
        /// <param name="cssClass">A css class.</param>
        /// <param name="predicate">a predicate, if it returns true then the css class is added to the list.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(string cssClass, Func<bool> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            AddInner(cssClass, predicate());
            return this;
        }

        /// <summary>
        /// Adds the tuples to the list as css classes. The first parameter of the
        /// tuple is used as css class and it is added only if the second value is true.
        /// </summary>
        /// <param name="tuple">
        /// A tuple where the first parameter is a css class and the second
        /// value is a bool which determines if the class should be added.
        /// </param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(params (string, bool)[] tuple)
        {
            if (tuple == null || tuple.Length == 0)
            {
                return this;
            }

            foreach (var item in tuple)
            {
                AddInner(item.Item1, item.Item2);
            }

            return this;
        }

        /// <summary>
        /// Adds the tuples to the list as css classes. The first parameter of the
        /// tuple is used as css class and it is added only if the second function returns true.
        /// </summary>
        /// <param name="tuple">
        /// A tuple where the first parameter is a css class and the second
        /// value is a function which determines if the class should be added.
        /// </param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(params (string, Func<bool>)[] tuple)
        {
            if (tuple == null || tuple.Length == 0)
            {
                return this;
            }

            foreach (var item in tuple)
            {
                AddInner(item.Item1, item.Item2());
            }

            return this;
        }

        /// <summary>
        /// Adds the strings as classes to the list. If a string contains more than
        /// one css class, then it is broken to parts.
        /// </summary>
        /// <param name="cssList">A css list enumeration.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(IEnumerable<string> cssList)
        {
            if (cssList == null)
            {
                return this;
            }

            foreach (var value in cssList)
            {
                AddInner(value);
            }

            return this;
        }

        /// <summary>
        /// Adds the css classes from the other CssDefinition.
        /// </summary>
        /// <param name="cssDefinition">A CssDefinition instance.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(CssClassList cssDefinition)
        {
            if (cssDefinition == null)
            {
                return this;
            }

            foreach (var value in cssDefinition._cssClasses)
            {
                AddInner(value);
            }

            return this;
        }

        /// <summary>
        /// Adds the enum value as css class to the list. The Enum name is converted to css class
        /// name with <see cref="CssBuilderOptions.EnumToClassNameConverter"/>. This conversion is cached
        /// which is bound to the options.
        /// </summary>
        /// <param name="enumValue">An enum value.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(Enum enumValue)
        {
            if (enumValue == null)
            {
                return this;
            }

            var cssClass = _cache.GetOrAdd(enumValue, (ev) => _options.EnumToClassNameConverter.Invoke(ev));
            AddInner(cssClass);
            return this;
        }

        /// <summary>
        /// Adds the properties as css classes to the list. For class name the property name used
        /// after converted with <see cref="CssBuilderOptions.PropertyToClassNameConverter"/>. The properties
        /// added only if it is a boolean with value true. On the object all property must be bool type.
        /// The conversion method is cached which is bound to the options.
        /// </summary>
        /// <param name="values">An object that has only bool property. Preferably an anonymous type.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(object values)
        {
            if (values == null)
            {
                return this;
            }

            var type = values.GetType();
            var extractor = _cache.GetOrAdd(type, CreateExtractor);
            extractor(values, AddInner);

            return this;
        }

        /// <summary>
        /// Checks if the dictionary has the class element. If yes then adds it's value as classes from it.
        /// The dictionary can be null.
        /// </summary>
        /// <param name="attributes">The attributes dictionary.</param>
        /// <returns>Returns with this so the calls can be chained.</returns>
        public CssClassList Add(IReadOnlyDictionary<string, object> attributes)
        {
            if (attributes != null
                && attributes.TryGetValue("class", out var css)
                && css != null)
            {
                AddInner(css as string ?? css.ToString());
            }

            return this;
        }

        /// <summary>
        /// Indicates if the className is added to this.
        /// </summary>
        /// <param name="className">The class name to check.</param>
        /// <returns>True if already added; otherwise false.</returns>
        public bool HasClass(string className)
        {
            return _cssClasses.Contains(className);
        }

        /// <summary>
        /// Returns with the finished css class definition list.
        /// </summary>
        /// <returns>The class names separated by spaces.</returns>
        public override string ToString()
        {
            return string.Join(Separator, _cssClasses);
        }

        private ProcessCssDelegate CreateExtractor(Type type)
        {
            var lines = new List<Expression>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var valuesParam = Expression.Parameter(typeof(object));
            var addMethod = Expression.Parameter(typeof(AddCssDelegate));
            var valuesVar = Expression.Variable(type);
            var castedValuesParam = Expression.Convert(valuesParam, type);
            var valuesVarAssigment = Expression.Assign(valuesVar, castedValuesParam);
            lines.Add(valuesVarAssigment);
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(bool))
                {
                    throw new Exception($"Only boolean properties allowed for the css builder. Invalid poperty: {type.Name}.{property.Name} (Type: {property.PropertyType}");
                }

                var conditionGetter = Expression.Property(valuesVar, property);
                var className = _options.PropertyToClassNameConverter(property);
                var classNameConstant = Expression.Constant(className);
                var invokation = Expression.Invoke(addMethod, classNameConstant, conditionGetter);
                lines.Add(invokation);
            }

            var body = Expression.Block(new ParameterExpression[] { valuesVar }, lines);
            var method = Expression.Lambda<ProcessCssDelegate>(body, valuesParam, addMethod);
            return method.Compile();
        }

        private void AddInner(string value, bool condition = true)
        {
            if (string.IsNullOrEmpty(value) || !condition)
            {
                return;
            }

            foreach (var cssClass in value.Split(_separatorArray, StringSplitOptions.RemoveEmptyEntries))
            {
                if (_options.ExcludeDuplication && HasClass(cssClass))
                {
                    continue;
                }

                _cssClasses.Add(cssClass);
            }
        }
    }
}
