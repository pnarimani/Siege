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
            if (_document == null) _document = gameObject.AddComponent<UIDocument>();

            _root = new VisualElement();
            _root.style.position = Position.Absolute;
            _root.style.left = 0;
            _root.style.right = 0;
            _root.style.top = 0;
            _root.style.bottom = 0;
            _root.style.backgroundColor = new Color(0f, 0f, 0f, 0.9f);
            _root.style.alignItems = Align.Center;
            _root.style.justifyContent = Justify.Center;
            _root.style.display = DisplayStyle.None;
            _root.AddToClassList("game-over-screen");

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0.1f, 0.08f, 0.08f, 1f);
            panel.style.paddingTop = 32;
            panel.style.paddingBottom = 32;
            panel.style.paddingLeft = 40;
            panel.style.paddingRight = 40;
            panel.style.maxWidth = 600;
            panel.style.width = new Length(80, LengthUnit.Percent);
            panel.style.alignItems = Align.Center;
            panel.AddToClassList("game-over-screen__panel");

            _reasonLabel = new Label();
            _reasonLabel.style.fontSize = 28;
            _reasonLabel.style.color = new Color(0.9f, 0.3f, 0.3f);
            _reasonLabel.style.marginBottom = 16;
            _reasonLabel.AddToClassList("game-over-screen__reason");
            panel.Add(_reasonLabel);

            _narrativeLabel = new Label();
            _narrativeLabel.style.fontSize = 14;
            _narrativeLabel.style.color = new Color(0.8f, 0.8f, 0.8f);
            _narrativeLabel.style.marginBottom = 20;
            _narrativeLabel.style.whiteSpace = WhiteSpace.Normal;
            _narrativeLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
            _narrativeLabel.AddToClassList("game-over-screen__narrative");
            panel.Add(_narrativeLabel);

            _statsLabel = new Label();
            _statsLabel.style.fontSize = 14;
            _statsLabel.style.color = new Color(0.7f, 0.7f, 0.7f);
            _statsLabel.style.marginBottom = 24;
            _statsLabel.style.whiteSpace = WhiteSpace.Normal;
            _statsLabel.AddToClassList("game-over-screen__stats");
            panel.Add(_statsLabel);

            var menuBtn = new Button { text = "Return to Menu" };
            menuBtn.style.paddingTop = 10;
            menuBtn.style.paddingBottom = 10;
            menuBtn.style.paddingLeft = 24;
            menuBtn.style.paddingRight = 24;
            menuBtn.AddToClassList("game-over-screen__menu-btn");
            menuBtn.clicked += () => SceneManager.LoadScene("Boot");
            panel.Add(menuBtn);

            _root.Add(panel);
            _document.rootVisualElement?.Add(_root);
        }

        void Start()
        {
            var lossSystem = Resolver.Resolve<LossConditionSystem>();
            lossSystem.GameOver += OnGameOver;

            if (_document.rootVisualElement != null && !_document.rootVisualElement.Contains(_root))
                _document.rootVisualElement.Add(_root);
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
