using System.Text;

namespace NotionRecurringTasksCopier
{
    internal static class Utils
    {
        public static string SnakeToPascal(this string s)
        {
            if (s == null)
            {
                return null;
            }

            string[] words = s.Split('_');

            var sb = new StringBuilder();
            foreach (string word in words)
            {
                sb.Append(word.Substring(0, 1).ToUpper() + word.Substring(1));
            }

            return sb.ToString();
        }
    }
}
