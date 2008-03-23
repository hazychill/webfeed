using System;

namespace WebFeed.Atom10 {
  public delegate void DocumentTypeDetectedEventHandler(object sender, DocumentTypeDetectedEventArgs e);

  public class DocumentTypeDetectedEventArgs : EventArgs {
    private Type _documentType;
    private bool _haltFurtherProcess;
    
    public DocumentTypeDetectedEventArgs(Type documentType, bool haltFurtherProcess) {
      _documentType = documentType;
      _haltFurtherProcess = haltFurtherProcess;
    }
    
    public DocumentTypeDetectedEventArgs(Type documentType) : this(documentType, false) { }

    public Type DocumentType {
      get { return _documentType; }
    }
    
    public bool HaltFurtherProcess {
      get { return _haltFurtherProcess; }
      set { _haltFurtherProcess = value; }
    }
  }
}