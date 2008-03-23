using System;
using System.Xml;
using System.IO;
using WebFeed.Atom10;

public class Program {
  public static void Main(string[] args) {
    XmlWriterSettings writerSettings = new XmlWriterSettings();
    writerSettings.Indent = true;
    XmlReaderSettings readerSettings = new XmlReaderSettings();
    readerSettings.IgnoreWhitespace = true;
    if (args.Length == 0) {
      Stream input = Console.OpenStandardInput();
      AtomXmlReader.Plain.ReadDocument(input, readerSettings).WriteDocument(Console.Out, writerSettings);
    }
    else {
      foreach (string feedLocation in args) {
        AtomXmlReader.Plain.ReadDocument(feedLocation, readerSettings).WriteDocument(Console.Out, writerSettings);
        break;
      }
    }
  }
}