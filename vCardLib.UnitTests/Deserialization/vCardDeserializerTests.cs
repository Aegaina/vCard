using Microsoft.VisualStudio.TestTools.UnitTesting;
using vCardLib.Deserialization;
using System;
using System.Collections.Generic;
using System.Text;
using vCardLib.Models;
using System.Diagnostics;

namespace vCardLib.Deserialization.Tests
{
    [TestClass()]
    public class vCardDeserializerTests
    {
        [TestMethod()]
        public void CellPhoneTest()
        {
            string filePath = @"D:\Cathy\Documents\通讯\Contacts\通讯录\Thunderbird\Thunderbird.vcf";
            IEnumerable<vCard> contacts = vCardDeserializer.FromFile(filePath);
            Assert.IsNotNull(contacts);
            Assert.IsTrue(contacts.Any());

            int total = 0;
            foreach (vCard contact in contacts)
            {
                Assert.IsNotNull(contact);

                if (!contact.PhoneNumbers.Any())
                {
                    continue;
                }

                StringBuilder msg = new StringBuilder();
                msg.Append(contact.FormattedName);

                int count = 0;
                foreach (TelephoneNumber phoneNumber in contact.PhoneNumbers)
                {
                    Assert.IsNotNull(phoneNumber);

                    if (phoneNumber.Type.HasFlag(Enums.TelephoneNumberType.Cell))
                    {
                        count++;
                        msg.Append(',');
                        msg.Append(phoneNumber.Number);
                    }
                }
                if (count > 0)
                {
                    total++;
                    Trace.WriteLine(msg.ToString());
                }
            }

            Trace.WriteLine(string.Format("共{0}个联系人有手机号", total));
        }
    }
}