using JetBrains.Annotations;
using System.Reflection;
using System.Text.Json.Serialization.Converters;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Based on corefx/src/System.Text.Json/src/System/Text/Json/Serialization/JsonStringEnumConverter.cs
    /// </summary>
    public class JsonStringEnumConverterWithAttributeSupport : JsonConverterFactory
    {
        private readonly JsonNamingPolicy _namingPolicy;
        private readonly EnumConverterOptions _converterOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="namingPolicy">Naming policy for writing enum values [Optional.</param>
        /// <param name="allowIntegerValues">True to allow undefined enum values. When true, if an enum value isn't defined it will output as a number rather than a string.</param>
        /// <param name="parseEnumMemberAttribute">Parse using <see cref="Runtime.Serialization.EnumMemberAttribute"/>, default is <see langword="true"/>.</param>
        /// <param name="parseDisplayAttribute">Parse using <see cref="ComponentModel.DataAnnotations.DisplayAttribute"/>, default is <see langword="false"/>.</param>
        /// <param name="parseDescriptionAttribute">Parse using <see cref="ComponentModel.DescriptionAttribute"/>, default is <see langword="false"/>.</param>
        public JsonStringEnumConverterWithAttributeSupport([CanBeNull] JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true,
            bool parseEnumMemberAttribute = true, bool parseDisplayAttribute = false, bool parseDescriptionAttribute = false)
        {
            _namingPolicy = namingPolicy;
            _converterOptions = allowIntegerValues
                ? EnumConverterOptions.AllowNumbers | EnumConverterOptions.AllowStrings
                : EnumConverterOptions.AllowStrings;

            if (parseEnumMemberAttribute)
            {
                _converterOptions |= EnumConverterOptions.ParseEnumMemberAttribute;
            }

            if (parseDisplayAttribute)
            {
                _converterOptions |= EnumConverterOptions.ParseDisplayAttribute;
            }

            if (parseDescriptionAttribute)
            {
                _converterOptions |= EnumConverterOptions.ParseDescriptionAttribute;
            }
        }
        
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public JsonStringEnumConverterWithAttributeSupport() : this(null)
        {

        }

        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(JsonValueConverterEnumWithAttributeSupport<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                new object[] { _converterOptions, _namingPolicy },
                culture: null);

            return converter;
        }
    }
}
