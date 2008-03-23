using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace WebFeed.Atom10 {

  public abstract class AtomDocument : AtomBaseElement {
    protected Uri                        _id;
    protected AtomTextConstruct          _title;
    protected DateTime                   _updated;
    private   AtomTextConstruct          _rights;
    private   IList<AtomPersonConstruct> _authors;
    private   IList<AtomPersonConstruct> _contributors;
    private   IList<AtomCategory>        _categories;
    private   IList<AtomLink>            _links;

    public AtomDocument(Uri id, AtomTextConstruct title, DateTime updated) : base() {
      if (id == null) {
        throw new ArgumentNullException("", "id");
      }
      if (title == null) {
        throw new ArgumentNullException("", "title");
      }
      _id = id;
      _title = title;
      _updated = updated;

      _rights       = null;
      _authors      = null;
      _contributors = null;
      _categories   = null;
      _links        = null;
    }

    protected AtomDocument() { }

    
    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    public abstract void WriteDocument(XmlWriter writer);

    public void WriteDocument(TextWriter textWriter, XmlWriterSettings settings) {
      using (XmlWriter writer = XmlWriter.Create(textWriter, settings)) {
        WriteDocument(writer);
      }
    }

    public void WriteDocument(TextWriter textWriter) {
      WriteDocument(textWriter, new XmlWriterSettings());
    }

    public void WriteDocument(Stream outputStream, XmlWriterSettings settings) {
      using (TextWriter textWriter = new StreamWriter(outputStream, settings.Encoding)) {
        WriteDocument(textWriter, settings);
      }
    }

    public void WriteDocument(Stream outputStream) {
      WriteDocument(outputStream, new XmlWriterSettings());
    }

    public void WriteDocument(string filePath, XmlWriterSettings settings) {
      using (Stream outputStream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
        WriteDocument(outputStream, settings);
      }
    }

    public void WriteDocument(string filePath) {
      WriteDocument(filePath, new XmlWriterSettings());
    }

    protected override void WriteElementContent(XmlWriter writer) {
      writer.WriteStartElement("id");
      writer.WriteString(_id.OriginalString);
      writer.WriteEndElement();

      _title.WriteTo(writer, "title");

      writer.WriteStartElement("updated");
      writer.WriteString(_updated.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
      writer.WriteEndElement();

      foreach (AtomPersonConstruct author in GetAuthors()) {
        author.WriteTo(writer, "author");
      }

      foreach (AtomPersonConstruct contributor in GetContributors()) {
        contributor.WriteTo(writer, "contributor");
      }

      foreach (AtomCategory category in GetCategories()) {
        category.WriteTo(writer);
      }

      foreach (AtomLink link in GetLinks()) {
        link.WriteTo(writer);
      }

      if (_rights != null) {
        _rights.WriteTo(writer, "rights");
      }
    }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public virtual Uri ID {
      get { return _id; }
      set {
        if (value == null) throw new ArgumentNullException("", "ID");
        _id = value;
      }
    }
    
    public virtual AtomTextConstruct Title {
      get { return _title; }
      set {
        if (value == null) throw new ArgumentNullException("", "Title");
        _title = value;
      }
    }

    public virtual DateTime Updated {
      get { return _updated; }
      set { _updated = value; }
    }

    public AtomTextConstruct Rights {
      get { return _rights; }
      set { _rights = value; }
    }

    public IList<AtomPersonConstruct> Authors {
      get { return _authors; }
    }

    public void AddAuthor(AtomPersonConstruct author) {
      Helper.AddItem<AtomPersonConstruct>(ref _authors, author);
    }

    public IEnumerable<AtomPersonConstruct> GetAuthors() {
      return Helper.GetItems<AtomPersonConstruct>(_authors);
    }

    public IList<AtomPersonConstruct> Contributors {
      get { return _contributors; }
    }

    public void AddContributor(AtomPersonConstruct contributor) {
      Helper.AddItem<AtomPersonConstruct>(ref _contributors, contributor);
    }

    public IEnumerable<AtomPersonConstruct> GetContributors() {
      return Helper.GetItems<AtomPersonConstruct>(_contributors);
    }

    public IList<AtomCategory> Categories {
      get { return _categories; }
    }

    public void AddCategory(AtomCategory category) {
      Helper.AddItem<AtomCategory>(ref _categories, category);
    }

    public IEnumerable<AtomCategory> GetCategories() {
      return Helper.GetItems<AtomCategory>(_categories);
    }

    public IList<AtomLink> Links {
      get { return _links; }
    }

    public void AddLink(AtomLink link) {
      Helper.AddItem<AtomLink>(ref _links, link);
    }

    public IEnumerable<AtomLink> GetLinks() {
      return Helper.GetItems<AtomLink>(_links);
    }
  }
}
