using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegaina.Common;

namespace vCardLib.Models
{
    /// <summary>
    /// vCard扩展
    /// </summary>
    public static class vCardExt
    {
        /// <summary>
        /// 获取修正后的名称
        /// </summary>
        /// <param name="card">名片</param>
        public static string GetRevisedName(this vCard card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (!string.IsNullOrWhiteSpace(card.FormattedName))
            {
                if (card.FormattedName.Contains(' ') && !card.FormattedName.IsLetterOrWhitespace())
                {
                    return card.FormattedName.Replace(" ", string.Empty);
                }
                else
                {
                    return card.FormattedName;
                }
            }
            else
            {
                string familyName = card.Name.Value.FamilyName;
                string givenName = card.Name.Value.GivenName;

                bool? letterOnly = null;
                if (!string.IsNullOrWhiteSpace(familyName))
                {
                    letterOnly = familyName.IsAsciiLetter();
                }
                if (!string.IsNullOrWhiteSpace(givenName))
                {
                    if (letterOnly == null)
                    {
                        letterOnly = givenName.IsAsciiLetter();
                    }
                    else if (!givenName.IsAsciiLetter())
                    {
                        letterOnly = false;
                    }
                }

                if (letterOnly != null)
                {
                    if (letterOnly.Value)
                    {
                        return string.Format("{0} {1}", givenName, familyName).Trim();
                    }
                    else
                    {
                        return string.Format("{0}{1}", familyName, givenName);
                    }
                }
                else if (card.Organization != null && !string.IsNullOrWhiteSpace(card.Organization.Value.Name))
                {
                    return card.Organization.Value.Name;
                }
                else
                {
                    throw new ArgumentException("指定的名片不包含任何名称");
                }
            }
        }

        /// <summary>
        /// 检查指定的字符串是否只包含字母或空格
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        public static bool IsLetterOrWhitespace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Trim();
            foreach (char c in str)
            {
                if (!AsciiChar.IsLetter(c) && !Char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}