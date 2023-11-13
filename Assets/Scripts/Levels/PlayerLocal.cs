using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;


namespace ScaleTravel
{

    public class PlayerLocal : MonoBehaviour
    {
        public static PlayerLocal Instance;

        public List<ProfileData> ExistingProfiles = null;
        public SaveData HeroData;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("PlayerLocal instance..."); // Destroy
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

#if !UNITY_WEBGL
        private void Update()
        {
            if (Input.GetKey("escape"))
            {

                Debug.Log("Quit (Escape)");
                Application.Quit();
            }
        }
#endif


        #region Player data

        // P_IDplayer.json
        [System.Serializable]
        public class SaveData
        {
            public ProfileData Profile;
            public List<Level> Levels;
        }
        [System.Serializable]
        public class Level
        {
            public int LevelID;
            public float Time;
            public string DisplayTime;
            public string Date;
        }
        [System.Serializable]
        public class ProfileData
        {
            public string PlayerID;
            public string PlayerName = "Hero";
            public float TotalPlayedTime;
            public Lang PlayerLanguage = Lang.EN;
            public float MusicVolume = 0.0f;
            public float SoundVolume = 0.0f;
        }

        public void GetExistingProfiles()
        {
            List<ProfileData> existingProfiles = new List<ProfileData>();

            try
            {
                DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath);
                FileInfo[] files = d.GetFiles("P_*.json");

                foreach (var file in files)
                {
                    //Debug.Log("GetExistingProfiles - " + file.Name);

                    string json = File.ReadAllText(Application.persistentDataPath + "/" + file.Name);
                    SaveData data = JsonUtility.FromJson<SaveData>(json);

                    existingProfiles.Add(data.Profile);

                }
                ExistingProfiles = existingProfiles.OrderBy(p => p.PlayerName).ToList();
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
                ExistingProfiles = new List<ProfileData>(){ HeroData.Profile };
            }
        }

        public void SaveProfile(ProfileData profile)
        {
            SaveData data = new SaveData();

            if (string.IsNullOrEmpty(profile.PlayerName)) profile.PlayerName = "Hero";

            if (String.IsNullOrEmpty(profile.PlayerID))
            {
                profile.PlayerID = Guid.NewGuid().ToString("N");
                ExistingProfiles.Add(profile);
                ExistingProfiles = ExistingProfiles.OrderBy(p => p.PlayerName).ToList();
                data.Profile = new ProfileData();
                data.Levels = new List<Level>();
            }
            else
            {
                data = Load(profile.PlayerID);
            }
            data.Profile = profile;
            
            try
            {
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.persistentDataPath + "/P_" + profile.PlayerID + ".json", json);
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
            }
            HeroData = data;
        }
        
        public void SaveLevel(string playerID, int levelID, float playedTime, float time, string displayTime)
        {
            //Debug.Log("P_ Save player level");
            SaveData data = Load(playerID);
            
            Level level = new Level
            {
                LevelID = levelID,
                Time = time,
                DisplayTime = displayTime,
                Date = DateTime.Now.ToString()
            };

            bool isLevelInArray = false;
            if (data.Levels == null) data.Levels = new List<Level>();
            for(var i = 0; i < data.Levels.Count; i++)
            {
                if(data.Levels[i].LevelID == levelID)
                {
                    isLevelInArray = true;
                    if(data.Levels[i].Time > time) data.Levels[i] = level;
                }
            }
            if (!isLevelInArray) data.Levels.Add(level);
                
            try
            {
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.persistentDataPath + "/P_" + playerID + ".json", json);
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
            }

            HeroData = data;
            SaveLevelScore(levelID, time, displayTime);
        }

        public SaveData Load(string playerID)
        {
            //Debug.Log(playerID);
            try
            {
                string path = Application.persistentDataPath + "/P_" + playerID + ".json";
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    SaveData data = JsonUtility.FromJson<SaveData>(json);

                    //Debug.Log("Load data " + data.Profile.PlayerID);

                    return data ?? HeroData;
                }
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
            }
            return HeroData;
        }

#endregion

#region Level scores

        // L_LevelID.json
        [System.Serializable]
        public class LevelSaveData
        {
            public List<LevelScore> Scores;
        }
        [System.Serializable]
        public class LevelScore
        {
            public string PlayerID;
            public string Player;
            public float Time;
            public string DisplayTime;
            public string Date;
        }

        private void SaveLevelScore(int levelID, float time, string displayTime)
        {
            LevelSaveData data = LoadScore(levelID);
            if (data.Scores == null) data.Scores = new List<LevelScore>();
            data.Scores.Add(new LevelScore()
            {
                PlayerID = HeroData.Profile.PlayerID,
                Player = HeroData.Profile.PlayerName,
                Time = time, 
                DisplayTime = displayTime,
                Date = DateTime.Now.ToString()
            });
            data.Scores = data.Scores.OrderBy(s => s.Time).Take(10).ToList();
            
            try
            {
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.persistentDataPath + "/L_" + levelID + ".json", json);
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
            }
        }

        public LevelSaveData LoadScore(int levelID)
        {
            try
            {
                string path = Application.persistentDataPath + "/L_" + levelID + ".json";
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    LevelSaveData data = JsonUtility.FromJson<LevelSaveData>(json);

                    return data ?? new LevelSaveData();
                }
            }
            catch
            {
                Debug.Log("Cannot access " + Application.persistentDataPath + " - PlayerLocal data not saved");
            }
            return new LevelSaveData();
        }

#endregion

#region Language

        public class MenuText
        {
            public string NewProfile;
            public string PlayerName;
            public string TotalPlayTime;
            public string Language;
            public string MusicVolume;
            public string SoundVolume;
        }
        public MenuText GetMenuText()
        {
            var menuText = new MenuText();

            switch (HeroData.Profile.PlayerLanguage)
            {
                case Lang.EN:
                    menuText.NewProfile = "New local profile";
                    menuText.PlayerName = "Hero name";
                    menuText.TotalPlayTime = "Total play time";
                    menuText.Language = "Language";
                    menuText.MusicVolume = "Music";
                    menuText.SoundVolume = "Sounds";
                    break;

                case Lang.FR:
                    menuText.NewProfile = "Nouveau profil local";
                    menuText.PlayerName = "Nom du héros";
                    menuText.TotalPlayTime = "Temps total de jeu";
                    menuText.Language = "Langue";
                    menuText.MusicVolume = "Musique";
                    menuText.SoundVolume = "Sons";
                    break;
            }

            return menuText;
        }

        public class LevelText
        {
            public string Time;
            public string BestScore;
        }
        public LevelText GetLevelText()
        {
            var levelText = new LevelText();

            switch (HeroData.Profile.PlayerLanguage)
            {
                case Lang.EN:
                    levelText.Time = "Time";
                    levelText.BestScore = "Best";
                    break;

                case Lang.FR:
                    levelText.Time = "Temps";
                    levelText.BestScore = "Meilleurs";
                    break;
            }

            return levelText;
        }

#endregion

        public string FormatTime(float timeToDisplay, bool isWithMs = false)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            if (!isWithMs)
            {
                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                float milliSeconds = (timeToDisplay % 1) * 1000;
                return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
            }
        }

        public string FormatTotalTime(float timeToDisplay)
        {
            int hours = Mathf.FloorToInt(Mathf.FloorToInt(timeToDisplay / 3600));
            int minutes = hours > 0 ? Mathf.FloorToInt( (timeToDisplay - hours*3600 ) / 60) : Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            return string.Format("{0} h  {1} m  {2} s", hours, minutes, seconds);
        }

    }

}
