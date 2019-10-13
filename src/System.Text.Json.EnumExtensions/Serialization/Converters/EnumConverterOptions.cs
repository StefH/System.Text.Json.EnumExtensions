// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization.Converters
{
    /// <summary>
    /// Based on corefx/src/System.Text.Json/src/System/Text/Json/Serialization/Converters/EnumConverterOptions.cs
    /// </summary>
    [Flags]
    internal enum EnumConverterOptions
    {
        /// <summary>
        /// Allow string values.
        /// </summary>
        AllowStrings = 0b00000001,

        /// <summary>
        /// Allow number values.
        /// </summary>
        AllowNumbers = 0b00000010,

        /// <summary>
        /// Try parsing the value using <see cref="System.Runtime.Serialization.EnumMemberAttribute"/>.
        /// </summary>
        ParseEnumMemberAttribute = 0b00000100,

        /// <summary>
        /// Try parsing the value using <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/>.
        /// </summary>
        ParseDisplayAttribute = 0b00001000,

        /// <summary>
        /// Try parsing the value using <see cref="System.ComponentModel.DescriptionAttribute"/>.
        /// </summary>
        ParseDescriptionAttribute = 0b00010000
    }
}