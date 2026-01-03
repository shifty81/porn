using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VisualNovel.Utils
{
    /// <summary>
    /// Utility functions for visual novel framework
    /// </summary>
    public static class VNUtils
    {
        /// <summary>
        /// Wraps text to fit within a specified width
        /// </summary>
        public static string WrapText(string text, int maxLineLength)
        {
            if (string.IsNullOrEmpty(text) || maxLineLength <= 0)
                return text;

            string[] words = text.Split(' ');
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            int currentLineLength = 0;

            foreach (string word in words)
            {
                if (currentLineLength + word.Length + 1 > maxLineLength)
                {
                    result.Append('\n');
                    currentLineLength = 0;
                }
                else if (currentLineLength > 0)
                {
                    result.Append(' ');
                    currentLineLength++;
                }

                result.Append(word);
                currentLineLength += word.Length;
            }

            return result.ToString();
        }

        /// <summary>
        /// Sanitizes a string to be used as a filename
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            string invalid = new string(System.IO.Path.GetInvalidFileNameChars());
            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), "");
            }
            return fileName;
        }

        /// <summary>
        /// Formats time in seconds to MM:SS format
        /// </summary>
        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        /// <summary>
        /// Generates a unique ID based on timestamp
        /// </summary>
        public static string GenerateUniqueId()
        {
            return System.DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// Interpolates between two colors with easing
        /// </summary>
        public static Color LerpColor(Color a, Color b, float t, AnimationCurve curve = null)
        {
            if (curve != null)
            {
                t = curve.Evaluate(t);
            }
            return Color.Lerp(a, b, t);
        }

        /// <summary>
        /// Clamps a Vector2 within bounds
        /// </summary>
        public static Vector2 ClampVector2(Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(
                Mathf.Clamp(value.x, min.x, max.x),
                Mathf.Clamp(value.y, min.y, max.y)
            );
        }

        /// <summary>
        /// Converts a hex color string to Color
        /// </summary>
        public static Color HexToColor(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length == 6)
                hex += "FF"; // Add alpha if not present

            if (hex.Length != 8)
            {
                Debug.LogError($"Invalid hex color: {hex}");
                return Color.white;
            }

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Converts a Color to hex string
        /// </summary>
        public static string ColorToHex(Color color)
        {
            Color32 c = color;
            return $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
        }

        /// <summary>
        /// Shuffles a list randomly
        /// </summary>
        public static void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        /// <summary>
        /// Gets a random element from an array
        /// </summary>
        public static T GetRandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
                return default(T);

            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Gets a random element from a list
        /// </summary>
        public static T GetRandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);

            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Checks if a point is within screen bounds
        /// </summary>
        public static bool IsPointInScreen(Vector2 point)
        {
            return point.x >= 0 && point.x <= Screen.width &&
                   point.y >= 0 && point.y <= Screen.height;
        }

        /// <summary>
        /// Smoothly damps a float value
        /// </summary>
        public static float SmoothDamp(float current, float target, ref float velocity, float smoothTime)
        {
            return Mathf.SmoothDamp(current, target, ref velocity, smoothTime);
        }

        /// <summary>
        /// Parse a string to an enum value safely
        /// </summary>
        public static T ParseEnum<T>(string value, T defaultValue) where T : struct
        {
            if (System.Enum.TryParse<T>(value, out T result))
            {
                return result;
            }
            return defaultValue;
        }
    }

    /// <summary>
    /// Coroutine helper for MonoBehaviour-less coroutines
    /// </summary>
    public class CoroutineHelper : MonoBehaviour
    {
        private static CoroutineHelper instance;
        public static CoroutineHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("CoroutineHelper");
                    instance = go.AddComponent<CoroutineHelper>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// Extension methods for Unity types
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Sets the alpha of a Color
        /// </summary>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Sets the x component of a Vector2
        /// </summary>
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        /// <summary>
        /// Sets the y component of a Vector2
        /// </summary>
        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        /// <summary>
        /// Sets the z component of a Vector3
        /// </summary>
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}
