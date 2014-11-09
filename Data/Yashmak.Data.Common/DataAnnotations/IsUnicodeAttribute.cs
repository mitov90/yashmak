namespace Yashmak.Data.Common.DataAnnotations
{
    using System;

    public class IsUnicodeAttribute : Attribute
    {
        private readonly bool isUnicode;

        public IsUnicodeAttribute(bool isUnicode)
        {
            this.isUnicode = isUnicode;
        }

        public bool IsUnicode
        {
            get
            {
                return this.isUnicode;
            }
        }
    }
}
