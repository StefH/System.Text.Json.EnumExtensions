# System.Text.Json.Extensions
Some extensions to the JsonStringEnumConverter which supports attributes like EnumMember, Display and Description


## Info
| | |
|-|-|
| &nbsp;&nbsp;**Build Azure** | [![Build Status](https://dev.azure.com/stef/System.Text.Json.EnumExtensions/_apis/build/status/StefH.System.Text.Json.EnumExtensions?branchName=refs%2Fpull%2F7%2Fmerge)](https://dev.azure.com/stef/System.Text.Json.EnumExtensions/_build/latest?definitionId=28&branchName=refs%2Fpull%2F7%2Fmerge) |
| &nbsp;&nbsp;**NuGet** | [![NuGet: EnumExtensions.System.Text.Json](https://buildstats.info/nuget/EnumExtensions.System.Text.Json)](https://www.nuget.org/packages/EnumExtensions.System.Text.Json)
| &nbsp;&nbsp;**MyGet (preview)** | [![MyGet: EnumExtensions.System.Text.Json](https://buildstats.info/myget/system_text_json_enumextensions/EnumExtensions.System.Text.Json?includePreReleases=true)](https://www.myget.org/feed/system_text_json_enumextensions/package/nuget/EnumExtensions.System.Text.Json) |


## Installing
You can install from NuGet using the following command in the package manager window:

`Install-Package EnumExtensions.System.Text.Json`

Or via the Visual Studio NuGet package manager.

If you use the `dotnet` command:

`dotnet add package EnumExtensions.System.Text.Json`


## Usage Example - EnumMember

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
