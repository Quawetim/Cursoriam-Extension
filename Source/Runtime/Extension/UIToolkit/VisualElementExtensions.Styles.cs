using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace BadPractice.Cursoriam.Extension
{
    public static partial class VisualElementExtensions
    {
        public static T AsBoxPanel<T>(this T element, string title = null)
            where T : VisualElement
        {
            var borderColor = new Color(0.15f, 0.15f, 0.15f);
            element.SetBorderStyle(1, borderColor);
            element.SetBorderRadius(4, 4, 4, 4);
            element.SetPadding(8, 8, 8, 8);
            element.SetMargin(top: 5, bottom: 5);
            element.SetBackColor(new Color(0.22f, 0.22f, 0.22f));

            if (!string.IsNullOrEmpty(title))
            {
                var titleLabel = new Label(title)
                    .SetFontStyle(FontStyle.Bold)
                    .SetMargin(bottom: 5);

                element.Add(titleLabel);
            }

            return element;
        }

        /// <summary>
        /// Преобразует элемент в <c>InfoBox</c> с поддержкой ссылок.
        /// </summary>
        /// <param name="element">Исходный VisualElement контейнер.</param>
        /// <param name="title">Заголовок панели инфобокса (отображается жирным шрифтом сверху). Может быть <c>null</c>.</param>
        /// <param name="text">Текст сообщения. Поддерживает многострочность (<c>\n</c>) и специальные псевдотеги.</param>
        /// <param name="messageType">Тип сообщения (Info, Warning, Error, None). Влияет на цвет маркера и тип отображаемой иконки.</param>
        /// <remarks>
        /// <para><b>Доступный синтаксис внутри параметра <paramref name="text"/>:</b></para>
        /// <list type="bullet">
        /// <item><description><c>*</c> в начале строки автоматически заменяется на маркер списка (•).</description></item>
        /// <item><description><c>[hotkey:Alt+F4]</c> — хоткей.</description></item>
        /// <item><description><c>[file:Assets/config.json]</c> — кликабельная ссылка на файл.</description></item>
        /// <item><description><c>[code:Assets/Script.cs]</c> — кликабельная ссылка на код, открывается сразу в IDE.</description></item>
        /// <item><description><c>http://...</c> или <c>https://...</c> — URL.</description></item>
        /// </list>
        /// <para><b>Пример использования:</b></para>
        /// <code>
        /// root.AsInfoBox("Заголовок", 
        ///     "* Ссылка на файл [file:Assets/config.json]\n" +
        ///     "* Ссылка на код [code:Assets/Script.cs]\n" +
        ///     "* Хоткей [hotkey:Alt+F4].\n" +,
        ///     "* URL https://google.com",
        ///     HelpBoxMessageType.Info);
        /// </code>
        /// </remarks>
        public static T AsInfoBox<T>(this T element, string title, string text, HelpBoxMessageType messageType = HelpBoxMessageType.Info)
            where T : VisualElement
        {
            var markerDefaultColor = new Color(0.12f, 0.45f, 0.55f);
            var markerWarningColor = new Color(0.95f, 0.6f, 0.1f);
            var markerErrorColor = new Color(0.85f, 0.25f, 0.25f);

            var hotkeyColor = new Color(0.95f, 0.77f, 0.06f).ToHex();
            var webLinkColor = new Color(0.2f, 0.6f, 0.86f).ToHex();
            var pathLinkColor = new Color(0.85f, 0.85f, 0.5f).ToHex();
            var codeLinkColor = new Color(0.24f, 0.78f, 0.69f).ToHex();

            var helpBox = element.AsBoxPanel(title);

            var contentContainer = new VisualElement()
                .SetFlex(FlexDirection.Row)
                .SetJustifyContent(Justify.SpaceBetween)
                .SetPadding(6, 6, 6, 8)
                .SetMargin(top: 4);

            var leftContent = new VisualElement()
                .SetFlex(FlexDirection.Row)
                .SetFlexGrow(1)
                .SetAlignItems(Align.Center);

            var markerColor = messageType switch
            {
                HelpBoxMessageType.Warning => markerWarningColor,
                HelpBoxMessageType.Error => markerErrorColor,
                _ => markerDefaultColor
            };

            var leftMarker = new VisualElement()
                .SetWidth(4)
                .SetMargin(right: 10)
                .SetBackColor(markerColor)
                .SetAlignSelf(Align.Stretch);
            leftContent.Add(leftMarker);

            var textLinesContainer = new VisualElement()
                .SetFlexGrow(1);

            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var hotkeyRegex = new Regex(@"\[hotkey:(.*?)\]", RegexOptions.Compiled);
            var urlRegex = new Regex(@"https?://[^\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var fileRegex = new Regex(@"\[file:(.*?)\]", RegexOptions.Compiled);
            var codeRegex = new Regex(@"\[code:(.*?)\]", RegexOptions.Compiled);

            foreach (var line in lines)
            {
                var processedLine = line.Trim();
                if (string.IsNullOrEmpty(processedLine))
                    continue;

                // list dots
                if (processedLine.StartsWith("*"))
                {
                    processedLine = "•" + processedLine[1..];
                }

                string detectedFilePath = null;
                string detectedCodePath = null;

                // hotkey
                processedLine = hotkeyRegex.Replace(processedLine, m => $"<b><color={hotkeyColor}>[{m.Groups[1].Value}]</color></b>");

                // file
                processedLine = fileRegex.Replace(processedLine, m =>
                {
                    var fullPath = m.Groups[1].Value.Trim();
                    detectedFilePath = fullPath;
                    var fileName = Path.GetFileName(fullPath);
                    return $"<i><size=14><color={pathLinkColor}>{fileName}</color></size></i>";
                });

                // code
                processedLine = codeRegex.Replace(processedLine, m =>
                {
                    var fullPath = m.Groups[1].Value.Trim();
                    detectedCodePath = fullPath;
                    var fileName = Path.GetFileName(fullPath);
                    return $"<b><size=14><color={codeLinkColor}>{fileName}</color></size></b>";
                });

                // web url
                var urlMatch = urlRegex.Match(processedLine);
                if (urlMatch.Success)
                {
                    var url = urlMatch.Value;
                    processedLine = processedLine.Replace(url, $"<a href=\"{url}\"><color={webLinkColor}>{url}</color></a>");
                }

                var lineLabel = new Label(processedLine)
                    .SetMargin(bottom: 3)
                    .EnableRichText(true);

                lineLabel.style.whiteSpace = WhiteSpace.Normal;

                if (!string.IsNullOrEmpty(detectedFilePath) || !string.IsNullOrEmpty(detectedCodePath))
                {
                    lineLabel.SetCursor(UnityDefaultCursor.ECursorType.Link);

                    var cachedFilePath = detectedFilePath;
                    var cachedCodePath = detectedCodePath;

                    lineLabel.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (evt.button != 0)
                            return;

                        evt.StopPropagation();

                        if (!string.IsNullOrEmpty(cachedFilePath))
                        {
                            ExecuteFileClick(cachedFilePath);
                        }
                        else if (!string.IsNullOrEmpty(cachedCodePath))
                        {
                            ExecuteCodeClick(cachedCodePath);
                        }
                    });
                }

                textLinesContainer.Add(lineLabel);
            }
            leftContent.Add(textLinesContainer);

            contentContainer.Add(leftContent);

            if (messageType != HelpBoxMessageType.None)
            {
                var iconElement = new VisualElement()
                    .SetWidth(16f)
                    .SetHeight(16f)
                    .SetFlexShrink(0f)
                    .SetMargin(left: 8f)
                    .SetAlignSelf(Align.FlexStart);

                var iconUssClass = messageType switch
                {
                    HelpBoxMessageType.Info => "unity-help-box__icon--info",
                    HelpBoxMessageType.Warning => "unity-help-box__icon--warning",
                    HelpBoxMessageType.Error => "unity-help-box__icon--error",
                    _ => ""
                };

                iconElement.AddToClassList("unity-help-box__icon");
                if (!string.IsNullOrEmpty(iconUssClass))
                {
                    iconElement.AddToClassList(iconUssClass);
                }

                contentContainer.Add(iconElement);
            }

            helpBox.Add(contentContainer);

            return helpBox;
        }

        public static T AsIndicator<T>(this T element)
            where T : VisualElement
        {
            element
                .SetWidth(14)
                .SetHeight(14)
                .SetBorderStyle(1, Color.black)
                .SetBorderRadius(7, 7, 7, 7)
                .SetMargin(left: 8)
                .SetCursor(new StyleCursor(StyleKeyword.Auto));

            return element;
        }

        public static Button AsCross(this Button element, Color color, string tooltip = null)
        {
            element
                .SetText("X")
                .SetBackColor(color)
                .SetColor(Color.white)
                .SetHeight(18)
                .SetWidth(20)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(10);

            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                element.SetTooltip(tooltip);
            }

            return element;
        }

        public static Button AsFocus(this Button element, Color color, string tooltip = null)
        {
            element
                .SetText("Фокус")
                .SetBackColor(color)
                .SetColor(Color.white)
                .SetHeight(20)
                .SetWidth(50)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(10)
                .SetMargin(right: 4);

            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                element.SetTooltip(tooltip);
            }

            return element;
        }

        private static void ExecuteFileClick(string path)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (!asset)
                    return;

                UnityEditor.Selection.activeObject = asset;
                UnityEditor.EditorGUIUtility.PingObject(asset);
            };
#endif
        }

        private static void ExecuteCodeClick(string path)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (!asset)
                {
                    var className = Path.GetFileNameWithoutExtension(path);
                    var guids = UnityEditor.AssetDatabase.FindAssets($"{className} t:MonoScript");
                    if (guids.Length > 0)
                    {
                        var scriptPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scriptPath);
                    }
                }

                if (!asset)
                    return;

                if (UnityEditor.AssetDatabase.OpenAsset(asset))
                    return;

                UnityEditor.Selection.activeObject = asset;
                UnityEditor.EditorGUIUtility.PingObject(asset);
            };
        }
#endif
    }
}