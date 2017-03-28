using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace CSharpControls.Charting
{
  public sealed class PlotTrend : INotifyPropertyChanged
  {
    public PlotTrend(string seriesName, Brush lineColor, Thickness pointThickness, IEnumerable<PlotPoints> points)
    {
      Points = new ObservableCollection<PlotPoints>();

      this.SeriesName = seriesName;
      this.LineColor = lineColor;
      this.PointThickness = pointThickness;
      this.Points.ClearAndAddRange(points);

      this.Points.CollectionChanged += NotifyChangedCollection;
    }

    #region SeriesName
    private string _seriesName;
    public string SeriesName
    {
      get { return _seriesName; }
      set
      {
        _seriesName = value;
        OnPropertyChanged(nameof(SeriesName));
      }
    }
    #endregion

    #region LineColor
    private Brush _lineColor;
    public Brush LineColor
    {
      get { return _lineColor; }
      set
      {
        _lineColor = value;
        OnPropertyChanged(nameof(LineColor));
      }
    } 
    #endregion

    private Thickness _pointThickness;

    public event PropertyChangedEventHandler PropertyChanged;

    public Thickness PointThickness
    {
      get => _pointThickness;
      set
      {
        _pointThickness = value;
        OnPropertyChanged(nameof(PointThickness));
      }
    }

    public ObservableCollection<PlotPoints> Points { get; }
               
    private void NotifyChangedCollection(object sender, EventArgs e)
    {
      this.OnPropertyChanged(nameof(Points));
    }
              
    public void OnPropertyChanged(string info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }
  }
}
