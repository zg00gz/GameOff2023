using System;
using UnityEngine;

namespace ScaleTravel
{
    
    public enum Lang
    {
        EN,
        FR
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableData/Level", order = 1)]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private int _LevelID;
        [SerializeField]
        private float[] _RunCupTime;

        public int LevelID => _LevelID;
        public float[] RunCupTime => _RunCupTime;


        #region Language

        [Serializable]
        public class EN
        {
            public string GroupLevelName;
            public string LevelName;
            public string LevelScale;
            public string GoWord;
            public string EndWord;
        }
        [SerializeField]
        private EN _English;

        [Serializable]
        public class FR
        {
            public string GroupLevelName;
            public string LevelName;
            public string LevelScale;
            public string GoWord;
            public string EndWord;
        }
        [SerializeField]
        private FR _French;

        private string _GroupLevelName;
        private string _LevelName;
        private string _LevelScale;
        private string _GoWord;
        private string _EndWord;

        public string GroupLevelName => _GroupLevelName;
        public string LevelName => _LevelName;
        public string LevelScale => _LevelScale;
        public string GoWord => _GoWord;
        public string EndWord => _EndWord;

        public void SetLanguage(Lang _Language)
        {
            switch (_Language)
            {
                case Lang.EN:
                    _GroupLevelName = _English.GroupLevelName;
                    _LevelName = _English.LevelName;
                    _LevelScale = _English.LevelScale;
                    _GoWord = _English.GoWord;
                    _EndWord = _English.EndWord;
                    break;

                case Lang.FR:
                    _GroupLevelName = _French.GroupLevelName;
                    _LevelName = _French.LevelName;
                    _LevelScale = _French.LevelScale;
                    _GoWord = _French.GoWord;
                    _EndWord = _French.EndWord;
                    break;
            }
        }

        #endregion

    }
}