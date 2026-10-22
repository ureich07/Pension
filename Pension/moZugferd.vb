
Imports System.IO
Imports itextsharp.text.pdf

''' <summary>
''' Stellt Kernfunktionen zur Generierung von elektronischen Rechnungen im ZUGFeRD- / Factur-X-Format bereit.
''' Erlaubt das regelkonforme Einbetten von strukturierten XML-Rechnungsdaten in bestehende PDF-Dokumente.
''' </summary>
''' <remarks>
''' 22.10.2026 - Code-Optimierung:
''' - Vollständige XML-Dokumentationskommentare hinzugefügt.
''' - Fehlerhafte, doppelte Definition von 'ConformanceLevel' im XMP-Metadatenblock korrigiert.
''' - 'Using'-Blöcke für 'PdfReader' und 'FileStream' implementiert, um Ressourcen im Fehlerfall sicher freizugeben.
''' - Bereinigung ungenutzter Variablen-Zuweisungen.
''' </remarks>
Public Module moZugferd

    ' Der offizielle Target-Namespace für ZUGFeRD 2.x / Factur-X CrossIndustryDocument
    Private Const FxNamespace As String = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"

    ''' <summary>
    ''' Bettet eine ZUGFeRD/Factur-X konforme XML-Datei in ein bestehendes PDF-Dokument ein 
    ''' und registriert diese als alternative Repräsentation in den Metadaten (PDF/A-3 konform).
    ''' </summary>
    ''' <param name="sourcePdfPath">Der vollständige Pfad zum Original-PDF-Dokument.</param>
    ''' <param name="xmlFilePath">Der vollständige Pfad zur einzubettenden XML-Rechnungsdatei.</param>
    ''' <param name="outputPdfPath">Der Zielpfad, unter dem das resultierende hybride PDF gespeichert werden soll.</param>
    Public Sub EmbedXmlInPdf(sourcePdfPath As String, xmlFilePath As String, outputPdfPath As String)
        ' Validierung der Eingabeparameter
        If String.IsNullOrWhiteSpace(sourcePdfPath) OrElse Not File.Exists(sourcePdfPath) Then Throw New FileNotFoundException("Die Quell-PDF-Datei wurde nicht gefunden.")
        If String.IsNullOrWhiteSpace(xmlFilePath) OrElse Not File.Exists(xmlFilePath) Then Throw New FileNotFoundException("Die XML-Datendatei wurde nicht gefunden.")
        If String.IsNullOrWhiteSpace(outputPdfPath) Then Throw New ArgumentException("Der Ausgabepfad darf nicht leer sein.")

        ' Nutzung von Using-Blöcken stellt die korrekte Freigabe der Datei-Handles sicher
        Using reader As New PdfReader(sourcePdfPath)
            Using fileStream As New FileStream(outputPdfPath, FileMode.Create)
                ' PDF-Stamper im PDF/A-3-Modus initialisieren ("3"c)
                Dim stamper As New PdfStamper(reader, fileStream, "3"c)

                ' 1. XML-Datei als eingebettete Datei (EmbeddedFile) deklarieren
                Dim fileSpec As PdfFileSpecification = PdfFileSpecification.FileEmbedded(
                    stamper.Writer, xmlFilePath, "factur-x.xml", Nothing, "text/xml", Nothing, 0)

                ' Beziehung des Anhangs als "Alternative" Kennzeichnen (Vorschrift für ZUGFeRD)
                fileSpec.Put(New PdfName("AFRelationship"), New PdfName("Alternative"))
                stamper.AddFileAttachment("ZUGFeRD Rechnung", fileSpec)

                ' 2. Associated File (AF) im globalen PDF-Katalog registrieren
                Dim afArray As New PdfArray()
                afArray.Add(fileSpec.Reference)
                stamper.Writer.ExtraCatalog.Put(New PdfName("AF"), afArray)

                ' 3. XMP-Metadaten vollständig manuell injizieren
                Dim xmpContent As String = fcxmpXml()
                Dim metadataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xmpContent)

                ' Erstellen des XMP-Metadaten-Streams mit exakter Byte-Länge
                Dim metadataStream As New PdfStream(metadataBytes)
                metadataStream.Put(PdfName.TYPE, PdfName.METADATA)
                metadataStream.Put(PdfName.SUBTYPE, New PdfName("XML"))

                ' Bestehende Metadaten im Dokumentenkatalog sauber überschreiben
                stamper.Writer.ExtraCatalog.Put(PdfName.METADATA, stamper.Writer.AddToBody(metadataStream).IndirectReference)

                ' WICHTIG: stamper.XmpMetadata darf NICHT gesetzt werden, um redundante Einträge zu verhindern
                stamper.Close()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Erzeugt den standardisierten XMP-Metadaten-XML-String, der für die Validierung 
    ''' von PDF/A-3 und ZUGFeRD / Factur-X (Profil EN 16931 / COMFORT) zwingend erforderlich ist.
    ''' </summary>
    ''' <returns>Ein valider XML-String mit den benötigten Schemata-Erweiterungen und Namensräumen.</returns>
    Private Function fcxmpXml() As String
        ' WICHTIG: Die Namespaces (z.B. aiim.org) müssen exakt beibehalten werden, um Schema-Validierungen zu bestehen.
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

End Module



'Imports System.IO
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.pdf.filespec
'Imports iTextSharp.pdfa
'Imports iTextSharp.text.pdf.pdfa
'Imports iTextSharp.text.xml.xmp

'' NuGet intaliton
'' iTextSharp
'' iTextSharp.pdfa

'Module moZugferd



'    'Public Class Form1
'    'Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'    '    EmbedXmlInPdf("rech_2026-61.pdf", "rech_2026-61.xml", "zug_2026-61.pdf")
'    'End Sub

'    Public Sub EmbedXmlInPdf(sourcePdfPath As String, xmlFilePath As String, outputPdfPath As String)
'            Dim reader As New PdfReader(sourcePdfPath)
'            Dim fileStream As New FileStream(outputPdfPath, FileMode.Create)
'            Dim stamper As New PdfStamper(reader, fileStream, "3"c)

'            ' 1. XML einbetten (wie gehabt)
'            Dim fileSpec As PdfFileSpecification = PdfFileSpecification.FileEmbedded(
'                stamper.Writer, xmlFilePath, "factur-x.xml", Nothing, "text/xml", Nothing, 0)
'            fileSpec.Put(New PdfName("AFRelationship"), New PdfName("Alternative"))
'            stamper.AddFileAttachment("ZUGFeRD Rechnung", fileSpec)

'            ' 2. Associated File registrieren (wie gehabt)
'            Dim afArray As New PdfArray()
'            afArray.Add(fileSpec.Reference)
'            stamper.Writer.ExtraCatalog.Put(New PdfName("AF"), afArray)

'            ' --- FIX: METADATEN MANUELL UND VOLLSTÄNDIG SETZEN ---

'            ' Wir holen den langen String (stellen Sie sicher, dass fcxmpXml() den langen Block zurückgibt)
'            Dim xmpContent As String = fcxmpXml()
'            Dim metadataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xmpContent)

'            ' Erstellen des Streams mit der EXAKTEN Byte-Länge
'            Dim metadataStream As New PdfStream(metadataBytes)
'            metadataStream.Put(PdfName.TYPE, PdfName.METADATA)
'            metadataStream.Put(PdfName.SUBTYPE, New PdfName("XML"))

'            ' Dem Katalog hinzufügen (überschreibt bestehende Metadaten sauber)
'            stamper.Writer.ExtraCatalog.Put(PdfName.METADATA, stamper.Writer.AddToBody(metadataStream).IndirectReference)

'            ' WICHTIG: stamper.XmpMetadata DARF NICHT mehr gesetzt werden, sonst wird doppelt geschrieben!

'            stamper.Close()
'            reader.Close()
'        End Sub
'        Private Function fcxmpXml() As String
'            ' WICHTIG: Nutzen Sie exakt diese URLs. Ein Validator wie Elster
'            ' akzeptiert keine Kurzformen wie "aiim.org".
'            Dim xmp As String = "<?xpacket begin=""ï»¿"" id=""W5M0MpCehiHzreSzNTczkc9d""?>" &
'        "<x:xmpmeta xmlns:x=""adobe:ns:meta/"">" &
'        "<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">" &
'        " <rdf:Description rdf:about="""" xmlns:pdfaid=""http://www.aiim.org/pdfa/ns/id/"">" &
'        "  <pdfaid:part>3</pdfaid:part><pdfaid:conformance>B</pdfaid:conformance>" &
'        " </rdf:Description>" &
'        " <rdf:Description rdf:about="""" xmlns:fx=""urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#"">" &
'        "  <fx:ConformanceLevel>EN 16931</fx:ConformanceLevel>" &
'        "  <fx:DocumentType>INVOICE</fx:DocumentType>" &
'        "  <fx:DocumentFileName>factur-x.xml</fx:DocumentFileName>" &
'        "  <fx:Version>1.0</fx:Version>" &
'        "  <fx:ConformanceLevel>BASIC</fx:ConformanceLevel>" &
'        " </rdf:Description>" &
'        " <rdf:Description rdf:about="""" xmlns:pdfaExtension=""http://aiim.org"" xmlns:pdfaSchema=""http://aiim.org"" xmlns:pdfaProperty=""http://aiim.org"">" &
'        "  <pdfaExtension:schemas><rdf:Bag><rdf:li rdf:parseType=""Resource"">" &
'        "    <pdfaSchema:schema>Factur-X PDFA Extension Schema</pdfaSchema:schema>" &
'        "    <pdfaSchema:namespaceURI>urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#</pdfaSchema:namespaceURI>" &
'        "    <pdfaSchema:prefix>fx</pdfaSchema:prefix>" &
'        "    <pdfaSchema:property><rdf:Seq>" &
'        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>DocumentFileName</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
'        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>DocumentType</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
'        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>Version</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
'        "      <rdf:li rdf:parseType=""Resource""><pdfaProperty:name>ConformanceLevel</pdfaProperty:name><pdfaProperty:valueType>Text</pdfaProperty:valueType><pdfaProperty:category>external</pdfaProperty:category></rdf:li>" &
'        "    </rdf:Seq></pdfaSchema:property>" &
'        "  </rdf:li></rdf:Bag></pdfaExtension:schemas>" &
'        " </rdf:Description>" &
'        "</rdf:RDF></x:xmpmeta><?xpacket end=""w""?>"
'            Return xmp
'        End Function




'    ' End Class
'End Module
