using System;
using System.Collections.Generic;
using System.Xml;
using EngineeringUnitsCore.Data.Entities;

namespace EngineeringUnitsCore.Data.DbInitializer
{
    public class XmlHandler
    {
        private static readonly Dictionary<string, Entities.DimensionalClass> _dimensionalClasses = new Dictionary<string, Entities.DimensionalClass>();
        private static readonly Dictionary<string, QuantityType> _quantityTypes = new Dictionary<string, QuantityType>();

        public static List<UnitOfMeasure> GetUnits(string path)
        {
            var uomNodes = ReadXmlNodes(path);
            return CreateUnits(uomNodes);
        }

        private static XmlNodeList ReadXmlNodes(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);
            var root = doc.DocumentElement.ChildNodes[1];
            return root.ChildNodes;
        }

        private static List<UnitOfMeasure> CreateUnits(XmlNodeList nodes)
        {
            _dimensionalClasses.Add("", null);
            
            var units = new List<UnitOfMeasure>();

            foreach (XmlNode node in nodes)
            {
                var id = node.Attributes[0].InnerText;
                var annotation = node.Attributes[1].InnerText;
                var name = node.SelectSingleNode("Name").InnerText;
                var qTypes = GetQuantityTypes(node.SelectNodes("QuantityType"));
                var dClass = GetDimensionalClass(node.SelectSingleNode("DimensionalClass"));
                var cNode = node.SelectSingleNode("ConversionToBaseUnit");

                UnitOfMeasure unit;

                if (cNode == null)
                {
                    unit = new UnitOfMeasure(id, annotation, name, _dimensionalClasses[dClass], qTypes);
                }
                else
                {
                    var conversionToBaseUnit = GetConversionToBaseUnit(cNode);
                    unit = new CustomaryUnit(id, annotation, name, _dimensionalClasses[dClass], qTypes,
                        conversionToBaseUnit);
                }

                units.Add(unit);
            }

            return units;
        }

        private static ConversionToBaseUnit GetConversionToBaseUnit(XmlNode cNode)
        {
            var baseUnit = cNode.Attributes[0].InnerText;

            ConversionToBaseUnit conversionToBaseUnit;

            var conversion = cNode.FirstChild;

            switch (conversion.Name)
            {
                case "Factor":
                    var factor = Convert.ToDouble(conversion.InnerText);
                    conversionToBaseUnit = new ConversionToBaseUnit(baseUnit, factor);
                    break;
                case "Fraction":
                    var numString = conversion.SelectSingleNode("Numerator").InnerText;
                    var numerator = Convert.ToDouble(numString);
                    var denomString = conversion.SelectSingleNode("Denominator").InnerText;
                    var denominator = Convert.ToDouble(denomString);
                    conversionToBaseUnit = new ConversionToBaseUnit(baseUnit, numerator, denominator);
                    break;
                default:
                    var a = Convert.ToDouble(conversion.SelectSingleNode("A").InnerText);
                    var b = Convert.ToDouble(conversion.SelectSingleNode("B").InnerText);
                    var c = Convert.ToDouble(conversion.SelectSingleNode("C").InnerText);
                    var d = Convert.ToDouble(conversion.SelectSingleNode("D").InnerText);
                    conversionToBaseUnit = new ConversionToBaseUnit(baseUnit, a, b, c, d);
                    break;
            }
            
            return conversionToBaseUnit;
        }

        private static List<QuantityType> GetQuantityTypes(XmlNodeList qtNodes)
        {
            var quantityTypes = new List<QuantityType>();

            foreach (XmlNode qType in qtNodes)
            {
                var notation = qType.InnerText;

                QuantityType qt;

                if (!_quantityTypes.ContainsKey(notation))
                {
                    qt = new QuantityType(notation);
                    _quantityTypes.Add(notation, qt);
                }
                else
                    qt = _quantityTypes[notation];

                quantityTypes.Add(qt);
            }

            return quantityTypes;
        }

        private static string GetDimensionalClass(XmlNode dNode)
        {
            if (dNode == null)
                return "";

            var notation = dNode.InnerText;

            if (!_dimensionalClasses.ContainsKey(notation))
                _dimensionalClasses.Add(notation, new Entities.DimensionalClass(notation));

            return notation;
        }
    }
}