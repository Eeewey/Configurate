using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurate
{
    public class GraphConfigure
    {

        public class GraphConfig
        {
            public ViewType Type;
            public bool IsActive { get; set; }
            public FunctionSeries Series { get; set; }
        }

        private PlotModel _points { get; set; }

        public List<GraphConfig> GraphConfs { get; set; }

        public GraphConfigure()
        {
            if (GraphConfs == null)
            {
                GraphConfs = new List<GraphConfig>();
            }

            var plot = new PlotModel { Title = "Result" };

            _points = plot;
        }

        public GraphConfig GetGraphConf(ViewType type)
        {
            foreach (var graph in GraphConfs)
            {
                if (graph.Type == type) return graph;
            }

            return null;
        }

        public PlotModel GetPoints()
        {

            return _points;
        }

        public void ConfGraphPidRes(List<Data> list, string title, OxyColor color)
        {
            var points = new List<DataPoint>();

            var line = new LineSeries()
            {
                Title = title,
                Color = color,
                StrokeThickness = 2
            };

            foreach (var data in list)
            {
                var point = new DataPoint(data.Time, data.Value);

                line.Points.Add(point);
            }

            _points.Legends.Add(new Legend()
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.RightBottom,
            });


            _points.Series.Add(line);
        }
    }
}
