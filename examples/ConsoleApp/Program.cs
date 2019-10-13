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

    class Program
    {
        static void Main(string[] args)
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