namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Toolkit.Windows.HotKey;

    /// <summary>
    /// Provides functionality for mapping shortcuts/hot keys retrieved from *.url files and *.lnk files.
    /// E.g. 1601 is CTRL + ALT + A...
    /// </summary>
    public static class SystemHotKeyLookupTable
    {
        private static readonly IDictionary<Int32, Tuple<HotKeyModifier, Key>> urlHotKeyLookup = new Dictionary<Int32, Tuple<HotKeyModifier, Key>>();

        static SystemHotKeyLookupTable()
        {
            // Attention: Special characters are currently not supported because the MahApps HotKeyTextBox does not support at least one of them.

            const Int32 shiftKeyValue = 256;
            const Int32 controlKeyValue = 512;
            const Int32 altKeyValue = 1024;

            var allowAllPossibleCombinationsMap = new List<Tuple<Key, Int32>>();

            // A - Z
            allowAllPossibleCombinationsMap.AddRange(Enumerable.Range(65, 26).Select(number => new Tuple<Key, Int32>((Key) (number - 21), number)));

            // 0 - 9
            allowAllPossibleCombinationsMap.AddRange(Enumerable.Range(49, 9).Select(number => new Tuple<Key, Int32>((Key) (number - 15), number)));

            // F1 - F12
            var functionKeyMapping = Enumerable.Range(112, 12).Select(number => new Tuple<Key, Int32>((Key) (number - 22), number)).ToList();
            allowAllPossibleCombinationsMap.AddRange(functionKeyMapping);

            foreach (var keyMapping in allowAllPossibleCombinationsMap)
            {
                urlHotKeyLookup.Add(keyMapping.Item2 + shiftKeyValue + altKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Shift | HotKeyModifier.Alt, keyMapping.Item1));
                urlHotKeyLookup.Add(keyMapping.Item2 + shiftKeyValue + controlKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Shift | HotKeyModifier.Control, keyMapping.Item1));
                urlHotKeyLookup.Add(keyMapping.Item2 + controlKeyValue + altKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Alt | HotKeyModifier.Control, keyMapping.Item1));
                urlHotKeyLookup.Add(keyMapping.Item2 + shiftKeyValue + altKeyValue + controlKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Shift | HotKeyModifier.Alt | HotKeyModifier.Control, keyMapping.Item1));
            }

            foreach (var keyMapping in functionKeyMapping)
            {
                urlHotKeyLookup.Add(keyMapping.Item2 + shiftKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Shift, keyMapping.Item1));
                urlHotKeyLookup.Add(keyMapping.Item2 + controlKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Control, keyMapping.Item1));
                urlHotKeyLookup.Add(keyMapping.Item2 + altKeyValue, new Tuple<HotKeyModifier, Key>(HotKeyModifier.Alt, keyMapping.Item1));
            }
        }

        public static Boolean TryGetHotKey(Int32 possibleHotKey, out HotKeyModifier hotKeyModifier, out Key hotKey)
        {
            hotKeyModifier = HotKeyModifier.None;
            hotKey = Key.None;

			if (!urlHotKeyLookup.TryGetValue(possibleHotKey, out Tuple<HotKeyModifier, Key> result))
			{
				return false;
			}

			hotKeyModifier = result.Item1;
            hotKey = result.Item2;

            return true;
        }
    }
}
