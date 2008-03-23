using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomUri : AtomBaseElement {
    private Uri _value;

    public AtomUri(Uri value) {
      if (value == null) {
        throw new ArgumentNullException("value");
      }
      _value = value;
    }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    internal void WriteTo(XmlWriter writer, string elementName) {
      WriteStartElement(writer, elementName);
      WriteElementContent(writer);
      writer.WriteEndElement();
    }

    protected override void WriteStartElement(XmlWriter writer) {
      WriteStartElement(writer, "dummy");
    }

    protected virtual void WriteStartElement(XmlWriter writer, string elementName) {
      WriteStartTag(writer, elementName);
      WriteAttributes(writer);
    }

    protected override void WriteStartTag(XmlWriter writer) {
      WriteStartTag(writer, "dummy");
    }

    protected virtual void WriteStartTag(XmlWriter writer, string elementName) {
      writer.WriteStartElement(elementName);
    }

    protected override void WriteElementContent(XmlWriter writer) {
      writer.WriteString(_value.OriginalString);
    }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public Uri Value {
      get { return _value; }
      set { _value = value; }
    }
  }
}