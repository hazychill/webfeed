using System;
using System.IO;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomTextConstruct : AtomBaseElement {
    protected string _text;
    protected AtomContentType _type;

    public AtomTextConstruct(string text, AtomContentType type) : base() {
      if (text == null) {
        throw new ArgumentNullException("'text' must not be null.", "text");
      }

      if (!IsPreparedType(type)) {
        throw new ArgumentException("type must be one of 'text', 'html' or 'xhtml'.", "type");
      }
      
      _text = text;
      _type = type;
    }

    public AtomTextConstruct(string text)
      : this(text, null) { }

    protected AtomTextConstruct() : base() { }

    public static implicit operator string(AtomTextConstruct val) {
      return val.Text;
    }

    public static implicit operator AtomTextConstruct(string text) {
      return new AtomTextConstruct(text);
    }

    public override string ToString() {
      return _text;
    }

    public static bool IsPreparedType(AtomContentType type) {
      if (type == null) {
        return true;
      }

      if (type == AtomContentType.Text) {
        return true;
      }
      if (type == AtomContentType.Html) {
        return true;
      }
      if (type == AtomContentType.XHtml) {
        return true;
      }

      return false;
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
    
    protected override void WriteAttributes(XmlWriter writer) {
      if (_type == null || _type.Value == null) {
      }
      else {
        writer.WriteAttributeString("type", _type.Value);
      }
      
      base.WriteAttributes(writer);
    }

    protected override void WriteElementContent(XmlWriter writer) {
      if (!string.IsNullOrEmpty(_text)) {
        if (_type == AtomContentType.XHtml) {
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


    public virtual string Text {
      get { return _text; }
      set {
        if (value == null) {
          throw new ArgumentNullException("'Text' must not be null.", "Text");
        }
        _text = value;
      }
    }

    public virtual AtomContentType Type {
      get { return _type; }
      set {
        if (!IsPreparedType(value)) {
          throw new ArgumentException("type must be one of 'text', 'html' or 'xhtml'.", "Type");
        }
        _type = value;
      }
    }

    public XmlReader ContentXmlReader {
      get {
        if (_type != AtomContentType.XHtml) {
          throw new InvalidOperationException("Content text cannot be interpreted as xml unless its type is xhtml.");
        }
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.CloseInput = true;
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        TextReader tr = new StringReader(_text);
        return XmlReader.Create(tr, settings);
      }
    }
  }
}
