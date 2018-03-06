using System;
using System.Collections;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class ThemePalettePipeBind
    {
        public ThemePalettePipeBind(Hashtable palette)
        {
            this.themePalette = new Dictionary<string, string>(palette.Count);
            foreach (object obj in palette)
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
                string text = dictionaryEntry.Key as string;
                string value = dictionaryEntry.Value as string;
                if (!string.IsNullOrEmpty(text))
                {
                    this.themePalette.Add(text, value);
                }
            }
        }

        public ThemePalettePipeBind(IDictionary<string, string> palette)
        {
            this.themePalette = palette;
        }

        public IDictionary<string, string> ThemePalette
        {
            get
            {
                return this.themePalette;
            }
        }

        private IDictionary<string, string> themePalette;
    }
}
