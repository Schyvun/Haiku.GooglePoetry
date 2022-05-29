using BepInEx;
using BepInEx.Configuration;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Haiku.GooglePoetry
{
    [BepInPlugin("haiku.googlePoetry", "Google Poetry", "1.0.0.1")]
    public sealed class GooglePoetryClass : BaseUnityPlugin
    {
        Dictionary<string, string> googleTranslatedLanguage = new();
        void Awake()
        {
            On.LocalizationSystem.GetLocalizedValue += TryFindValue;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("Haiku.GooglePoetry.translated_en_Input.txt");
            if (stream == null)
            {
                Debug.LogError("Couldn't find translated Text");
                return;
            }
            StreamReader strReader = new(stream);
            string line;
            while ((line = strReader.ReadLine()) != null)
            {
                string[] vk = line.Split(';', ';');
                if (vk.Length != 3) continue;
                vk = new[] { vk[0], vk[2] };
                googleTranslatedLanguage[vk[0]] = vk[1];
            }
        }

        private string TryFindValue(On.LocalizationSystem.orig_GetLocalizedValue orig, string key)
        {
            if (googleTranslatedLanguage.TryGetValue(key, out string result))
            {
                return result;
            }
            Debug.LogError("Couldn't find key: " + key + " in GoogleTranslatedDict. Use normal instead");
            return orig(key);
        }
    }
}