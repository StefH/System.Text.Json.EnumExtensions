namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Copied from corefx/src/System.Text.Json/src/System/Text/Json/Serialization/JsonDefaultNamingPolicy.cs 
    /// </summary>
    internal class JsonDefaultNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name;
    }
}