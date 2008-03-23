using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomGenerator : AtomBaseElement {
    private Uri    _uri;
    private string _version;
    private string _name;

    public AtomGenerator(string name, Uri uri, string version) {
      _uri = uri;
      _version = version;
      _name = name;
    }

    public AtomGenerator(string name)
      : this(name, null, null) { }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteStartTag(XmlWriter writer) {
      writer.WriteStartElement("generator");
    }

    protected override void WriteAttributes(XmlWriter writer) {
      if (_uri != null) {
        writer.WriteAttributeString("uri", _uri.OriginalString);
      }
      if (_version != null) {
        writer.WriteAttributeString("version", _version);
      }
      
      base.WriteAttributes(writer);
    }

    protected override void WriteElementContent(XmlWriter writer) {
      writer.WriteString(_name);
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public Uri Uri {
      get { return _uri; }
      set { _uri = value; }
    }

    public string Version {
      get { return _version; }
      set { _version = value; }
    }

    public string Name {
      get { return _name; }
      set { _name = value; }
    }
  }
}