using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives
{
    [XmlRoot("path")]
    public class SvgPath : SvgColoredItem, ISvgItem
    {
        [XmlAttribute("d")]
        public string PathData
        {
            get => string.Join(" ", PathPartList);
            set
            {
                PathPartList.Clear();
                var raw = value;
                var matches = Regex.Matches(raw, SvgPathPart.PartPattern);
                foreach (Match m in matches)
                {
                    var parts = m.Value.Split(' ');
                    var type = parts[0];
                    var coordinates = parts[1].Split(',');
                    if (string.IsNullOrEmpty(coordinates[0])) coordinates = null;
                    PathPartList.Add(new SvgPathPart()
                    {
                        Parameters = coordinates?.Select(s => Convert.ToDouble(s, CultureInfo.InvariantCulture)).ToList(),
                        SvgPartType = (SvgPartType)Convert.ToChar(type)
                    });
                }
            }
        }

        /// <summary>
        /// backing field for PathData
        /// </summary>
        [XmlIgnore]
        public readonly List<SvgPathPart> PathPartList = new List<SvgPathPart>();
    }
}
