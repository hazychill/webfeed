using System;
using System.Net.Mail;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomPersonConstruct : AtomBaseElement {
    string _name;
    AtomUri _uri;
    MailAddress _email;

    public AtomPersonConstruct(string name, AtomUri uri, MailAddress email) : base() {
      if (name == null) {
        throw new ArgumentNullException("", "name");
      }
      _name = name;
      _uri = uri;
      _email = email;
    }

    public AtomPersonConstruct(string name)
      : this(name, null, null) { }

    public static implicit operator string(AtomPersonConstruct person) {
      return person.Name;
    }

    public static implicit operator AtomPersonConstruct(string name) {
      return new AtomPersonConstruct(name);
    }

    public override string ToString() {
      return _name;
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
      writer.WriteElementString("name", _name);
      if (_uri != null) {
        _uri.WriteTo(writer, "uri");
      }
      if (_email != null) {
        writer.WriteElementString("email", _email.Address);
      }
    }
    
    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public string Name {
      get { return _name; }
      set {
        if (value == null) {
          throw new ArgumentNullException("", "Name");
        }
        _name = value;
      }
    }

    public AtomUri Uri {
      get { return _uri; }
      set { _uri = value; }
    }

    public MailAddress Email {
      get { return _email; }
      set { _email = value; }
    }
  }
}
