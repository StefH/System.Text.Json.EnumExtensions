# System.Text.Json.Extensions
Some extensions to the JsonStringEnumConverter which supports attributes like EnumMember, Display and Description


## Info
| | |
|-|-|
| &nbsp;&nbsp;**Build Azure** | [![Build Status](https://dev.azure.com/stef/System.Text.Json.EnumExtensions/_apis/build/status%2FStefH.System.Text.Json.EnumExtensions?branchName=TargetFrameworks)](https://dev.azure.com/stef/System.Text.Json.EnumExtensions/_build/latest?definitionId=28&branchName=TargetFrameworks) |
| &nbsp;&nbsp;**NuGet** | [![NuGet: EnumExtensions.System.Text.Json](https://img.shields.io/nuget/v/EnumExtensions.System.Text.Json)](https://www.nuget.org/packages/EnumExtensions.System.Text.Json)
| &nbsp;&nbsp;**MyGet (preview)** | [![MyGet: EnumExtensions.System.Text.Json](https://img.shields.io/myget/system_text_json_enumextensions/vpre/EnumExtensions.System.Text.Json?includePreReleases=true)](https://www.myget.org/feed/system_text_json_enumextensions/package/nuget/EnumExtensions.System.Text.Json) |


## Installing
You can install from NuGet using the following command in the package manager window:

`Install-Package EnumExtensions.System.Text.Json`

Or via the Visual Studio NuGet package manager.

If you use the `dotnet` command:

`dotnet add package EnumExtensions.System.Text.Json`


## Option 1: Usage Example - EnumMember

### Define Enum and add attributes
Define an Enum and annotate the Enum fields with the [EnumMemberAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.enummemberattribute?view=netstandard-2.0):
``` c#
enum WeatherType
{
    [EnumMember(Value = "Zonnig")]
    Sunny,

    [EnumMember(Value = "Helder")]
    Clear
}
```

### Add Converter
Add the new `JsonStringEnumConverterWithAttributeSupport` to the Converters via the JsonSerializerOptions:
``` c#
var options = new JsonSerializerOptions();
options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport());
```

### Serialize an object
``` c#
var weatherForecast = new WeatherForecast
{
    WeatherType = WeatherType.Sunny
};

var weatherForecastSerialized = JsonSerializer.Serialize(weatherForecast, options);
Console.WriteLine(weatherForecastSerialized); // {"WeatherType":"Zonnig"}
```

### Deserialize an object
Deserialize works by using the same **options**:
``` c#
var json = "{\"WeatherType\":\"Zonnig\"}";
var weatherForecastDeserialized = JsonSerializer.Deserialize<WeatherForecast>(json, options);
```


## Option 2: Usage Example - EnumMember
It's also possible to annotate the Enum with a `[JsonConverter]` so that you don't need to manually registerd the `JsonStringEnumConverterWithAttributeSupport` to the Converters via the JsonSerializerOptions.

### Define Enum and add attributes
Define an Enum
- add the `[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]` to the Enum
- annotate the Enum fields with the [EnumMemberAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.enummemberattribute?view=netstandard-2.0):
``` c#
[JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
enum WeatherType
{
    [EnumMember(Value = "Zonnig")]
    Sunny,

    [EnumMember(Value = "Helder")]
    Clear
}
```

### Serializing and Deserialize an object
This works the same as using Option 1.

Note that only Enum values which are annotated with `EnumMember` are supported.


## Usage Example - Display and Description
It's also possible to annotate Enum fields with these attributes:
- [DisplayAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute?view=netframework-4.8)
- [DescriptionAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.descriptionattribute?view=netframework-4.8)


### Define Enum and add attributes
``` c#
enum WeatherType
{
    [EnumMember(Value = "Zonnig")]
    Sunny,

    [Display(Name = "Helder")]
    Clear,

    [Description("Bewolkt")]
    Cloudy
}
```

### Add Converter
**!** By default, the Display and Description are disabled, use the following line to enable these.
``` c#
var options = new JsonSerializerOptions();
options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport(null, true, true, true, true));
```

Serializing and Deserializing works the same.


## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=StefH) and [Dapper Plus](https://dapper-plus.net/?utm_source=StefH) are major sponsors and proud to contribute to the development of **System.Text.Json.Extensions**.

[![Entity Framework Extensions](https://raw.githubusercontent.com/StefH/resources/main/sponsor/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=StefH)

[![Dapper Plus](https://raw.githubusercontent.com/StefH/resources/main/sponsor/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=StefH)