﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls.Primitives;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Catalog;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace DataAssistant
{
    class gridTooltipCreator : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            XmlElement elem = value as XmlElement;
            string tip = elem.InnerText.Trim();
            if (elem != null)
            {
                XmlDocument xmlDoc = DataAssistant.Dockpane1View.getXmlDocument();
                string xpath = "";
                if(elem.Name.ToString().StartsWith("Source"))
                    xpath = "//SourceField[@Name='" + elem.InnerText + "']";
                else
                    xpath = "//TargetField[@Name='" + elem.InnerText + "']";
                XmlNode node =  xmlDoc.SelectSingleNode(xpath);
                try
                {
                    // construct a label string from the Source or Target Field information in the xml file
                    string nm = node.Attributes.GetNamedItem("Name").InnerText;
                    if (elem.InnerText != DataAssistant.Dockpane1View.getNoneFieldName())
                    {
                        string alias = getValue(node, "AliasName") + ", ";
                        string typ = getValue(node, "Type");
                        string len = getValue(node, "Length");
                        if(len != "")
                            len = "(" + len + ")";
                        tip = alias + " " + typ + len;
                        return tip.Trim();
                    }
                    else
                        return tip;
                }
                catch
                { 
                    return tip;
                }
            }
            else
                return tip;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }
        private string getValue(XmlNode node,string attribute)
        {
            string val = "";
            if (node.Attributes.GetNamedItem(attribute) != null)
            {
                try { val = node.Attributes.GetNamedItem(attribute).InnerText; }
                catch { }
            }
            return val;
        }
    }
}
