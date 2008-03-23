CSC20 = %windir%\Microsoft.NET\Framework\v2.0.50727\csc.exe
CSC35 = %windir%\Microsoft.NET\Framework\v3.5\csc.exe
CSC = $(CSC35)

CSFILES_ATOM = \
	Atom\AtomBaseElement.cs \
	Atom\AtomCategory.cs \
	Atom\AtomContent.cs \
	Atom\AtomContentType.cs \
	Atom\AtomDocument.cs \
	Atom\AtomEntry.cs \
	Atom\AtomExtensionEventArgs.cs \
	Atom\AtomFeed.cs \
	Atom\AtomFormatException.cs \
	Atom\AtomGenerator.cs \
	Atom\AtomLink.cs \
	Atom\AtomNameTable.cs \
	Atom\AtomOutOfLineContent.cs \
	Atom\AtomPersonConstruct.cs \
	Atom\AtomSource.cs \
	Atom\AtomTextConstruct.cs \
	Atom\AtomUri.cs \
	Atom\AtomXmlReader.cs \
	Atom\DocumentTypeDetectedEventArgs.cs \
	Atom\Helper.cs \
	Atom\IAtomXmlReader.cs \

CSFILES_FILTERS = \
	Filters\FeedFilter.cs \
	Filters\FilterHandler.cs \
	Filters\RegexFilter.cs \
	Filters\SiteFilter.cs \

release : WebFeed.dll
debug   : WebFeed_debug.dll
test    : disptest.exe dispfeed.exe dispentry.exe

WebFeed.dll : $(CSFILES_ATOM) $(CSFILES_FILTERS)
	$(CSC) /target:library /out:$@ $**

WebFeed_debug.dll : $(CSFILES_ATOM) $(CSFILES_FILTERS)
	$(CSC) /debug /d:DEBUG /target:library /out:$@ $**

disptest.exe : WebFeed_debug.dll disptest.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll disptest.cs

dispfeed.exe : WebFeed_debug.dll dispfeed.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll dispfeed.cs

dispentry.exe : WebFeed_debug.dll dispentry.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll dispentry.cs

clean :
	del WebFeed*.dll
	del WebFeed*.pdb
	del disp*.exe
	del disp*.pdb
