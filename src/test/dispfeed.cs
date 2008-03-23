using System;
using System.Xml;
using System.IO;
using WebFeed.Atom10;

public class Program {
  public static void Main(string[] args) {
    AtomXmlReader reader = new AtomXmlReader();
    reader.DocumentTypeDetected += delegate(object sender, DocumentTypeDetectedEventArgs e) {
      if (!e.DocumentType.IsAssignableFrom(typeof(AtomFeed))) {
        e.HaltFurtherProcess = true;
      }
    };
    try {
      if (args.Length == 0) {
        Stream input = Console.OpenStandardInput();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        reader.ReadDocument(input).WriteDocument(Console.Out, settings);
      }
      else {
        foreach (string feedLocation in args) {
          XmlWriterSettings settings = new XmlWriterSettings();
          settings.Indent = true;
          reader.ReadDocument(feedLocation).WriteDocument(Console.Out, settings);
          break;
        }
      }
    }
    catch (ArgumentException e) {
      Console.Error.WriteLine(e.Message);
    }
  }
}