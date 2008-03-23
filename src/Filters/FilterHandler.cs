using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using WebFeed.Atom10;
using WebFeed.Filters;

namespace WebFeed.Filters {
  public abstract class FilterHandler : RegexFilter, IHttpHandler {
    
    public bool IsReusable {
      get { return true; }
    }
    
    public virtual void ProcessRequest(HttpContext context) {
      AtomFeed feed = this.Create();

      UriBuilder ub = new UriBuilder(context.Request.Url);
      ub.Port = -1;
      AtomLink self = new AtomLink(ub.Uri, "self");
      feed.AddLink(self);

      // Obsolated, only for compatibility
      AtomLink link = GetFeedLink();
      if (link != null) {
        if (feed.Links != null) {
          feed.Links.Clear();
        }
        feed.AddLink(link);
      }

      context.Response.ContentType = "application/atom+xml;type=feed";
      context.Response.ContentEncoding = new UTF8Encoding();
      using (TextWriter output = context.Response.Output) {
        using (XmlWriter writer = XmlWriter.Create(output)) {
          feed.WriteDocument(writer);
        }
      }
    }

    protected override Uri GetFeedID() {
      return this.SiteUri;
    }

    [Obsolete("Use WebFeed.Filters.FeedFilter.GetFeedLinks() instead.")]
    protected virtual AtomLink GetFeedLink() {
      return null;
    }

    protected override IEnumerable<AtomLink> GetFeedLinks(){
      yield return new AtomLink(this.SiteUri, "alternate");
    }
  }
}
