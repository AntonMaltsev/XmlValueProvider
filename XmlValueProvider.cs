using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace MVCNet.Utils
{
    public class XmlValueProvider: IValueProvider
{
    private Dictionary<string, object> _values;

    public XmlValueProvider(ControllerContext controllerContext)
    {
        if (controllerContext == null)
        {
            throw new ArgumentNullException("controllerContext");
        }

        _values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        this.GetValuesFromRequest(controllerContext);
    }

    protected void GetValuesFromRequest(ControllerContext controllerContext)
    {
        if ((controllerContext.HttpContext.Request.ContentType ?? String.Empty).Contains("application/xml"))
        {
            using (StreamReader stream = new StreamReader(controllerContext.HttpContext.Request.InputStream))
            {                
                string content = stream.ReadToEnd();
                XDocument xmlInput = XDocument.Parse(content);
                if(xmlInput != null)
                {
                    XElement root = xmlInput.Root;
                    if(root != null && root.HasElements)
                    {
                        IEnumerable<XElement> elements = root.Elements();
                        foreach (var element in elements)
                        {
                            if (element.NodeType == XmlNodeType.Element)
                            { 
                            if (!_values.Keys.Contains(element.Name.ToString()))
                             {
                                 _values.Add(element.Name.ToString(), element.Value);
                             }
                            }
                        }
                    }
                }
            }
        }       
    }

    public bool ContainsPrefix(string prefix)
    {
        return _values.Keys.Contains(prefix.ToLower());
    }

    public ValueProviderResult GetValue(string key)
    {
        object value;
        if (_values.TryGetValue(key.ToLower(), out value))
        {
            return new ValueProviderResult(value, value.ToString(), CultureInfo.InvariantCulture);
        }
        return null;
    }
}

    public class XmlValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new XmlValueProvider(controllerContext);
        }

    }
}
