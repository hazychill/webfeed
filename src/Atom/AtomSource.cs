using System;
using System.Collections.Generic;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomSource : AtomBaseElement {

    private Uri                        _id;
    private AtomTextConstruct          _title;
    private AtomTextConstruct          _subtitle;
    private DateTime?                  _updated;
    private IList<AtomPersonConstruct> _authors;
    private IList<AtomPersonConstruct> _contributors;
    private IList<AtomLink>            _links;
    private IList<AtomCategory>        _categories;
    private AtomGenerator              _generator;
    private AtomUri                    _icon;
    private AtomUri                    _logo;
    private AtomTextConstruct          _rights;

    public AtomSource() : base() {
      _id           = null;
      _title        = null;
      _subtitle     = null;
      _updated      = null;
      _authors      = null;
      _contributors = null;
      _links        = null;
      _categories   = null;
      _generator    = null;
      _icon         = null;
      _rights       = null;
    }

    public AtomSource(AtomFeed sourceFeed)
      : base(sourceFeed.XmlBase, sourceFeed.XmlLang) {
      _id           = sourceFeed.ID;
      _title        = sourceFeed.Title;
      _subtitle     = sourceFeed.Subtitle;
      _updated      = sourceFeed.Updated;
      _authors      = sourceFeed.Authors;
      _contributors = sourceFeed.Contributors;
      _links        = sourceFeed.Links;
      _categories   = sourceFeed.Categories;
      _generator    = sourceFeed.Generator;
      _icon         = sourceFeed.Icon;
      _rights       = sourceFeed.Rights;

    }


    /////////////////////////////////
    // Xml writing methods - Start //
    /////////////////////////////////

    protected override void WriteStartTag(XmlWriter writer) {
      writer.WriteStartElement("source");
    }

    protected override void WriteElementContent(XmlWriter writer) {
      if (_id != null) {
        writer.WriteStartElement("id");
        writer.WriteString(_id.OriginalString);
        writer.WriteEndElement();
      }

      if (_title != null) {
        _title.WriteTo(writer, "title");
      }

      if (_updated != null) {
        writer.WriteStartElement("updated");
        writer.WriteString(_updated.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        writer.WriteEndElement();
      }

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

      if (Rights != null) {
        Rights.WriteTo(writer, "rights");
      }

      if (Generator != null) {
        Generator.WriteTo(writer);
      }

      if (Icon != null) {
        Icon.WriteTo(writer, "icon");
      }

      if (Logo != null) {
        Logo.WriteTo(writer, "logo");
      }

      if (Subtitle != null) {
        Subtitle.WriteTo(writer, "subtitle");
      }
    }

    ///////////////////////////////
    // Xml writing methods - End //
    ///////////////////////////////


    public Uri ID {
      get { return _id; }
      set { _id = value; }
    }

    public AtomTextConstruct Title {
      get { return _title; }
      set { _title = value; }
    }

    public AtomTextConstruct Subtitle {
      get { return _subtitle; }
      set { _subtitle = value; }
    }

    public DateTime? Updated {
      get { return _updated; }
      set { _updated = value; }
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

    public IList<AtomLink> Links {
      get { return _links; }
    }

    public void AddLink(AtomLink link) {
      Helper.AddItem<AtomLink>(ref _links, link);
    }

    public IEnumerable<AtomLink> GetLinks() {
      return Helper.GetItems<AtomLink>(_links);
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

    public AtomTextConstruct Rights {
      get { return _rights; }
      set { _rights = value; }
    }
  }
}
