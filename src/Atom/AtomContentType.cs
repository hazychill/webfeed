using System;

namespace WebFeed.Atom10 {
  public class AtomContentType {
    private string _value;

    private static readonly AtomContentType _typeText;
    private static readonly AtomContentType _typeHtml;
    private static readonly AtomContentType _typeXHtml;

    static AtomContentType() {
      _typeText = new AtomContentType("text");
      _typeHtml = new AtomContentType("html");
      _typeXHtml = new AtomContentType("xhtml");
    }

    public AtomContentType(string value) {
      if (value == null) {
        throw new ArgumentNullException("", "value");
      }
      _value = value;
    }

    public override bool Equals(object obj) {
      if (object.ReferenceEquals(this, obj)) {
        return true;
      }

      if (obj is AtomContentType) {
        AtomContentType cType = obj as AtomContentType;
        return _value.Equals(cType.Value, StringComparison.Ordinal);
      }
      else {
        return false;
      }
    }

    public override int GetHashCode() {
      if (_value == null) {
        return base.GetHashCode();
      }
      else {
        return _value.ToLower().GetHashCode();
      }
    }

    public static bool operator==(AtomContentType x, AtomContentType y) {
      if ((object)x != null) {
        return x.Equals(y);
      }
      else {
        return ((object)y == null);
      }
    }

    public static bool operator!=(AtomContentType x, AtomContentType y) {
      return !(x == y);
    }

    public string Value {
      get { return _value; }
    }

    public static AtomContentType Text {
      get { return _typeText; }
    }

    public static AtomContentType Html {
      get { return _typeHtml; }
    }

    public static AtomContentType XHtml {
      get { return _typeXHtml; }
    }
  }
}