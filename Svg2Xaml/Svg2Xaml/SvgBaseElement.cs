﻿////////////////////////////////////////////////////////////////////////////////
//
//  SvgBaseElement.cs - This file is part of Svg2Xaml.
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
//  $LastChangedRevision: 18569 $
//  $LastChangedDate: 2009-03-18 14:05:21 +0100 (Wed, 18 Mar 2009) $
//  $LastChangedBy: unknown $
//
////////////////////////////////////////////////////////////////////////////////
using System.Xml;
using System.Xml.Linq;

namespace Svg2Xaml
{

  //****************************************************************************
  /// <summary>
  ///   Base class for all other SVG elements.
  /// </summary>
  class SvgBaseElement
  {

    //==========================================================================
    public readonly SvgDocument Document;

    //==========================================================================
    public readonly string Reference = null;

    //==========================================================================
    public SvgSVGElement Root
    {
      get
      {
        return Document.Root;
      }
    }

    //==========================================================================
    public readonly SvgBaseElement Parent;

    //==========================================================================
    public readonly string Id = null;

    //==========================================================================
    public readonly XElement Element;

    //==========================================================================
    protected SvgBaseElement(SvgDocument document, SvgBaseElement parent, XElement element)
    {
      Document = document;
      Parent   = parent;

      // Create attributes from styles...
      XAttribute style_attribute = element.Attribute("style");
      if(style_attribute != null)
      {
        foreach(string property in style_attribute.Value.Split(';'))
        {
          string[] tokens = property.Split(':');
          if(tokens.Length == 2)
            try
            {
              element.SetAttributeValue(tokens[0], tokens[1]);
            }
            catch(XmlException)
            {
              continue;
            }
        }
        style_attribute.Remove();
      }

      XAttribute id_attribute = element.Attribute("id");
      if(id_attribute != null)
        Document.Elements[Id = id_attribute.Value] = this;

      XAttribute href_attribute = element.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"));
      if(href_attribute != null)
      {
        string reference = href_attribute.Value;
        if(reference.StartsWith("#"))
          Reference = reference.Substring(1);
      }

      Element = element;
    }

  } // class SvgBaseElement

}
