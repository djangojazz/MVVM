Imports System.Data.SqlClient

Public Module Selects


  Public Function RunQuery(Of T)(proc As String, Optional parms As List(Of SqlParameter) = Nothing) As List(Of T)
    Using cn = New SqlConnection(GetEnterpriseTestDatabase)
      Using cmd = New SqlCommand(proc, cn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 60

        If parms IsNot Nothing Then parms.ForEach(Sub(parm) cmd.Parameters.Add(parm))

        Using adapter As New SqlDataAdapter()
          Using table As New DataTable()
            cn.Open()
            adapter.SelectCommand = cmd
            table.Locale = Globalization.CultureInfo.InvariantCulture
            adapter.Fill(table)
            cn.Close()

            Return DataConverter.ConvertTo(Of T)(table)
          End Using
        End Using
      End Using
    End Using
  End Function

  Public Function GetDemandTrends(data As DemandTrendInput) As List(Of DemandTrendOutput)
    Dim serialized = data.SerializeToXml()
    Dim params = New List(Of SqlParameter)({New SqlParameter("@Input", serialized)})
    Return RunQuery(Of DemandTrendOutput)("vis.SelectDemandTrendDetails", params)
  End Function

  Public Function GetDemandLocations() As List(Of DemandLocation)
    Dim items = RunQuery(Of DemandLocationDb)("vis.ListLocations")
    Return items.Select(Function(x) New DemandLocation(x.GDKEY, x.Company, x.Division, x.Branch, x.Name)).ToList()
  End Function
End Module
