using UnityEngine;

namespace BadPractice.Cursoriam.Extension
{
    public static partial class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            var newColor = color;
            newColor.a = alpha;
            return newColor;
        }

        public static Color MultiplyAlpha(this Color color, float alpha)
        {
            var newColor = color;
            newColor.a *= alpha;
            return newColor;
        }

        public static string ToHex(this Color color, bool withAlpha = false)
        {
            Color32 c32 = color;
            var hex = $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}";

            if (withAlpha)
            {
                hex = $"{hex}{c32.a:X2}";
            }

            return hex;
        }
    }
}