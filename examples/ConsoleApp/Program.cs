using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp
{
    enum WeatherType
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

    class WeatherForecast
    {
        public WeatherType WeatherTypeEnumMember { get; set; }

        public WeatherType WeatherTypeDisplay { get; set; }

        public WeatherType WeatherTypeDescription { get; set; }

        public WeatherType WeatherTypeInteger { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
    enum SpaceWeatherType
    {
        Unknown,

        [EnumMember(Value = "Solar Flare")]
        SolarFlare,

        [Display(Name = "CME")]
        CoronalMassEjection,

        [Description("Heavy Bombardment")]
        HeavyBombardment,

        [Description("Empty Void")]
        EmptyVoid
    }
    
    class SpaceWeatherForecast
    {
        public SpaceWeatherType WeatherTypeEnumMember { get; set; }

        public SpaceWeatherType WeatherTypeDisplay { get; set; }

        public SpaceWeatherType WeatherTypeDescription { get; set; }

        public SpaceWeatherType WeatherTypeInteger { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DemoWithJsonOptions();
            DemoWithConverterAttribute();
        }

        private static void DemoWithConverterAttribute()
        {
            var weatherForecast = new SpaceWeatherForecast()
                                  {
                                      WeatherTypeEnumMember = SpaceWeatherType.SolarFlare,
                                      WeatherTypeDisplay = SpaceWeatherType.CoronalMassEjection,
                                      WeatherTypeDescription = SpaceWeatherType.HeavyBombardment,
                                      WeatherTypeInteger = SpaceWeatherType.EmptyVoid
                                  };
            
            var weatherForecastSerialized = JsonSerializer.Serialize(weatherForecast);
            Console.WriteLine($"WeatherForecast:\r\n{weatherForecastSerialized}");
            Console.WriteLine();

            var json = "{\"WeatherTypeEnumMember\":\"Solar Flare\",\"WeatherTypeDisplay\":\"CME\",\"WeatherTypeDescription\":\"Heavy Bombardment\",\"WeatherTypeInteger\":4}";
            var weatherForecastDeserialized = JsonSerializer.Deserialize<WeatherForecast>(json);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeEnumMember);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeDisplay);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeDescription);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeInteger);
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

            var json = "{\"WeatherTypeEnumMember\":\"Zonnig\",\"WeatherTypeDisplay\":\"Helder\",\"WeatherTypeDescription\":\"Bewolkt\",\"WeatherTypeInteger\":4}";
            var weatherForecastDeserialized = JsonSerializer.Deserialize<WeatherForecast>(json, options);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeEnumMember);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeDisplay);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeDescription);
            Console.WriteLine(weatherForecastDeserialized.WeatherTypeInteger);
        }
    }
}