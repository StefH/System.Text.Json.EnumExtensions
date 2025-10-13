using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp;

internal enum WeatherType
{
    Unknown,

    [EnumMember(Value = "Zonnig")]
    Sunny,

    [Display(Name = "Helder")]
    Clear,

    [Description("Bewolkt")]
    Cloudy,

    [Description("Sneeuw")]
    Snow
}

internal class WeatherForecast
{
    public WeatherType WeatherTypeEnumMember { get; set; }

    public WeatherType WeatherTypeDisplay { get; set; }

    public WeatherType WeatherTypeDescription { get; set; }

    public WeatherType WeatherTypeInteger { get; set; }
}

/// <summary>
/// Only `EnumMember` is supported when using Parameterless constructor
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
internal enum SpaceWeatherType
{
    Unknown = 0,

    [EnumMember(Value = "Solar_Flare")]
    SolarFlare,

    [EnumMember(Value = "CME")]
    CoronalMassEjection,

    HeavyBombardment,

    [EnumMember(Value = "Empty_Void")]
    EmptyVoid
}

internal class SpaceWeatherForecast
{
    public SpaceWeatherType WeatherTypeEnumMember1 { get; set; }

    public SpaceWeatherType WeatherTypeEnumMember2 { get; set; }

    public SpaceWeatherType WeatherTypeEnumMember3 { get; set; }

    public SpaceWeatherType WeatherTypeInteger { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
internal enum EnumWithStringValue
{
    [EnumMember(Value = "21")]
    Email,

    [EnumMember(Value = "22")]
    Print,

    [EnumMember(Value = "23")]
    Fax
}

internal class EnumWithStringValues
{
    public EnumWithStringValue Email { get; set; }

    public EnumWithStringValue Fax { get; set; }

    public EnumWithStringValue Print { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<GenericAttribute1>))]
internal enum GenericAttribute1
{
    [EnumMember(Value = "E")]
    Email,

    [Display(Name = "P")]
    Print,

    [Description("F")]
    Fax
}

[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport<GenericAttribute2>))]
internal enum GenericAttribute2
{
    [EnumMember(Value = "E")]
    Email,

    [Display(Name = "P")]
    Print,

    [Description("F")]
    Fax
}

internal class Program
{
    private static void Main(string[] args)
    {
        DemoWithGenericConverterAttribute();
        Console.WriteLine(new string('-', 120));
        DemoWithJsonOptions();
        Console.WriteLine(new string('-', 120));
        DemoWithConverterAttribute();
        Console.WriteLine(new string('-', 120));
        DemoWithConverterAttributeIssue9();
    }

    private static void DemoWithGenericConverterAttribute()
    {
        var x1 = new
        {
            GenericAttribute1.Email,
            GenericAttribute1.Fax,
            GenericAttribute1.Print
        };
        var serialized1 = JsonSerializer.Serialize(x1);
        Console.WriteLine($"x1:\r\n{serialized1}");
        Console.WriteLine();

        var x2 = new
        {
            GenericAttribute2.Email,
            GenericAttribute2.Fax,
            GenericAttribute2.Print
        };
        var serialized2 = JsonSerializer.Serialize(x2);
        Console.WriteLine($"x2:\r\n{serialized2}");
        Console.WriteLine();
    }

    private static void DemoWithJsonOptions()
    {
        var weatherForecast = new WeatherForecast
        {
            WeatherTypeEnumMember = WeatherType.Sunny,
            WeatherTypeDisplay = WeatherType.Clear,
            WeatherTypeDescription = WeatherType.Cloudy,
            WeatherTypeInteger = WeatherType.Snow
        };

        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport(null, true, true, true, true));

        var weatherForecastSerialized = JsonSerializer.Serialize(weatherForecast, options);
        Console.WriteLine($"WeatherForecast:\r\n{weatherForecastSerialized}");
        Console.WriteLine();

        var json = """{"WeatherTypeEnumMember":"Zonnig","WeatherTypeDisplay":"Helder","WeatherTypeDescription":"Bewolkt","WeatherTypeInteger":4}""";
        var weatherForecastDeserialized = JsonSerializer.Deserialize<WeatherForecast>(json, options)!;
        Console.WriteLine(weatherForecastDeserialized.WeatherTypeEnumMember);
        Console.WriteLine(weatherForecastDeserialized.WeatherTypeDisplay);
        Console.WriteLine(weatherForecastDeserialized.WeatherTypeDescription);
        Console.WriteLine(weatherForecastDeserialized.WeatherTypeInteger);
    }

    private static void DemoWithConverterAttribute()
    {
        var spaceWeatherForecast = new SpaceWeatherForecast
        {
            WeatherTypeEnumMember1 = SpaceWeatherType.SolarFlare,
            WeatherTypeEnumMember2 = SpaceWeatherType.CoronalMassEjection,
            WeatherTypeEnumMember3 = SpaceWeatherType.HeavyBombardment,
            WeatherTypeInteger = SpaceWeatherType.EmptyVoid
        };

        var spaceWeatherForecastSerialized = JsonSerializer.Serialize(spaceWeatherForecast);
        Console.WriteLine($"SpaceWeatherForecast:\r\n{spaceWeatherForecastSerialized}");
        Console.WriteLine();

        var json = """{"WeatherTypeEnumMember1":"Solar_Flare","WeatherTypeEnumMember2":"CME","WeatherTypeEnumMember3":"HeavyBombardment","WeatherTypeInteger":4}""";
        var spaceWeatherForecastDeserialized = JsonSerializer.Deserialize<SpaceWeatherForecast>(json)!;
        Console.WriteLine(spaceWeatherForecastDeserialized.WeatherTypeEnumMember1);
        Console.WriteLine(spaceWeatherForecastDeserialized.WeatherTypeEnumMember2);
        Console.WriteLine(spaceWeatherForecastDeserialized.WeatherTypeEnumMember3);
        Console.WriteLine(spaceWeatherForecastDeserialized.WeatherTypeInteger);
    }

    private static void DemoWithConverterAttributeIssue9()
    {
        var enumWithStringValues = new EnumWithStringValues
        {
            Email = EnumWithStringValue.Email,
            Fax = EnumWithStringValue.Fax,
            Print = EnumWithStringValue.Print
        };

        var serialized = JsonSerializer.Serialize(enumWithStringValues);
        Console.WriteLine($"EnumWithStringValues:\r\n{serialized}");
        Console.WriteLine();

        var json = """{"Email":"21","Fax":"23","Print":"22"}""";
        var deserialized = JsonSerializer.Deserialize<EnumWithStringValues>(json)!;
        Console.WriteLine(deserialized.Email);
        Console.WriteLine(deserialized.Fax);
        Console.WriteLine(deserialized.Print);
    }
}