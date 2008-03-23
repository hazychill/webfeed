using System;
using System.Collections.Generic;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomEntry : AtomDocument {

    AtomContent       _content;
    DateTime?         _published;
    AtomSource        _source;
    AtomTextConstruct _summary;

    public AtomEntry(Uri id, AtomTextConstruct title, DateTime updated)
      : base(id, title, updated) {

        _content   = null;
        _published = null;
        _source    = null;
        _summary   = null;
      }

    public AtomEntry(string id, string title, DateTime updated)
      : this(new Uri(id, UriKind.Absolute), new AtomTextConstruct(title), updated) { }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    public override void WriteDocument(XmlWriter writer) {
      WriteStartElement(writer, true);
      WriteElementContent(writer);
      writer.WriteEndElement();
    }

    protected override void WriteStartElement(XmlWriter writer) {
      WriteStartElement(writer, false);
    }

    protected virtual void WriteStartElement(XmlWriter writer, bool writeNs) {
      WriteStartTag(writer, writeNs);
      WriteAttributes(writer);
    }

    protected override void WriteStartTag(XmlWriter writer) {
      WriteStartTag(writer, false);
    }

    protected virtual void WriteStartTag(XmlWriter writer, bool writeNs) {
      if (writeNs) {
        writer.WriteStartElement("entry", "http://www.w3.org/2005/Atom");
      }
      else {
        writer.WriteStartElement("entry");
      }
    }

    protected override void WriteElementContent(XmlWriter writer) {
      base.WriteElementContent(writer);

      if (_content != null) {
        _content.WriteTo(writer, "content");
      }

      if (_summary != null) {
        _summary.WriteTo(writer, "summary");
      }

      if (_published != null && _published.HasValue) {
        writer.WriteStartElement("published");
        writer.WriteString(_published.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        writer.WriteEndElement();
      }

      if (_source != null) {
        _source.WriteTo(writer);
      }

    }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


  public AtomContent Content {
    get { return _content; }
    set { _content = value; }
  }

  public DateTime? Published {
    get { return _published; }
    set { _published = value; }
  }

  public AtomSource Source {
    get { return _source; }
    set { _source = value; }
  }

  public AtomTextConstruct Summary {
    get { return _summary; }
    set { _summary = value; }
  }
}
}
