using JetBrains.Annotations;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace SystemTextJsonExample
{
    /// <summary>
    /// Based on corefx/src/System.Text.Json/src/System/Text/Json/Serialization/JsonStringEnumConverter.cs
    /// </summary>
    public class JsonStringEnumConverterWithAttributeSupport : JsonConverterFactory
    {
        private readonly JsonNamingPolicy _namingPolicy;
        private readonly EnumConverterOptions _converterOptions;

        /// <summary>
        /// Constructor. Creates the <see cref="JsonStringEnumConverter"/> with these default options:
        /// - default naming policy
        /// - allowing integer values
        /// - allowing parsing from <see cref="System.Runtime.Serialization.EnumMemberAttribute"/>
        /// </summary>
        public JsonStringEnumConverterWithAttributeSupport()
            : this(namingPolicy: null, allowIntegerValues: true)
        {
            // An empty constructor is needed for construction via attributes
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="namingPolicy">Optional naming policy for writing enum values.</param>
        /// <param name="allowIntegerValues">True to allow undefined enum values. When true, if an enum value isn't defined it will output as a number rather than a string.</param>
        /// <param name="parseEnumMemberAttribute">Parse using <see cref="System.Runtime.Serialization.EnumMemberAttribute"/>, default is <see langword="true"/>.</param>
        /// <param name="parseDisplayAttribute">Parse using <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/>, default is <see langword="false"/>.</param>
        /// <param name="parseDescriptionAttribute">Parse using <see cref="System.ComponentModel.DescriptionAttribute"/>, default is <see langword="false"/>.</param>
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
