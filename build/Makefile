CSC20 = %windir%\Microsoft.NET\Framework\v2.0.50727\csc.exe
CSC35 = %windir%\Microsoft.NET\Framework\v3.5\csc.exe
CSC = $(CSC35)

DIR_SRC = ..\src
DIR_SRC_ATOM = $(DIR_SRC)\Atom
DIR_SRC_FILTERS = $(DIR_SRC)\Filters
DIR_SRC_TEST = $(DIR_SRC)\test

SRC_ATOM = \
	$(DIR_SRC_ATOM)\AtomBaseElement.cs \
	$(DIR_SRC_ATOM)\AtomCategory.cs \
	$(DIR_SRC_ATOM)\AtomContent.cs \
	$(DIR_SRC_ATOM)\AtomContentType.cs \
	$(DIR_SRC_ATOM)\AtomDocument.cs \
	$(DIR_SRC_ATOM)\AtomEntry.cs \
	$(DIR_SRC_ATOM)\AtomExtensionEventArgs.cs \
	$(DIR_SRC_ATOM)\AtomFeed.cs \
	$(DIR_SRC_ATOM)\AtomFormatException.cs \
	$(DIR_SRC_ATOM)\AtomGenerator.cs \
	$(DIR_SRC_ATOM)\AtomLink.cs \
	$(DIR_SRC_ATOM)\AtomNameTable.cs \
	$(DIR_SRC_ATOM)\AtomOutOfLineContent.cs \
	$(DIR_SRC_ATOM)\AtomPersonConstruct.cs \
	$(DIR_SRC_ATOM)\AtomSource.cs \
	$(DIR_SRC_ATOM)\AtomTextConstruct.cs \
	$(DIR_SRC_ATOM)\AtomUri.cs \
	$(DIR_SRC_ATOM)\AtomXmlReader.cs \
	$(DIR_SRC_ATOM)\DocumentTypeDetectedEventArgs.cs \
	$(DIR_SRC_ATOM)\Helper.cs \
	$(DIR_SRC_ATOM)\IAtomXmlReader.cs \

SRC_FILTERS = \
	$(DIR_SRC_FILTERS)\FeedFilter.cs \
	$(DIR_SRC_FILTERS)\FilterHandler.cs \
	$(DIR_SRC_FILTERS)\RegexFilter.cs \
	$(DIR_SRC_FILTERS)\SiteFilter.cs \

SRCS = $(SRC_ATOM) $(SRC_FILTERS)

release : WebFeed.dll
debug   : WebFeed_debug.dll
test    : disptest.exe dispfeed.exe dispentry.exe

WebFeed.dll : $(SRCS)
	$(CSC) /target:library /out:$@ $**

WebFeed_debug.dll : $(SRCS)
	$(CSC) /debug /d:DEBUG /target:library /out:$@ $**

disptest.exe : WebFeed_debug.dll $(DIR_SRC_TEST)\disptest.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll $(DIR_SRC_TEST)\disptest.cs

dispfeed.exe : WebFeed_debug.dll $(DIR_SRC_TEST)\dispfeed.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll $(DIR_SRC_TEST)\dispfeed.cs

dispentry.exe : WebFeed_debug.dll $(DIR_SRC_TEST)\dispentry.cs
	csc /debug /d:DEBUG /r:WebFeed_Debug.dll $(DIR_SRC_TEST)\dispentry.cs

clean :
	del WebFeed*.dll
	del WebFeed*.pdb
	del disp*.exe
	del disp*.pdb
