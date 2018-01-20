using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SqlExport
{
    internal static class XmlNodeExtensions
    {
        public static XmlNode WriteStartElement(this XmlNode node, string elementName)
        {
            var element = (node.OwnerDocument ?? node as XmlDocument).CreateElement(elementName);
            node.AppendChild(element);

            return element;
        }

        public static XmlNode WriteElementString(this XmlNode node, string elementName, string value)
        {
            var element = node.OwnerDocument.CreateElement(elementName);
            element.InnerText = value;
            node.AppendChild(element);

            return node;
        }

        public static XmlNode WriteAttributeString(this XmlNode node, string attributeName, string value)
        {
            var attribute = node.OwnerDocument.CreateAttribute(attributeName);
            attribute.Value = value;
            node.Attributes.Append(attribute);

            return node;
        }

        public static XmlNode WriteEndElement(this XmlNode node)
        {
            return node.ParentNode;
        }

        // TODO - Delete
        ////public static void SetValue(this XmlNode parent, string xpath, OptionValue optionValue)
        ////{
        ////    // grab the next node name in the xpath; or return parent if empty
        ////    string[] partsOfXPath = xpath.Trim('/').Split('/');
        ////    string nextNodeInXPath = partsOfXPath.First();
        ////    if (string.IsNullOrEmpty(nextNodeInXPath))
        ////    {
        ////        return;
        ////    }

        ////    // get or create the node from the name
        ////    XmlNode node = parent.SelectSingleNode(nextNodeInXPath);
        ////    if (node == null)
        ////    {
        ////        var doc = (parent.OwnerDocument ?? parent as XmlDocument);
        ////        if (nextNodeInXPath.StartsWith("@"))
        ////        {
        ////            node = parent.Attributes.Append(doc.CreateAttribute(nextNodeInXPath.Substring(1)));
        ////        }
        ////        else
        ////        {
        ////            node = parent.AppendChild(doc.CreateElement(nextNodeInXPath));
        ////        }
        ////    }

        ////    if (partsOfXPath.Length == 1)
        ////    {
        ////        if (node is XmlElement)
        ////        {
        ////            node.InnerText = optionValue.GetValue<object>().ToString();
        ////        }
        ////        else
        ////        {
        ////            node.Value = optionValue.GetValue<object>().ToString();
        ////        }
        ////    }
        ////    else
        ////    {
        ////        // rejoin the remainder of the array as an xpath expression and recurse
        ////        string rest = string.Join("/", partsOfXPath.Skip(1).ToArray());

        ////        SetValue(node, rest, optionValue);
        ////    }
        ////}
    }
}
