using System.Text;

namespace Services.Models
{
    public class PayslipInfo
    {
        public string Name { get; set; }
        public string PayPeriod { get; set; }
        public int GrossIncome { get; set; }
        public int IncomeTax { get; set; }
        public int NetIncome
        {
            get
            {
                return GrossIncome - IncomeTax;
            }
        }
        public int Super { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("PayslipInfo::");
            builder.AppendFormat("Name:{0};", Name);
            builder.AppendFormat("PayPeriod:{0};", PayPeriod);
            builder.AppendFormat("GrossIncome:{0};", GrossIncome);
            builder.AppendFormat("IncomeTax:{0};", IncomeTax);
            builder.AppendFormat("Super:{0};", Super);
            builder.AppendFormat("NetIncome:{0};", NetIncome);
            return builder.ToString();
        }
    }
}
