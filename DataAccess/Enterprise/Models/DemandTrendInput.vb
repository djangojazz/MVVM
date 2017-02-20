﻿Imports System.Xml.Serialization

<Serializable>
Public Class DemandTrendInput
  <XmlAttribute>
  Public Property Grouping As String
  <XmlAttribute>
  Public Property FIKey As Integer
  <XmlAttribute>
  Public Property StartDate As Date
  <XmlAttribute>
  Public Property EndDate As Date


  Public Property DemandPlans As List(Of Integer)

  Public Property DemandLocations As List(Of Integer)

  Public Sub New()

  End Sub

  Public Sub New(fiKey As Integer, startDate As Date, endDate As Date, grouping As String, demandPlans As List(Of Integer), demandLocations As List(Of Integer))
    Me.FIKey = fiKey
    Me.StartDate = startDate
    Me.EndDate = endDate
    Me.Grouping = grouping
    Me.DemandPlans = demandPlans
    Me.DemandLocations = demandLocations
  End Sub
End Class
