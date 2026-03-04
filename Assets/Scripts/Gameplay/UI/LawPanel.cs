using AutofacUnity;
using Siege.Gameplay.Laws;
using Siege.Gameplay.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    public class LawPanel : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset _rowTemplate;

        UIDocument _document;
        VisualElement _root;
        ScrollView _scrollView;

        GameState _state;
        LawDispatcher _lawDispatcher;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;
            _root = root.Q("Overlay");
            _scrollView = root.Q<ScrollView>("ScrollView");
            root.Q<SiegeButton>("CloseBtn").Clicked += Hide;
        }

        void Start()
        {
            _state = Resolver.Resolve<GameState>();
            _lawDispatcher = Resolver.Resolve<LawDispatcher>();
        }

        void Update()
        {
            if (_state == null || _lawDispatcher == null) return;
            if (_root.style.display == DisplayStyle.None) return;

            _scrollView.Clear();

            foreach (var law in _lawDispatcher.AllLaws)
            {
                var row = _rowTemplate.Instantiate();
                row.Q<Label>("NameLabel").text = law.Name;
                row.Q<Label>("DescLabel").text = law.Description;

                if (law.IsEnacted)
                {
                    row.Q("EnactedBadge").style.display = DisplayStyle.Flex;
                    row.Q<SiegeButton>("EnactBtn").style.display = DisplayStyle.None;
                }
                else
                {
                    bool canEnact = _lawDispatcher.CanEnact(law.Id);
                    var enactBtn = row.Q<SiegeButton>("EnactBtn");
                    enactBtn.SetEnabled(canEnact);
                    if (!canEnact) enactBtn.AddToClassList("law-panel__enact-btn--disabled");
                    string lawId = law.Id;
                    enactBtn.Clicked += () => _lawDispatcher.TryEnact(lawId);
                }

                _scrollView.Add(row);
            }
        }

        public bool IsShown => _root.style.display == DisplayStyle.Flex;
        public void Show() => _root.style.display = DisplayStyle.Flex;
        public void Hide() => _root.style.display = DisplayStyle.None;
    }
}
