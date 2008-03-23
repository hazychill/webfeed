using System;
using System.Collections.Generic;
using WebFeed.Atom10;

namespace WebFeed.Filters {
  public abstract class FeedFilter {
    public AtomFeed Create() {
      AtomFeed feed = new AtomFeed(GetFeedID(), GetFeedTitle(), DateTime.MinValue);

      foreach (AtomPersonConstruct author in GetFeedAuthors()) {
        feed.AddAuthor(author);
      }

      foreach (AtomCategory category in GetFeedCategories()) {
        feed.AddCategory(category);
      }

      foreach (AtomPersonConstruct contributor in GetFeedContributors()) {
        feed.AddContributor(contributor);
      }

      feed.Generator = GetFeedGenerator();

      feed.Icon = GetFeedIcon();

      foreach (AtomLink link in GetFeedLinks()) {
        feed.AddLink(link);
      }

      feed.Logo = GetFeedLogo();

      feed.Rights = GetFeedRights();

      feed.Subtitle = GetFeedSubtitle();

      foreach (AtomEntry entry in GetFeedEntries()) {
        feed.Entries.Add(entry);
        if (entry.Updated > feed.Updated) {
          feed.Updated = entry.Updated;
        }
      }

      if (feed.Entries.Count == 0) {
        feed.Updated = GetFeedUpdated();
      }

      return feed;
    }

    protected virtual IEnumerable<AtomPersonConstruct> GetFeedAuthors() { yield break; }
    protected virtual IEnumerable<AtomCategory> GetFeedCategories() { yield break; }
    protected virtual IEnumerable<AtomPersonConstruct> GetFeedContributors() { yield break; }
    protected virtual AtomGenerator GetFeedGenerator() { return null; }
    protected virtual AtomUri GetFeedIcon() { return null; }
    protected abstract Uri GetFeedID();
    protected virtual IEnumerable<AtomLink> GetFeedLinks() { yield break; }
    protected virtual AtomUri GetFeedLogo() { return null; }
    protected virtual AtomTextConstruct GetFeedRights() { return null; }
    protected virtual AtomTextConstruct GetFeedSubtitle() { return null; }
    protected abstract AtomTextConstruct GetFeedTitle();
    protected virtual DateTime GetFeedUpdated() { return DateTime.Now; }
    protected abstract IEnumerable<AtomEntry> GetFeedEntries();
  }
}
