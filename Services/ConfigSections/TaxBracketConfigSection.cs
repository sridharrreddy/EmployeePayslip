using System.Collections.Generic;
using System.Configuration;

namespace Services.ConfigSections
{
    public class TaxBracketConfigSection
    {
        public static string TAXBRACKETCONFIGSECTION_NAME = "taxBracketConfigSection";
        public List<TaxBracketConfig> TaxBrackets { get; set; }

        public static TaxBracketConfigSection LoadSettings()
        {
            TaxBracketConfigSection settings = (dynamic)ConfigurationManager.GetSection(TAXBRACKETCONFIGSECTION_NAME);
            return settings;
        }
    }
}
