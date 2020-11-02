using System.Collections.Generic;

namespace Blazorify.Utilities.Styling
{
    /// <summary>
    /// Represents a style property and value pair.
    /// </summary>
    public struct StyleDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleDeclaration"/> struct.
        /// </summary>
        /// <param name="property">The property component of the style element.</param>
        /// <param name="value">The value component of the style element.</param>
        public StyleDeclaration(string property, string value)
        {
            Property = property;
            Value = value;
        }

        /// <summary>
        /// Gets the property component of the style element.
        /// </summary>
        public string Property { get; }

        /// <summary>
        /// Gets the value component of the style element.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Checks if left equals to right by Property and Value.
        /// </summary>
        /// <param name="left">Left element.</param>
        /// <param name="right">Right element.</param>
        /// <returns>true if the Property and Value equals by case sensitive comparison.</returns>
        public static bool operator ==(StyleDeclaration left, StyleDeclaration right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if left is not equal to right by Property and Value.
        /// </summary>
        /// <param name="left">Left element.</param>
        /// <param name="right">Right element.</param>
        /// <returns>true if the Property and Value is not equal by case sensitive comparison.</returns>
        public static bool operator !=(StyleDeclaration left, StyleDeclaration right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is StyleDeclaration element &&
                   Property == element.Property &&
                   Value == element.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hashCode = -1027930222;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Property);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        /// <summary>
        /// Returns the composed "Property:Value" pair as string.
        /// </summary>
        /// <returns>The "Property:Value" string.</returns>
        public override string ToString()
        {
            return $"{Property}:{Value}";
        }
    }
}
