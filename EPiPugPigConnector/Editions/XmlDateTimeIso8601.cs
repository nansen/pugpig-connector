using System;

namespace EPiPugPigConnector.Editions
{
    public class XmlDateTimeIso8601
    {
        public DateTime DateTimeValue { get; set; }
        private string stringValue;

        public string StringValue
        {
            get
            {
                // Should be ISO 8601 datetime http://tools.ietf.org/html/rfc3339
                // Such as 2011-08-08T15:02:28+00:00
                stringValue = XmlHelper.GetDateTimeXmlFormatted(DateTimeValue);
                return stringValue;
            }
            set { stringValue = value; }
        }
        

        public XmlDateTimeIso8601(DateTime dateTimeValue)
        {
            DateTimeValue = dateTimeValue;
        }
    }
}