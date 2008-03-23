using System;
using System.Collections.Generic;
using System.Xml;

namespace WebFeed.Atom10 {
  public abstract class AtomBaseElement {
    private string _xmlBase;
    private string _xmlLang;

    public AtomBaseElement(string xmlBase, string xmlLang) {
      _xmlBase = xmlBase;
      _xmlLang = xmlLang;
    }

    public AtomBaseElement() : this(null, null) { }

    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    internal void WriteTo(XmlWriter writer) {
      WriteStartElement(writer);
      WriteElementContent(writer);
      writer.WriteEndElement();
    }

    protected virtual void WriteStartElement(XmlWriter writer) {
      WriteStartTag(writer);
      WriteAttributes(writer);
    }

    protected abstract void WriteStartTag(XmlWriter writer);

    protected virtual void WriteAttributes(XmlWriter writer) {
      if (!string.IsNullOrEmpty(_xmlBase)) {
        writer.WriteAttributeString("xml", "base", "http://www.w3.org/XML/1998/namespace", _xmlBase);
      }
      if (!string.IsNullOrEmpty(_xmlLang)) {
        writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", _xmlLang);
      }
    }

    protected virtual void WriteElementContent(XmlWriter writer) { }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////
    
    
    public string XmlBase {
      get { return _xmlBase; }
      set { _xmlBase = value; }
    }

    public string XmlLang {
      get { return _xmlLang; }
      set { _xmlLang = value; }
    }
 }
}
