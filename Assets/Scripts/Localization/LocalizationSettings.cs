using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Cars
{
    [CreateAssetMenu(fileName = "localization_settings.asset", menuName = "Localization/Create Settings")]
    public class LocalizationSettings : ScriptableObject
    {
        private static LocalizationSettings _instance;

        public static LocalizationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<LocalizationSettings>("localization_settings");
                }
                return _instance;
            }
        }

        public string PrefKey = "lang";
        public SystemLanguage DefaultLanguage = SystemLanguage.English;
        public SupportedLanguage[] SupportedLanguages;

#if UNITY_EDITOR
        [ContextMenu("CSV/Export")]
        public void ExportCSV()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanel("Save localization as CSV", "", "loc.csv", "csv");
            var dict = new Dictionary<string, Dictionary<string, string>>();
            var langFiles = SupportedLanguages.Select(z => z.ResourceFile).Distinct().ToArray();

            foreach (var langFile in langFiles)
            {
                var resource = Resources.Load<LocalizationResource>(langFile);
                if (resource.Terms == null)
                {
                    continue;
                }

                foreach (LocalizationTerm term in resource.Terms)
                {
                    if (!dict.TryGetValue(term.Key, out var subDict))
                    {
                        dict[term.Key] = subDict = new Dictionary<string, string>();
                    }
                    subDict[langFile] = term.Value;
                }
            }

            StringBuilder csv = new StringBuilder();
            csv.Append("key;").AppendLine(string.Join(";", langFiles));

            foreach (var kvp in dict)
            {
                var line = new string[langFiles.Length + 1];
                line[0] = kvp.Key;

                for (int i = 0; i < langFiles.Length; i++)
                {
                    var langFile = langFiles[i];
                    if (kvp.Value.ContainsKey(langFile))
                    {
                        line[i + 1] = kvp.Value[langFile];
                    }
                    else
                    {
                        line[i + 1] = string.Empty;
                    }
                }
                csv.AppendLine(string.Join(";", line));
            }
            File.WriteAllText(path, csv.ToString(), Encoding.Unicode);
        }

        [ContextMenu("CSV/Import")]
        public void ImportCVS()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Save localization as CSV", "", "csv");

            var fileContent = File.ReadAllLines(path);
            var headers = fileContent[0].Split(';');
            var mapping = new Dictionary<string, int>(headers.Length);

            for (int i = 1; i < headers.Length; i++)
            {
                mapping[headers[i]] = i;
            }

            var dict = new Dictionary<string, Dictionary<string, string>>();

            for (int i = 1; i < fileContent.Length; i++)
            {
                var line = fileContent[i].Split(';');
                var key = line[0];
                var value = new Dictionary<string, string>();

                foreach (var map in mapping)
                {
                    value[map.Key] = line[map.Value];
                }
                dict[key] = value;
            }

            for (int i = 1; i < headers.Length; i++)
            {
                var lang = headers[i];
                var resource = Resources.Load<LocalizationResource>(lang);
                resource.Terms = new List<LocalizationTerm>(dict.Count);

                foreach (var kvp in dict)
                {
                    resource.Terms.Add(
                        new LocalizationTerm()
                        {
                            Key = kvp.Key,
                            Value = kvp.Value[lang]
                        });
                }
            }
        }

        private void OnValidate()
        {
            if (SupportedLanguages.Length == 0)
            {
                CreateDefaultLanguage();
            }
            CheckAllLanguages();
        }

        [ContextMenu("Check All Terms")]
        private void CheckAllTerms()
        {
            Dictionary<SystemLanguage, HashSet<string>> keys = new Dictionary<SystemLanguage, HashSet<string>>();
            HashSet<string> uniqueKeys = new HashSet<string>();
            foreach (SupportedLanguage lang in SupportedLanguages)
            {
                LocalizationResource file = Resources.Load<LocalizationResource>(lang.ResourceFile);
                keys[lang.Language] = new HashSet<string>();

                if(file.Terms == null)
                    continue;

                foreach (LocalizationTerm term in file.Terms)
                {
                    uniqueKeys.Add(term.Key);
                    keys[lang.Language].Add(term.Key);
                }
            }

            foreach (var langPair in keys)
            {
                var keySet = langPair.Value;
                keySet.SymmetricExceptWith(uniqueKeys);
                if (keySet.Count == 0) continue;
                foreach (var key in keySet)
                {
                    Debug.LogWarning($"Key <b>{key}</b> not found in {langPair.Key}");
                }
            }
        }

        private void CheckAllLanguages()
        {
            HashSet<SystemLanguage> usedLanguage = new HashSet<SystemLanguage>();
            foreach (SupportedLanguage lang in SupportedLanguages)
            {
                if (!IsExists(lang.ResourceFile))
                {
                    lang.ResourceFile = CreateNewResource(lang.Language);
                }

                if (usedLanguage.Contains(lang.Language))
                {
                    Debug.LogWarning($"{lang.Language} already in use");
                }
                else
                {
                    usedLanguage.Add(lang.Language);
                }
            }
        }

        private static string CreateNewResource(SystemLanguage langLanguage)
        {
            var resourceName = $"loc_{langLanguage.ToString().ToLower()}";
            if (IsExists(resourceName)) return resourceName;
            UnityEditor.AssetDatabase.CreateAsset(CreateInstance<LocalizationResource>(), $"Assets/Resources/{resourceName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            return resourceName;
        }

        private static bool IsExists(string resourceFile)
        {
            return Resources.Load<LocalizationResource>(resourceFile) != null;
        }

        private void CreateDefaultLanguage()
        {
            DefaultLanguage = SystemLanguage.English;
            SupportedLanguages = new[]
            {
                new SupportedLanguage
                {
                    Language = DefaultLanguage
                }
            };
        }
#endif
    }
}