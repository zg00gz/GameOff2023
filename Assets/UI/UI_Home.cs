using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace ScaleTravel
{

    public class UI_Home : MonoBehaviour
    {
        [SerializeField] LevelData[] m_Levels;
        [SerializeField] AudioMixer m_MasterMixer;
        [SerializeField] VisualTreeAsset m_ScoreLineTemplate;

        private GroupBox _GroupTitle;
        private GroupBox _GroupFirstProfile;
        private GroupBox _GroupProfiles;
        private GroupBox _GroupLevels;
        private GroupBox _GroupParams;
        private GroupBox _GroupScores;

        private Button _Btn_OK_EN;
        private Button _Btn_OK_FR;

        private Button _Btn_Go;
        private Button _Btn_NewProfile;
        private DropdownField _Ddl_Profiles;

        private Label _ProfileName;
        private Button _Btn_DisplayProfiles;
        private Button _Btn_DisplayParams;
        private Button _Btn_OK;

        private List<GroupBox> _Groupbox_levelGroups;
        private List<Label> _Label_LevelGroups;
        private List<Button> _Btn_Levels;
        private List<Button> _Btn_Scores;
        private List<VisualElement> _CupLevels;

        private Button _Btn_OK_Scores;

        IEnumerator LoadHome()
        {
            yield return new WaitUntil(() => PlayerLocal.Instance != null);

            if(!string.IsNullOrEmpty(PlayerLocal.Instance.HeroData.Profile.PlayerID))
            {
                // From level
                //Debug.Log("Wait level screen...");
                SetAudioMixerVolume();
                yield return new WaitForSeconds(1f);
                DisplayLevels();
            }
            else
            {
                // Game start
                //Debug.Log("Wait profile screen...");
                yield return new WaitForSeconds(1f);

                Debug.Log("Get profiles...");
                PlayerLocal.Instance.ExistingProfiles = null;
                //Debug.Log((PlayerLocal.Instance.ExistingProfiles != null) + " " + Time.time);
                PlayerLocal.Instance.GetExistingProfiles();
                yield return new WaitUntil(() => PlayerLocal.Instance.ExistingProfiles != null);
                //Debug.Log((PlayerLocal.Instance.ExistingProfiles != null) + " " + Time.time);


                switch (PlayerLocal.Instance.ExistingProfiles.Count)
                {
                    case 0:
                        DisplayCreateProfile();
                        break;
                    default:
                        DisplayProfiles();
                        break;
                }
            }
        }

        private void OnEnable()
        {
            UnityEngine.Cursor.visible = true;

            var uiDocument = GetComponent<UIDocument>();
            _GroupTitle = uiDocument.rootVisualElement.Q<GroupBox>("GroupTitle");
            _GroupFirstProfile = uiDocument.rootVisualElement.Q<GroupBox>("GroupFirstProfile");
            _GroupProfiles = uiDocument.rootVisualElement.Q<GroupBox>("GroupProfiles");
            _GroupLevels = uiDocument.rootVisualElement.Q<GroupBox>("GroupLevels");
            _GroupParams = uiDocument.rootVisualElement.Q<GroupBox>("GroupParams");
            _GroupScores = uiDocument.rootVisualElement.Q<GroupBox>("GroupScores");

            _GroupFirstProfile.style.display = DisplayStyle.None;
            _GroupProfiles.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.None;
            _GroupScores.style.display = DisplayStyle.None;
            StartCoroutine(LoadHome());


            // First Profile
            _Btn_OK_EN = uiDocument.rootVisualElement.Q<Button>("Btn_OK_EN");
            _Btn_OK_EN.RegisterCallback<ClickEvent>(delegate { CreateProfile(Lang.EN); });
            _Btn_OK_FR = uiDocument.rootVisualElement.Q<Button>("Btn_OK_FR");
            _Btn_OK_FR.RegisterCallback<ClickEvent>(delegate { CreateProfile(Lang.FR); });


            // Select profile/ New profile
            _Btn_Go = uiDocument.rootVisualElement.Q<Button>("Btn_Go");
            _Btn_NewProfile = uiDocument.rootVisualElement.Q<Button>("Btn_NewProfile");
            _Ddl_Profiles = uiDocument.rootVisualElement.Q<DropdownField>("ddl_Profiles");
            _Btn_Go.RegisterCallback<ClickEvent>(SelectProfile);
            _Btn_NewProfile.RegisterCallback<ClickEvent>(delegate { DisplayCreateProfile(); } );

            _ProfileName = uiDocument.rootVisualElement.Q<Label>("label_profileName");
            _Btn_DisplayProfiles = uiDocument.rootVisualElement.Q<Button>("btn_displayProfiles");
            _Btn_DisplayProfiles.RegisterCallback<ClickEvent>(delegate { DisplayProfiles(); });


            // Levels
            _Label_LevelGroups = new List<Label>();
            _Groupbox_levelGroups = new List<GroupBox>();
            for( var i=1; i < 5; i++)
            {
                _Label_LevelGroups.Add(uiDocument.rootVisualElement.Q<Label>("Group-"+ i +"-Name"));
                _Groupbox_levelGroups.Add(uiDocument.rootVisualElement.Q<GroupBox>("Group-"+ i ));
            }
           
            _Btn_Levels = new List<Button>();
            _CupLevels = new List<VisualElement>();
            _Btn_Scores = new List<Button>();
            foreach (LevelData level in m_Levels)
            {
                _Btn_Levels.Add(_GroupLevels.Q<Button>("Level-"+level.LevelID));
                _Btn_Levels[_Btn_Levels.Count - 1].RegisterCallback<ClickEvent>(delegate { LoadScene(level.LevelID); });
                _CupLevels.Add(_GroupLevels.Q<VisualElement>("Cup-" + level.LevelID));
                _Btn_Scores.Add(_GroupLevels.Q<Button>("Scores-" + level.LevelID));
                _Btn_Scores[_Btn_Levels.Count - 1].RegisterCallback<ClickEvent>(delegate { DisplayScores(level.LevelID); });
            }

            // Parameters/Options
            _Btn_DisplayParams = uiDocument.rootVisualElement.Q<Button>("btn_displayParam");
            _Btn_DisplayParams.RegisterCallback<ClickEvent>(delegate { DisplayUpdateProfile(); });
            _Btn_OK = uiDocument.rootVisualElement.Q<Button>("Btn_OK");
            _Btn_OK.RegisterCallback<ClickEvent>(delegate { UpdateProfile(); });

            _GroupParams.Q<Slider>("slider_music").RegisterCallback<ChangeEvent<float>>(UpdateMusicVolume);
            _GroupParams.Q<Slider>("slider_sounds").RegisterCallback<ChangeEvent<float>>(UpdateSoundVolume);

            _Btn_OK_Scores = uiDocument.rootVisualElement.Q<Button>("Btn_OK_Scores");
            _Btn_OK_Scores.RegisterCallback<ClickEvent>(delegate { HideScores(); });
        }

        private void SetLabels()
        {
            Lang lang = PlayerLocal.Instance.HeroData.Profile.PlayerLanguage;
            PlayerLocal.MenuText menuText = PlayerLocal.Instance.GetMenuText();

            // Select profile/ New profile
            _Btn_NewProfile.text = menuText.NewProfile;

            _GroupLevels.Q<Label>("label_playTime").text = menuText.TotalPlayTime;

            // Params
            _GroupParams.Q<TextField>("input_PlayerName").label = menuText.PlayerName;
            _GroupParams.Q<RadioButtonGroup>("radio_PlayerLanguage").label = menuText.Language;
            _GroupParams.Q<RadioButton>("radio_FR").text = lang == Lang.FR ? "Français" : "French";
            _GroupParams.Q<Slider>("slider_music").label = menuText.MusicVolume;
            _GroupParams.Q<Slider>("slider_sounds").label = menuText.SoundVolume;

            // Levels
            for (var i = 0; i < _Btn_Levels.Count; i++)
            {
                m_Levels[i].SetLanguage(lang);
                _Btn_Levels[i].text = m_Levels[i].LevelName;
            }
            _Label_LevelGroups[0].text = m_Levels[0].GroupLevelName;
            _Label_LevelGroups[1].text = m_Levels[3].GroupLevelName;
            _Label_LevelGroups[2].text = m_Levels[9].GroupLevelName;
            _Label_LevelGroups[3].text = m_Levels[14].GroupLevelName;
        }

        private void DisplayCreateProfile()
        {
            TextField inputPlayerName = _GroupFirstProfile.Q<TextField>("input_PlayerName");
            inputPlayerName.value = "Hero";

            _GroupProfiles.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.None;
            _GroupScores.style.display = DisplayStyle.None;
            _GroupFirstProfile.style.display = DisplayStyle.Flex;
        }
        private void CreateProfile(Lang lang)
        {
            Debug.Log("Create profile");
            TextField inputPlayerName = _GroupFirstProfile.Q<TextField>("input_PlayerName");

            PlayerLocal.ProfileData profile = new PlayerLocal.ProfileData();
            profile.PlayerName = inputPlayerName.text != "" ? inputPlayerName.text : "Hero";
            profile.PlayerLanguage = lang;
            PlayerLocal.Instance.SaveProfile(profile);
            
            DisplayLevels();
        }

        private void DisplayUpdateProfile()
        {
            TextField inputPlayerName = _GroupFirstProfile.Q<TextField>("input_PlayerName");
            inputPlayerName.value = "Hero";
            _GroupParams.Q<TextField>("input_PlayerName").value = PlayerLocal.Instance.HeroData.Profile.PlayerName;
            _GroupParams.Q<RadioButton>("radio_EN").value = PlayerLocal.Instance.HeroData.Profile.PlayerLanguage == Lang.EN ? true : false;
            _GroupParams.Q<RadioButton>("radio_FR").value = PlayerLocal.Instance.HeroData.Profile.PlayerLanguage == Lang.FR ? true : false;
            _GroupParams.Q<Slider>("slider_music").value = PlayerLocal.Instance.HeroData.Profile.MusicVolume;
            _GroupParams.Q<Slider>("slider_sounds").value = PlayerLocal.Instance.HeroData.Profile.SoundVolume;

            _GroupFirstProfile.style.display = DisplayStyle.None;
            _GroupProfiles.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.None;
            _GroupScores.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.Flex;
        }
        private void UpdateProfile()
        {
            Debug.Log("Update profile");

            PlayerLocal.ProfileData profile = PlayerLocal.Instance.HeroData.Profile;
            profile.PlayerName = _GroupParams.Q<TextField>("input_PlayerName").text;
            profile.PlayerLanguage = _GroupParams.Q<RadioButton>("radio_EN").value ? Lang.EN : Lang.FR;
            profile.MusicVolume = _GroupParams.Q<Slider>("slider_music").value;
            profile.SoundVolume = _GroupParams.Q<Slider>("slider_sounds").value;

            PlayerLocal.Instance.SaveProfile(profile);

            DisplayLevels();
        }

        private void DisplayProfiles()
        {
            _GroupFirstProfile.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.None;
            StartCoroutine(LoadExistingProfiles());
        }
        IEnumerator LoadExistingProfiles()
        {
            if(PlayerLocal.Instance.ExistingProfiles.Count == 0)
            {
                Debug.Log("Get profiles...");
                PlayerLocal.Instance.ExistingProfiles = new List<PlayerLocal.ProfileData>();
                PlayerLocal.Instance.GetExistingProfiles();
                yield return new WaitUntil(() => PlayerLocal.Instance.ExistingProfiles.Count > 0);
            }

            List<string> profiles = PlayerLocal.Instance.ExistingProfiles.Select(p => p.PlayerName).ToList();
            _Ddl_Profiles.choices.Clear();
            _Ddl_Profiles.choices = profiles;

            if (!string.IsNullOrEmpty(PlayerLocal.Instance.HeroData.Profile.PlayerID))
                _Ddl_Profiles.index = PlayerLocal.Instance.ExistingProfiles.Select(p => p.PlayerID).ToList().IndexOf(PlayerLocal.Instance.HeroData.Profile.PlayerID);
            else
                _Ddl_Profiles.index = 0;

            _GroupProfiles.style.display = DisplayStyle.Flex;
        }

        private void SelectProfile(ClickEvent evt)
        {
            var playerID = PlayerLocal.Instance.ExistingProfiles[_Ddl_Profiles.index].PlayerID;
            PlayerLocal.Instance.HeroData = PlayerLocal.Instance.Load(playerID);
            SetAudioMixerVolume();
            DisplayLevels();
        }

        private void SetAudioMixerVolume()
        {
            m_MasterMixer.SetFloat("musicVol", PlayerLocal.Instance.HeroData.Profile.MusicVolume);
            m_MasterMixer.SetFloat("soundVol", PlayerLocal.Instance.HeroData.Profile.SoundVolume);
        }

        private void DisplayLevels()
        {
            _GroupFirstProfile.style.display = DisplayStyle.None;
            _GroupProfiles.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.None;
            _GroupScores.style.display = DisplayStyle.None;
            _GroupTitle.style.display = DisplayStyle.Flex;
            SetLabels();
            _ProfileName.text = PlayerLocal.Instance.HeroData.Profile.PlayerName;
            _GroupLevels.Q<Label>("TotalPlayTime").text = PlayerLocal.Instance.FormatTotalTime(PlayerLocal.Instance.HeroData.Profile.TotalPlayedTime);


            foreach (Button btn_level in _Btn_Levels)
            {
                btn_level.SetEnabled(false);
            }
            _Btn_Levels[0].SetEnabled(true);


            var playerLevels = PlayerLocal.Instance.HeroData.Levels.OrderBy(p => p.LevelID).ToList();

            for (var i = 0; i < _Btn_Levels.Count; i++)
            {
                _Btn_Levels[i].RemoveFromClassList("hero-button-level-default");
                _Btn_Levels[i].RemoveFromClassList("hero-button-level-bronze");
                _Btn_Levels[i].RemoveFromClassList("hero-button-level-silver");
                _Btn_Levels[i].RemoveFromClassList("hero-button-level-gold");
                _CupLevels[i].RemoveFromClassList("hero-cup-level-default");
                _CupLevels[i].RemoveFromClassList("hero-cup-level-bronze");
                _CupLevels[i].RemoveFromClassList("hero-cup-level-silver");
                _CupLevels[i].RemoveFromClassList("hero-cup-level-gold");
            }

            for (var i = 0; i < playerLevels.Count; i++)
            {
                if(playerLevels[i].Time <= m_Levels[i].RunCupTime[0])
                {
                    _Btn_Levels[i].AddToClassList("hero-button-level-gold");
                    _CupLevels[i].AddToClassList("hero-cup-level-gold");
                }
                else if (playerLevels[i].Time <= m_Levels[i].RunCupTime[1])
                {
                    _Btn_Levels[i].AddToClassList("hero-button-level-silver");
                    _CupLevels[i].AddToClassList("hero-cup-level-silver");
                }
                else if (playerLevels[i].Time <= m_Levels[i].RunCupTime[2])
                {
                    _Btn_Levels[i].AddToClassList("hero-button-level-bronze");
                    _CupLevels[i].AddToClassList("hero-cup-level-bronze");
                }
                else
                {
                    _Btn_Levels[i].AddToClassList("hero-button-level-default");
                    _CupLevels[i].AddToClassList("hero-cup-level-default");
                }

                // Active next level
                if (m_Levels.Count() > i+1)
                {
                    _Btn_Levels[i+1].SetEnabled(true);
                }
            }

            _GroupLevels.style.display = DisplayStyle.Flex;
        }

        private void HideScores()
        {
            _GroupScores.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.Flex;
        }
        private void DisplayScores(int levelID)
        {
            //Debug.Log("Display scores");
            _GroupFirstProfile.style.display = DisplayStyle.None;
            _GroupProfiles.style.display = DisplayStyle.None;
            _GroupLevels.style.display = DisplayStyle.None;
            _GroupParams.style.display = DisplayStyle.None;

            var levelData = m_Levels[levelID - 1];
            _GroupScores.Q<Label>("label_scores_groupLevel").text = levelData.GroupLevelName;
            _GroupScores.Q<Label>("label_scores_level").text = levelData.LevelName;
            _GroupScores.Q<Label>("label_scores_cup_0").text = PlayerLocal.Instance.FormatTime(levelData.RunCupTime[0]);
            _GroupScores.Q<Label>("label_scores_cup_1").text = PlayerLocal.Instance.FormatTime(levelData.RunCupTime[1]);
            _GroupScores.Q<Label>("label_scores_cup_2").text = PlayerLocal.Instance.FormatTime(levelData.RunCupTime[2]);

            _GroupScores.Q<Label>("label_scores_player").text = PlayerLocal.Instance.HeroData.Profile.PlayerName;

            if (PlayerLocal.Instance.HeroData.Levels != null && PlayerLocal.Instance.HeroData.Levels.Count > 0)
            {
                var playerLevalData = PlayerLocal.Instance.HeroData.Levels.Where(p => p.LevelID == levelID).ToList();
                if(playerLevalData.Count > 0)
                {
                    _GroupScores.Q<Label>("label_scores_time").text = playerLevalData[0].DisplayTime;
                    _GroupScores.Q<Label>("label_scores_date").text = playerLevalData[0].Date;
                }
                else
                {
                    _GroupScores.Q<Label>("label_scores_time").text = "--:--:---";
                    _GroupScores.Q<Label>("label_scores_date").text = "--/--/-- --:--:--";
                }
            }
            else
            {
                _GroupScores.Q<Label>("label_scores_time").text = "--:--:---";
                _GroupScores.Q<Label>("label_scores_date").text = "--/--/-- --:--:--";
            }

            PlayerLocal.LevelSaveData scores = PlayerLocal.Instance.LoadScore(levelID);
            var groupScoreLines = _GroupScores.Q<GroupBox>("GroupScoreLines");
            groupScoreLines.Clear();

            int nbScores = scores.Scores != null ? scores.Scores.Count : 0;

            for ( var i=0; i < nbScores; i++)
            {
                var newLine = m_ScoreLineTemplate.Instantiate();
                newLine.Q<Label>("scoreLine_position").text = (i + 1).ToString();
                newLine.Q<Label>("scoreLine_player").text = scores.Scores[i].Player;
                newLine.Q<Label>("scoreLine_time").text = scores.Scores[i].DisplayTime;
                newLine.Q<Label>("scoreLine_date").text = scores.Scores[i].Date;

                groupScoreLines.Add(newLine);
            }
            for ( var i = nbScores + 1; i < 11; i++)
            {
                var newLine = m_ScoreLineTemplate.Instantiate();
                newLine.Q<Label>("scoreLine_position").text = i.ToString();
                newLine.Q<Label>("scoreLine_player").text = "----------";
                newLine.Q<Label>("scoreLine_time").text = "--:--:---";
                newLine.Q<Label>("scoreLine_date").text = "--/--/-- --:--:--";

                groupScoreLines.Add(newLine);
            }

            _GroupScores.style.display = DisplayStyle.Flex;
        }

        private void LoadScene(int levelID)
        {
            SceneManager.LoadScene("Level-" + levelID.ToString());
        }

        private void UpdateMusicVolume(ChangeEvent<float> evt)
        {
            m_MasterMixer.SetFloat("musicVol", evt.newValue);
        }

        private void UpdateSoundVolume(ChangeEvent<float> evt)
        {
            m_MasterMixer.SetFloat("soundVol", evt.newValue);
        }

    }

}
