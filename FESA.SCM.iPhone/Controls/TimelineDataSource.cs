using System;
using System.Collections.Generic;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;

namespace FESA.SCM.iPhone.Controls
{
    public class TimelineDataSource : SFChartDataSource
    {
        private readonly NSMutableArray _dataPoints;
        public TimelineDataSource()
        {
            _dataPoints = new NSMutableArray();
            var reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            SetDataForArray(reportVm.UserOcupabilityThroughTime);
        }
        public override nint GetNumberOfDataPoints(SFChart chart, nint index)
        {
            return (int)_dataPoints.Count;
        }

        public override SFSeries GetSeries(SFChart chart, nint index)
        {
            var series = new SFLineSeries
            {
                EnableTooltip = true,
                EnableAnimation = true
            };
            return series;
        }

        public override nint NumberOfSeriesInChart(SFChart chart)
        {
            return 1;
        }

        public override SFChartDataPoint GetDataPoint(SFChart chart, nint index, nint seriesIndex)
        {
            return _dataPoints.GetItem<SFChartDataPoint>((nuint)index);
        }
        private void SetDataForArray(IEnumerable<OcupabilityThroughTime> ocupabilities)
        {
            foreach (var ocupability in ocupabilities)
            {
                _dataPoints.Add(new SFChartDataPoint(FromObject(ocupability.AssignmentDate.Month), FromObject(ocupability.Quantity)));
            }
        }
    }
}