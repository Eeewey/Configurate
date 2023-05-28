using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Configurate
{
    public class GraphVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        

        /*public void ConfGraph(IList<CheckBoxData> list)
        {
            if (list == null) return;

            foreach(var data in list)
            {
                if(data.IsChecked)
                {
                    var series = new FunctionSeries();
                    var line = new LineSeries();

                    switch (data.Type)
                    {
                        case ViewType.PReg: 
                            series = new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos");

                            line = new LineSeries()
                            {
                                Title = series.Title,
                                Color = OxyColors.Red,
                                StrokeThickness = 2
                            };
                            break;
                        case ViewType.IReg:
                            series = new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin");

                            line = new LineSeries()
                            {
                                Title = series.Title,
                                Color = OxyColors.Green,
                                StrokeThickness = 2
                            };
                            break;
                        case ViewType.DReg:
                            series = new FunctionSeries(Math.Tan, 0, 10, 0.1, "tan");

                            line = new LineSeries()
                            {
                                Title = series.Title,
                                Color = OxyColors.Blue,
                                StrokeThickness = 2
                            };
                            break;
                    }

                    foreach(var point in series.Points)
                    {
                        line.Points.Add(point);
                    }

                    Points.Legends.Add(new Legend()
                    {
                        LegendTitle = "Legend",
                        LegendPosition = LegendPosition.RightBottom,
                    });

                    GraphConfs.Add(new GraphConf
                    {
                        Type = data.Type,
                        IsActive = data.IsChecked,
                        Series = series
                    });

                    Points.Series.Add(line);
                }
                else if (!data.IsChecked)
                {
                    var conf = GetGraphConf(data.Type);

                    if (conf == null) continue;

                    Points.Series.Remove(conf.Series);

                    GraphConfs.Remove(conf);
                }
            }
        }*/
    }
}
