using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "Your XML URL";
        public static string xmlErrorURL = "Your Error XML URL";
        public static string xsdURL = "Your XSD URL";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
            StringBuilder errorMessages = new StringBuilder();
            try {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                string xsdContent = DownloadContent(xsdUrl);
                using (StringReader xsdReader = new StringReader(xsdContent)) {
                    schemaSet.Add(null, XmlReader.Create(xsdReader));
                }
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.ValidationEventHandler += (sender, e) => {
                    errorMessages.AppendLine(e.Message);
                };
                string xmlContent = DownloadContent(xmlUrl);
                using (StringReader xmlReader = new StringReader(xmlContent))
                using (XmlReader reader = XmlReader.Create(xmlReader, settings)) {
                    while (reader.Read()) {}
                }
            } catch (Exception e) {
                return e.Message;
            }

            if (errorMessages.Length > 0) {
                return errorMessages.ToString();
            } else {
                return "No Error";
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            try {
                string xmlContent = DownloadContent(xmlUrl);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                return jsonText;
            } catch (Exception e) {
                return e.Message;
            }
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}