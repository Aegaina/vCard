using System.Text;
using vCardLib.Deserialization;
using vCardLib.Models;

namespace Aegaina.vCardApp.Core
{
    public class ContactsComparator
    {
        private readonly ContactsFile file1;
        private readonly ContactsFile file2;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="file1">要比较的第一个vCard文件</param>
        /// <param name="file2">要比较的第二个vCard文件</param>
        public ContactsComparator(string file1, string file2)
        {
            this.file1 = new ContactsFile(file1);
            this.file2 = new ContactsFile(file2);
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <returns>结果</returns>
        public string Compare()
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("-------------------------------");
            Compare(file1, file2, msg);
            msg.AppendLine("-------------------------------");
            Compare(file2, file1, msg);
            msg.AppendLine("-------------------------------");
            return msg.ToString();
        }

        /// <summary>
        /// 按指定方向比较
        /// </summary>
        /// <param name="src">源文件</param>
        /// <param name="target">目标文件</param>
        /// <param name="msg">消息容器</param>
        private void Compare(ContactsFile srcFile, ContactsFile targetFile, StringBuilder msg)
        {
            if (srcFile == null)
            {
                throw new ArgumentNullException(nameof(srcFile));
            }
            if (targetFile == null)
            {
                throw new ArgumentNullException(nameof(targetFile));
            }
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            foreach (KeyValuePair<string, vCard> item in srcFile.Index)
            {
                if (!targetFile.Index.ContainsKey(item.Key))
                {
                    msg.Append(srcFile.Name).Append(" 中的联系人 ").Append(item.Key).Append(" 在 ").Append(targetFile.Name).AppendLine(" 中不存在！");
                }
            }
        }
    }
}