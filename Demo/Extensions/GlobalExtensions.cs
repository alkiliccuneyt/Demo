using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
// ReSharper disable All
#pragma warning disable CS0618
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8601
#pragma warning disable CS8603
namespace Demo.Extensions;

public static class GlobalExtensions
{
    public static string SerializeToXml<T>(this T value)
    {
        if ((object) value == null)
            return string.Empty;
        try
        {
            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };
            var xmlSerializer = new XmlSerializer(typeof (T));
            var output = new StringWriter();
            using var xmlWriter = XmlWriter.Create((TextWriter) output, settings);
            xmlSerializer.Serialize(xmlWriter, (object) value);
            return output.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred", ex);
        }
    }

    public static T Deserialize<T>(string msg)
    {
        using var input = new StringReader(msg);
        var settings = new XmlReaderSettings()
        {
            DtdProcessing = DtdProcessing.Parse,
            XmlResolver = (XmlResolver) null
        };
        using var xmlReader = XmlReader.Create((TextReader) input, settings);
        return (T) new XmlSerializer(typeof (T)).Deserialize(xmlReader);
    }
    public static T JDeserialize<T>(string msg)
    {
        var model = JsonConvert.DeserializeObject<T>(msg);
        return model;
    }
    
    public static string TurkishCharacterToEnglish(string text) {
        char[] turkishChars = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö', 'ü', 'Ü' };
        char[] englishChars = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U' };

        // Match chars
        for (var i = 0; i < turkishChars.Length; i++)
            text = text.Replace(turkishChars[i], englishChars[i]);

        return text;
    }
    
    public static string StringCapitalize(string str) {
        str = Regex.Replace(str ?? string.Empty, @"(?<=\b(?:mc|mac)?)[a-zA-Z](?<!'s\b)", m => m.Value.ToUpper());
        return str;
    }
}