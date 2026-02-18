using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileNameChengerCore
{

    public class CConfig
    {
        public const string DefultSerchPattern = "*.bmp;*.jpg";
        public const string DefultFileNamePattern = "yyyyMMdd-HHmmss-ffff";

        string strFileName = "";
        public string ConfigFileName
        {
            get { return strFileName; }
        }

        string strFileNameFrom = "";
        public string FileNameFrom
        {
            get { return strFileNameFrom; }
            set { strFileNameFrom = value; }
        }

        string strFileNameTo = "";
        public string FileNameTo
        {
            get { return strFileNameTo; }
            set { strFileNameTo = value; }
        }

        string strSerchPattern = DefultSerchPattern;
        public string SerchPattern
        {
            get { return strSerchPattern; }
            set { strSerchPattern = value; }
        }

        string strFileNamePattern = DefultFileNamePattern;
        public string FileNamePattern
        {
            get { return strFileNamePattern; }
            set { strFileNamePattern = value; }
        }

        public CConfig(string in_FileName)
        {
            strFileName = in_FileName;
        }

        string strXPathFileNameTo = "FileNameChenger/Config/FileNameTo";
        string strXPathFileNameFrom = "FileNameChenger/Config/FileNameFrom";
        string strXPathSerchPattern = "FileNameChenger/Config/SerchPattern";
        string strXPathFileNamePattern = "FileNameChenger/Config/FileNamePattern";

        private string getConfigValue(System.Xml.XmlDocument in_Doc, string in_XPath, string in_DefultValue)
        {
            string resultValue = in_DefultValue;
            System.Xml.XmlNode node = in_Doc.SelectSingleNode(in_XPath);
            if (node != null)
            {
                resultValue = node.InnerText.Trim();
            }

            return resultValue;
        }

        public void getConfig()
        {
            if (!File.Exists(strFileName))
            {
                return;
            }

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(strFileName);

            strFileNameTo = getConfigValue(doc, strXPathFileNameTo, "");
            strFileNameFrom = getConfigValue(doc, strXPathFileNameFrom, "");
            strSerchPattern = getConfigValue(doc, strXPathSerchPattern, DefultSerchPattern);
            strFileNamePattern = getConfigValue(doc, strXPathFileNamePattern, DefultFileNamePattern);
        }

        private void setConfigValue(System.Xml.XmlDocument in_Doc, string in_XPath, string in_Value)
        {
            System.Xml.XmlNode node = in_Doc.SelectSingleNode(in_XPath);
            if (node != null)
            {
                node.InnerText = in_Value;
            }
        }
         

        public void putConfig()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            if (!File.Exists(strFileName))
            {
                string strXml =
                "<?xml version=\"1.0\"?>\n"
              + "<FileNameChenger>\n"
              + "   <Config>\n"
              + "       <FileNameFrom>" + strFileNameFrom + "</FileNameFrom>\n"
              + "       <FileNameTo>" + strFileNameTo + "</FileNameTo>\n"
              + "       <SerchPattern>" + DefultSerchPattern + "</SerchPattern>\n"
              + "       <FileNamePattern>" + DefultFileNamePattern + "</FileNamePattern>\n"
              + "   </Config>\n"
              + "</FileNameChenger>\n";

                doc.InnerXml = strXml;
            }
            else
            {
                doc.Load(strFileName);

                setConfigValue(doc, strXPathFileNameTo, strFileNameTo);
                setConfigValue(doc, strXPathFileNameFrom, strFileNameFrom);
            }

            doc.Save(strFileName);

        }
    }
}
