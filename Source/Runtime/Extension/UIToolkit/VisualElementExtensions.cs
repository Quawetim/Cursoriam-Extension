using System;
using UnityEngine;
using UnityEngine.UIElements;
using static BadPractice.Cursoriam.Extension.UnityDefaultCursor;

namespace BadPractice.Cursoriam.Extension
{
    public static partial class VisualElementExtensions
    {
        public static T SetEnabledSelf<T>(this T element, bool value)
            where T : VisualElement
        {
            element.enabledSelf = value;
            return element;
        }

        public static T SetName<T>(this T element, string value)
            where T : VisualElement
        {
            element.name = value;
            return element;
        }

        public static T SetTooltip<T>(this T element, string value)
            where T : VisualElement
        {
            element.tooltip = value;
            return element;
        }

        public static TElement SetText<TElement, TValue>(this TElement element, TValue value)
            where TElement : VisualElement
        {
            if (element == null)
                return null;

            var textValue = value?.ToString() ?? string.Empty;

            switch (element)
            {
                case TextElement textElement:
                {
                    textElement.text = textValue;
                    break;
                }
                case TextField textField:
                {
                    textField.value = textValue;
                    break;
                }
                case ProgressBar progressBar:
                {
                    progressBar.title = textValue;
                    break;
                }
                case Foldout foldout:
                {
                    foldout.text = textValue;
                    break;
                }
            }

            return element;
        }

        public static T EnableRichText<T>(this T element, bool value)
            where T : TextElement
        {
            element.enableRichText = value;
            return element;
        }

        public static TField SetValue<TField, TValue>(this TField element, TValue value)
            where TField : BaseField<TValue>
        {
            element.value = value;
            return element;
        }

        public static EnumField SetValue<TEnum>(this EnumField element, TEnum value)
            where TEnum : Enum
        {
            element.value = value;
            return element;
        }

        public static Foldout SetValue(this Foldout element, bool value)
        {
            element.value = value;
            return element;
        }

        public static EnumField WithCallback<TValue>(this EnumField element, Action<TValue> changeCallback)
            where TValue : Enum
        {
            element.RegisterValueChangedCallback(t => changeCallback?.Invoke((TValue)t.newValue));
            return element;
        }

        public static T SetHeight<T>(this T element, float value, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            return element.SetHeight(new Length(value, unit));
        }

        public static T SetHeight<T>(this T element, Length value)
            where T : VisualElement
        {
            element.style.height = new(value);
            return element;
        }

        public static T SetMaxHeight<T>(this T element, float value, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            return element.SetMaxHeight(new Length(value, unit));
        }

        public static T SetMaxHeight<T>(this T element, Length value)
            where T : VisualElement
        {
            element.style.maxHeight = new(value);
            return element;
        }

        public static T SetWidth<T>(this T element, float value, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            return element.SetWidth(new Length(value, unit));
        }

        public static T SetWidth<T>(this T element, Length value)
            where T : VisualElement
        {
            element.style.width = new(value);
            return element;
        }

        public static T SetMaxWidth<T>(this T element, float value, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            return element.SetMaxWidth(new Length(value, unit));
        }

        public static T SetMaxWidth<T>(this T element, Length value)
            where T : VisualElement
        {
            element.style.maxWidth = new(value);
            return element;
        }

        public static T SetFlex<T>(this T element, FlexDirection value)
            where T : VisualElement
        {
            element.style.flexDirection = value;
            return element;
        }

        public static T SetFlexGrow<T>(this T element, float value)
            where T : VisualElement
        {
            element.style.flexGrow = value;
            return element;
        }

        public static T SetFlexShrink<T>(this T element, float value)
            where T : VisualElement
        {
            element.style.flexShrink = value;
            return element;
        }

        public static T SetFlexGrow<T>(this T element, StyleKeyword value)
            where T : VisualElement
        {
            element.style.flexGrow = new(value);
            return element;
        }

        public static T SetJustifyContent<T>(this T element, Justify value)
            where T : VisualElement
        {
            element.style.justifyContent = value;
            return element;
        }

        public static T SetBorderStyle<T>(this T element,
            (float width, Color color)? top = null,
            (float width, Color color)? right = null,
            (float width, Color color)? bottom = null,
            (float width, Color color)? left = null)
            where T : VisualElement
        {
            if (top is not null)
            {
                element.style.borderTopWidth = top.Value.width;
                element.style.borderTopColor = top.Value.color;
            }

            if (right is not null)
            {
                element.style.borderRightWidth = right.Value.width;
                element.style.borderRightColor = right.Value.color;
            }

            if (bottom is not null)
            {
                element.style.borderBottomWidth = bottom.Value.width;
                element.style.borderBottomColor = bottom.Value.color;
            }

            if (left is not null)
            {
                element.style.borderLeftWidth = left.Value.width;
                element.style.borderLeftColor = left.Value.color;
            }

            return element;
        }

        public static T SetBorderStyle<T>(this T element, float width, Color color)
            where T : VisualElement
        {
            var border = (width, color);
            return element.SetBorderStyle(border, border, border, border);
        }

        public static T SetBorderRadius<T>(this T element, float? topLeft = null, float? topRight = null,
            float? bottomLeft = null, float? bottomRight = null)
            where T : VisualElement
        {
            if (topLeft is not null)
            {
                element.style.borderTopLeftRadius = topLeft.Value;
            }

            if (topRight is not null)
            {
                element.style.borderTopRightRadius = topRight.Value;
            }

            if (bottomLeft is not null)
            {
                element.style.borderBottomLeftRadius = bottomLeft.Value;
            }

            if (bottomRight is not null)
            {
                element.style.borderBottomRightRadius = bottomRight.Value;
            }

            return element;
        }

        public static T SetPadding<T>(this T element, float? top = null, float? right = null,
            float? bottom = null, float? left = null)
            where T : VisualElement
        {
            if (top is not null)
            {
                element.style.paddingTop = top.Value;
            }

            if (right is not null)
            {
                element.style.paddingRight = right.Value;
            }

            if (bottom is not null)
            {
                element.style.paddingBottom = bottom.Value;
            }

            if (left is not null)
            {
                element.style.paddingLeft = left.Value;
            }

            return element;
        }

        public static T SetMargin<T>(this T element, float? top = null, float? right = null,
            float? bottom = null, float? left = null)
            where T : VisualElement
        {
            if (top is not null)
            {
                element.style.marginTop = top.Value;
            }

            if (right is not null)
            {
                element.style.marginRight = right.Value;
            }

            if (bottom is not null)
            {
                element.style.marginBottom = bottom.Value;
            }

            if (left is not null)
            {
                element.style.marginLeft = left.Value;
            }

            return element;
        }

        public static T SetColor<T>(this T element, Color value)
            where T : VisualElement
        {
            element.style.color = value;
            return element;
        }

        public static T SetBackColor<T>(this T element, Color value) where T : VisualElement
        {
            element.style.backgroundColor = value;
            return element;
        }

        public static T SetBackColor<T>(this T element, StyleKeyword value) where T : VisualElement
        {
            element.style.backgroundColor = new(value);
            return element;
        }

        public static T SetAlignSelf<T>(this T element, Align value) where T : VisualElement
        {
            element.style.alignSelf = value;
            return element;
        }

        public static T SetAlignContent<T>(this T element, Align value)
            where T : VisualElement
        {
            element.style.alignContent = value;
            return element;
        }

        public static T SetAlignItems<T>(this T element, Align value)
            where T : VisualElement
        {
            element.style.alignItems = value;
            return element;
        }

        public static T SetImageBack<T>(this T element, Sprite value)
            where T : VisualElement
        {
            element.style.backgroundImage = new(value);
            return element;
        }

        public static Button SetAction(this Button element, Action action)
        {
            element.clicked += action;
            return element;
        }

        public static T SetFontStyle<T>(this T element, FontStyle value)
            where T : VisualElement
        {
            element.style.unityFontStyleAndWeight = value;
            return element;
        }

        public static T SetFontSize<T>(this T element, float value, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            return element.SetFontSize(new Length(value, unit));
        }

        public static T SetFontSize<T>(this T element, Length value)
            where T : VisualElement
        {
            element.style.fontSize = new(value);
            return element;
        }

        public static T SetDisplayStyle<T>(this T element, DisplayStyle value)
            where T : VisualElement
        {
            element.style.display = value;
            return element;
        }

        public static TField SetLabel<TField, TValue>(this TField element, string value)
            where TField : BaseField<TValue>
        {
            element.label = value;
            return element;
        }

        public static TextField SetLabel(this TextField element, string value)
        {
            return element.SetLabel<TextField, string>(value);
        }

        public static FloatField SetLabel(this FloatField element, string value)
        {
            return element.SetLabel<FloatField, float>(value);
        }

        public static IntegerField SetLabel(this IntegerField element, string value)
        {
            return element.SetLabel<IntegerField, int>(value);
        }

        public static EnumField SetLabel(this EnumField element, string value)
        {
            return element.SetLabel<EnumField, Enum>(value);
        }

        public static Vector3Field SetLabel(this Vector3Field element, string value)
        {
            return element.SetLabel<Vector3Field, Vector3>(value);
        }

        public static DropdownField SetLabel(this DropdownField element, string value)
        {
            return element.SetLabel<DropdownField, string>(value);
        }

        public static T SetCursor<T>(this T element, StyleCursor value)
            where T : VisualElement
        {
            element.style.cursor = value;
            return element;
        }

        public static T SetCursor<T>(this T element, ECursorType value)
            where T : VisualElement
        {
            element.style.cursor = new(DefaultCursor(value));
            return element;
        }

        public static T SetPosition<T>(this T element, Position value)
            where T : VisualElement
        {
            element.style.position = value;
            return element;
        }
    }
}