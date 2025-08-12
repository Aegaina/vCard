using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vCardLib.Deserialization;
using vCardLib.Models;

namespace Aegaina.vCardApp.Core
{
    /// <summary>
    /// 联系人文件
    /// </summary>
    public class ContactsFile
    {
        private Dictionary<string, vCard> index = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">路径</param>
        public ContactsFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Contacts = vCardDeserializer.FromFile(path);
            Path = path;
            Name = System.IO.Path.GetFileName(Path);

            if (Contacts == null || !Contacts.Any())
            {
                throw new ArgumentException(string.Format("未能从 {0} 中解析出联系人", Name));
            }
        }

        #region 属性

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public IEnumerable<vCard> Contacts { get; private set; }

        /// <summary>
        /// 索引
        /// </summary>
        public IReadOnlyDictionary<string, vCard> Index
        {
            get
            {
                lock (this)
                {
                    if (index == null)
                    {
                        index = new Dictionary<string, vCard>();
                        foreach (vCard contact in Contacts)
                        {
                            if (contact == null)
                            {
                                continue;
                            }

                            string contactName = contact.GetRevisedName();
                            if (!index.ContainsKey(contactName))
                            {
                                index.Add(contactName, contact);
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("{0}中的联系人 {1} 存在重名", Name, contactName));
                            }
                        }
                    }
                    return index;
                }
            }
        }

        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }
    }
}