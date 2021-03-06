﻿Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml
Imports System.Xml.Serialization

Module Extensions
  <Extension>
  Public Function SerializeToXml(Of T)(valueToSerialize As T) As String
    Dim ns As New XmlSerializerNamespaces({New XmlQualifiedName("", "")})
    Using sw As New IO.StringWriter()
      Using writer As XmlWriter = XmlWriter.Create(sw, New XmlWriterSettings() With {.OmitXmlDeclaration = True})
        Dim xmler = New XmlSerializer(valueToSerialize.GetType())
        xmler.Serialize(writer, valueToSerialize, ns)
      End Using

      Return sw.ToString()
    End Using
  End Function

  <Extension>
  Public Function DeserializeXml(Of T)(xmlToDeserialize As String) As T
    Dim serializer = New XmlSerializer(GetType(T))
    Using reader As TextReader = New StringReader(xmlToDeserialize)
      Return DirectCast(serializer.Deserialize(reader), T)
    End Using
  End Function
End Module
