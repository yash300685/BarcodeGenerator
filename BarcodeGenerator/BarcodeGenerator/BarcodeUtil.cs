using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BarcodeGenerator
{
   public  class BarcodeUtil
    {

        public static bool IsValidGtin(string code)
        {
            if (code != (new Regex("[^0-9]")).Replace(code, ""))
            {
                // is not numeric
                return false;
            }
            // pad with zeros to lengthen to 14 digits
            switch (code.Length)
            {
                case 8:
                    code = "000000" + code;
                    break;
                case 12:
                    code = "00" + code;
                    break;
                case 13:
                    code = "0" + code;
                    break;
                case 14:
                    break;
                default:
                    // wrong number of digits
                    return false;
            }
            // calculate check digit
            int[] a = new int[13];
            a[0] = int.Parse(code[0].ToString()) * 3;
            a[1] = int.Parse(code[1].ToString());
            a[2] = int.Parse(code[2].ToString()) * 3;
            a[3] = int.Parse(code[3].ToString());
            a[4] = int.Parse(code[4].ToString()) * 3;
            a[5] = int.Parse(code[5].ToString());
            a[6] = int.Parse(code[6].ToString()) * 3;
            a[7] = int.Parse(code[7].ToString());
            a[8] = int.Parse(code[8].ToString()) * 3;
            a[9] = int.Parse(code[9].ToString());
            a[10] = int.Parse(code[10].ToString()) * 3;
            a[11] = int.Parse(code[11].ToString());
            a[12] = int.Parse(code[12].ToString()) * 3;
            int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
            int check = (10 - (sum % 10)) % 10;
            // evaluate check digit
            int last = int.Parse(code[13].ToString());
            return check == last;
        }

        public static bool IsNumber(String text)
        {
            int i = 0;
            
            bool result = int.TryParse(text, out i);
            return result;
        }

        public static bool IsValidCode39(String text)
        {
            foreach (char c in text)
            {
                if (!Char.IsUpper(c)&&!Char.IsDigit(c) && c != '_' && c != '.' && c != '$' && c != '/' && c != '+' && c != '%' && !Char.IsWhiteSpace(c))
                    return false;
            }
            return true;
        }

        public static bool IsValidUPC(String text)
        {
            foreach (char c in text)
            {
                if (Char.IsDigit(c) &&  text.Length.Equals(11))
                {
                    return true;
                }
                else if(Char.IsDigit(c) && text.Length.Equals(12) && GetLastUPCCODE(text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
                
        }
        public static bool GetLastUPCCODE(string code)
        {
            string UPCNumber = code.Substring(0, 11);
            long chkDigitOdd = Convert.ToInt64(UPCNumber.Substring(0, 1)) +
            Convert.ToInt64(UPCNumber.Substring(2, 1)) + Convert.ToInt64(UPCNumber.Substring(4, 1))
            + Convert.ToInt64(UPCNumber.Substring(6, 1)) + Convert.ToInt64(UPCNumber.Substring(8,
            1)) + Convert.ToInt64(UPCNumber.Substring(10, 1));
            chkDigitOdd = (3 * chkDigitOdd);
            long chkDigitEven = Convert.ToInt64(UPCNumber.Substring(1, 1)) +
            Convert.ToInt64(UPCNumber.Substring(3, 1)) + Convert.ToInt64(UPCNumber.Substring(5, 1))
            + Convert.ToInt64(UPCNumber.Substring(7, 1)) + Convert.ToInt64(UPCNumber.Substring(9,
            1));
            long chkDigitSubtotal = (300 - (chkDigitEven + chkDigitOdd));
            string chkDigit = chkDigitSubtotal.ToString();
            chkDigit = chkDigit.Substring(chkDigit.Length - 1, 1);
          
            int last = int.Parse(code[11].ToString());
            return int.Parse(chkDigit) == last;
        }

        public static bool IsValidURL(String code)
        {
            if (Uri.IsWellFormedUriString(code, UriKind.Absolute))
            {
                return true;
            }

            return false;
        }

    }
}
