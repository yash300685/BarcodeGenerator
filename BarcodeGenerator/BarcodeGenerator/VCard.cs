using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeGenerator
{
    class VCard
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string JobTitle { get; set; }

        public string StreetAddress { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string HomePage { get; set; }

       

        public override string ToString()

        {

            var builder = new StringBuilder();

            builder.AppendLine("BEGIN:VCARD");

            builder.AppendLine("VERSION:2.1");

            // Name

            builder.AppendLine("N:" + LastName + ";" + FirstName);

            // Full name

            builder.AppendLine("FN:" + FirstName + " " + LastName);

            // Address

            builder.Append("ADR;HOME;PREF:;;");

            builder.Append(StreetAddress + ";");

            builder.Append(City + ";;");

            builder.Append(Zip + ";");

            builder.AppendLine(CountryName);

            // Other data

            builder.AppendLine("ORG:" + Organization);

            builder.AppendLine("TITLE:" + JobTitle);

            builder.AppendLine("TEL;HOME;VOICE:" + Phone);

            builder.AppendLine("TEL;CELL;VOICE:" + Mobile);

            builder.AppendLine("URL:" + HomePage);

            builder.AppendLine("EMAIL;type=INTERNET;type=WORK;type=pref:" + Email);

            builder.AppendLine("END:VCARD");

            return builder.ToString();

        }
    }
}
