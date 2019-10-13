# JsonStringEnumConverterWithAttributeSupport
An extended version from the JsonStringEnumConverter which supports attributes like EnumMember, Display and Description


## Info
| | |
|-|-|
| &nbsp;&nbsp;**Build Azure** | [![Build Status](https://dev.azure.com/stef/GraphQL.EntityFrameworkCore.DynamicLinq/_apis/build/status/StefH.GraphQL.EntityFrameworkCore.DynamicLinq)](https://dev.azure.com/stef/GraphQL.EntityFrameworkCore.DynamicLinq/_build/latest?definitionId=26) |
| &nbsp;&nbsp;**Codecov** | [![codecov](https://codecov.io/gh/StefH/GraphQL.EntityFrameworkCore.DynamicLinq/branch/master/graph/badge.svg)](https://codecov.io/gh/StefH/GraphQL.EntityFrameworkCore.DynamicLinq) |
| &nbsp;&nbsp;**NuGet** | [![NuGet: GraphQL.EntityFrameworkCore.DynamicLinq](https://buildstats.info/nuget/GraphQL.EntityFrameworkCore.DynamicLinq)](https://www.nuget.org/packages/GraphQL.EntityFrameworkCore.DynamicLinq)
| &nbsp;&nbsp;**MyGet (preview)** | [![MyGet: GraphQL.EntityFrameworkCore.DynamicLinq](https://buildstats.info/myget/graphql_entityframeworkcore_dynamiclinq/GraphQL.EntityFrameworkCore.DynamicLinq)](https://www.myget.org/feed/graphql_entityframeworkcore_dynamiclinq/package/nuget/GraphQL.EntityFrameworkCore.DynamicLinq) |


## Installing
You can install from NuGet using the following command in the package manager window:
`Install-Package Blazored.FormExtensions`

Or via the Visual Studio NuGet package manager.


## Usage Example - EnumMember

### Define Enum and add attributes
Define an Enum and annotate the Enum fields with the [EnumMemberAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.enummemberattribute?view=netstandard-2.0):
``` c#
public enum WeatherType
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
public enum WeatherType
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