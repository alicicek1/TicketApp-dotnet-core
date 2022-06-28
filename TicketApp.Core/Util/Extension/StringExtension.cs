using System;
using System.Text;
using Newtonsoft.Json;

namespace TicketApp.Infrastructure.Extension
{
    public static class StringExtension
    {
        public static string ToJSON<T>(this T value, bool apostropheCheck = false)
        {
            if (value == null) return string.Empty;

            string json = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                MaxDepth = Int32.MaxValue,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            json = apostropheCheck ? json.Replace("'", "\\'").Replace("\"", "\\\"") : json;
            json = json.RemoveRepeatedWhiteSpace();
            json = json.Replace(Environment.NewLine, string.Empty);
            return json;
        }

        public static string RemoveRepeatedWhiteSpace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            string[] tmp = value.ToString().Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string word in tmp)
            {
                sb.Append(word.Replace("\r", "").Replace("\n", "").Replace("\t", "") /*.Replace("\\" , "")*/ + " ");
            }

            return sb.ToString().TrimEnd();
        }
    }
}