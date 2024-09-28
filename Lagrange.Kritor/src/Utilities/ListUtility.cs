using System.Collections.Generic;

namespace Lagrange.Kritor.Utilities;

public static class ListUtility {
    public static List<TItem> AddWithReturnSelf<TItem>(this List<TItem> list, TItem item) {
        list.Add(item);
        return list;
    }
}