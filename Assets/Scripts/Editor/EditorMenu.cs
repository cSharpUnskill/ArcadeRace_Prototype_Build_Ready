using UnityEditor;
using UnityEngine;

namespace Cars
{
    public static class EditorMenu
    {
        [MenuItem("Game/Localization/English")]
        public static void SetLanguageEnglish()
        {
            Localization.SetLanguage(SystemLanguage.English);
            UpdateCheckbox();
        }

        [MenuItem("Game/Localization/Russian")]
        public static void SetLanguageRussian()
        {
            Localization.SetLanguage(SystemLanguage.Russian);
            UpdateCheckbox();
        }

        [MenuItem("Game/Localization/Reload", priority = 200)]
        public static void ReloadLocalization()
        {
            Localization.Load();
        }

        private static void UpdateCheckbox()
        {
            Menu.SetChecked("Game/Localization/English", Localization.CurrentLanguage == SystemLanguage.English);
            Menu.SetChecked("Game/Localization/Russian", Localization.CurrentLanguage == SystemLanguage.Russian);
        }

        [InitializeOnLoadMethod]
        public static void LoadCheckboxes()
        {
            EditorApplication.delayCall += UpdateCheckbox;
        }
    }
}