Imports System.Xml.Serialization

<Serializable>
Public Class DemandTrendInput
  <XmlAttribute>
  Public Property FIKey As Integer
  <XmlAttribute>
  Public Property StartDate As Date
  <XmlAttribute>
  Public Property EndDate As Date

  Public Property DemandPlans As List(Of Integer)

  Public Property DemandLocations As List(Of Integer)
End Class
