using System.Collections;
using NHibernate.Event.Default;
using NHibernate.Util;

namespace App.Repository.Infrastructure
{
    public class CustomNHibernateMergeEventListener : DefaultMergeEventListener
    {
        protected override IDictionary GetMergeMap(object anything)
        {
            var cache = (EventCache) anything;

            var result = IdentityMap.Instantiate(cache.Count);

            foreach (DictionaryEntry entry in cache)
            {
                result[entry.Value] = entry.Key;
            }

            return result;
        }
    }
}