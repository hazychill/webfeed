using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomLink : AtomBaseElement {
    Uri     _href;
    string  _rel;
    string  _type;
    string  _hrefLang;
    string  _title;
    long    _length;

    public AtomLink(Uri href, string rel, string type, string hrefLang, string title, long length) : base() {
      if (href == null) {
        throw new ArgumentNullException("", "href");
      }
      if (rel!=null && rel.Length==0) {
        throw new FormatException("rel must contain at least one charactor.");
      }
      if (type!=null && !IsValidMediaType(type)) throw new FormatException("Invalid type.");
      _href = href;
      _rel = rel;
      _type = type;
      _hrefLang = hrefLang;
      _title = title;
      _length = length;
    }

    public AtomLink(Uri href, string rel)
      : this(href, rel, null, null, null, -1) { }

    public AtomLink(Uri href)
      : this(href, null, null, null, null, -1) { }

    internal static bool IsValidMediaType(string mediaType) {
      return (mediaType.IndexOf('/') != -1);
    }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteStartTag(XmlWriter writer) {
      writer.WriteStartElement("link");
    }

    protected override void WriteAttributes(XmlWriter writer) {
      if (_rel != null) {
        writer.WriteAttributeString("rel", _rel);
      }
      writer.WriteAttributeString("href", _href.OriginalString);
      if (_type != null) {
        writer.WriteAttributeString("type", _type);
      }
      if (_hrefLang != null) {
        writer.WriteAttributeString("hreflang", _hrefLang);
      }
      if (_title != null) {
        writer.WriteAttributeString("title", _title);
      }
      if (_length >= 0) {
        writer.WriteAttributeString("length", _length.ToString());
      }
      
      base.WriteAttributes(writer);
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public Uri HRef {
      get { return _href; }
      set {
        if (value == null) {
          throw new ArgumentNullException("HRef");
        }
        _href = value;
      }
    }

    public string Rel {
      get { return _rel; }
      set { _rel = value; }
    }

    public string Type {
      get { return _type; }
      set { _type = value; }
    }

    public string HRefLang {
      get { return _hrefLang; }
      set { _hrefLang = value; }
    }

    public string Title {
      get { return _title; }
      set { _title = value; }
    }

    public long Length {
      get { return _length; }
      set { _length = value; }
    }

  }
}
