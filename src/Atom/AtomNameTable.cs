using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public class AtomNameTable {

    //private XmlNameTable _nameTable;

    private object _ns_Atom;
    private object _ns_namespace;
    private object _ns_xmlns;
    private object _ns_xhtml;
    private object _ns_schematron;
    private object _ns_Local;

    private object _ln_base;
    private object _ln_lang;
    private object _ln_type;
    private object _ln_name;
    private object _ln_uri;
    private object _ln_email;
    private object _ln_feed;
    private object _ln_entry;
    private object _ln_content;
    private object _ln_src;
    private object _ln_author;
    private object _ln_category;
    private object _ln_term;
    private object _ln_scheme;
    private object _ln_label;
    private object _ln_contributor;
    private object _ln_generator;
    private object _ln_version;
    private object _ln_icon;
    private object _ln_id;
    private object _ln_logo;
    private object _ln_link;
    private object _ln_href;
    private object _ln_rel;
    private object _ln_hreflang;
    private object _ln_title;
    private object _ln_length;
    private object _ln_published;
    private object _ln_rights;
    private object _ln_source;
    private object _ln_subtitle;
    private object _ln_summary;
    private object _ln_updated;
    private object _ln_div;
    
    public AtomNameTable(XmlNameTable nameTable) {
      if (nameTable == null) {
        throw new ArgumentNullException("nameTable");
      }
      
      _ns_Atom        = nameTable.Add("http://www.w3.org/2005/Atom");
      _ns_xmlns       = nameTable.Add("http://www.w3.org/2000/xmlns/");
      _ns_namespace   = nameTable.Add("http://www.w3.org/XML/1998/namespace");
      _ns_xhtml       = nameTable.Add("http://www.w3.org/1999/xhtml");
      _ns_schematron  = nameTable.Add("http://www.ascc.net/xml/schematron");
      _ns_Local       = nameTable.Add(string.Empty);

      _ln_base        = nameTable.Add("base");
      _ln_lang        = nameTable.Add("lang");
      _ln_type        = nameTable.Add("type");
      _ln_name        = nameTable.Add("name");
      _ln_uri         = nameTable.Add("uri");
      _ln_email       = nameTable.Add("email");
      _ln_feed        = nameTable.Add("feed");
      _ln_entry       = nameTable.Add("entry");
      _ln_content     = nameTable.Add("content");
      _ln_src         = nameTable.Add("src");
      _ln_author      = nameTable.Add("author");
      _ln_category    = nameTable.Add("category");
      _ln_term        = nameTable.Add("term");
      _ln_scheme      = nameTable.Add("scheme");
      _ln_label       = nameTable.Add("label");
      _ln_contributor = nameTable.Add("contributor");
      _ln_generator   = nameTable.Add("generator");
      _ln_version     = nameTable.Add("version");
      _ln_icon        = nameTable.Add("icon");
      _ln_id          = nameTable.Add("id");
      _ln_logo        = nameTable.Add("logo");
      _ln_link        = nameTable.Add("link");
      _ln_href        = nameTable.Add("href");
      _ln_rel         = nameTable.Add("rel");
      _ln_hreflang    = nameTable.Add("hreflang");
      _ln_title       = nameTable.Add("title");
      _ln_length      = nameTable.Add("length");
      _ln_published   = nameTable.Add("published");
      _ln_rights      = nameTable.Add("rights");
      _ln_source      = nameTable.Add("source");
      _ln_subtitle    = nameTable.Add("subtitle");
      _ln_summary     = nameTable.Add("summary");
      _ln_updated     = nameTable.Add("updated");
      _ln_div         = nameTable.Add("div");
    }

    public object NamespaceUriAtom {
      get { return _ns_Atom; }
    }

    public object NamespaceUriXml {
      get { return _ns_namespace; }
    }

    public object NamespaceUriXmlns {
      get { return _ns_xmlns; }
    }

    public object NamespaceUriXHtml {
      get { return _ns_xhtml; }
    }

    public object NamespaceUriSchematron {
      get { return _ns_schematron; }
    }

    public object NamespaceUriLocal {
      get { return _ns_Local; }
    }

    public object LocalNameBase {
      get { return _ln_base; }
    }

    public object LocalNameLang {
      get { return _ln_lang; }
    }

    public object LocalNameType {
      get { return _ln_type; }
    }

    public object LocalNameName {
      get { return _ln_name; }
    }

    public object LocalNameUri {
      get { return _ln_uri; }
    }

    public object LocalNameEmail {
      get { return _ln_email; }
    }

    public object LocalNameFeed {
      get { return _ln_feed; }
    }

    public object LocalNameEntry {
      get { return _ln_entry; }
    }

    public object LocalNameContent {
      get { return _ln_content; }
    }

    public object LocalNameSrc {
      get { return _ln_src; }
    }

    public object LocalNameAuthor {
      get { return _ln_author; }
    }

    public object LocalNameCategory {
      get { return _ln_category; }
    }

    public object LocalNameTerm {
      get { return _ln_term; }
    }

    public object LocalNameScheme {
      get { return _ln_scheme; }
    }

    public object LocalNameLable {
      get { return _ln_label; }
    }

    public object LocalNameContributor {
      get { return _ln_contributor; }
    }

    public object LocalNameGenerator {
      get { return _ln_generator; }
    }

    public object LocalNameVersion {
      get { return _ln_version; }
    }

    public object LocalNameIcon {
      get { return _ln_icon; }
    }

    public object LocalNameID {
      get { return _ln_id; }
    }

    public object LocalNameLogo {
      get { return _ln_logo; }
    }

    public object LocalNameLink {
      get { return _ln_link; }
    }

    public object LocalNameHRef {
      get { return _ln_href; }
    }

    public object LocalNameRel {
      get { return _ln_rel; }
    }

    public object LocalNameHRefLang {
      get { return _ln_hreflang; }
    }

    public object LocalNameTitle {
      get { return _ln_title; }
    }

    public object LocalNameLength {
      get { return _ln_length; }
    }

    public object LocalNamePublished {
      get { return _ln_published; }
    }

    public object LocalNameRights {
      get { return _ln_rights; }
    }

    public object LocalNameSource {
      get { return _ln_source; }
    }

    public object LocalNameSubtitle {
      get { return _ln_subtitle; }
    }

    public object LocalNameSummary {
      get { return _ln_summary; }
    }

    public object LocalNameUpdated {
      get { return _ln_updated; }
    }

    public object LocalNameDiv {
      get { return _ln_div; }
    }
  }
}