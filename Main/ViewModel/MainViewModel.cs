using CSharpControls;
using CSharpControls.Business;
using CSharpControls.Charting;
using CSharpControls.Converters.Instances;
using CSharpControls.Types;
using CSharpDataAccess.Enterprise;
using CSharpDataAccess.Enterprise.Models;
using Main.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Main
{
  public class MainViewModel : BaseViewModel
  {
    private List<PlotPoints> _lastPoints = new List<PlotPoints>();
    private int _countsTest;
    private bool _loaded = false;

    public MainViewModel()
    {
      InstanceConverter = new InstanceInSetToStringConverter();
      LocationCollection = new ObservableCollectionContentNotifying<DemandLocation>();
      ChartData = new ObservableCollectionContentNotifying<PlotTrend>();

      LocationCollection.ClearAndAddRange(Selects.GetDemandLocations().Take(5));
      var locs = new List<int> { 18, 55 };
      locs.ForEach(x => LocationCollection.Single(y => y.LocationID == x).IsUsed = true);
      UpdateHeader();
      SelectedItem = TrendChoices.FiscalPeriod;
      UpdateChartData();
      LocationCollection.OnCollectionItemChanged += UpdateHeader;
    }

    #region Properties
    public List<int> SelectedLocations
    {
      get => LocationCollection.Any() ? LocationCollection.Where(x => x.IsUsed == true).Select(x => x.LocationID).ToList() : null;
    }

    public TrendChoices[] Array
    {
      get => (TrendChoices[])Enum.GetValues(typeof(TrendChoices));
    }

    public InstanceInSetToStringConverter InstanceConverter { get; }
    public ObservableCollectionContentNotifying<PlotTrend> ChartData { get; }
    public ObservableCollectionContentNotifying<DemandLocation> LocationCollection { get; }

    #region Open
    private bool _open;
    public bool Open
    {
      get { return _open; }
      set
      {
        _open = value;
        if (_loaded & !Open)
        {
          //Dim s = String.Empty
          //SelectedLocations.ForEach(Sub(x) s += x.ToString + Environment.NewLine)
          //MessageBox.Show(s)
          UpdateHeader();
        }
        OnPropertyChanged(nameof(Open));
        _loaded = true;
      }
    }
    #endregion

    #region TestText
    private string _testText;
    public string TestText
    {
      get { return _testText; }
      set
      {
        _testText = value;
        OnPropertyChanged(nameof(TestText));
      }
    }
    #endregion

    #region LocationHeader
    private string _LocationHeader;
    public string LocationHeader
    {
      get { return _LocationHeader; }
      set
      {
        _LocationHeader = value;
        OnPropertyChanged(nameof(LocationHeader));
      }
    }
    #endregion

    #region SelectedItem
    private TrendChoices _selectedItem;
    public TrendChoices SelectedItem
    {
      get { return _selectedItem; }
      set
      {
        _selectedItem = value;
        OnPropertyChanged(nameof(SelectedItem));
        UpdateChartData();
      }
    }
    #endregion

    #region SelectedLocation
    private DemandLocation _selectedLocation;
    public DemandLocation SelectedLocation
    {
      get { return _selectedLocation; }
      set
      {
        _selectedLocation = value;
        OnPropertyChanged(nameof(SelectedLocation));
      }
    }
    #endregion

    #region XTicks
    private int _xTicks;
    public int XTicks
    {
      get { return _xTicks; }
      set
      {
        _xTicks = value;
        OnPropertyChanged(nameof(XTicks));
      }
    }
    #endregion

    #region Text
    private string _text;

    public string Text
    {
      get { return _text; }
      set
      {
        _text = value;
        OnPropertyChanged(nameof(Text));
      }
    }
    #endregion
    #endregion

    #region Commands
    public DelegateCommand<DemandLocation> CommandSelectedLocation { get; }

    private void CommandSelectedLocationExecute(DemandLocation obj)
    {
      SelectedLocation = obj;
    }

    public DelegateCommand<string> TestCommand { get; }

    private void TestCommandExecute(string input)
    {
      LocationCollection.ClearAndAddRange(Selects.GetDemandLocations().Take(10));
    }
    #endregion

    #region Methods
    private void UpdateHeader(object sender, ObservableCollectionContentChangedArgs e)
    {
      UpdateHeader();
    }

    private void UpdateHeader()
    {
      var itemsSelected = LocationCollection.Where(x => x.IsUsed == true).Select(x => x.ToString());
      var headerUpdated = itemsSelected.Any() ? string.Join(", ", itemsSelected) : "No Items";
      LocationHeader = headerUpdated;
    }

    #region "Line Graph parts"
    public void UpdateChartData()
    {
      var input = new DemandTrendInput(2278, new DateTime(2017, 2, 25), new DateTime(2017, 5, 1), SelectedItem.ToString(), new List<int> { 2, 25 });
      var serializedInput = input.SerializeToXml();

      var demands = Selects.GetDemandTrends(serializedInput);

      var demand = demands.Select(x => new PlotPoints(new PlotPoint<double>(x.Grouping), new PlotPoint<decimal>(x.DemandQty)));
      var ad = demands.Select(x => new PlotPoints(new PlotPoint<double>(x.Grouping), new PlotPoint<decimal>(x.DemandAdQty)));

      if (InstanceConverter != null)
      {
        XTicks = demands.Count > 0 ? demands.Count - 1 : 1;
        InstanceConverter.OptionalHeader = SelectedItem.ToString();
        InstanceConverter.FirstPosition = demands.Select(x => x.Grouping).First();

        ChartData.ClearAndAddRange(new List<PlotTrend> { new PlotTrend("Demand", Brushes.Blue, new Thickness(2), demand), new PlotTrend("Ad", Brushes.Red, new Thickness(2), ad) });
      }
    }


    private void AddingLinesForLineChart()
    {
      _lastPoints = new List<PlotPoints>{
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 12)), new PlotPoint<double>(1200)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 12)), new PlotPoint<double>(1200)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 10)), new PlotPoint<double>(950))
      };

      var o = new ObservableCollection<PlotPoints>{
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 5)), new PlotPoint<double>(400)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 11)), new PlotPoint<double>(800)),
      _lastPoints[0]
      };

      var o2 = new List<PlotPoints>{
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 5)), new PlotPoint<double>(150)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 11)), new PlotPoint<double>(720)),
      _lastPoints[1]
      };

      var o3 = new List<PlotPoints>{
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 1)), new PlotPoint<double>(300)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 5)), new PlotPoint<double>(1720)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 7)), new PlotPoint<double>(420)),
      new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 8)), new PlotPoint<double>(920)),
      _lastPoints[2]
      };

      var o4 = new List<PlotPoints> { new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 5)), new PlotPoint<double>(1920)) };
      var o5 = new List<PlotPoints> { new PlotPoints(new PlotPoint<DateTime>(new DateTime(2017, 2, 5)), new PlotPoint<double>(2920)) };

      ChartData.ClearAndAddRange(new List<PlotTrend>{
        new PlotTrend("First", Brushes.Blue, new Thickness(2), o),
        new PlotTrend("Second", Brushes.Red, new Thickness(2), o2),
        new PlotTrend("Third", Brushes.Purple, new Thickness(2), o3),
        new PlotTrend("Fourth", Brushes.Brown, new Thickness(2), o4)
      });

      var distinctCounts = (ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).Distinct().Count() - 1);
      XTicks = distinctCounts > 0 ? distinctCounts : 1;
    }

    private void LinePlotAdding()
    {
      dynamic newPoints = new List<PlotPoints>();
      

      for (int i = 1; i <= _lastPoints.Count; i++)
      {
        newPoints.Add(new PlotPoints(new PlotPoint<DateTime>((((PlotPoint<DateTime>)_lastPoints[i - 1].X).Point).AddDays(1)), new PlotPoint<double>(((PlotPoint<double>)_lastPoints[i - 1].Y).Point * 1.95)));
      }

      _lastPoints = newPoints;
      ChartData[0].Points.Add(_lastPoints[0]);
      ChartData[1].Points.Add(_lastPoints[1]);

      dynamic distinctCounts = (ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).Distinct().Count() - 1);
      XTicks = distinctCounts > 0 ? distinctCounts : 1;
    }

    #endregion
    #endregion
  }
}
