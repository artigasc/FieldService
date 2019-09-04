using System;
using System.Collections.Generic;
using System.Linq;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class OrdersDataSource : SFChartDataSource
    {
        private readonly NSMutableArray _fieldDataPoints;
        private readonly NSMutableArray _scheduledDataPoints;
        public OrdersDataSource()
        {
            _fieldDataPoints = new NSMutableArray();
            _scheduledDataPoints = new NSMutableArray();
            var reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            SetDataForArray(reportVm.OrdersPerClient.Where(x => x.AssignmentType == Enumerations.AssignmentType.FieldService), _fieldDataPoints);
            SetDataForArray(reportVm.OrdersPerClient.Where(x => x.AssignmentType == Enumerations.AssignmentType.Scheduled), _scheduledDataPoints);
        }
        public override nint GetNumberOfDataPoints(SFChart chart, nint index)
        {
            return (int)_fieldDataPoints.Count;
        }

        public override SFSeries GetSeries(SFChart chart, nint index)
        {
            var series = new SFBarSeries {EnableTooltip = true};
            series.DataMarker.ShowLabel = true;
            series.DataMarker.LabelStyle.Font = UIFont.BoldSystemFontOfSize(10);
            series.LegendIcon = SFChartLegendIcon.Rectangle;
            series.EnableAnimation = true;
            series.EnableDataPointSelection = true;
            series.Label = index == 1 ? new NSString("Servicio de Campo") : new NSString("Programado");
            return series;
        }

        public override nint NumberOfSeriesInChart(SFChart chart)
        {
            return 2;
        }

        public override SFChartDataPoint GetDataPoint(SFChart chart, nint index, nint seriesIndex)
        {
            return seriesIndex == 1
                ? _fieldDataPoints.GetItem<SFChartDataPoint>((nuint) index)
                : _scheduledDataPoints.GetItem<SFChartDataPoint>((nuint) index);
        }

        private void SetDataForArray(IEnumerable<OrdersPerClient> orders, NSMutableArray array)
        {
            foreach (var order in orders)
            {
                array.Add(new SFChartDataPoint(FromObject(order.CompanyName), FromObject(order.Quantity)));
            }
        }
    }
}