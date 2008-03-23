using System;

namespace WebFeed.Atom10 {
  public class AtomFormatException : Exception {
    private string _currentName;

    public AtomFormatException(string message, string currentName, Exception innerException) : base(message, innerException) {
      _currentName = currentName;
    }

    public AtomFormatException(string message, string currentName) : this(message, currentName, null) { }

    public AtomFormatException(string message, Exception innerException) : this(message, null, innerException) { }

    public AtomFormatException(string message) : this(message, null, null) { }
    
    public AtomFormatException() : this(null, null, null) { }

    public string CurrentName {
      get { return _currentName; }
    }
  }
}