using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomCategory : AtomBaseElement {
    string _term;
    Uri _scheme;
    string _label;

    public AtomCategory(string term, Uri scheme, string label)  : base() {
      if (term == null) {
        throw new ArgumentNullException("Argument must not be null.", "term");
      }

      _term = term;
      _scheme = scheme;
      _label = label;
    }

    public AtomCategory(string term)
      : this(term, null, null) { }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteStartTag(XmlWriter writer) {
      writer.WriteStartElement("category");
    }

    protected override void WriteAttributes(XmlWriter writer) {
      writer.WriteAttributeString("term", _term);
      if (_scheme != null) {
        writer.WriteAttributeString("scheme", _scheme.OriginalString);
      }
      if (_label != null) {
        writer.WriteAttributeString("label", _label);
      }
      
      base.WriteAttributes(writer);
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////
    
    
    public string Term {
      get { return _term; }
      set {
        if (value == null) {
          throw new ArgumentNullException("Argument must not be null.", "Term");
        }
        _term = value;
      }
    }

    public Uri Scheme {
      get { return _scheme; }
      set { _scheme = value; }
    }

    public string Label {
      get { return _label; }
      set { _label = value; }
    }
  }
}
