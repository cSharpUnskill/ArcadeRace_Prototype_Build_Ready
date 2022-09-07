using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Cars
{
    public static class Localization
    {
        public static SystemLanguage CurrentLanguage { get; private set; }
        private static ILookup<string, string> _termsMap;
        private static bool IsLoaded => _termsMap != null;
        public static event Action OnLanguageChanged;
        public static TMP_FontAsset SuggestedFont { get; private set; }

        public static void Load()
        {
            var lang = PlayerPrefs.GetString(LocalizationSettings.Instance.PrefKey, null);

            CurrentLanguage = !Enum.TryParse(lang, out SystemLanguage localizationLanguage)
                ? localizationLanguage
                : DetectLanguage();

            LoadTermsMap();
        }

        private static void LoadTermsMap()
        {
            SupportedLanguage language = LocalizationSettings.Instance.SupportedLanguages.First(z => z.Language == CurrentLanguage);
            LocalizationResource resource = Resources.Load<LocalizationResource>(language.ResourceFile);

            _termsMap = resource.Terms.ToLookup(item => item.Key, item => item.Value);

            SuggestedFont = resource.Font;

            OnLanguageChanged?.Invoke();
        }

        private static SystemLanguage DetectLanguage()
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            foreach (SupportedLanguage lang in LocalizationSettings.Instance.SupportedLanguages)
            {
                if (lang.Language == systemLanguage)
                    return lang.Language;
            }
            return LocalizationSettings.Instance.DefaultLanguage;
        }

        public static void SetLanguage(SystemLanguage lang)
        {
            CurrentLanguage = lang;

            PlayerPrefs.SetString(LocalizationSettings.Instance.PrefKey, lang.ToString());
            PlayerPrefs.Save();

            LoadTermsMap();
        }

        public static string GetTerm(string key, Dictionary<string, string> parameters = null)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            if (!IsLoaded)
                Load();

            var result = _termsMap[key].FirstOrDefault();
            if (result != null)
            {
                if (parameters != null && parameters.Count > 0)
                    parameters.Aggregate(result, (current, parameter) => current.Replace($"{parameter.Key}", parameter.Value));

                return result;
            }

            if (Application.isPlaying)
                Debug.LogWarning($"{key} not found in {CurrentLanguage}");

            return $">>{key}<<";
        }

        private static readonly Regex Plurals = new Regex("(\\[(\\d+-?\\d*:\\w*,?)+\\])");

        public static string GetPlural(string key, int quantity = 1)
        {
            var term = GetTerm(key);
            var qty = CalculatePluralQuantity(quantity);
            return Plurals.Replace(term, x => PluralReplacer(x, qty));
        }

        private static string PluralReplacer(Capture x, int qty)
        {
            var pluralGroup = x.Value.Trim('[', ']');
            var variants = pluralGroup.Split(',');
            foreach (var variant in variants)
            {
                var data = variant.Split(':');
                var range = new PluralRange(data[0]);
                if (range.InRange(qty))
                {
                    return data[1];
                }
            }
            return null;
        }

        private static int CalculatePluralQuantity(int quantity)
        {
            var qty = Math.Abs(quantity) % 100;
            if (qty == 0 && quantity > 0)
            {
                qty = 100;
            }
            else
            {
                qty = qty < 20 ? qty : qty % 10;
                if (qty == 0 && quantity > 0)
                {
                    qty = 20;
                }
            }
            return qty;
        }
    }

    public class PluralRange
    {
        private readonly int max;
        private readonly int min;

        public PluralRange(string range)
        {
            var values = range.Split('-');
            max = min = int.Parse(values[0]);
            if (values.Length != 2) return;
            var secondValue = values[1];
            max = string.IsNullOrEmpty(secondValue) ? int.MaxValue : int.Parse(secondValue);
        }

        public bool InRange(int val)
        {
            return val >= min && val <= max;
        }
    }
}