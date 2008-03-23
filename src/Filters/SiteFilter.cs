using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using WebFeed.Atom10;

namespace WebFeed.Filters {
  public abstract class SiteFilter : FeedFilter {
    protected override IEnumerable<AtomEntry> GetFeedEntries() {
      string source = GetSource();
      return GetFeedEntries(source);
    }

    protected abstract IEnumerable<AtomEntry> GetFeedEntries(string source);

    protected virtual string GetSource() {
      using (WebClient client = new WebClient()) {
        client.Encoding = SiteEncoding;
        return client.DownloadString(SiteUri);
      }
    }

    protected abstract Uri SiteUri { get; }
    protected abstract Encoding SiteEncoding { get; }
  }
}
