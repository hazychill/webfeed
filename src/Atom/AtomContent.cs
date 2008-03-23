using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomContent : AtomTextConstruct {
    protected Uri _src;

    public AtomContent(string text, AtomContentType type) : base(text) {
      _type = type;
      _src = null;
    }

    public AtomContent(string text)
      : this(text, null) { }

    protected AtomContent() : base() { }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteStartTag(XmlWriter writer) {
      writer.WriteStartElement("content");
    }

    protected override void WriteElementContent(XmlWriter writer) {
      if (!string.IsNullOrEmpty(_text)) {
        if (_type == null || _type.Value == null) {
          writer.WriteString(_text);
          return;
        }

        if (Helper.IsXmlType(_type)) {
          writer.WriteRaw(_text);
        }
        else if (_type == AtomContentType.Html) {
          writer.WriteCData(_text);
        }
        else {
          writer.WriteString(_text);
        }
      }
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public override AtomContentType Type {
      get { return _type; }
      set { _type = value; }
    }

    public virtual Uri Src {
      get { return _src; }
      set {
        throw new NotSupportedException();
      }
    }
  }
}