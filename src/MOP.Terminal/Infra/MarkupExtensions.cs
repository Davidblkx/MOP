using Spectre.Console;
using System.Collections.Generic;

namespace MOP.Terminal.Infra
{
    public static class MarkupExtensions
    {
        public static string Color(this string txt, Color c)
        {
            return txt.SetMarkup(c.ToString());
        }

        public static string Success(this string txt)
        {
            return txt.Color(Spectre.Console.Color.Green1);
        }

        public static string Error(this string txt)
        {
            return txt.Color(Spectre.Console.Color.Red1);
        }

        public static string Warn(this string txt)
        {
            return txt.Color(Spectre.Console.Color.Yellow1);
        }

        public static string SetMarkup(this string txt, string markup)
        {
            var markupParts = new List<string> { markup };
            if (txt.GetMarkup() is string str)
            {
                markupParts.Add(str);
                txt = txt.Replace($"[{str}]", "");
            }
            var start = $"[{string.Join(' ', markupParts)}]";
            return $"{start}{txt}[/]";
        }

        public static string? GetMarkup(this string txt)
        {
            if (txt.IndexOf("[") == 0)
            {
                var end = txt.IndexOf("]");
                if (end <= 1) return null;
                return txt.Substring(1, end - 1);
            }

            return null;
        }
    }
}
