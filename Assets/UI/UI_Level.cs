using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace ScaleTravel
{

    public class UI_Level : MonoBehaviour
    {
        private GroupBox _GroupTitles;

        // Level - Run
        private GroupBox _GroupTimer;
        private GroupBox _GroupLevelDone;
        private GroupBox _GroupKeys;
        private Label _Time;


        private Button _Btn_Home;
        private Button _Btn_Retry;


        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            _GroupTitles = uiDocument.rootVisualElement.Q<GroupBox>("GroupTitles");
            _GroupLevelDone = uiDocument.rootVisualElement.Q<GroupBox>("GroupLevelDone");
            _GroupTimer = uiDocument.rootVisualElement.Q<GroupBox>("GroupTimer");
            _GroupKeys = uiDocument.rootVisualElement.Q<GroupBox>("GroupKeys");

            _GroupTitles.style.display = DisplayStyle.None;
            _GroupLevelDone.style.display = DisplayStyle.None;
            _GroupTimer.style.display = DisplayStyle.None;
            _GroupKeys.style.display = DisplayStyle.None;

            _Time = uiDocument.rootVisualElement.Q<Label>("Time");

            // Buttons
            _Btn_Home = uiDocument.rootVisualElement.Q<Button>("btn_home");
            _Btn_Retry = uiDocument.rootVisualElement.Q<Button>("btn_retry");

            _Btn_Home.RegisterCallback<ClickEvent>(delegate { Home(); });
            _Btn_Retry.RegisterCallback<ClickEvent>(delegate { Retry(); });
        }

        private void OnDisable()
        {
            _Btn_Home.UnregisterCallback<ClickEvent>(delegate { Home(); });
            _Btn_Retry.UnregisterCallback<ClickEvent>(delegate { Retry(); });
        }

        private void Home()
        {
            GameManager.Instance.Home();
        }

        private void Retry()
        {
            GameManager.Instance.Retry();
        }

        public void StartLevelScreen()
        {
            PlayerLocal.LevelText levelText = PlayerLocal.Instance.GetLevelText();

            _GroupTitles.Q<Label>("labelLevel_groupLevelName").text = GameManager.Instance.LevelValues.GroupLevelName;
            _GroupTitles.Q<Label>("labelLevel_levelName").text = GameManager.Instance.LevelValues.LevelName;

            // TODO si gamepad sprite background sinon lettre => gérer avec class ?
            _GroupTitles.Q<Label>("label_menu").text = levelText.Home;
            _GroupTitles.Q<Label>("label_retry").text = levelText.Retry;

            _GroupLevelDone.Q<Label>("label_title_best").text = levelText.BestTime;
            _GroupLevelDone.Q<Label>("label_endNext").text = levelText.JumpToContinue;
            _GroupLevelDone.Q<Label>("label_endNext").style.display = DisplayStyle.None;

            _GroupTitles.style.display = DisplayStyle.Flex;
        }

        public void UpdateKeys(int keys)
        {
            if (keys == 0)
            {
                _GroupKeys.style.display = DisplayStyle.None;
                return;
            }

            _GroupKeys.Q<Label>("label_keys").text = "x" + keys;
            _GroupKeys.style.display = DisplayStyle.Flex;
        }

        public void DisplayTimer(string timeToDisplay)
        {
            //Debug.Log("UI DisplayTimer");
            _Time.text = timeToDisplay;
            _GroupTimer.style.display = DisplayStyle.Flex;
        }
        public void HideTimer()
        {
            _GroupTimer.style.display = DisplayStyle.None;
        }
        public void TimerCup(int cupId)
        {
            var spriteCup = _GroupTimer.Q<VisualElement>("sprite_cupTimer");
            SetCupClassById(spriteCup, cupId);
        }
        public void UpdateTimer(string timeToDisplay)
        {
            _Time.text = timeToDisplay;
        }
        public void EndLevelScreen(float time = 0.0f, string displayTime = "")
        {
            //Debug.Log("UI EndLevelScreen => " + displayTime);
            _GroupLevelDone.Q<Label>("label_endWord").text = GameManager.Instance.LevelValues.EndWord;
            _GroupLevelDone.Q<Label>("label_time").text = displayTime;

            // Player
            var spriteCup = _GroupLevelDone.Q<VisualElement>("sprite_cup");
            SetCupClass(spriteCup, time);
            
            // Best
            var groupBest = _GroupLevelDone.Q<GroupBox>("GroupEndBest");
            var isBestSamePlayer = PlayerLocal.Instance.HeroData.Profile.PlayerName == PlayerLocal.Instance.LevelBestPlayer;

            if (time == PlayerLocal.Instance.LevelBestTime && isBestSamePlayer)
            {
                groupBest.style.display = DisplayStyle.None;
            }
            else
            {
                var spriteBestCup = _GroupLevelDone.Q<VisualElement>("sprite_cup_best");
                SetCupClass(spriteBestCup, PlayerLocal.Instance.LevelBestTime);

                _GroupLevelDone.Q<Label>("label_bestTime").text = PlayerLocal.Instance.LevelBestDisplayTime;
                _GroupLevelDone.Q<Label>("label_bestProfile").text = isBestSamePlayer ? "" : "(" + PlayerLocal.Instance.LevelBestPlayer + ")";

                groupBest.style.display = DisplayStyle.Flex;
            }

            // Level times
            _GroupLevelDone.Q<Label>("label_time_gold").text = PlayerLocal.Instance.FormatTime(GameManager.Instance.LevelValues.RunCupTime[0]);
            _GroupLevelDone.Q<Label>("label_time_silver").text = PlayerLocal.Instance.FormatTime(GameManager.Instance.LevelValues.RunCupTime[1]);
            _GroupLevelDone.Q<Label>("label_time_bronze").text = PlayerLocal.Instance.FormatTime(GameManager.Instance.LevelValues.RunCupTime[2]);

            _GroupTimer.style.display = DisplayStyle.None;
            _GroupLevelDone.style.display = DisplayStyle.Flex;

            GameManager.Instance.NextLevelAction();
        }

        public void DisplayNextLevelAction()
        {
            _GroupLevelDone.Q<Label>("label_endNext").style.display = DisplayStyle.Flex;
        }

        private void SetCupClass(VisualElement spriteCup, float time)
        {
            spriteCup.RemoveFromClassList("scale-cup-level-gold");
            spriteCup.RemoveFromClassList("scale-cup-level-silver");
            spriteCup.RemoveFromClassList("scale-cup-level-bronze");
            spriteCup.style.display = DisplayStyle.Flex;

            if (time <= GameManager.Instance.LevelValues.RunCupTime[0])
            {
                //Debug.Log("Gold");
                spriteCup.AddToClassList("scale-cup-level-gold");
            }
            else if (time <= GameManager.Instance.LevelValues.RunCupTime[1])
            {
                //Debug.Log("Silver");
                spriteCup.AddToClassList("scale-cup-level-silver");
            }
            else if (time <= GameManager.Instance.LevelValues.RunCupTime[2])
            {
                //Debug.Log("Bronze");
                spriteCup.AddToClassList("scale-cup-level-bronze");
            }
            else
            {
                spriteCup.style.display = DisplayStyle.None;
            }
        }

        private void SetCupClassById(VisualElement spriteCup, int cupId)
        {
            spriteCup.RemoveFromClassList("scale-cup-level-gold");
            spriteCup.RemoveFromClassList("scale-cup-level-silver");
            spriteCup.RemoveFromClassList("scale-cup-level-bronze");
            spriteCup.style.display = DisplayStyle.Flex;

            switch (cupId)
            {
                case 1:
                    spriteCup.AddToClassList("scale-cup-level-gold");
                    break;
                case 2:
                    spriteCup.AddToClassList("scale-cup-level-silver");
                    break;
                case 3:
                    spriteCup.AddToClassList("scale-cup-level-bronze");
                    break;
                default:
                    spriteCup.style.display = DisplayStyle.None;
                    break;
            }
        }

    }

}
