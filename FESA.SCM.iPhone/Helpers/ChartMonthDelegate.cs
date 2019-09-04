using System;
using Foundation;
using Syncfusion.SfChart.iOS;

namespace FESA.SCM.iPhone.Helpers
{
    public class ChartMonthDelegate : SFChartDelegate
    {
        public override NSString GetFormattedAxisLabel(SFChart chart, NSString label, NSObject value, SFAxis axis)
        {
            if (!axis.Equals(chart.PrimaryAxis)) return label;

            var formattedLabel = GetMonthString(label);
            label = new NSString(formattedLabel);
            return label;
        }

        private string GetMonthString(string label)
        {
            string month;
            switch (label)
            {
                case "1":
                    month = "Enero";
                    break;
                case "2":
                    month = "Febrero";
                    break;
                case "3":
                    month = "Marzo";
                    break;
                case "4":
                    month = "Abril";
                    break;
                case "5":
                    month = "Mayo";
                    break;
                case "6":
                    month = "Junio";
                    break;
                case "7":
                    month = "Julio";
                    break;
                case "8":
                    month = "Agosto";
                    break;
                case "9":
                    month = "Septiembre";
                    break;
                case "10":
                    month = "Octubre";
                    break;
                case "11":
                    month = "Noviembre";
                    break;
                case "12":
                    month = "Diciembre";
                    break;
                default:
                    month = string.Empty;
                    break;
            }
            return month;
        }
    }
}