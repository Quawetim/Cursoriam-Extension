using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BadPractice.Cursoriam.Extension
{
    public static partial class VisualElementExtensions
    {
        public static DropdownField Setup(this DropdownField element, string label, List<string> values, int index, Action<string> onChange = null)
        {
            element.label = label;
            element.choices = values;
            element.index = index;

            if (onChange != null)
            {
                element.RegisterValueChangedCallback(evt => onChange.Invoke(evt.newValue));
            }

            return element;
        }

        public static Slider Setup(this Slider element, string label, float min, float max, float value, bool showInput = true)
        {
            element.label = label;
            element.lowValue = min;
            element.highValue = max;
            element.value = value;
            element.showInputField = showInput;
            element.SetFlexGrow(1);
            return element;
        }

        public static SliderInt Setup(this SliderInt element, string label, int min, int max, int value, bool showInput = true)
        {
            element.label = label;
            element.lowValue = min;
            element.highValue = max;
            element.value = value;
            element.showInputField = showInput;
            element.SetFlexGrow(1);
            return element;
        }

        public static HelpBox Setup(this HelpBox element, string text, HelpBoxMessageType messageType = HelpBoxMessageType.Info)
        {
            element.text = text;
            element.messageType = messageType;
            return element;
        }
    }
}