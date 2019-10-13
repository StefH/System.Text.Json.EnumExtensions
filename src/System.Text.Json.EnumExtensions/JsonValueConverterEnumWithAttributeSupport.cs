using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SystemTextJsonExample;

namespace System.Text.Json.Serialization.Converters
{
    /// <summary>
    /// Based on corefx/src/System.Text.Json/src/System/Text/Json/Serialization/Converters/JsonValueConverterEnum.cs
    /// </summary>
    /// <typeparam name="T">Generic Enum type.</typeparam>
    internal class JsonValueConverterEnumWithAttributeSupport<T> : JsonConverter<T>
        where T : struct, Enum
    {
        private static readonly TypeCode s_enumTypeCode = Type.GetTypeCode(typeof(T));

        // Odd type codes are conveniently signed types (for enum backing types).
        private static readonly string s_negativeSign = ((int)s_enumTypeCode % 2) == 0 ? null : NumberFormatInfo.CurrentInfo.NegativeSign;

        private readonly EnumConverterOptions _converterOptions;
        private readonly JsonNamingPolicy _namingPolicy;
        private readonly ConcurrentDictionary<string, string> _nameCache;

        private bool ParseFromAttribute =>
            _converterOptions.HasFlag(EnumConverterOptions.ParseEnumMemberAttribute) ||
            _converterOptions.HasFlag(EnumConverterOptions.ParseDisplayAttribute) ||
            _converterOptions.HasFlag(EnumConverterOptions.ParseDescriptionAttribute);

        public override bool CanConvert(Type type)
        {
            return type.IsEnum;
        }

        public JsonValueConverterEnumWithAttributeSupport(EnumConverterOptions options)
            : this(options, namingPolicy: null)
        {
        }

        public JsonValueConverterEnumWithAttributeSupport(EnumConverterOptions options, JsonNamingPolicy namingPolicy)
        {
            _converterOptions = options;
            if (namingPolicy != null)
            {
                _nameCache = new ConcurrentDictionary<string, string>();
            }
            else
            {
                namingPolicy = new JsonDefaultNamingPolicy();
            }
            _namingPolicy = namingPolicy;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType token = reader.TokenType;

            if (token == JsonTokenType.String)
            {
                if (!_converterOptions.HasFlag(EnumConverterOptions.AllowStrings))
                {
                    throw new JsonException();
                }

                string enumString = reader.GetString();
                if (!Enum.TryParse(enumString, out T value) &&
                    !Enum.TryParse(enumString, ignoreCase: true, out value) &&
                    !(ParseFromAttribute && TryParseFromAttribute(enumString, out value)))
                {
                    throw new JsonException();
                }

                return value;
            }

            if (token != JsonTokenType.Number || !_converterOptions.HasFlag(EnumConverterOptions.AllowNumbers))
            {
                throw new JsonException();
            }

            switch (s_enumTypeCode)
            {
                // Switch cases ordered by expected frequency
                case TypeCode.Int32:
                    if (reader.TryGetInt32(out int int32))
                    {
                        return Unsafe.As<int, T>(ref int32);
                    }
                    break;
                case TypeCode.UInt32:
                    if (reader.TryGetUInt32(out uint uint32))
                    {
                        return Unsafe.As<uint, T>(ref uint32);
                    }
                    break;
                case TypeCode.UInt64:
                    if (reader.TryGetUInt64(out ulong uint64))
                    {
                        return Unsafe.As<ulong, T>(ref uint64);
                    }
                    break;
                case TypeCode.Int64:
                    if (reader.TryGetInt64(out long int64))
                    {
                        return Unsafe.As<long, T>(ref int64);
                    }
                    break;

                // When utf8reader/writer will support all primitive types we should remove custom bound checks
                // https://github.com/dotnet/corefx/issues/36125
                case TypeCode.SByte:
                    if (reader.TryGetInt32(out int byte8) && JsonHelpers.IsInRangeInclusive(byte8, sbyte.MinValue, sbyte.MaxValue))
                    {
                        sbyte byte8Value = (sbyte)byte8;
                        return Unsafe.As<sbyte, T>(ref byte8Value);
                    }
                    break;
                case TypeCode.Byte:
                    if (reader.TryGetUInt32(out uint ubyte8) && JsonHelpers.IsInRangeInclusive(ubyte8, byte.MinValue, byte.MaxValue))
                    {
                        byte ubyte8Value = (byte)ubyte8;
                        return Unsafe.As<byte, T>(ref ubyte8Value);
                    }
                    break;
                case TypeCode.Int16:
                    if (reader.TryGetInt32(out int int16) && JsonHelpers.IsInRangeInclusive(int16, short.MinValue, short.MaxValue))
                    {
                        short shortValue = (short)int16;
                        return Unsafe.As<short, T>(ref shortValue);
                    }
                    break;
                case TypeCode.UInt16:
                    if (reader.TryGetUInt32(out uint uint16) && JsonHelpers.IsInRangeInclusive(uint16, ushort.MinValue, ushort.MaxValue))
                    {
                        ushort ushortValue = (ushort)uint16;
                        return Unsafe.As<ushort, T>(ref ushortValue);
                    }
                    break;
            }

            throw new JsonException();
        }

        private static bool IsValidIdentifier(string value)
        {
            // Trying to do this check efficiently. When an enum is converted to
            // string the underlying value is given if it can't find a matching
            // identifier (or identifiers in the case of flags).
            //
            // The underlying value will be given back with a digit (e.g. 0-9) possibly
            // preceded by a negative sign. Identifiers have to start with a letter
            // so we'll just pick the first valid one and check for a negative sign
            // if needed.
            return value[0] >= 'A' && (s_negativeSign == null || !value.StartsWith(s_negativeSign));
        }

        private bool TryParseFromAttribute(string value, out T resolvedEnumValue)
        {
            resolvedEnumValue = default(T);

            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (TryGetCustomAttributeValue(enumValue.ToString(), out string attributeValue) && string.Equals(value, attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    resolvedEnumValue = enumValue;
                    return true;
                }
            }

            return false;
        }

        private bool TryGetCustomAttributeValue(string enumValueAsString, out string attributeValue)
        {
            attributeValue = null;

            MemberInfo[] memberInfo = typeof(T).GetMember(enumValueAsString);
            if (memberInfo != null && memberInfo.Length > 0)
            {
                if (_converterOptions.HasFlag(EnumConverterOptions.ParseEnumMemberAttribute))
                {
                    var enumMemberAttribute = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttribute != null)
                    {
                        attributeValue = enumMemberAttribute.Value;
                        return attributeValue != null;
                    }
                }

                if (_converterOptions.HasFlag(EnumConverterOptions.ParseDisplayAttribute))
                {
                    var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null)
                    {
                        attributeValue = displayAttribute.Name;
                        return attributeValue != null;
                    }
                }

                if (_converterOptions.HasFlag(EnumConverterOptions.ParseDisplayAttribute))
                {
                    var descriptionAttribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttribute != null)
                    {
                        attributeValue = descriptionAttribute.Description;
                        return attributeValue != null;
                    }
                }

                return false;
            }

            return false;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // If strings are allowed, attempt to write it out as a string value
            if (_converterOptions.HasFlag(EnumConverterOptions.AllowStrings))
            {
                string original = value.ToString();
                if (_nameCache != null && _nameCache.TryGetValue(original, out string transformed))
                {
                    writer.WriteStringValue(transformed);
                    return;
                }

                if (IsValidIdentifier(original))
                {
                    if (!(ParseFromAttribute && TryGetCustomAttributeValue(original, out transformed)))
                    {
                        transformed = _namingPolicy.ConvertName(original);
                    }

                    writer.WriteStringValue(transformed);

                    if (_nameCache != null)
                    {
                        _nameCache.TryAdd(original, transformed);
                    }
                    return;
                }
            }

            if (!_converterOptions.HasFlag(EnumConverterOptions.AllowNumbers))
            {
                throw new JsonException();
            }

            switch (s_enumTypeCode)
            {
                case TypeCode.Int32:
                    writer.WriteNumberValue(Unsafe.As<T, int>(ref value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(Unsafe.As<T, uint>(ref value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(Unsafe.As<T, ulong>(ref value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(Unsafe.As<T, long>(ref value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(Unsafe.As<T, short>(ref value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(Unsafe.As<T, ushort>(ref value));
                    break;
                case TypeCode.Byte:
                    writer.WriteNumberValue(Unsafe.As<T, byte>(ref value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(Unsafe.As<T, sbyte>(ref value));
                    break;
                default:
                    throw new JsonException();
            }
        }
    }
}