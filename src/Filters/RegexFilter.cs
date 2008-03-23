using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using WebFeed.Atom10;

namespace WebFeed.Filters {
  public abstract class RegexFilter : SiteFilter {

    private static Dictionary<char, char> _s_numberMap;
    static HashAlgorithm _s_hashAlgorithm;

    static RegexFilter() {
      _s_numberMap = new Dictionary<char, char>();
      _s_numberMap.Add('0', '0');
      _s_numberMap.Add('1', '1');
      _s_numberMap.Add('2', '2');
      _s_numberMap.Add('3', '3');
      _s_numberMap.Add('4', '4');
      _s_numberMap.Add('5', '5');
      _s_numberMap.Add('6', '6');
      _s_numberMap.Add('7', '7');
      _s_numberMap.Add('8', '8');
      _s_numberMap.Add('9', '9');
      _s_numberMap.Add('‚O', '0');
      _s_numberMap.Add('‚P', '1');
      _s_numberMap.Add('‚Q', '2');
      _s_numberMap.Add('‚R', '3');
      _s_numberMap.Add('‚S', '4');
      _s_numberMap.Add('‚T', '5');
      _s_numberMap.Add('‚U', '6');
      _s_numberMap.Add('‚V', '7');
      _s_numberMap.Add('‚W', '8');
      _s_numberMap.Add('‚X', '9');
      _s_numberMap.Add('—ë', '0');
      _s_numberMap.Add('ˆê', '1');
      _s_numberMap.Add('“ñ', '2');
      _s_numberMap.Add('ŽO', '3');
      _s_numberMap.Add('Žl', '4');
      _s_numberMap.Add('ŒÜ', '5');
      _s_numberMap.Add('˜Z', '6');
      _s_numberMap.Add('Žµ', '7');
      _s_numberMap.Add('”ª', '8');
      _s_numberMap.Add('‹ã', '9');

      _s_hashAlgorithm = MD5.Create();
    }

    protected override IEnumerable<AtomEntry> GetFeedEntries(string source) {
      if (!string.IsNullOrEmpty(source)) {
        Regex regex = GetRegex();
        MatchCollection matches = regex.Matches(source);
        
        int entryCount = Math.Min(MaxEntry, matches.Count);

        int matchIndex = (ReverseEntry) ? (matches.Count-1) : (0);

        while (entryCount > 0) {
          if (matchIndex<0 || matches.Count<=matchIndex) {
            break;
          }

          AtomEntry entry = GetFeedEntry(matches[matchIndex]);
          matchIndex = (ReverseEntry) ? (matchIndex-1) : (matchIndex+1);

          if (entry.Updated > MinEntryUpdated) {
            entryCount--;
            yield return entry;
          }
        }
      }
    }

    protected virtual AtomEntry GetFeedEntry(Match match) {
      // atomId
      Uri id = GetEntryID(match);
      // atomTitle
      AtomTextConstruct title = GetEntryTitle(match);
      // atomUpdated
      DateTime updated = GetEntryUpdated(match);

      AtomEntry entry = new AtomEntry(id, title, updated);

      // atomAuthor*
      // Obsoleted, only for compatibility
      entry.AddAuthor(GetEntryAuthor(match));
      if (entry.Authors == null) {
        foreach (AtomPersonConstruct author in GetEntryAuthors(match)) {
          entry.AddAuthor(author);
        }
      }

      // atomCategory*
      // Obsoleted, only for compatibility
      entry.AddCategory(GetEntryCategory(match));
      if (entry.Categories == null) {
        foreach (AtomCategory category in GetEntryCategories(match)) {
          entry.AddCategory(category);
        }
      }

      // atomContent?
      entry.Content = GetEntryContent(match);

      // atomContributor*
      // Obsoleted, only for compatibility
      entry.AddContributor(GetEntryContributor(match));
      if (entry.Contributors == null) {
        foreach (AtomPersonConstruct contributor in GetEntryContributors(match)) {
          entry.AddContributor(contributor);
        }
      }

      // atomLink*
      // Obsoleted, only for compatibility
      entry.AddLink(GetEntryLink(match));
      if (entry.Links == null) {
        foreach (AtomLink link in GetEntryLinks(match)) {
          entry.AddLink(link);
        }
      }

      // atomPublished?
      entry.Published = GetEntryPublished(match);

      // atomRights?
      entry.Rights = GetEntryRights(match);

      // atomSource?
      entry.Source = GetEntrySource(match);

      // atomSummary?
      entry.Summary = GetEntrySummary(match);

      return entry;
    }

    protected virtual Uri GetEntryID(Match match) {
      Uri id;

      if (Uri.TryCreate(match.Groups["id"].Value, UriKind.Absolute, out id)) {
        return id;
      }

      if (match.Groups["fragment"].Success) {
        return GetFragmentUri(match);
      }

      string idElement = GetIDElement(match);
      
      string uriString = string.Format("tag:{0},{1}:{2}",
                                       SiteUri.Host,
                                       2007,
                                       GetIDElement(match));
      return new Uri(uriString);
    }


    protected virtual string GetIDElement(Match match) {
      if (match.Groups["idelement"].Success) {
        return match.Groups["idelement"].Value;
      }

      string titleText = GetEntryTitle(match).Text;
      string timeText;
      DateTime? entryPublished = GetEntryPublished(match);
      if (entryPublished.HasValue) {
        timeText = entryPublished.Value.Ticks.ToString();
      }
      else {
        timeText = GetEntryUpdated(match).Ticks.ToString();
      }
      byte[] idBytes = new UTF8Encoding().GetBytes(string.Join("/", new string[] { timeText, titleText }));
      byte[] hashBytes;
      lock (_s_hashAlgorithm) {
        hashBytes = _s_hashAlgorithm.ComputeHash(idBytes);
      }
      return GetByteArrayHexString(hashBytes);
    }

    protected string GetByteArrayHexString(byte[] bytes) {
      char[] stringBase = new char[bytes.Length*2];
      for (int i = 0; i < bytes.Length; i++) {
        string byteStr = bytes[i].ToString("x2");;
        stringBase[2*i] = byteStr[0];
        stringBase[2*i+1] = byteStr[1];
      }

      return new string(stringBase);
    }

    protected virtual Uri GetFragmentUri(Match match) {
      string fragment = match.Groups["fragment"].Value;
      UriBuilder uriBldr = new UriBuilder(SiteUri);
      uriBldr.Port = -1;
      uriBldr.Fragment = fragment;

      return uriBldr.Uri;
    }
    
    protected virtual AtomTextConstruct GetEntryTitle(Match match) {
      return match.Groups["title"].Value;
    }

    protected virtual DateTime GetEntryUpdated(Match match) {
      string yearString   = ConvertNumberString(match.Groups["year"].Value);
      string monthString  = ConvertNumberString(match.Groups["month"].Value);
      string dayString    = ConvertNumberString(match.Groups["day"].Value);
      string hourString   = ConvertNumberString(match.Groups["hour"].Value);
      string minuteString = ConvertNumberString(match.Groups["minute"].Value);
      string secondString = ConvertNumberString(match.Groups["second"].Value);

      if (string.IsNullOrEmpty(monthString) ||
          string.IsNullOrEmpty(dayString)) {
        throw new ArgumentException("match");
      }

      int month = int.Parse(monthString);
      int day = int.Parse(dayString);

      int year;
      if (string.IsNullOrEmpty(yearString)) {
        DateTime now = DateTime.Now;
        if (now.Month >= month) {
          year = now.Year;
        }
        else {
          year = now.Year - 1;
        }
      }
      else {
        year = int.Parse(yearString);
      }

      DateTime updated = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);

      if (!string.IsNullOrEmpty(hourString)) {
        updated = updated.AddHours(double.Parse(hourString));
      }
      if (!string.IsNullOrEmpty(minuteString)) {
        updated = updated.AddMinutes(double.Parse(minuteString));
      }
      if (!string.IsNullOrEmpty(secondString)) {
        updated = updated.AddSeconds(double.Parse(secondString));
      }

      return updated;
    }

    private string ConvertNumberString(string inputStr) {
      char[] outputWorkArray = new char[inputStr.Length];
      for (int i = 0; i < inputStr.Length; i++) {
        char cIn = inputStr[i];
        char cOut;
        if (_s_numberMap.TryGetValue(cIn, out cOut)) {
          outputWorkArray[i] = cOut;
        }
        else {
          throw new FormatException();
        }
      }

      return new string(outputWorkArray);
    }

    [Obsolete("Use WebFeed.Filters.RegexFilter.GetEntryAuthors(Match match) instead.")]
    protected virtual AtomPersonConstruct GetEntryAuthor(Match match) {
      return null;
    }

    protected virtual IEnumerable<AtomPersonConstruct> GetEntryAuthors(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["author"].Value)) {
        yield return new AtomPersonConstruct(match.Groups["author"].Value);
      }
      else {
        yield break;
      }
    }

    [Obsolete("Use WebFeed.Filters.RegexFilter.GetEntryCategories(Match match) instead.")]
    protected virtual AtomCategory GetEntryCategory(Match match) {
      return null;
    }

    protected virtual IEnumerable<AtomCategory> GetEntryCategories(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["category"].Value)) {
        yield return new AtomCategory(match.Groups["category"].Value);
      }
      else {
        yield break;
      }
    }

    [Obsolete("Use WebFeed.Filters.RegexFilter.GetEntryContributors(Match match) instead.")]
    protected virtual AtomPersonConstruct GetEntryContributor(Match match) {
      return null;
    }

    protected virtual IEnumerable<AtomPersonConstruct> GetEntryContributors(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["contributor"].Value)) {
        yield return new AtomPersonConstruct(match.Groups["contributor"].Value);
      }
      else {
        yield break;
      }
    }

    [Obsolete("Use WebFeed.Filters.RegexFilter.GetEntryLinks(Match match) instead.")]
    protected virtual AtomLink GetEntryLink(Match match) {
      return null;
    }

    protected virtual IEnumerable<AtomLink> GetEntryLinks(Match match) {
      Uri href;

      if (Uri.TryCreate(match.Groups["link"].Value, UriKind.Absolute, out href)) {
        yield return new AtomLink(href, "alternate");
      }

      if (match.Groups["fragment"].Success) {
        href = GetEntryID(match);
        yield return new AtomLink(href, "alternate");
      }

      yield break;
    }

    protected virtual AtomTextConstruct GetEntryRights(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["rights"].Value)) {
        return new AtomTextConstruct(match.Groups["rights"].Value);
      }
      else {
        return null;
      }
    }

    protected virtual AtomContent GetEntryContent(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["content"].Value)) {
        string text = CheckRelativeUrl(match.Groups["content"].Value);
        return new AtomContent(text, AtomContentType.Html);
      }
      else {
        return null;
      }
    }

    protected virtual AtomSource GetEntrySource(Match match) {
      return null;
    }

    protected virtual AtomTextConstruct GetEntrySummary(Match match) {
      if (!string.IsNullOrEmpty(match.Groups["summary"].Value)) {
        string text = CheckRelativeUrl(match.Groups["summary"].Value);
        return new AtomContent(text, AtomContentType.Html);
      }
      else {
        return null;
      }
    }

    static Regex relativeUrlRegex = new Regex("(?i:(?<attr>src|href))(?:\\s*=\\s*\"(?!http://)(?<link>[^\"]+)\"|\\s*=\\s*\'(?!http://)(?<link>[^\']+)\'|=(?!http://|[\'\"])(?<link>[^\\s]+))");

    public string CheckRelativeUrl(string text) {
      Uri baseUri = SiteUri;
      return relativeUrlRegex.Replace(text,
                                      delegate (Match cm) {
                                        string r = cm.Groups["link"].Value;
                                        Uri relativeUri;
                                        if (Uri.TryCreate(r, UriKind.Relative, out relativeUri)) {
                                          Uri uri = new Uri(baseUri, r);
                                          return string.Format("{0}=\"{1}\"", cm.Groups["attr"], uri.AbsoluteUri);
                                        }
                                        else {
                                          return cm.Value;
                                        }
                                      });
    }
    
    protected virtual DateTime? GetEntryPublished(Match match) {
      return null;
    }

    protected virtual Regex GetRegex() {
      return new Regex(Pattern, RegexOptions.Singleline);
    }

    protected virtual int MaxEntry { 
      get { return int.MaxValue; }
    }

    protected virtual bool ReverseEntry {
      get { return false; }
    }

    protected virtual DateTime MinEntryUpdated {
      get { return DateTime.MinValue; }
    }

    protected abstract string Pattern { get; }
  }
}
