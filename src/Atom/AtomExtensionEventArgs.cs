using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public delegate void AtomExtensionEventHandler(object sender, AtomExtensionEventArgs e);

  public class AtomExtensionEventArgs : EventArgs {
    private AtomBaseElement _readingElement;
    private XmlNode _extensionNode;

    private static XmlDocument _doc;

    public AtomExtensionEventArgs(AtomBaseElement readingElement, XmlNode extensionNode) : base() {
      _readingElement = readingElement;
      _extensionNode = extensionNode;
    }

    public AtomExtensionEventArgs(AtomBaseElement readingElement, XmlReader reader) {
      if (reader.NodeType != XmlNodeType.Attribute && reader.NodeType != XmlNodeType.Element) {
        throw new InvalidOperationException("The reader must be positioned on start element node or attribute node when initializing AtomExtensionEventArgs.");
      }
      
      _readingElement = readingElement;
      
      if (_doc == null) {
        _doc = new XmlDocument();
      }
      _extensionNode = _doc.ReadNode(reader);
    }

    public AtomBaseElement ReadingElement {
      get { return _readingElement; }
    }

    public XmlNode ExtensionNode {
      get { return _extensionNode; }
    }
  }
}