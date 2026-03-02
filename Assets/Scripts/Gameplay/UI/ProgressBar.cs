using FastSpring;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class ProgressBar : VisualElement
    {
        public enum LabelMode { Prefix, OnBar }

        [UxmlAttribute, CreateProperty]
        public LabelMode LabelPlacement
        {
            get => _labelMode;
            set { _labelMode = value; UpdateLabelVisibility(); }
        }

        [UxmlAttribute(name: "label"), CreateProperty]
        public string Label
        {
            get => _prefixLabel.text;
            set { _prefixLabel.text = value; _onBarLabel.text = value; }
        }

        [UxmlAttribute(name: "icon"), CreateProperty]
        public Sprite Icon
        {
            get => _icon.sprite;
            set
            {
                _icon.sprite = value;
                _icon.style.display = value != null ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        [UxmlAttribute(name: "increase-delay")] public float IncreaseDelay { get; set; } = 0.5f;
        [UxmlAttribute(name: "decrease-delay")] public float DecreaseDelay { get; set; } = 0.5f;

        [UxmlObjectReference("fill-spring")]
        public SpringDefinition FillSpring { get; set; } = new() { Frequency = 12f, DampingRatio = 0.8f };

        [UxmlObjectReference("trail-spring")]
        public SpringDefinition TrailSpring { get; set; } = new() { Frequency = 8f, DampingRatio = 0.9f };

        readonly VisualElement _track, _decreaseFill, _increaseFill, _foregroundFill;
        readonly TextElement _prefixLabel, _onBarLabel;
        readonly Image _icon;

        readonly Spring<float> _foreground = new();
        readonly Spring<float> _increase = new();
        readonly Spring<float> _decrease = new();

        float _updateTimer;
        float _targetFill;
        LabelMode _labelMode;

        public ProgressBar()
        {
            AddToClassList("progress-bar");
            pickingMode = PickingMode.Ignore;

            _prefixLabel = new TextElement { pickingMode = PickingMode.Ignore };
            _prefixLabel.AddToClassList("progress-bar__prefix-label");
            Add(_prefixLabel);

            _track = new VisualElement { pickingMode = PickingMode.Ignore };
            _track.AddToClassList("progress-bar__track");
            Add(_track);

            _decreaseFill = new VisualElement { pickingMode = PickingMode.Ignore };
            _decreaseFill.AddToClassList("progress-bar__decrease");
            SetAbsoluteFill(_decreaseFill);
            _track.Add(_decreaseFill);

            _increaseFill = new VisualElement { pickingMode = PickingMode.Ignore };
            _increaseFill.AddToClassList("progress-bar__increase");
            SetAbsoluteFill(_increaseFill);
            _track.Add(_increaseFill);

            _foregroundFill = new VisualElement { pickingMode = PickingMode.Ignore };
            _foregroundFill.AddToClassList("progress-bar__fill");
            SetAbsoluteFill(_foregroundFill);
            _track.Add(_foregroundFill);

            var barContent = new VisualElement { pickingMode = PickingMode.Ignore };
            barContent.AddToClassList("progress-bar__bar-content");
            barContent.style.position = Position.Absolute;
            barContent.style.top = barContent.style.bottom = barContent.style.left = barContent.style.right = 0;
            barContent.style.flexDirection = FlexDirection.Row;
            barContent.style.alignItems = Align.Center;
            _track.Add(barContent);

            _icon = new Image { pickingMode = PickingMode.Ignore };
            _icon.AddToClassList("progress-bar__icon");
            _icon.style.display = DisplayStyle.None;
            barContent.Add(_icon);

            _onBarLabel = new TextElement { pickingMode = PickingMode.Ignore };
            _onBarLabel.AddToClassList("progress-bar__label");
            barContent.Add(_onBarLabel);

            UpdateLabelVisibility();

            RegisterCallback<AttachToPanelEvent>(OnAttach);
            RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        void UpdateLabelVisibility()
        {
            bool isPrefix = _labelMode == LabelMode.Prefix;
            if (_prefixLabel != null) _prefixLabel.style.display = isPrefix ? DisplayStyle.Flex : DisplayStyle.None;
            if (_onBarLabel != null) _onBarLabel.style.display = isPrefix ? DisplayStyle.None : DisplayStyle.Flex;
        }

        void OnAttach(AttachToPanelEvent _)
        {
            InitSpring(_foreground, FillSpring);
            InitSpring(_increase, FillSpring);
            InitSpring(_decrease, TrailSpring);
            schedule.Execute(UpdateVisuals).Every(0);
            FixedUpdateRunner.Add(FixedUpdate);
        }

        void OnDetach(DetachFromPanelEvent _)
        {
            FixedUpdateRunner.Remove(FixedUpdate);
        }

        static void InitSpring(Spring<float> spring, SpringDefinition def)
        {
            if (def == null) return;
            spring.UpdateDefinition(def.Frequency, def.DampingRatio);
            spring.Interpolation = def.Interpolation;
        }

        public void Update01(float normalized)
        {
            if (Mathf.Approximately(_targetFill, normalized))
                return;

            bool decreasing = _foreground.Current > normalized;
            _targetFill = normalized;

            if (decreasing)
            {
                _foreground.MoveInstantly(normalized);
                _updateTimer = DecreaseDelay;
            }
            else
            {
                _increase.MoveInstantly(normalized);
                _updateTimer = IncreaseDelay;
            }
        }

        public void Set01(float normalized)
        {
            _foreground.MoveInstantly(normalized);
            _increase.MoveInstantly(normalized);
            _decrease.MoveInstantly(normalized);
            _targetFill = normalized;
        }

        void UpdateVisuals()
        {
            if (_updateTimer > 0)
            {
                _updateTimer -= Time.deltaTime;
                if (_updateTimer <= 0)
                {
                    _foreground.Target = _targetFill;
                    _increase.Target = _targetFill;
                    _decrease.Target = _targetFill;
                }
            }

            _foreground.Interpolate();
            _increase.Interpolate();
            _decrease.Interpolate();

            SetFillWidth(_foregroundFill, _foreground.InterpolatedCurrent);
            SetFillWidth(_increaseFill, _increase.InterpolatedCurrent);
            SetFillWidth(_decreaseFill, _decrease.InterpolatedCurrent);
        }

        void FixedUpdate()
        {
            _foreground.Update();
            _increase.Update();
            _decrease.Update();
        }

        static void SetAbsoluteFill(VisualElement el)
        {
            el.style.position = Position.Absolute;
            el.style.top = el.style.bottom = el.style.left = 0;
        }

        static void SetFillWidth(VisualElement el, float normalized)
        {
            el.style.width = new StyleLength(new Length(normalized * 100f, LengthUnit.Percent));
        }
    }
}
