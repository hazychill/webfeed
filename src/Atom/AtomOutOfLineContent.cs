using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomOutOfLineContent : AtomContent {
    public AtomOutOfLineContent(AtomContentType type, Uri src)
      : base() {
        _text = null;
        _type = type;
        _src = src;
      }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteAttributes(XmlWriter writer) {
      writer.WriteAttributeString("type", _type.Value);
      writer.WriteAttributeString("src", _src.OriginalString);
      if (XmlBase != null) {
        writer.WriteAttributeString("xml", "base", "http://www.w3.org/XML/1998/namespace", XmlBase);
      }
      if (XmlLang != null) {
        writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", XmlLang);
      }
    }

    protected override void WriteElementContent(XmlWriter writer) { }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public override string Text {
      get { return null; }
      set {
        throw new InvalidOperationException("Text of AtomOutOfLineContent must be empty.");
      }
    }

    public override Uri Src {
      get { return _src; }
      set {
        if (_src == null) {
          throw new ArgumentNullException("Src");
        }
        _src = value;
      }
    }
  }
}