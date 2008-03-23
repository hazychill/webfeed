using System;
using System.Xml;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;

namespace WebFeed.Atom10 {
  public class AtomXmlReader : IAtomXmlReader {

    protected AtomNameTable _nameTable;
    private static readonly AtomXmlReader _plainReader = new AtomPlainReader();

    private object _lock;
    
    private AtomExtensionEventHandler _readExtensionDelegate;
    private DocumentTypeDetectedEventHandler _documentTypeDetectedDelegate;

    public virtual event AtomExtensionEventHandler ReadExtension {
      add {
        lock (_lock) {
          _readExtensionDelegate += value;
        }
      }
      
      remove {
        lock (_lock) {
          _readExtensionDelegate -= value;
        }
      }
    }

    public virtual event DocumentTypeDetectedEventHandler DocumentTypeDetected {
      add {
        lock (_lock) {
          _documentTypeDetectedDelegate += value;
        }
      }
      
      remove {
        lock (_lock) {
          _documentTypeDetectedDelegate -= value;
        }
      }
    }

    public AtomXmlReader() {
      _lock = new object();
    }

    public AtomDocument ReadDocument(XmlReader reader) {
      if (reader.ReadState != ReadState.Initial) {
        throw new InvalidOperationException();
      }

      AtomDocument doc = null;

      lock (_lock) {
        _nameTable = new AtomNameTable(reader.NameTable);

        if (reader.IsStartElement((string)_nameTable.LocalNameFeed, (string)_nameTable.NamespaceUriAtom)) {
          bool halt = RaiseDocumentTypeDetectedEvent(typeof(AtomFeed));
          if (halt) {
            throw new ArgumentException("Further processing fot AtomFeed was halted.");
          }
          doc = ReadFeedInner(reader);
        }
        else if (reader.IsStartElement((string)_nameTable.LocalNameEntry, (string)_nameTable.NamespaceUriAtom)) {
          bool halt = RaiseDocumentTypeDetectedEvent(typeof(AtomEntry));
          if (halt) {
            throw new ArgumentException("Further processing fot AtomEntry was halted.");
          }
          doc = ReadEntryInner(reader);
        }
      }

      if (doc == null) {
        throw new AtomFormatException("The document is neigher feed document nor entry document.");
      }

      return doc;
    }

    public AtomDocument ReadDocument(TextReader textReader) {
      using (XmlReader reader = XmlReader.Create(textReader)) {
        return ReadDocument(reader);
      }
    }

    public AtomDocument ReadDocument(TextReader textReader, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(textReader, settings)) {
        return ReadDocument(reader);
      }
    }

    public AtomDocument ReadDocument(Stream inputStream) {
      using (XmlReader reader = XmlReader.Create(inputStream)) {
        return ReadDocument(reader);
      }
    }

    public AtomDocument ReadDocument(Stream inputStream, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(inputStream, settings)) {
        return ReadDocument(reader);
      }
    }

    public AtomDocument ReadDocument(string feedLocation) {
      using (XmlReader reader = XmlReader.Create(feedLocation)) {
        return ReadDocument(reader);
      }
    }

    public AtomDocument ReadDocument(string feedLocation, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(feedLocation, settings)) {
        return ReadDocument(reader);
      }
    }

    public AtomFeed ReadFeed(XmlReader reader) {
      if (reader.ReadState != ReadState.Initial) {
        throw new InvalidOperationException();
      }

      AtomFeed feed = null;

      lock (_lock) {
        _nameTable = new AtomNameTable(reader.NameTable);

        if (reader.IsStartElement((string)_nameTable.LocalNameFeed, (string)_nameTable.NamespaceUriAtom)) {
          feed = ReadFeedInner(reader);
        }
      }

      if (feed == null) {
        throw new AtomFormatException("The document is not a feed document.");
      }

      return feed;
    }

    public AtomFeed ReadFeed(TextReader textReader) {
      using (XmlReader reader = XmlReader.Create(textReader)) {
        return ReadFeed(reader);
      }
    }

    public AtomFeed ReadFeed(TextReader textReader, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(textReader, settings)) {
        return ReadFeed(reader);
      }
    }

    public AtomFeed ReadFeed(Stream inputStream) {
      using (XmlReader reader = XmlReader.Create(inputStream)) {
        return ReadFeed(reader);
      }
    }

    public AtomFeed ReadFeed(Stream inputStream, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(inputStream, settings)) {
        return ReadFeed(reader);
      }
    }

    public AtomFeed ReadFeed(string feedLocation) {
      using (XmlReader reader = XmlReader.Create(feedLocation)) {
        return ReadFeed(reader);
      }
    }

    public AtomFeed ReadFeed(string feedLocation, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(feedLocation, settings)) {
        return ReadFeed(reader);
      }
    }

    public AtomEntry ReadEntry(XmlReader reader) {
      if (reader.ReadState != ReadState.Initial) {
        throw new InvalidOperationException();
      }

      AtomEntry entry = null;

      lock (_lock) {
        _nameTable = new AtomNameTable(reader.NameTable);

        if (reader.IsStartElement((string)_nameTable.LocalNameEntry, (string)_nameTable.NamespaceUriAtom)) {
          entry = ReadEntryInner(reader);
        }
      }

      if (entry == null) {
        throw new AtomFormatException("The document is not a entry document.");
      }

      return entry;
    }

    public AtomEntry ReadEntry(TextReader textReader) {
      using (XmlReader reader = XmlReader.Create(textReader)) {
        return ReadEntry(reader);
      }
    }

    public AtomEntry ReadEntry(TextReader textReader, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(textReader, settings)) {
        return ReadEntry(reader);
      }
    }

    public AtomEntry ReadEntry(Stream inputStream) {
      using (XmlReader reader = XmlReader.Create(inputStream)) {
        return ReadEntry(reader);
      }
    }

    public AtomEntry ReadEntry(Stream inputStream, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(inputStream, settings)) {
        return ReadEntry(reader);
      }
    }

    public AtomEntry ReadEntry(string feedLocation) {
      using (XmlReader reader = XmlReader.Create(feedLocation)) {
        return ReadEntry(reader);
      }
    }

    public AtomEntry ReadEntry(string feedLocation, XmlReaderSettings settings) {
      using (XmlReader reader = XmlReader.Create(feedLocation, settings)) {
        return ReadEntry(reader);
      }
    }

    protected virtual AtomFeed ReadFeedInner(XmlReader reader) {
      AtomFeed feed = CreateFeedTemplate();
      bool readID = false;
      bool readTitle = false;
      bool readUpdated = false;

      using (XmlReader feedReader = reader.ReadSubtree()) {

        feedReader.MoveToContent();

        while (feedReader.MoveToNextAttribute()) {
          object ns = feedReader.NamespaceURI;
          object ln = feedReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              feed.XmlBase = feedReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              feed.XmlLang = feedReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(feed, feedReader);
          }
        }

        feedReader.MoveToContent();

        while (feedReader.Read()) {
          if (feedReader.NodeType == XmlNodeType.Element) {
            if ((object)feedReader.NamespaceURI == _nameTable.NamespaceUriAtom) {

              object localName = feedReader.LocalName;

              if (localName == _nameTable.LocalNameEntry) {
                AtomEntry entry = ReadEntryInner(feedReader);
                feed.Entries.Add(entry);
              }
              else if (localName == _nameTable.LocalNameLink) {
                AtomLink link = ReadLink(feedReader);
                feed.AddLink(link);
              }
              else if (localName == _nameTable.LocalNameID) {
                Uri id = ReadID(feedReader, feed);
                feed.ID = id;
                readID = true;
              }
              else if (localName == _nameTable.LocalNameTitle) {
                AtomTextConstruct title = ReadTextConstruct(feedReader);
                feed.Title = title;
                readTitle = true;
              }
              else if (localName == _nameTable.LocalNameUpdated) {
                DateTime updated = ReadDateTime(feedReader);
                feed.Updated = updated;
                readUpdated = true;
              }
              else if (localName == _nameTable.LocalNameAuthor) {
                AtomPersonConstruct author = ReadPersonConstruct(feedReader);
                feed.AddAuthor(author);
              }
              else if (localName == _nameTable.LocalNameCategory) {
                AtomCategory category = ReadCategory(feedReader);
                feed.AddCategory(category);
              }
              else if (localName == _nameTable.LocalNameContributor) {
                AtomPersonConstruct contributor = ReadPersonConstruct(feedReader);
                feed.AddContributor(contributor);
              }
              else if (localName == _nameTable.LocalNameGenerator) {
                AtomGenerator generator = ReadGenerator(feedReader);
                feed.Generator = generator;
              }
              else if (localName == _nameTable.LocalNameIcon) {
                AtomUri icon = ReadIriReference(feedReader);
                feed.Icon = icon;
              }
              else if (localName == _nameTable.LocalNameLogo) {
                AtomUri logo = ReadIriReference(feedReader);
                feed.Logo = logo;
              }
              else if (localName == _nameTable.LocalNameRights) {
                AtomTextConstruct rights = ReadTextConstruct(feedReader);
                feed.Rights = rights;
              }
              else if (localName == _nameTable.LocalNameSubtitle) {
                AtomTextConstruct subtitle = ReadTextConstruct(feedReader);
                feed.Subtitle = subtitle;
              }
              else {
                RaiseExtensionEvent(feed, feedReader);
              }
            }
            else {
              RaiseExtensionEvent(feed, feedReader);
            }
          }
        }
      }

      if (readID      == false ||
          readTitle   == false ||
          readUpdated == false) {
        throw new AtomFormatException("Missing id or title or updated.", "feed");
      }

      return feed;
    }

    protected virtual AtomEntry ReadEntryInner(XmlReader reader) {
      AtomEntry entry = CreateEntryTemplate();
      bool readID = false;
      bool readTitle = false;
      bool readUpdated = false;

      using (XmlReader entryReader = reader.ReadSubtree()) {
        entryReader.MoveToContent();

        while (entryReader.MoveToNextAttribute()) {
          object ns = entryReader.NamespaceURI;
          object ln = entryReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              entry.XmlBase = entryReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              entry.XmlLang = entryReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(entry, entryReader);
          }
        }

        entryReader.MoveToContent();

        while (entryReader.Read()) {
          if (entryReader.NodeType == XmlNodeType.Element) {
            object localName = entryReader.LocalName;

            if ((object)entryReader.NamespaceURI == _nameTable.NamespaceUriAtom) {
              if (localName == _nameTable.LocalNameID) {
                Uri id = ReadID(entryReader, entry);
                entry.ID = id;
                readID = true;
              }
              else if (localName == _nameTable.LocalNameTitle) {
                AtomTextConstruct title = ReadTextConstruct(entryReader);
                entry.Title = title;
                readTitle = true;
              }
              else if (localName == _nameTable.LocalNameUpdated) {
                DateTime updated = ReadDateTime(entryReader);
                entry.Updated = updated;
                readUpdated = true;
              }
              else if (localName == _nameTable.LocalNameLink) {
                AtomLink link = ReadLink(entryReader);
                entry.AddLink(link);
              }
              else if (localName == _nameTable.LocalNameAuthor) {
                AtomPersonConstruct author = ReadPersonConstruct(entryReader);
                entry.AddAuthor(author);
              }
              else if (localName == _nameTable.LocalNameCategory) {
                AtomCategory category = ReadCategory(entryReader);
                entry.AddCategory(category);
              }
              else if (localName == _nameTable.LocalNameContent) {
                AtomContent content = ReadContent(entryReader);
                entry.Content = content;
              }
              else if (localName == _nameTable.LocalNameContributor) {
                AtomPersonConstruct contributor = ReadPersonConstruct(entryReader);
                entry.AddContributor(contributor);
              }
              else if (localName == _nameTable.LocalNamePublished) {
                DateTime published = ReadDateTime(entryReader);
                entry.Published = published;
              }
              else if (localName == _nameTable.LocalNameRights) {
                AtomTextConstruct rights = ReadTextConstruct(entryReader);
                entry.Rights = rights;
              }
              else if (localName == _nameTable.LocalNameSummary) {
                AtomTextConstruct summary = ReadTextConstruct(entryReader);
                entry.Summary = summary;
              }
              else if (localName == _nameTable.LocalNameSource) {
                AtomSource source = ReadSource(entryReader);
                entry.Source = source;
              }
              else {            // Namespace is atom, but localname is unknown.
                RaiseExtensionEvent(entry, entryReader);
              }
            }
            else {              // Unknown namespace.
              RaiseExtensionEvent(entry, entryReader);
            }
          }
        }
      }

      if (readID      == false ||
          readTitle   == false ||
          readUpdated == false) {
        throw new AtomFormatException("Missing id or title or updated.", "entry");
      }

      return entry;
    }

    protected virtual AtomLink ReadLink(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomLink link = CreateLinkTemplate();
      bool readHRef = false;

      while (reader.MoveToNextAttribute()) {
        object ns = reader.NamespaceURI;
        object ln = reader.LocalName;

        if (ns == _nameTable.NamespaceUriXml) {
          if (ln == _nameTable.LocalNameBase) {
            link.XmlBase = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameLang) {
            link.XmlLang = reader.Value;
            continue;
          }
        }
        else if (ns == _nameTable.NamespaceUriLocal) {
          if (ln == _nameTable.LocalNameHRef) {
            Uri uri = new Uri(reader.Value, UriKind.RelativeOrAbsolute);
            link.HRef = uri;
            readHRef = true;
            continue;
          }
          else if (ln == _nameTable.LocalNameRel) {
            link.Rel = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameType) {
            link.Type = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameHRefLang) {
            link.HRefLang = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameTitle) {
            link.Title = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameLength) {
            try {
              long length = long.Parse(reader.Value);
              link.Length = length;
            }
            catch (ArgumentNullException e) {
              throw new AtomFormatException("Length attribute value is null.", e);
            }
            catch (FormatException e) {
              throw new AtomFormatException("Length attribute value has invalid format.", e);
            }
            catch (OverflowException e) {
              throw new AtomFormatException("Parsing length attribute value results in an overflow.", e);
            }
            continue;
          }
        }

        if (ns != _nameTable.NamespaceUriXmlns) {
          RaiseExtensionEvent(link, reader);
        }
      }

      reader.MoveToContent();
      if (!reader.IsEmptyElement) {
        reader.Read();
      }

      if (readHRef == false) {
        throw new AtomFormatException("Missing href.", "link");
      }

      return link;
    }

    protected virtual Uri ReadID(XmlReader reader, AtomBaseElement readingElement) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      Uri id;

      using (XmlReader idReader = reader.ReadSubtree()) {
        idReader.MoveToContent();
        
        while (idReader.MoveToNextAttribute()) {
          if ((object)idReader.NamespaceURI != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(readingElement, idReader);
          }
        }
        
        idReader.MoveToContent();
        string iriString = idReader.ReadString();
        id = new Uri(iriString, UriKind.Absolute);

        while (idReader.Read()) { }
      }

      return id;
    }

    public AtomUri ReadIriReference(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomUri atomUri = CreateUriTemplate();

      using (XmlReader iriRefReader = reader.ReadSubtree()) {
        iriRefReader.MoveToContent();

        while (iriRefReader.MoveToNextAttribute()) {
          object ns = iriRefReader.NamespaceURI;
          object ln = iriRefReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              atomUri.XmlBase = iriRefReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              atomUri.XmlLang = iriRefReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(atomUri, iriRefReader);
          }
        }

        iriRefReader.MoveToContent();

        Uri uri = new Uri(iriRefReader.ReadString(), UriKind.RelativeOrAbsolute);
        atomUri.Value = uri;

        while (iriRefReader.Read()) { }
      }

      return atomUri;
    }

    protected virtual AtomTextConstruct ReadTextConstruct(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomTextConstruct textConstruct = CreateTextConstructTemplate();

      using (XmlReader innerReader = reader.ReadSubtree()) {
        innerReader.MoveToContent();

        while (innerReader.MoveToNextAttribute()) {
          object ns = innerReader.NamespaceURI;
          object ln = innerReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              textConstruct.XmlBase = innerReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              textConstruct.XmlLang = innerReader.Value;
              continue;
            }
          }
          else if (ns == _nameTable.NamespaceUriLocal) {
            if (ln == _nameTable.LocalNameType) {
              switch (innerReader.Value) {
              case "text":
                textConstruct.Type = AtomContentType.Text;
                break;
              case "html":
                textConstruct.Type = AtomContentType.Html;
                break;
              case "xhtml":
                textConstruct.Type = AtomContentType.XHtml;
                break;
              default:
                throw new AtomFormatException("Unkonwn type of text construct.");
              }
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(textConstruct, innerReader);
          }
        }

        innerReader.MoveToContent();

        string text;
        
        if (textConstruct.Type == AtomContentType.XHtml) {
          text = innerReader.ReadInnerXml();
        }
        else {
          text = innerReader.ReadString();
        }
        textConstruct.Text = text;

        while (innerReader.Read()) { }
      }

      return textConstruct;
    }

    protected virtual DateTime ReadDateTime(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      DateTime dt = DateTime.MinValue;
      using (XmlReader dateTimeReader = reader.ReadSubtree()) {
        dateTimeReader.Read();
        dt = dateTimeReader.ReadElementContentAsDateTime();

        while (dateTimeReader.Read()) { }
      }

      return dt;
    }

    protected virtual AtomPersonConstruct ReadPersonConstruct(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomPersonConstruct person = CreatePersonConstructTemplate();
      bool readName = false;

      using (XmlReader personReader = reader.ReadSubtree()) {
        personReader.MoveToContent();

        while (personReader.MoveToNextAttribute()) {
          object ns = personReader.NamespaceURI;
          object ln = personReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              person.XmlBase = personReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              person.XmlLang = personReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(person, personReader);
          }
        }

        personReader.MoveToContent();

        while (personReader.Read()) {
          if (personReader.NodeType == XmlNodeType.Element) {
            if ((object)personReader.NamespaceURI == _nameTable.NamespaceUriAtom) {
              object localName = personReader.LocalName;

              if (localName == _nameTable.LocalNameName) {
                string name = personReader.ReadString();
                person.Name = name;
                readName = true;
              }
              else if (localName == _nameTable.LocalNameUri) {
                AtomUri uri = ReadIriReference(personReader);
                person.Uri = uri;
              }
              else if (localName == _nameTable.LocalNameEmail) {
                string addrStr = personReader.ReadString();
                MailAddress address = new MailAddress(addrStr);
                person.Email = address;
              }
              else {
                RaiseExtensionEvent(person, personReader);
              }
            }
            else {
              RaiseExtensionEvent(person, personReader);
            }
          }
        }
      }

      if (readName == false) {
        throw new AtomFormatException("Missing name of person construct.");
      }

      return person;
    }

    protected virtual AtomCategory ReadCategory(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomCategory category = CreateCategoryTemplate();
      bool readTerm = false;

      reader.MoveToContent();

      while (reader.MoveToNextAttribute()) {
        object ns = reader.NamespaceURI;
        object ln = reader.LocalName;

        if (ns == _nameTable.NamespaceUriXml) {
          if (ln == _nameTable.LocalNameBase) {
            category.XmlBase = reader.Value;
            continue;
          }
          else if (ln == _nameTable.LocalNameLang) {
            category.XmlLang = reader.Value;
            continue;
          }
        }
        else if (ns == _nameTable.NamespaceUriLocal) {
          if (ln == _nameTable.LocalNameTerm) {
            category.Term = reader.Value;
            readTerm = true;
            continue;
          }
          else if (ln == _nameTable.LocalNameScheme) {
            Uri scheme = new Uri(reader.Value, UriKind.Absolute);
            category.Scheme = scheme;
            continue;
          }
          else if (ln == _nameTable.LocalNameLable) {
            category.Label = reader.Value;
            continue;
          }
        }

        if (ns != _nameTable.NamespaceUriXmlns) {
          RaiseExtensionEvent(category, reader);
        }
      }

      if (readTerm == false) {
        throw new AtomFormatException("Missing term of category.", "category");
      }

      reader.MoveToContent();
      if (!reader.IsEmptyElement) {
        reader.Read();
      }

      return category;
    }

    protected virtual AtomGenerator ReadGenerator(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomGenerator generator = CreateGeneratorTemplate();

      using (XmlReader genReader = reader.ReadSubtree()) {
        genReader.MoveToContent();

        while (genReader.MoveToNextAttribute()) {
          object ns = genReader.NamespaceURI;
          object ln = genReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              generator.XmlBase = genReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              generator.XmlLang = genReader.Value;
              continue;
            }
          }
          else if (ns == _nameTable.NamespaceUriLocal) {
            if (ln == _nameTable.LocalNameUri) {
              Uri uri = new Uri(genReader.Value, UriKind.RelativeOrAbsolute);
              generator.Uri = uri;
              continue;
            }
            else if (ln == _nameTable.LocalNameVersion) {
              generator.Version = genReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriLocal) {
            RaiseExtensionEvent(generator, genReader);
          }
        }

        genReader.MoveToContent();

        string name = genReader.ReadString();
        generator.Name = name;

        while (genReader.Read()) { }
      }

      return generator;
    }

    protected virtual AtomContent ReadContent(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      if (reader.MoveToAttribute((string)_nameTable.LocalNameSrc, (string)_nameTable.NamespaceUriLocal)) {
        reader.MoveToContent();
        return ReadOutOfLineContent(reader);
      }

      reader.MoveToContent();

      AtomContent content = CreateContentTemplate();

      using (XmlReader innerReader = reader.ReadSubtree()) {
        innerReader.MoveToContent();

        while (innerReader.MoveToNextAttribute()) {
          object ns = innerReader.NamespaceURI;
          object ln = innerReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              content.XmlBase = innerReader.Value;
              continue;
            }

            else if (ln == _nameTable.LocalNameLang) {
              content.XmlLang = innerReader.Value;
              continue;
            }
          }
          else if (ns == _nameTable.NamespaceUriLocal) {
            if (ln == _nameTable.LocalNameType) {
              content.Type = new AtomContentType(innerReader.Value);
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(content, innerReader);
          }
        }

        innerReader.MoveToContent();

        string text;
        if (content.Type != null &&
            (content.Type == AtomContentType.XHtml ||
             content.Type.Value != null &&
             (content.Type.Value.EndsWith("/xml") ||
              content.Type.Value.EndsWith("+xml")))) {
          text = innerReader.ReadInnerXml();
        }
        else {
          text = innerReader.ReadString();
        }
        content.Text = text;

        while (innerReader.Read()) { }
      }

      return content;
    }

    protected virtual AtomOutOfLineContent ReadOutOfLineContent(XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException();
      }

      AtomOutOfLineContent content = CreateOutOfLineContentTemplate();
      bool readSrc = false;

      using (XmlReader innerReader = reader.ReadSubtree()) {
        innerReader.MoveToContent();

        while (innerReader.MoveToNextAttribute()) {
          object ns = innerReader.NamespaceURI;
          object ln = innerReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              content.XmlBase = innerReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              content.XmlLang = innerReader.Value;
              continue;
            }
          }
          else if (ns == _nameTable.NamespaceUriLocal) {
            if (ln == _nameTable.LocalNameType) {
              content.Type = new AtomContentType(innerReader.Value);
              continue;
            }
            else if (ln == _nameTable.LocalNameSrc) {
              Uri src = new Uri(innerReader.Value, UriKind.RelativeOrAbsolute);
              content.Src = src;
              readSrc = true;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(content, innerReader);
          }
        }

        if (readSrc == false) {
          throw new AtomFormatException("Cannot read element as atomOutOfLineContent because src attribute is not present.", content);
        }

        innerReader.MoveToContent();

        if (!innerReader.IsEmptyElement) {
          innerReader.Read();
          if (innerReader.NodeType != XmlNodeType.EndElement) {
            throw new AtomFormatException("The content text of atom:content with nonnull src must be empty.", "content");
          }
        }

        while (innerReader.Read()) { }
      }

      return content;
    }

    protected virtual AtomSource ReadSource(XmlReader reader) {
      AtomSource source = CreateSourceTemplate();

      using (XmlReader sourceReader = reader.ReadSubtree()) {

        sourceReader.MoveToContent();

        while (sourceReader.MoveToNextAttribute()) {
          object ns = sourceReader.NamespaceURI;
          object ln = sourceReader.LocalName;

          if (ns == _nameTable.NamespaceUriXml) {
            if (ln == _nameTable.LocalNameBase) {
              source.XmlBase = sourceReader.Value;
              continue;
            }
            else if (ln == _nameTable.LocalNameLang) {
              source.XmlLang = sourceReader.Value;
              continue;
            }
          }

          if (ns != _nameTable.NamespaceUriXmlns) {
            RaiseExtensionEvent(source, sourceReader);
          }
        }

        sourceReader.MoveToContent();

        while (sourceReader.Read()) {
          if (sourceReader.NodeType == XmlNodeType.Element) {
            if ((object)sourceReader.NamespaceURI == _nameTable.NamespaceUriAtom) {

              object localName = sourceReader.LocalName;

              if (localName == _nameTable.LocalNameLink) {
                AtomLink link = ReadLink(sourceReader);
                source.AddLink(link);
              }
              else if (localName == _nameTable.LocalNameID) {
                Uri id = ReadID(sourceReader, source);
                source.ID = id;
              }
              else if (localName == _nameTable.LocalNameTitle) {
                AtomTextConstruct title = ReadTextConstruct(sourceReader);
                source.Title = title;
              }
              else if (localName == _nameTable.LocalNameUpdated) {
                DateTime? updated = ReadDateTime(sourceReader);
                source.Updated = updated;
              }
              else if (localName == _nameTable.LocalNameAuthor) {
                AtomPersonConstruct author = ReadPersonConstruct(sourceReader);
                source.AddAuthor(author);
              }
              else if (localName == _nameTable.LocalNameCategory) {
                AtomCategory category = ReadCategory(sourceReader);
                source.AddCategory(category);
              }
              else if (localName == _nameTable.LocalNameContributor) {
                AtomPersonConstruct contributor = ReadPersonConstruct(sourceReader);
                source.AddContributor(contributor);
              }
              else if (localName == _nameTable.LocalNameGenerator) {
                AtomGenerator generator = ReadGenerator(sourceReader);
                source.Generator = generator;
              }
              else if (localName == _nameTable.LocalNameIcon) {
                AtomUri icon = ReadIriReference(sourceReader);
                source.Icon = icon;
              }
              else if (localName == _nameTable.LocalNameLogo) {
                AtomUri logo = ReadIriReference(sourceReader);
                source.Logo = logo;
              }
              else if (localName == _nameTable.LocalNameRights) {
                AtomTextConstruct rights = ReadTextConstruct(sourceReader);
                source.Rights = rights;
              }
              else if (localName == _nameTable.LocalNameSubtitle) {
                AtomTextConstruct subtitle = ReadTextConstruct(sourceReader);
                source.Subtitle = subtitle;
              }
              else {
                RaiseExtensionEvent(source, sourceReader);
              }
            }
            else {
              RaiseExtensionEvent(source, sourceReader);
            }
          }
        }
      }

      return source;
    }

    protected void RaiseExtensionEvent(AtomBaseElement readingElement, XmlReader reader) {
      AtomExtensionEventHandler temp = _readExtensionDelegate;
      if (temp != null) {
        AtomExtensionEventArgs e = new AtomExtensionEventArgs(readingElement, reader);
        temp(this, e);
      }
      else {
        if (reader.NodeType == XmlNodeType.Element) {
          using (XmlReader subReader = reader.ReadSubtree()) {
            while (subReader.Read());
          }
        }
      }
    }

    private bool RaiseDocumentTypeDetectedEvent(Type documentType) {
      DocumentTypeDetectedEventHandler temp = _documentTypeDetectedDelegate;
      if (temp != null) {
        DocumentTypeDetectedEventArgs e = new DocumentTypeDetectedEventArgs(documentType);
        temp(this, e);
        return e.HaltFurtherProcess;
      }
      else {
        return false;
      }
    }

    private static readonly Uri _dummyUri = new Uri("http://dummy/");
    private static readonly AtomTextConstruct _dummyTitle = new AtomTextConstruct("dummy", AtomContentType.Text);
    private static readonly DateTime _dummyUpdated = DateTime.MinValue;

    protected virtual AtomFeed CreateFeedTemplate() {
      return new AtomFeed(_dummyUri, _dummyTitle, _dummyUpdated);
    }

    protected virtual AtomEntry CreateEntryTemplate() {
      return new AtomEntry(_dummyUri, _dummyTitle, _dummyUpdated);
    }

    protected virtual AtomLink CreateLinkTemplate() {
      return new AtomLink(_dummyUri);
    }

    protected virtual AtomUri CreateUriTemplate() {
      return new AtomUri(_dummyUri);
    }

    protected virtual AtomTextConstruct CreateTextConstructTemplate() {
      return new AtomTextConstruct(string.Empty);
    }

    protected virtual AtomPersonConstruct CreatePersonConstructTemplate() {
      return new AtomPersonConstruct(string.Empty);
    }

    protected virtual AtomCategory CreateCategoryTemplate() {
      return new AtomCategory(string.Empty);
    }

    protected virtual AtomGenerator CreateGeneratorTemplate() {
      return new AtomGenerator(string.Empty);
    }

    protected virtual AtomContent CreateContentTemplate() {
      return new AtomContent(string.Empty);
    }

    protected virtual AtomOutOfLineContent CreateOutOfLineContentTemplate() {
      return new AtomOutOfLineContent(null, _dummyUri);
    }

    protected virtual AtomSource CreateSourceTemplate() {
      return new AtomSource();
    }

    protected AtomNameTable NameTable {
      get { return _nameTable; }
    }

    public static AtomXmlReader Plain {
      get {
        return _plainReader;
      }
    }

    private class AtomPlainReader : AtomXmlReader {
      public override event AtomExtensionEventHandler ReadExtension {
        add {
          throw new NotSupportedException();
        }
        remove {
          throw new NotSupportedException();
        }
      }

      public override event DocumentTypeDetectedEventHandler DocumentTypeDetected {
        add {
          throw new NotSupportedException();
        }
        remove {
          throw new NotSupportedException();
        }
      }
    }
  }
}
