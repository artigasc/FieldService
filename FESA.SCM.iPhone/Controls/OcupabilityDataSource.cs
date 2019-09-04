using System;
using System.Collections.Generic;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;

namespace FESA.SCM.iPhone.Controls
{
    public class OcupabilityDataSource : SFChartDataSource
    {
        private readonly NSMutableArray _dataPoints;
        public OcupabilityDataSource()
        {
            _dataPoints = new NSMutableArray();
            var reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            SetDataForArray(reportVm.OcupabilityLevels);
        }
        public override nint GetNumberOfDataPoints(SFChart chart, nint index)
        {
            return (int)_dataPoints.Count;
        }

        public override SFSeries GetSeries(SFChart chart, nint index)
        {
            var series = new SFDoughnutSeries();
            series.DataMarker.ShowLabel = true;
            series.ExplodeOnTouch = true;
            series.DataMarker.LabelContent = SFChartLabelContent.Percentage;
            series.EnableAnimation = true;
            series.EnableDataPointSelection = true;
            return series;
        }

        public override nint NumberOfSeriesInChart(SFChart chart)
        {
            return 1;
        }

        public override SFChartDataPoint GetDataPoint(SFChart chart, nint index, nint seriesIndex)
        {
            return _dataPoints.GetItem<SFChartDataPoint>((nuint) index);
        }

        private void SetDataForArray(IEnumerable<Ocupability> ocupabilities)
        {
            foreach (var ocupability in ocupabilities)
            {
                _dataPoints.Add(new SFChartDataPoint(FromObject(Utils.GetUserStatusText(ocupability.UserStatus)),FromObject(ocupability.Percentage)));
            }
        }
    }
}