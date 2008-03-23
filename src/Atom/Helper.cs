using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebFeed.Atom10 {
  public static class Helper {

    public static void AddRange<T>(IList<T> list, IEnumerable<T> addList) {
      ForEach<T>(addList, delegate(T item) {
        list.Add(item);
      });
    }

    public static IEnumerable<U> ConvertAll<T, U>(IEnumerable<T> list, Converter<T, U> conv) {
      foreach (T item in list) {
        yield return conv(item);
      }
    }

    public static bool Exists<T>(IEnumerable<T> list, Predicate<T> pred) {
      foreach (T item in list) {
        if (pred(item)) {
          return true;
        }
      }

      return false;
    }

    public static T Find<T>(IEnumerable<T> list, Predicate<T> pred) {
      foreach (T item in list) {
        if (pred(item)) {
          return item;
        }
      }

      return default(T);
    }

    public static IEnumerable<T> FindAll<T>(IEnumerable<T> list, Predicate<T> pred) {
      List<T> matchList = new List<T>();
      ForEach<T>(list, delegate(T item) {
        if (pred(item)) {
          matchList.Add(item);
        }
      });

      return matchList;
    }

    public static void ForEach<T>(IEnumerable<T> list, Action<T> action) {
      foreach (T item in list) {
        action(item);
      }
    }

    public static void RemoveAll<T>(IList<T> list, Predicate<T> pred) {
      ForEach<T>(FindAll<T>(list, pred), delegate(T item) {
        list.Remove(item);
      });
    }

    public static bool TrueForAll<T>(IEnumerable<T> list, Predicate<T> pred) {
      foreach (T item in list) {
        if (!pred(item)) {
          return false;
        }
      }

      return true;
    }

    public static void AddItem<T>(ref IList<T> list, T item) {
      if (item != null) {
        if (list == null) {
          list = new List<T>();
        }
        list.Add(item);
      }
    }

    public static IEnumerable<T> GetItems<T>(IList<T> list) {
      if (list != null) {
        foreach (T item in list) {
          yield return item;
        }
      }
    }

    public static bool IsAlternateLink(AtomLink link) {
      if (link == null) {
        return false;
      }

      if (string.IsNullOrEmpty(link.Rel)) {
        return true;
      }

      if (string.Equals(link.Rel, "alternate", StringComparison.OrdinalIgnoreCase)) {
        return true;
      }

      return false;
    }

    public static bool IsXmlType(AtomContentType type) {
      if (type == AtomContentType.XHtml) {
        return true;
      }

      Match m = Regex.Match(type.Value, "^[^/]+/(?:[^/]*?\\+)?(?<type>[^;]+)");
      if (m.Success) {
        return m.Groups["type"].Value.Equals("xml", StringComparison.OrdinalIgnoreCase);
      }

      return false;
    }
  }
}
