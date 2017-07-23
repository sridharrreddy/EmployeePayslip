namespace Services.ConfigSections
{
    public class TaxBracketConfig
    {
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public float TaxInCentsPerDollar { get; set; }
        public int BaseTax { get; set; }
    }
}
