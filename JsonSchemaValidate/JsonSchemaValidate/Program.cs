using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class Program
{
    public static void Main()
    {
        //var json = @"{""name"":""ulbert"",""departmentList"":[{""dname"":""IT"",""dname"":""non IT""},{""dname"":""non IT""}]}";
        //var json = @"{""name"":""ulbert"",""departmentList"":[{""dname"":""IT""},{""dname"":""non IT""}]}";
        var json = @"{""name"":""ulbert"",""name"":""asd"",""departmentList"":[{""dname"":""IT""},{""dname"":""non IT""}]}";

        var objA = DeserializeObject<MyRequest>(json, new JsonSerializerSettings(), DuplicatePropertyNameHandling.Ignore);
        Console.WriteLine(".Ignore: " + objA.Name);

        var objB = DeserializeObject<MyRequest>(json, new JsonSerializerSettings(), DuplicatePropertyNameHandling.Replace);
        Console.WriteLine(".Replace: " + objB.Name);

        var objC = DeserializeObject<MyRequest>(json, new JsonSerializerSettings(), DuplicatePropertyNameHandling.Error);
        Console.WriteLine(".Error: " + objC.Name); // throws error before getting here
    }
    public static T DeserializeObject<T>(string json, JsonSerializerSettings settings, DuplicatePropertyNameHandling duplicateHandling)
    {
        JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
        Console.WriteLine("lg1 : " + json);
        using (var stringReader = new StringReader(json))
        { 
            Console.WriteLine("lg2 : " + stringReader);

        using (var jsonTextReader = new JsonTextReader(stringReader))
        {
            jsonTextReader.DateParseHandling = DateParseHandling.None;
            JsonLoadSettings loadSettings = new JsonLoadSettings
            {
                DuplicatePropertyNameHandling = duplicateHandling
            };
            var jtoken = JToken.ReadFrom(jsonTextReader, loadSettings);
            return jtoken.ToObject<T>(jsonSerializer);
        }
        }
    }
}
public class MyRequest
{
    public string Name { get; set; }
    public List<Department> departmentList { get; set; }
}

public class Department
{
    public string DName { get; set; }
}
