using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSharpDataAccess.Enterprise.Models
{
  [Serializable()]
  public class DemandTrendInput
  {
    [XmlAttribute()]
    public string Grouping { get; set; }
    [XmlAttribute()]
    public int FIKey { get; set; }
    [XmlAttribute()]
    public DateTime StartDate { get; set; }
    [XmlAttribute()]
    public DateTime EndDate { get; set; }

    public List<int> DemandLocations { get; set; }

    public DemandTrendInput()
    {
    }

    public DemandTrendInput(int fiKey, DateTime startDate, DateTime endDate, string grouping, List<int> demandLocations)
    {
      FIKey = fiKey;
      StartDate = startDate;
      EndDate = endDate;
      Grouping = grouping;
      DemandLocations = demandLocations;
    }
  }
}
