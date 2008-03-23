using System;
using System.Xml;

namespace WebFeed.Atom10 {
  public interface IAtomXmlReader {
    AtomFeed ReadFeed(XmlReader reader);
    AtomEntry ReadEntry(XmlReader reader);
    AtomDocument ReadDocument(XmlReader reader);
  }
}