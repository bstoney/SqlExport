namespace SqlExport.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the TextHelper class.
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// Converts Visual Basic code to plain text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A string.</returns>
        public static string VbToPlainText(string text)
        {
            var unformattedText = new StringBuilder(text);

            var isInString = false;
            var i = 0;
            while (i < unformattedText.Length)
            {
                var currentChar = unformattedText[i];
                var nextChar = i + 1 < unformattedText.Length ? (char?)unformattedText[i + 1] : null;
                if (isInString)
                {
                    if (currentChar == '"' && nextChar == '"')
                    {
                        unformattedText.Remove(i, 1);
                    }
                    else if (currentChar == '"')
                    {
                        isInString = false;
                        unformattedText.Remove(i, 1);
                        continue;
                    }
                }
                else
                {
                    switch (currentChar)
                    {
                        case '"':
                            isInString = true;
                            unformattedText.Remove(i, 1);
                            continue;
                        case '&':
                        case '_':
                            unformattedText.Remove(i, 1);
                            continue;
                    }
                }

                i++;
            }

            ////text = Regex.Replace(text, "(\" & _\\s*\")", "", RegexOptions.IgnoreCase);
            ////text = Regex.Replace(text, "(\" & vbnewline & _\\s*\")|(vbnewline)", "\r\n", RegexOptions.IgnoreCase);
            ////text = Regex.Replace(text, "\"([^\"]|$)", "${1}", RegexOptions.IgnoreCase);
            return unformattedText.ToString();
        }

        /// <summary>
        /// Converts C# code to plain text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A string.</returns>
        public static string CsharpToPlainText(string text)
        {
            // TODO Regex re;
            ////re = new Regex( "(\"\\s*\\+\\s*\")", RegexOptions.IgnoreCase );
            ////strData = re.Replace( strData, "" );
            ////re = new Regex( "([^\\\\]|^)\"", RegexOptions.IgnoreCase );
            ////strData = re.Replace( strData, "${1}" );
            ////strData = strData.Replace( "\\\"", "\"" );
            ////strData = strData.Replace( "\\n", "\r\n" );        
            return text;
        }
    }
}
