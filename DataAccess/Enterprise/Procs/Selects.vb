Imports System.Data.SqlClient

Public Module Selects


  Public Function RunQuery(proc As String, Optional parms As List(Of SqlParameter) = Nothing) As DataTable
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

            Return table
          End Using
        End Using
      End Using
    End Using
  End Function

  Public Function GetDemandTrends() As List(Of DemandTrendOutput)
    Dim data = New DemandTrendInput(2278, New Date(2017, 1, 1), New Date(2017, 5, 1), New List(Of Integer)({24, 26}), New List(Of Integer)({2, 25}))
    Dim serialized = data.SerializeToXml()
    Dim params = New List(Of SqlParameter)({New SqlParameter("@Input", serialized)})
    Dim items = RunQuery("vis.SelectDemandTrendDetails", params)

    Return DataConverter.ConvertTo(Of DemandTrendOutput)(items)
  End Function
End Module
