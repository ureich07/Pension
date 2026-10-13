Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.filespec
Imports iTextSharp.pdfa
Imports iTextSharp.text.pdf.pdfa
Imports iTextSharp.text.xml.xmp

' NuGet intaliton
' iTextSharp
' iTextSharp.pdfa

Module moZugferd



    'Public Class Form1
    'Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    EmbedXmlInPdf("rech_2026-61.pdf", "rech_2026-61.xml", "zug_2026-61.pdf")
    'End Sub

    Public Sub EmbedXmlInPdf(sourcePdfPath As String, xmlFilePath As String, outputPdfPath As String)
            Dim reader As New PdfReader(sourcePdfPath)
            Dim fileStream As New FileStream(outputPdfPath, FileMode.Create)
            Dim stamper As New PdfStamper(reader, fileStream, "3"c)

            ' 1. XML einbetten (wie gehabt)
            Dim fileSpec As PdfFileSpecification = PdfFileSpecification.FileEmbedded(
                stamper.Writer, xmlFilePath, "factur-x.xml", Nothing, "text/xml", Nothing, 0)
            fileSpec.Put(New PdfName("AFRelationship"), New PdfName("Alternative"))
            stamper.AddFileAttachment("ZUGFeRD Rechnung", fileSpec)

            ' 2. Associated File registrieren (wie gehabt)
            Dim afArray As New PdfArray()
            afArray.Add(fileSpec.Reference)
            stamper.Writer.ExtraCatalog.Put(New PdfName("AF"), afArray)

            ' --- FIX: METADATEN MANUELL UND VOLLSTÄNDIG SETZEN ---

            ' Wir holen den langen String (stellen Sie sicher, dass fcxmpXml() den langen Block zurückgibt)
            Dim xmpContent As String = fcxmpXml()
            Dim metadataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xmpContent)

            ' Erstellen des Streams mit der EXAKTEN Byte-Länge
            Dim metadataStream As New PdfStream(metadataBytes)
            metadataStream.Put(PdfName.TYPE, PdfName.METADATA)
            metadataStream.Put(PdfName.SUBTYPE, New PdfName("XML"))

            ' Dem Katalog hinzufügen (überschreibt bestehende Metadaten sauber)
            stamper.Writer.ExtraCatalog.Put(PdfName.METADATA, stamper.Writer.AddToBody(metadataStream).IndirectReference)

            ' WICHTIG: stamper.XmpMetadata DARF NICHT mehr gesetzt werden, sonst wird doppelt geschrieben!

            stamper.Close()
            reader.Close()
        End Sub
        Private Function fcxmpXml() As String
            ' WICHTIG: Nutzen Sie exakt diese URLs. Ein Validator wie Elster
            ' akzeptiert keine Kurzformen wie "aiim.org".
            Dim xmp As String = "<?xpacket begin=""ï»¿"" id=""W5M0MpCehiHzreSzNTczkc9d""?>" &
        "<x:xmpmeta xmlns:x=""adobe:ns:meta/"">" &
        "<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">" &
        " <rdf:Description rdf:about="""" xmlns:pdfaid=""http://www.aiim.org/pdfa/ns/id/"">" &
        "  <pdfaid:part>3</pdfaid:part><pdfaid:conformance>B</pdfaid:conformance>" &
        " </rdf:Description>" &
        " <rdf:Description rdf:about="""" xmlns:fx=""urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#"">" &
        "  <fx:ConformanceLevel>EN 16931</fx:ConformanceLevel>" &
        "  <fx:DocumentType>INVOICE</fx:DocumentType>" &
        "  <fx:DocumentFileName>factur-x.xml</fx:DocumentFileName>" &
        "  <fx:Version>1.0</fx:Version>" &
        "  <fx:ConformanceLevel>BASIC</fx:ConformanceLevel>" &
        " </rdf:Description>" &
        " <rdf:Description rdf:about="""" xmlns:pdfaExtension=""http://aiim.org"" xmlns:pdfaSchema=""http://aiim.org"" xmlns:pdfaProperty=""http://aiim.org"">" &
        "  <pdfaExtension:schemas><rdf:Bag><rdf:li rdf:parseType=""Resource"">" &
        "    <pdfaSchema:schema>Factur-X PDFA Extension Schema</pdfaSchema:schema>" &
        "    <pdfaSchema:namespaceURI>urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#</pdfaSchema:namespaceURI>" &
        "    <pdfaSchema:prefix>fx</pdfaSchema:prefix>" &
        "    <pdfaSchema:property><rdf:Seq>" &
        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>DocumentFileName</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>DocumentType</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>Version</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>ConformanceLevel</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
        "    </rdf:Seq></pdfaSchema:property>" &
        "  </rdf:li></rdf:Bag></pdfaExtension:schemas>" &
        " </rdf:Description>" &
        "</rdf:RDF></x:xmpmeta><?xpacket end=""w""?>"
            Return xmp
        End Function




    ' End Class
End Module
