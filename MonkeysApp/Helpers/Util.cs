using System.Collections.Generic;

using MonkeysApp.Models;
using System.Threading.Tasks;
using PCLStorage;
using System.Reflection;
using Plugin.EmbeddedResource;
using Newtonsoft.Json;

namespace MonkeysApp.Helpers
{
    public static class Util
    {
        public static List<Monkey> GenerateFriends()
        {
            var json = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("MonkeysApp")), "monkeydata.json");
            return JsonConvert.DeserializeObject<List<Monkey>>(json);
        }
    }
}