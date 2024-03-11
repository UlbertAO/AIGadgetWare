using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


// Parse JSON for any special character and skip any key if do not want to check for that

string json = @"
{
  ""name"": ""Ulbert Alain Odle"",
  ""guild_member"": true,
  ""supreme_being"": true,
  ""race"": ""Heteromorphic Demon"",
  ""epithet"": ""Demon-of-Great-Disaster"",
  ""created_npc"": {
    ""name"": ""Demiurge"",
    ""role"": ""Guardian of the 7th Floor"",
    ""abilities"": [""Strategic'o planning"", ""Manipulation"", ""Dark'o magic""]
  },
  ""personality"": {
    ""description"": ""Ulbert fixated on the concept of 'evil' due to his Chuunibyou tendencies."",
    ""traits"": [""Villain-like behavior"", ""Disillusioned with the real world""]
  },
  ""relationships"": {
    ""guildmates"": [""Demiurge"", ""Momonga"", ""Touch Me"", ""Peroroncino""],
    ""other_characters"": [""Bukubukuchagama"", ""Yamaiko"", ""Warrior Takemikazuchi"", ""Nishikienrai""]
  }
}
";

JObject obj = JObject.Parse(json);

string keyWithSpecialChars = "";

//mention list of keys not to check for spcl characters
List<string> keysNotToCheck = new List<string>();
keysNotToCheck.Add("epithet");

CheckForSpecialChars(obj);

Console.WriteLine(String.IsNullOrEmpty(keyWithSpecialChars) ? "JSON is free of special characters" : keyWithSpecialChars + " have special characters");



void CheckForSpecialChars(JToken token)
{

    
    if(token==null || keysNotToCheck.Contains(token.Path.ToString()))
    {
        return;
    }
    if (token.Type == JTokenType.Object)
    {
        foreach (JProperty child in token.Children<JProperty>())
        {
            CheckForSpecialChars(child.Value);
        }
    }
    else if (token.Type == JTokenType.Array)
    {
        foreach (JToken child in token.Children())
        {
            CheckForSpecialChars(child);
        }
    }
    else
    {
        if (Regex.IsMatch(token.ToString(), @"[^\w\s._\-\/]"))
        {
            keyWithSpecialChars +=  String.IsNullOrEmpty(keyWithSpecialChars) ? token.Path.ToString() : ", " + token.Path.ToString();
        }
    }
}