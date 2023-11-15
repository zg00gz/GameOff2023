using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace ScaleTravel
{

    public class UI_Level : MonoBehaviour
    {
        private GroupBox _GroupTitles;
        private GroupBox _PauseMenu;

        // Level - Run
        private Label _Time;
        private GroupBox _LevelDoneOrFailed;


        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            _GroupTitles = uiDocument.rootVisualElement.Q<GroupBox>("GroupTitles");
            _LevelDoneOrFailed = uiDocument.rootVisualElement.Q<GroupBox>("LevelDoneOrFailed");

            _GroupTitles.style.display = DisplayStyle.None;
            _LevelDoneOrFailed.style.display = DisplayStyle.None;

            _Time = uiDocument.rootVisualElement.Q<Label>("Time");
        }

        private void OnDisable()
        {

        }

        public void DisplayTitles()
        {
            //Debug.Log("UI DisplayTitle");
            _GroupTitles.Q<Label>("labelLevel_groupLevelName").text = GameManager.Instance.LevelValues.GroupLevelName;
            _GroupTitles.Q<Label>("labelLevel_levelName").text = GameManager.Instance.LevelValues.LevelName;
            _GroupTitles.style.display = DisplayStyle.Flex;
        }

        public void DisplayTimer(string timeToDisplay)
        {
            //Debug.Log("UI DisplayTimer");
            _Time.text = timeToDisplay;
            _Time.style.display = DisplayStyle.Flex;
        }
        public void UpdateTimer(string timeToDisplay)
        {
            _Time.text = timeToDisplay;
        }
        public void ElapsedTimeScreen(float time = 0.0f, string displayTime = "")
        {
            //Debug.Log("UI ElapsedTimeScreen => " + displayTime);

            if (GameManager.Instance.IsLevelDone)
            {
                _LevelDoneOrFailed.Q<Label>("label_endWord").text = GameManager.Instance.LevelValues.EndWord;
                _LevelDoneOrFailed.Q<Label>("label_time").text = displayTime;
                var spriteCup = _LevelDoneOrFailed.Q<VisualElement>("sprite_cup");
                spriteCup.style.display = DisplayStyle.Flex;

                if (time <= GameManager.Instance.LevelValues.RunCupTime[0])
                {
                    //Debug.Log("Gold");
                    spriteCup.AddToClassList("hero-cup-level-gold");
                }
                else if (time <= GameManager.Instance.LevelValues.RunCupTime[1])
                {
                    //Debug.Log("Silver");
                    spriteCup.AddToClassList("hero-cup-level-silver");
                }
                else if (time <= GameManager.Instance.LevelValues.RunCupTime[2])
                {
                    //Debug.Log("Bronze");
                    spriteCup.AddToClassList("hero-cup-level-bronze");
                }
                else
                {
                    spriteCup.style.display = DisplayStyle.None;
                }
            }
            else
            {
                _LevelDoneOrFailed.Q<Label>("label_endWord").text = PlayerLocal.Instance.HeroData.Profile.PlayerLanguage == Lang.EN ? "Elapsed time... Try again !" : "Temps écoulé... Essaie encore !";
                _LevelDoneOrFailed.Q<Label>("label_time").text = "M: Menu     R: Retry";
            }

            _Time.style.display = DisplayStyle.None;
            _LevelDoneOrFailed.style.display = DisplayStyle.Flex;
        }

        public void HideElapsedTimeScreen()
        {
            _LevelDoneOrFailed.style.display = DisplayStyle.None;
        }

    }

}
