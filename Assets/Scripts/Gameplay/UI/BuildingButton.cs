using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Siege.Gameplay.UI
{
    [UxmlElement]
    public partial class BuildingButton : VisualElement
    {
        const string 
            ClassName = "building-button",
            LabelClassName = "building-button__label",
            IconClassName = "building-button__icon",
            IconBackgroundClassName = "building-button__icon-background";

        readonly Image _icon;
        readonly Label _label;

        [CreateProperty]
        [UxmlAttribute("icon")]
        public Texture Icon
        {
            get => _icon.image;
            set => _icon.image = value;
        }

        [CreateProperty]
        [UxmlAttribute("label")]
        public string Label
        {
            get => _label.text;
            set => _label.text = value;
        }

        public BuildingButton()
        {
            var iconBg = new VisualElement();
            _icon = new Image();
            _label = new Label();

            AddToClassList(ClassName);
            _icon.AddToClassList(IconClassName);
            _label.AddToClassList(LabelClassName);
            iconBg.AddToClassList(IconBackgroundClassName);

            iconBg.Add(_icon);
            Add(iconBg);
            Add(_label);
        }
    }
}