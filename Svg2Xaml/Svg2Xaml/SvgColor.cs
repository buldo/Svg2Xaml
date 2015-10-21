////////////////////////////////////////////////////////////////////////////////
//
//  SvgColor.cs - This file is part of Svg2Xaml.
//
//    Copyright (C) 2009 Boris Richter <himself@boris-richter.net>
//
//  --------------------------------------------------------------------------
//
//  Svg2Xaml is free software: you can redistribute it and/or modify it under 
//  the terms of the GNU Lesser General Public License as published by the 
//  Free Software Foundation, either version 3 of the License, or (at your 
//  option) any later version.
//
//  Svg2Xaml is distributed in the hope that it will be useful, but WITHOUT 
//  ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//  FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public 
//  License for more details.
//  
//  You should have received a copy of the GNU Lesser General Public License 
//  along with Svg2Xaml. If not, see <http://www.gnu.org/licenses/>.
//
//  --------------------------------------------------------------------------
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Windows.Media;

namespace Svg2Xaml
{
    /// <summary>
    ///   Represents an RGB color.
    /// </summary>
    internal class SvgColor
    {
        public SvgColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public byte Red { get; }
        
        public byte Green { get; }
        
        public byte Blue { get; }
        
        public Color ToColor()
        {
            return Color.FromArgb(0xFF, Red, Green, Blue);
        }
        
        public static SvgColor Parse(string value)
        {
            if (value.StartsWith("#"))
            {
                string color = value.Substring(1).Trim();
                if (color.Length == 3)
                {
                    byte r = byte.Parse(string.Format("{0}{0}", color[0]), NumberStyles.HexNumber);
                    byte g = byte.Parse(string.Format("{0}{0}", color[1]), NumberStyles.HexNumber);
                    byte b = byte.Parse(string.Format("{0}{0}", color[2]), NumberStyles.HexNumber);
                    return new SvgColor(r, g, b);
                }

                if (color.Length == 6)
                {
                    byte r = byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                    byte g = byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                    byte b = byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
                    return new SvgColor(r, g, b);
                }
            }

            if (value.StartsWith("rgb"))
            {
                string color = value.Substring(3).Trim();
                if (color.StartsWith("(") && color.EndsWith(")"))
                {
                    color = color.Substring(1, color.Length - 2).Trim();

                    string[] components = color.Split(',');
                    if (components.Length == 3)
                    {
                        byte r, g, b;

                        components[0] = components[0].Trim();
                        if (components[0].EndsWith("%"))
                        {
                            components[0] = components[0].Substring(0, components[0].Length - 1).Trim();
                            r = (byte)(2.55 * double.Parse(components[0], CultureInfo.InvariantCulture.NumberFormat));
                        }
                        else
                        {
                            r = byte.Parse(components[0]);
                        }

                        components[1] = components[1].Trim();
                        if (components[1].EndsWith("%"))
                        {
                            components[1] = components[1].Substring(0, components[1].Length - 1).Trim();
                            g = (byte)(2.55 * double.Parse(components[1], CultureInfo.InvariantCulture.NumberFormat));
                        }
                        else
                        {
                            g = byte.Parse(components[1]);
                        }

                        components[2] = components[1].Trim();
                        if (components[2].EndsWith("%"))
                        {
                            components[2] = components[2].Substring(0, components[2].Length - 1).Trim();
                            b = (byte)(2.55 * double.Parse(components[2], CultureInfo.InvariantCulture.NumberFormat));
                        }
                        else
                        {
                            b = byte.Parse(components[2]);
                        }

                        return new SvgColor(r, g, b);
                    }
                }
            }

            if (value == "none")
                return null;


            switch (value)
            {
                case "black":
                    return new SvgColor(0, 0, 0);
                case "green":
                    return new SvgColor(0, 128, 0);
                case "silver":
                    return new SvgColor(192, 192, 192);
                case "lime":
                    return new SvgColor(0, 255, 0);
                case "gray":
                    return new SvgColor(128, 128, 128);
                case "olive":
                    return new SvgColor(128, 128, 0);
                case "white":
                    return new SvgColor(255, 255, 255);
                case "yellow":
                    return new SvgColor(255, 255, 0);
                case "maroon":
                    return new SvgColor(128, 0, 0);
                case "navy":
                    return new SvgColor(0, 0, 128);
                case "red":
                    return new SvgColor(255, 0, 0);
                case "blue":
                    return new SvgColor(0, 0, 255);
                case "purple":
                    return new SvgColor(128, 0, 128);
                case "teal":
                    return new SvgColor(0, 128, 128);
                case "fuchsia":
                    return new SvgColor(255, 0, 255);
                case "aqua":
                    return new SvgColor(0, 255, 255);
            }

            throw new ArgumentException(string.Format("Unsupported color value: {0}", value));

        }

    } // class SvgColor

}
