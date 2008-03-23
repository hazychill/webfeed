using System;
using System.Collections.Generic;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomFeed : AtomDocument {

    private AtomGenerator             _generator;
    private AtomUri                   _icon;
    private AtomUri                   _logo;
    private AtomTextConstruct         _subtitle;
    private IList<AtomEntry>          _entries;

    public AtomFeed(Uri id, AtomTextConstruct title, DateTime updated)
      : base(id, title, updated) {

        _generator = null;
        _icon      = null;
        _logo      = null;
        _subtitle  = null;

        _entries = new List<AtomEntry>();
    }

    public AtomFeed(string id, string title, DateTime updated)
      : this(new Uri(id, UriKind.Absolute), new AtomTextConstruct(title), updated) { }

    protected AtomFeed() : base() { }

    public AtomEntry FindEntry(Uri id) {
      return Helper.Find<AtomEntry>(_entries, delegate(AtomEntry entry) {
        return id.OriginalString.Equals(entry.ID.OriginalString, StringComparison.Ordinal);
      });
    }

    public void Merge(params AtomDocument[] docs) {
      foreach (AtomDocument doc in docs) {
        if (doc is AtomEntry) {
          AtomEntry entry = (AtomEntry)doc;
          _entries.Add(entry);
        }
        else if (doc is AtomFeed) {
          AtomFeed feed = (AtomFeed)doc;
          AtomSource source = new AtomSource(feed);
          foreach (AtomEntry entry in feed.Entries) {
            if (entry.Source == null) {
              entry.Source = source;
            }
            _entries.Add(entry);
          }
        }
      }
    }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    public override void WriteDocument(XmlWriter writer) {
      WriteStartElement(writer, true);
      WriteElementContent(writer);
      writer.WriteEndElement();
    }

    protected override void WriteStartElement(XmlWriter writer) {
      WriteStartTag(writer);
      WriteAttributes(writer);
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
        writer.WriteStartElement("feed", "http://www.w3.org/2005/Atom");
      }
      else {
        writer.WriteStartElement("feed");
      }
    }

    protected override void WriteElementContent(XmlWriter writer) {
      base.WriteElementContent(writer);

      if (_generator != null) {
        _generator.WriteTo(writer);
      }

      if (_icon != null) {
        _icon.WriteTo(writer, "icon");
      }

      if (_logo != null) {
        _logo.WriteTo(writer, "logo");
      }

      if (_subtitle != null) {
        _subtitle.WriteTo(writer, "subtitle");
      }

      foreach (AtomEntry entry in _entries) {
        entry.WriteTo(writer);
      }
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////

    public AtomGenerator Generator {
      get { return _generator; }
      set { _generator = value; }
    }

    public AtomUri Icon {
      get { return _icon; }
      set { _icon = value; }
    }

    public AtomUri Logo {
      get { return _logo; }
      set { _logo = value; }
    }

    public AtomTextConstruct Subtitle {
      get { return _subtitle; }
      set { _subtitle = value; }
    }

    public virtual IList<AtomEntry> Entries {
      get { return _entries; }
    }
  }
}
