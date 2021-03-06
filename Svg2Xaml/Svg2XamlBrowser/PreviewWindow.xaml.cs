﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Svg2Xaml;

namespace Svg2XamlBrowser
{

  //****************************************************************************
  public partial class PreviewWindow : Window
  {
    //==========================================================================
    public PreviewWindow(string fileName)
    {
      InitializeComponent();

      Title = fileName;

      var thread = new Thread(Load);
            thread.SetApartmentState(ApartmentState.STA);
        thread.Start(fileName);
    }

    //==========================================================================
    string LoadXml(Stream stream)
    {
      XmlDocument document = new XmlDocument();
      document.Load(stream);
      return document.OuterXml;
    }

    //==========================================================================
    string FormatXml(string xml)
    {
      XmlDocument document = new XmlDocument();
      document.LoadXml(xml);

      using(MemoryStream stream = new MemoryStream())
      {
        using(XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, IndentChars = "  ", NewLineChars = Environment.NewLine, Encoding = Encoding.UTF8 }))
        document.Save(writer);

        return Encoding.UTF8.GetString(stream.ToArray());
      }
    }

    //==========================================================================
    void Load(object state)
    {
      string file_name = state as string;

      try
      {
        DrawingImage svg_image;
        string svg ;

        if (file_name.EndsWith("svgz"))
        {
          using (FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
          using (GZipStream gzip_stream = new GZipStream(file_stream, CompressionMode.Decompress))
            svg_image = SvgReader.Load(gzip_stream, new SvgReaderOptions(false));

          using(FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
          using(GZipStream gzip_stream = new GZipStream(file_stream, CompressionMode.Decompress))
            svg = FormatXml(LoadXml(gzip_stream));

        }
        else
        {
          using (FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
            svg_image = SvgReader.Load(file_stream, new SvgReaderOptions(false));

          using(FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
            svg = FormatXml(LoadXml(file_stream));
        }


        if (svg_image != null)
        {
          svg_image.Freeze();

          string xaml = FormatXml(XamlWriter.Save(svg_image));

          Dispatcher.Invoke((Action)delegate
          {
            PreviewImage.Source = svg_image;
            SvgTextBox.Text = svg;
            XamlTextBox.Text = xaml;
          }, DispatcherPriority.Background);
        }
      }
      catch (Exception exception)
      {
      }

    }
  }
}
