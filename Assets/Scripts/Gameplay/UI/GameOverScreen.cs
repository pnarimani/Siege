using AutofacUnity;
using Siege.Gameplay.LossConditions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        UIDocument _document;
        VisualElement _root;
        Label _reasonLabel;
        Label _narrativeLabel;
        Label _statsLabel;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _reasonLabel = root.Q<Label>("ReasonLabel");
            _narrativeLabel = root.Q<Label>("NarrativeLabel");
            _statsLabel = root.Q<Label>("StatsLabel");
            root.Q<Button>("MenuBtn").clicked += () => SceneManager.LoadScene("Boot");
        }

        void Start()
        {
            var lossSystem = Resolver.Resolve<LossConditionSystem>();
            lossSystem.GameOver += OnGameOver;
        }

        void OnGameOver(GameEndState endState)
        {
            _reasonLabel.text = FormatReason(endState.Reason);
            _narrativeLabel.text = endState.NarrativeText ?? "";
            _statsLabel.text =
                $"Days Survived: {endState.DaysSurvived}\n" +
                $"Population Remaining: {endState.PopulationRemaining}\n" +
                $"Total Deaths: {endState.TotalDeaths}\n" +
                $"Laws Enacted: {endState.LawsEnacted}\n" +
                $"Zones Lost: {endState.ZonesLost}";

            Show();
        }

        static string FormatReason(GameOverReason reason) => reason switch
        {
            GameOverReason.KeepBreached => "The Keep Has Fallen",
            GameOverReason.CouncilRevolt => "The Council Has Revolted",
            GameOverReason.TotalCollapse => "Total Collapse",
            GameOverReason.Victory => "You Endured",
            _ => "Game Over"
        };

        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
