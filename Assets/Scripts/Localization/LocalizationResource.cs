using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cars
{
    [CreateAssetMenu(fileName = "localization.asset", menuName = "Localization/Create Resource")]
    public class LocalizationResource : ScriptableObject
    {
        public TMP_FontAsset Font;
        public List<LocalizationTerm> Terms;
    }

    [Serializable]
    public class LocalizationTerm
    {
        public string Key;
        public string Value;
    }
}