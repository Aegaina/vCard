using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegaina.vCardApp.Core.Tests
{
    [TestClass()]
    public class ContactsComparatorTests
    {
        [TestMethod()]
        public void CompareTest()
        {
            string file1 = @"D:\Cathy\Documents\通讯\Contacts\通讯录\Thunderbird\Thunderbird.vcf";
            string file2 = @"D:\Cathy\Documents\通讯\Contacts\通讯录\iCloud\iCloud vCard 20250803.vcf";
            ContactsComparator comparator = new ContactsComparator(file1, file2);
            string msg = comparator.Compare();
            Trace.WriteLine(msg);
        }
    }
}