Option Explicit On
Imports System.IO
Imports System.Text
'Imports System.Data.OleDb
'Imports System.Data.Odbc
Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient

Module moDB

    Dim db As New ADOX.Catalog()
    Dim table As New ADOX.Table
    Dim column As New ADOX.Column
    Dim column1 As New ADOX.Column
    Dim idx As ADOX.Index
    Dim relation As ADOX.Key
    Dim dbhandle As String

#Region "Strucktur Datenbasis"

    ''' <summary>
    ''' Stellt die vollständige Struktur der internen Datenbanktabellen als formatierten String bereit.
    ''' </summary>
    ''' <returns>Ein strukturierter String mit allen Tabellendefinitionen, Feldern und Längenangaben.</returns>
    ''' <remarks>
    ''' 16.12.2011 - Create
    ''' 25.09.2026 - Code-Optimierung:
    ''' - Veraltetes 'On Error Resume Next' entfernt.
    ''' - Performance-Optimierung: Umstellung von String-Verkettung (&) auf einen 'StringBuilder'.
    ''' - Speicherverbrauch drastisch reduziert durch Vordefinition der Builder-Kapazität.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch modernes 'Return' ersetzt.
    ''' </remarks>
    Function StruckturBD() As String
        lgStatusCheck = False

        ' StringBuilder mit einer großzügigen Startkapazität initialisieren, um Reallokationen zu vermeiden
        Dim sb As New StringBuilder(4096)

        ' --- Zimmer ---
        sb.AppendLine("<Zimmer>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("Art , varchar,100, ")
        sb.AppendLine("Betten , varchar,2, ")
        sb.AppendLine("Ausstattung, varchar,50, ")
        sb.AppendLine("FeWo, varchar,2, ")
        sb.AppendLine("IDObjekte, varchar,16, ")
        sb.AppendLine("Nummer, varchar,2, ")
        sb.AppendLine("<End>")

        ' --- Konten ---
        sb.AppendLine("<Konten>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("KTO, varchar,10, ")
        sb.AppendLine("BLZ, varchar,10, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("Typ, varchar,50, ")
        sb.AppendLine("IBAN, varchar,25, ")
        sb.AppendLine("BIC, varchar,15, ")
        sb.AppendLine("KZ , text,, ")
        sb.AppendLine("<End>")

        ' --- Objekte ---
        sb.AppendLine("<Objekte>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("Strasse, varchar,50, ")
        sb.AppendLine("HNr , varchar,5, ")
        sb.AppendLine("PLZ, varchar,5, ")
        sb.AppendLine("Ort, varchar,50, ")
        sb.AppendLine("Ortsteil, varchar,50, ")
        sb.AppendLine("Telefon, varchar,15, ")
        sb.AppendLine("RGB, varchar,20, ")
        sb.AppendLine("<End>")

        ' --- Nutzer ---
        sb.AppendLine("<Nutzer>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("KName, varchar,50, ")
        sb.AppendLine("PassWD, varchar,50, ")
        sb.AppendLine("Status, varchar,1, ")
        sb.AppendLine("<End>")

        ' --- BTexte ---
        sb.AppendLine("<BTexte>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,10, ")
        sb.AppendLine("Art, varchar,1, ")
        sb.AppendLine("BTextDe, text,, ")
        sb.AppendLine("BTextEn, text,, ")
        sb.AppendLine("BTextSp, text,, ")
        sb.AppendLine("BTextIt, text,, ")
        sb.AppendLine("BTextFr, text,, ")
        sb.AppendLine("BTextRu, text,, ")
        sb.AppendLine("BTextCz, text,, ")
        sb.AppendLine("BTextPl, text,, ")
        sb.AppendLine("ZZiel, varchar,10, ")
        sb.AppendLine("<End>")

        ' --- Buchung ---
        sb.AppendLine("<Buchung>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("BID, varchar,16, ")
        sb.AppendLine("Von, varchar,8, ")
        sb.AppendLine("VonZeit, varchar,4,1200")
        sb.AppendLine("Bis, varchar,8, ")
        sb.AppendLine("BisZeit, varchar,4,1200")
        sb.AppendLine("ObjID, varchar,16, ")
        sb.AppendLine("ZimID, varchar,16, ")
        sb.AppendLine("Variable, varchar,1, ")
        sb.AppendLine("KunID, varchar,16, ")
        sb.AppendLine("Kunde, varchar,50, ")
        sb.AppendLine("Name1, varchar,50, ")
        sb.AppendLine("Name2, varchar,50, ")
        sb.AppendLine("Personen, varchar,2, ")
        sb.AppendLine("Tiere, varchar,2, ")
        sb.AppendLine("Art , varchar,10, ")
        sb.AppendLine("Frueh, varchar,10, ")
        sb.AppendLine("Kategorie, varchar,50, ")
        sb.AppendLine("Preis, varchar,10, ")
        sb.AppendLine("Anzahlung, varchar,10, ")
        sb.AppendLine("Storno, varchar,3,100, ")
        sb.AppendLine("Summe, varchar,10,0, ")
        sb.AppendLine("Code, varchar,5,00000")
        sb.AppendLine("Werbung, varchar,50, ")
        sb.AppendLine("Info, text,, ")
        sb.AppendLine("Rechnung, varchar,2, ")
        sb.AppendLine("BuchDatum, varchar,8, ")
        sb.AppendLine("Sprache, varchar,1, ")
        sb.AppendLine("BText, varchar,16, ")
        sb.AppendLine("MText, varchar,255, ")
        sb.AppendLine("RID, varchar,16, ")
        sb.AppendLine("RDatum, varchar,8, ")
        sb.AppendLine("FPreis, varchar,10, ")
        sb.AppendLine("RDSenden, varchar,8, ")
        sb.AppendLine("BIDIndex, varchar,1,0")
        sb.AppendLine("IDRef, varchar,16,XXXXX")
        sb.AppendLine("RAID, varchar,16, ")
        sb.AppendLine("RADatum, varchar,8, ")
        sb.AppendLine("Bez, varchar,1,0")
        sb.AppendLine("ZDatum,varchar,8,00000000")
        sb.AppendLine("InternetNr,varchar,20, ")
        sb.AppendLine("<End>")

        ' --- Kunden ---
        sb.AppendLine("<Kunden>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Anrede, varchar,8, ")
        sb.AppendLine("Name1, varchar,50, ")
        sb.AppendLine("Name2, varchar,50, ")
        sb.AppendLine("Vorname, varchar,50, ")
        sb.AppendLine("Strasse, varchar,50, ")
        sb.AppendLine("PLZ, varchar,10, ")
        sb.AppendLine("Ort, varchar,50, ")
        sb.AppendLine("Land , varchar,10, ")
        sb.AppendLine("Telefon, varchar,50, ")
        sb.AppendLine("Telefax, varchar,50, ")
        sb.AppendLine("Funk, varchar,50, ")
        sb.AppendLine("EMail, varchar,50, ")
        sb.AppendLine("Pass, varchar,50, ")
        sb.AppendLine("Geb, varchar,8, ")
        sb.AppendLine("Info, text,, ")
        sb.AppendLine("Werbung, varchar,16, ")
        sb.AppendLine("<End>")

        ' --- Datev ---
        sb.AppendLine("<Datev>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("RechNr, varchar,12, ")
        sb.AppendLine("Umsatz1, varchar,7, ")
        sb.AppendLine("Mwst1, varchar,7, ")
        sb.AppendLine("MwstS1, varchar,3, ")
        sb.AppendLine("GKonto1, varchar,4, ")
        sb.AppendLine("Konto1 , varchar,4, ")
        sb.AppendLine("Umsatz2, varchar,7, ")
        sb.AppendLine("Mwst2, varchar,7, ")
        sb.AppendLine("MwstS2, varchar,3, ")
        sb.AppendLine("GKonto2, varchar,4, ")
        sb.AppendLine("Konto2 , varchar,4, ")
        sb.AppendLine("Datum, varchar,8, ")
        sb.AppendLine("BuchNr, varchar,6, ")
        sb.AppendLine("KunNr, varchar,16, ")
        sb.AppendLine("<End>")

        ' --- Partner ---
        sb.AppendLine("<Partner>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("KID, varchar,16, ")
        sb.AppendLine("Anrede, varchar,8, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("Vorname, varchar,50, ")
        sb.AppendLine("Geb, varchar,8, ")
        sb.AppendLine("Info, varchar,50, ")
        sb.AppendLine("<End>")

        ' --- Werbung ---
        sb.AppendLine("<Werbung>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Werbung, varchar,50, ")
        sb.AppendLine("Betreff, varchar,50, ")
        sb.AppendLine("KText, text,, ")
        sb.AppendLine("FText, text,, ")
        sb.AppendLine("Link, varchar,50, ")
        sb.AppendLine("Provision, varchar,5, ")
        sb.AppendLine("Color,varchar,15, ")
        sb.AppendLine("<End>")

        ' --- Preise ---
        sb.AppendLine("<Preise>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Beschreibung, varchar,50, ")
        sb.AppendLine("Sasion, varchar,1, ")
        sb.AppendLine("Kategorie, varchar,50, ")
        sb.AppendLine("Preis, varchar,50, ")
        sb.AppendLine("P3, varchar,50, ")
        sb.AppendLine("P4, varchar,50, ")
        sb.AppendLine("P5, varchar,50, ")
        sb.AppendLine("P6, varchar,50, ")
        sb.AppendLine("<End>")

        ' --- Personal1 ---
        sb.AppendLine("<Personal1>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("Vorname, varchar,50, ")
        sb.AppendLine("Strasse, varchar,50, ")
        sb.AppendLine("PLZ, varchar,8, ")
        sb.AppendLine("Ort, varchar,50, ")
        sb.AppendLine("Tel1, varchar,20, ")
        sb.AppendLine("Tel2, varchar,20, ")
        sb.AppendLine("Info, text,, ")
        sb.AppendLine("<End>")

        ' --- Personal2 ---
        sb.AppendLine("<Personal2>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("ADatum, varchar,8, ")
        sb.AppendLine("EDatum, varchar,8, ")
        sb.AppendLine("Personal, varchar,20, ")
        sb.AppendLine("Info, text,, ")
        sb.AppendLine("<End>")

        ' --- Bewertung ---
        sb.AppendLine("<Bewertung>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("BuchID, varchar,16, ")
        sb.AppendLine("Status, varchar,2, ")
        sb.AppendLine("<End>")

        ' --- System ---
        sb.AppendLine("<System>")
        sb.AppendLine("ID, Text,, ")
        sb.AppendLine("Pension, Text,, ")
        sb.AppendLine("Color, Text,, ")
        sb.AppendLine("Druckprofil, Text,, ")
        sb.AppendLine("Land, Text,, ")
        sb.AppendLine("Language, Text,, ")
        sb.AppendLine("Mail, Text,, ")
        sb.AppendLine("Mail_Rechnung, Text,, ")
        sb.AppendLine("Saison, Text,, ")
        sb.AppendLine("Zusatzkosten, Text,, ")
        sb.AppendLine("Copy, Text,, ")
        sb.AppendLine("<End>")

        ' --- Zusaetze ---
        sb.AppendLine("<Zusaetze>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("BuchID, varchar,16, ")
        sb.AppendLine("ZimID, varchar,16, ")
        sb.AppendLine("Menge, varchar,3, ")
        sb.AppendLine("Bezeichnung, varchar,100, ")
        sb.AppendLine("Betrag, varchar,10, ")
        sb.AppendLine("Steuer, varchar,10, ")
        sb.AppendLine("Gesamt, varchar,10, ")
        sb.AppendLine("Datum, varchar,8, ")
        sb.AppendLine("ZimNr, varchar,10, ")
        sb.AppendLine("Name, varchar,50, ")
        sb.AppendLine("<End>")

        ' --- Termine ---
        sb.AppendLine("<Termine>")
        sb.AppendLine("ID, varchar,16, ")
        sb.AppendLine("Datum, varchar,8, ")
        sb.AppendLine("TerminText, varchar,255, ")
        sb.AppendLine("Termin, varchar,8, ")
        sb.AppendLine("Zeit, varchar,8, ")
        sb.AppendLine("Aktive, varchar,1, ")
        sb.AppendLine("BID, varchar,16, ")
        sb.AppendLine("Zimmer, varchar,16, ")
        sb.AppendLine("<End>")

        Return sb.ToString()
    End Function


    '''' <summary>
    '''' Stellt die Struktur der Datenbank bereit
    '''' </summary>
    '''' <returns>
    '''' sSt as String
    '''' </returns>
    '''' <remarks>
    '''' 16.12.2011 Create
    ''''</remarks>
    'Function StruckturBD() As String
    '    On Error Resume Next
    '    Dim sSt As String
    '    lgStatusCheck = False
    '    sSt = ""
    '    sSt = sSt & "<Zimmer>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "Art , varchar,100, " & vbCrLf
    '    sSt = sSt & "Betten , varchar,2, " & vbCrLf
    '    sSt = sSt & "Ausstattung, varchar,50, " & vbCrLf
    '    sSt = sSt & "FeWo, varchar,2, " & vbCrLf
    '    sSt = sSt & "IDObjekte, varchar,16, " & vbCrLf
    '    sSt = sSt & "Nummer, varchar,2, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Konten>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "KTO, varchar,10, " & vbCrLf
    '    sSt = sSt & "BLZ, varchar,10, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "Typ, varchar,50, " & vbCrLf
    '    sSt = sSt & "IBAN, varchar,25, " & vbCrLf
    '    sSt = sSt & "BIC, varchar,15, " & vbCrLf
    '    sSt = sSt & "KZ , text,, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Objekte>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "Strasse, varchar,50, " & vbCrLf
    '    sSt = sSt & "HNr , varchar,5, " & vbCrLf
    '    sSt = sSt & "PLZ, varchar,5, " & vbCrLf
    '    sSt = sSt & "Ort, varchar,50, " & vbCrLf
    '    sSt = sSt & "Ortsteil, varchar,50, " & vbCrLf
    '    sSt = sSt & "Telefon, varchar,15, " & vbCrLf
    '    sSt = sSt & "RGB, varchar,20, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Nutzer>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "KName, varchar,50, " & vbCrLf
    '    sSt = sSt & "PassWD, varchar,50, " & vbCrLf
    '    sSt = sSt & "Status, varchar,1, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<BTexte>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,10, " & vbCrLf
    '    sSt = sSt & "Art, varchar,1, " & vbCrLf
    '    sSt = sSt & "BTextDe, text,, " & vbCrLf
    '    sSt = sSt & "BTextEn, text,, " & vbCrLf
    '    sSt = sSt & "BTextSp, text,, " & vbCrLf
    '    sSt = sSt & "BTextIt, text,, " & vbCrLf
    '    sSt = sSt & "BTextFr, text,, " & vbCrLf
    '    sSt = sSt & "BTextRu, text,, " & vbCrLf
    '    sSt = sSt & "BTextCz, text,, " & vbCrLf
    '    sSt = sSt & "BTextPl, text,, " & vbCrLf
    '    sSt = sSt & "ZZiel, varchar,10, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Buchung>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "BID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Von, varchar,8, " & vbCrLf
    '    sSt = sSt & "VonZeit, varchar,4,1200" & vbCrLf
    '    sSt = sSt & "Bis, varchar,8, " & vbCrLf
    '    sSt = sSt & "BisZeit, varchar,4,1200" & vbCrLf
    '    sSt = sSt & "ObjID, varchar,16, " & vbCrLf
    '    sSt = sSt & "ZimID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Variable, varchar,1, " & vbCrLf
    '    sSt = sSt & "KunID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Kunde, varchar,50, " & vbCrLf
    '    sSt = sSt & "Name1, varchar,50, " & vbCrLf
    '    sSt = sSt & "Name2, varchar,50, " & vbCrLf
    '    sSt = sSt & "Personen, varchar,2, " & vbCrLf
    '    sSt = sSt & "Tiere, varchar,2, " & vbCrLf
    '    sSt = sSt & "Art , varchar,10, " & vbCrLf
    '    sSt = sSt & "Frueh, varchar,10, " & vbCrLf
    '    sSt = sSt & "Kategorie, varchar,50, " & vbCrLf
    '    sSt = sSt & "Preis, varchar,10, " & vbCrLf
    '    sSt = sSt & "Anzahlung, varchar,10, " & vbCrLf
    '    sSt = sSt & "Storno, varchar,3,100, " & vbCrLf
    '    sSt = sSt & "Summe, varchar,10,0, " & vbCrLf
    '    sSt = sSt & "Code, varchar,5,00000" & vbCrLf
    '    sSt = sSt & "Werbung, varchar,50, " & vbCrLf
    '    sSt = sSt & "Info, text,, " & vbCrLf
    '    sSt = sSt & "Rechnung, varchar,2, " & vbCrLf
    '    sSt = sSt & "BuchDatum, varchar,8, " & vbCrLf
    '    sSt = sSt & "Sprache, varchar,1, " & vbCrLf
    '    sSt = sSt & "BText, varchar,16, " & vbCrLf
    '    sSt = sSt & "MText, varchar,255, " & vbCrLf
    '    sSt = sSt & "RID, varchar,16, " & vbCrLf
    '    sSt = sSt & "RDatum, varchar,8, " & vbCrLf
    '    sSt = sSt & "FPreis, varchar,10, " & vbCrLf
    '    sSt = sSt & "RDSenden, varchar,8, " & vbCrLf
    '    sSt = sSt & "BIDIndex, varchar,1,0" & vbCrLf
    '    sSt = sSt & "IDRef, varchar,16,XXXXX" & vbCrLf
    '    sSt = sSt & "RAID, varchar,16, " & vbCrLf
    '    sSt = sSt & "RADatum, varchar,8, " & vbCrLf
    '    sSt = sSt & "Bez, varchar,1,0" & vbCrLf
    '    sSt = sSt & "ZDatum,varchar,8,00000000" & vbCrLf
    '    sSt = sSt & "InternetNr,varchar,20, " & vbCrLf

    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Kunden>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Anrede, varchar,8, " & vbCrLf
    '    sSt = sSt & "Name1, varchar,50, " & vbCrLf
    '    sSt = sSt & "Name2, varchar,50, " & vbCrLf
    '    sSt = sSt & "Vorname, varchar,50, " & vbCrLf
    '    sSt = sSt & "Strasse, varchar,50, " & vbCrLf
    '    sSt = sSt & "PLZ, varchar,10, " & vbCrLf
    '    sSt = sSt & "Ort, varchar,50, " & vbCrLf
    '    sSt = sSt & "Land , varchar,10, " & vbCrLf
    '    sSt = sSt & "Telefon, varchar,50, " & vbCrLf
    '    sSt = sSt & "Telefax, varchar,50, " & vbCrLf
    '    sSt = sSt & "Funk, varchar,50, " & vbCrLf
    '    sSt = sSt & "EMail, varchar,50, " & vbCrLf
    '    sSt = sSt & "Pass, varchar,50, " & vbCrLf
    '    sSt = sSt & "Geb, varchar,8, " & vbCrLf
    '    sSt = sSt & "Info, text,, " & vbCrLf
    '    sSt = sSt & "Werbung, varchar,16, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Datev>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "RechNr, varchar,12, " & vbCrLf
    '    sSt = sSt & "Umsatz1, varchar,7, " & vbCrLf
    '    sSt = sSt & "Mwst1, varchar,7, " & vbCrLf
    '    sSt = sSt & "MwstS1, varchar,3, " & vbCrLf
    '    sSt = sSt & "GKonto1, varchar,4, " & vbCrLf
    '    sSt = sSt & "Konto1 , varchar,4, " & vbCrLf
    '    sSt = sSt & "Umsatz2, varchar,7, " & vbCrLf
    '    sSt = sSt & "Mwst2, varchar,7, " & vbCrLf
    '    sSt = sSt & "MwstS2, varchar,3, " & vbCrLf
    '    sSt = sSt & "GKonto2, varchar,4, " & vbCrLf
    '    sSt = sSt & "Konto2 , varchar,4, " & vbCrLf
    '    sSt = sSt & "Datum, varchar,8, " & vbCrLf
    '    sSt = sSt & "BuchNr, varchar,6, " & vbCrLf
    '    sSt = sSt & "KunNr, varchar,16, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Partner>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "KID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Anrede, varchar,8, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "Vorname, varchar,50, " & vbCrLf
    '    sSt = sSt & "Geb, varchar,8, " & vbCrLf
    '    sSt = sSt & "Info, varchar,50, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Werbung>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Werbung, varchar,50, " & vbCrLf
    '    sSt = sSt & "Betreff, varchar,50, " & vbCrLf
    '    sSt = sSt & "KText, text,, " & vbCrLf
    '    sSt = sSt & "FText, text,, " & vbCrLf
    '    sSt = sSt & "Link, varchar,50, " & vbCrLf
    '    sSt = sSt & "Provision, varchar,5, " & vbCrLf
    '    sSt = sSt & "Color,varchar,15, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Preise>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Beschreibung, varchar,50, " & vbCrLf
    '    sSt = sSt & "Sasion, varchar,1, " & vbCrLf '(N=Normal, V=Vorsasion, H=Haupsasion)
    '    sSt = sSt & "Kategorie, varchar,50, " & vbCrLf
    '    sSt = sSt & "Preis, varchar,50, " & vbCrLf
    '    sSt = sSt & "P3, varchar,50, " & vbCrLf
    '    sSt = sSt & "P4, varchar,50, " & vbCrLf
    '    sSt = sSt & "P5, varchar,50, " & vbCrLf
    '    sSt = sSt & "P6, varchar,50, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Personal1>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "Vorname, varchar,50, " & vbCrLf
    '    sSt = sSt & "Strasse, varchar,50, " & vbCrLf
    '    sSt = sSt & "PLZ, varchar,8, " & vbCrLf
    '    sSt = sSt & "Ort, varchar,50, " & vbCrLf
    '    sSt = sSt & "Tel1, varchar,20, " & vbCrLf
    '    sSt = sSt & "Tel2, varchar,20, " & vbCrLf
    '    sSt = sSt & "Info, text,, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Personal2>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "ADatum, varchar,8, " & vbCrLf
    '    sSt = sSt & "EDatum, varchar,8, " & vbCrLf
    '    sSt = sSt & "Personal, varchar,20, " & vbCrLf
    '    sSt = sSt & "Info, text,, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Bewertung>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "BuchID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Status, varchar,2, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<System>" & vbCrLf
    '    sSt = sSt & "ID, Text,, " & vbCrLf
    '    sSt = sSt & "Pension, Text,, " & vbCrLf
    '    sSt = sSt & "Color, Text,, " & vbCrLf
    '    sSt = sSt & "Druckprofil, Text,, " & vbCrLf
    '    sSt = sSt & "Land, Text,, " & vbCrLf
    '    sSt = sSt & "Language, Text,, " & vbCrLf
    '    sSt = sSt & "Mail, Text,, " & vbCrLf
    '    sSt = sSt & "Mail_Rechnung, Text,, " & vbCrLf
    '    sSt = sSt & "Saison, Text,, " & vbCrLf
    '    sSt = sSt & "Zusatzkosten, Text,, " & vbCrLf
    '    sSt = sSt & "Copy, Text,, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Zusaetze>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "BuchID, varchar,16, " & vbCrLf
    '    sSt = sSt & "ZimID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Menge, varchar,3, " & vbCrLf
    '    sSt = sSt & "Bezeichnung, varchar,100, " & vbCrLf
    '    sSt = sSt & "Betrag, varchar,10, " & vbCrLf
    '    sSt = sSt & "Steuer, varchar,10, " & vbCrLf
    '    sSt = sSt & "Gesamt, varchar,10, " & vbCrLf
    '    sSt = sSt & "Datum, varchar,8, " & vbCrLf
    '    sSt = sSt & "ZimNr, varchar,10, " & vbCrLf
    '    sSt = sSt & "Name, varchar,50, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    sSt = sSt & "<Termine>" & vbCrLf
    '    sSt = sSt & "ID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Datum, varchar,8, " & vbCrLf
    '    sSt = sSt & "TerminText, varchar,255, " & vbCrLf
    '    sSt = sSt & "Termin, varchar,8, " & vbCrLf
    '    sSt = sSt & "Zeit, varchar,8, " & vbCrLf
    '    sSt = sSt & "Aktive, varchar,1, " & vbCrLf
    '    sSt = sSt & "BID, varchar,16, " & vbCrLf
    '    sSt = sSt & "Zimmer, varchar,16, " & vbCrLf
    '    sSt = sSt & "<End>" & vbCrLf

    '    StruckturBD = sSt
    'End Function

    ''' <summary>
    ''' Stellt die Struktur der INI-Datei bereit
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 29.03.2011 Create
    ''' </remarks>
    Function StruckturINI(ByVal cTmp As String, ByVal cPath As String) As String
        Dim sb As New StringBuilder

        sb.Append(SEC_COMMON & "," & KEY_VERSION & "," & "1.0.0.0" & vbCrLf)
        sb.Append(SEC_COMMON & "," & KEY_SDIR & "," & "" & vbCrLf)
        sb.Append(SEC_COMMON & "," & KEY_DDIR & "," & "" & vbCrLf)
        sb.Append(SEC_COMMON & "," & KEY_ADIR & "," & "" & vbCrLf)

        sb.Append(SEC_BASIC & "," & KEY_COMPANY & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_STR & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_PLZ & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_ORT & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_TE1 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_TE2 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_TE3 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_TE4 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_Web & "," & "http://www.feworeich.de/" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_MWST1 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_MWST2 & "," & " " & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_KNr & "," & "0" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_BNr & "," & "0" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_RNr & "," & "0" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_UStNr & "," & "0" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_UStID & "," & "0" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_Fr & "," & "0" & vbCrLf)

        sb.Append(SEC_BASIC & "," & KEY_KKonto & "," & "1600" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_BKonto & "," & "1800" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_GKonto7 & "," & "4300" & vbCrLf)
        sb.Append(SEC_BASIC & "," & KEY_GKonto19 & "," & "4400" & vbCrLf)

        sb.Append(SEC_EMAIL & "," & KEY_SMTP & "," & " " & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_EMAIL & "," & " " & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_SERVER & "," & "0" & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_USER & "," & " " & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_PASS & "," & " " & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_MIME & "," & "1" & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_UUE & "," & "0" & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_HTML & "," & "0" & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_AUTOR & "," & " " & vbCrLf)
        sb.Append(SEC_EMAIL & "," & KEY_BETREFF & "," & " " & vbCrLf)

        sb.Append(SEC_PREIS & "," & KEY_Kate & "," & "Zimmerpreis pro Nacht;Wochenpreis ab 7 Nächte;ab 7 Nächte außer Feiertage + Weinfest;Sonntag - Donnerstagnacht;Freitag- oder Samstatagnacht;Himmelfahrt;Weinfest;Feiertag;Mind. 4 Nächte;Mind. 7 Nächte" & vbCrLf)
        sb.Append(SEC_PREIS & "," & KEY_UArt & "," & "Einzelzimmer;Standard - Doppelzimmer;Komfort - Doppelzimmer;Ferienwohnung für 2 bis 6 Pers. mit 2 Schlafstuben;Ferienwohnung für 2-4 Personen 1 Schlafstube;Rollstuhlgerechtes Appartement;Ferienzimmer" & vbCrLf)

        StruckturINI = Replace(sb.ToString, Chr(34), "")
    End Function

#End Region

#Region "Fuktionen zur Erstellung der Datenbasis"

    ''' <summary>
    ''' Procedure  : CreateDB
    ''' Created by : Uwe Reich
    ''' Date-Time  : 16.05.2007               Last Upate: 26.03.2011
    ''' </summary>
    ''' <param name="dbfile"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Erstellen der einzelnen Tabellen aus einer Resourcendatei
    ''' </remarks>
    Function CreateDB(ByVal dbfile As String) As Boolean
        Dim i As Long
        Dim sEnd As String
        Dim lStart As Boolean
        Dim arDaten() As String
        Dim arFields() As String
        Dim cTabelle As String
        Dim cFields As String
        Dim nMax As Integer
        CreateDB = True
        Try

            db = New ADOX.Catalog
            dbhandle = "Provider=Microsoft.Jet.OLEDB.4.0;"
            dbhandle = dbhandle & "Data Source=" & dbfile & ";"
            db.Create(dbhandle)

            sEnd = "<End>"
            ReDim arFields(0)

            arDaten = Split(StruckturBD, vbCrLf)

            nMax = UBound(arDaten) - 1
            cTabelle = ""
            For i = 0 To nMax
                If lStart = False Then
                    If InStr(1, Trim$(arDaten(i)), "<") <> 0 And Trim$(arDaten(i)) <> sEnd Then
                        cTabelle = Trim$(arDaten(i))
                        cTabelle = Replace(cTabelle, "<", "")
                        cTabelle = Replace(cTabelle, ">", "")
                        lStart = True
                    End If
                Else
                    If InStr(1, Trim$(arDaten(i)), sEnd) = 0 Then
                        arFields(UBound(arFields)) = arDaten(i)
                        ReDim Preserve arFields(UBound(arFields) + 1)
                    Else
                        cFields = Join(arFields, vbCrLf)
                        Call fktCreate_Tabelle(db, cTabelle, cFields)
                        ReDim arFields(0)
                        lStart = False
                    End If
                End If
            Next
            CreateDB = True
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            CreateDB = False
        End Try

    End Function


    ''' <summary>
    ''' Procedure  : fktCreate_Tabelle
    ''' Created by : Uwe Reich
    ''' Date-Time  : 16.05.2007               Last Upate: 16.05.2007
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="cName"></param>
    ''' <param name="cFields"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Erstellt die Tabelle aus den Daten "cFields", Neuanlage
    ''' </remarks>
    Function fktCreate_Tabelle(ByVal db As ADOX.Catalog, _
                               ByVal cName As String, _
                               ByVal cFields As String) As Boolean
        On Error GoTo fktCreate_Tabelle_Err
        Dim nFelder As Integer
        Dim vType As Object
        Dim nLen As Object
        Dim nMax As Integer
        Dim arFelder() As String
        Dim arInhalt() As String
100:    fktCreate_Tabelle = True

102:    table = New ADOX.Table
104:    table.Name = cName
106:    db.Tables.Append(table)

108:    arFelder = Split(cFields, vbCrLf)
110:    nMax = UBound(arFelder) - 1

112:    For nFelder = 0 To nMax
114:        If Trim$(arFelder(nFelder)) <> "" Then
116:            arInhalt = Split(arFelder(nFelder), ",")
118:            vType = SetType(Trim$(arInhalt(1)))
120:            nLen = 0
122:            If Trim$(arInhalt(1)) = "dbText" Then
124:                If Trim$(arInhalt(2)) = "" Then
126:                    nLen = 255
                    Else
128:                    nLen = Val(arInhalt(2))
                    End If
                End If
130:            column = New ADOX.Column
                With column
132:                .ParentCatalog = db : .Name = Trim$(arInhalt(0)) : .Type = vType : .DefinedSize = nLen
                End With
134:            db.Tables(cName).Columns.Append(column)
            End If
        Next
        '<EhFooter>
        Exit Function

fktCreate_Tabelle_Err:
        ErrReport(Err.Description, "ADR.moDatenBasis.fktCreate_Tabelle", Erl)
        Resume Next
        '</EhFooter>
    End Function

    ''' <summary>
    ''' Procedure  : fktCreate_Tabelle2
    ''' Created by : Uwe Reich
    ''' Date-Time  : 16.05.2007               Last Upate: 16.05.2007
    ''' </summary>
    ''' <param name="dbfile"></param>
    ''' <param name="cName"></param>
    ''' <param name="cFields"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Erstellt die Tabelle aus den Daten "cFields", Reparatur
    ''' </remarks>
    Function fktCreate_Tabelle2(ByVal dbfile As String, _
                               ByVal cName As String, _
                               ByVal cFields As String) As Boolean
        On Error GoTo fktCreate_Tabelle2_Err
        Dim nFelder As Integer
        Dim vType As Object
        Dim nLen As Object
        Dim nMax As Integer
        Dim arFelder() As String
        Dim arInhalt() As String
        Dim db, tbl
        fktCreate_Tabelle2 = True

        db = New ADOX.Catalog
        db = CreateObject("ADOX.Catalog")
100:    db.ActiveConnection = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & dbfile
102:    tbl = CreateObject("ADOX.Table")
        tbl.Name = cName
104:    db.Tables.Append(tbl)


106:    arFelder = Split(cFields, vbCrLf)
108:    nMax = UBound(arFelder) - 1

110:    For nFelder = 0 To nMax
112:        If Trim$(arFelder(nFelder)) <> "" Then
114:            arInhalt = Split(arFelder(nFelder), ",")
116:            vType = SetType(Trim$(arInhalt(1)))
                nLen = 0
118:            If Trim$(arInhalt(1)) = "dbText" Then
120:                If Trim$(arInhalt(2)) = "" Then
                        nLen = 255
                    Else
                        nLen = Val(arInhalt(2))
                    End If
                End If
                column = New ADOX.Column
                With column
122:                .ParentCatalog = db : .Name = Trim$(arInhalt(0)) : .Type = vType : .DefinedSize = nLen
                End With
124:            db.Tables(cName).Columns.Append(column)
            End If
        Next
        '<EhFooter>
        Exit Function

fktCreate_Tabelle2_Err:
        ErrReport(Err.Description, "ADR.moDatenBasis.fktCreate_Tabelle2", Erl)
        Resume Next
        '</EhFooter>
    End Function

    ''' <summary>
    ''' Procedure  : SetType
    ''' Created by : Uwe Reich
    ''' Date-Time  : 16.05.2007               Last Upate: 16.05.2007
    ''' </summary>
    ''' <param name="cfType"></param>
    ''' <returns>
    ''' Feldtypnummer
    ''' </returns>
    ''' <remarks>
    ''' Description: Ermittelt den Feldtype
    ''' </remarks>
    Private Function SetType(ByVal cfType As String) As Object
        On Error Resume Next
        SetType = ""
        Select Case cfType
            'Case Is = "dbBinary"
            '    SetType = ADOX.DataTypeEnum.adBinary
            Case Is = "dbBoolean"
                SetType = ADOX.DataTypeEnum.adBoolean
                'Case Is = "dbByte"
                '    SetType = ADOX.DataTypeEnum.adBSTR 'Byte
            Case Is = "dbChar"
                SetType = ADOX.DataTypeEnum.adVarWChar
            Case Is = "dbCurrency"
                SetType = ADOX.DataTypeEnum.adCurrency
            Case Is = "dbDate"
                SetType = ADOX.DataTypeEnum.adDate
            Case Is = "dbDecimal"
                SetType = ADOX.DataTypeEnum.adDecimal
            Case Is = "dbDouble"
                SetType = ADOX.DataTypeEnum.adDouble
            Case Is = "dbInteger"
                SetType = ADOX.DataTypeEnum.adInteger
            Case Is = "dbLong"
                SetType = ADOX.DataTypeEnum.adInteger
                'Case Is = "dbLongBinary"
                '    SetType = ADOX.DataTypeEnum.adLongVarBinary
            Case Is = "dbMemo"
                SetType = ADOX.DataTypeEnum.adLongVarWChar
                'Case Is = "dbNumeric"
                '    SetType = ADOX.DataTypeEnum.adNumeric
                'Case Is = "dbSingle"
                '    SetType = ADOX.DataTypeEnum.adSingle
            Case Is = "dbText"
                SetType = ADOX.DataTypeEnum.adVarWChar
                'Case Is = "dbTime"
                '    SetType = ADOX.DataTypeEnum.adDBTime
                'Case Is = "dbTimeStamp"
                '    SetType = ADOX.DataTypeEnum.adFileTime
                'Case Is = "dbVarBinary"
                '    SetType = ADOX.DataTypeEnum.adVarBinary

        End Select
    End Function

    ''' <summary>
    ''' Procedure  : CheckDB
    ''' Created by : Uwe Reich
    ''' Date-Time  : 19.05.2007               Last Upate: 19.05.2007    neue Änderung 18.02.2013  Erik Reich
    ''' </summary>
    ''' <param name="dbfile"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Überprüft die Datenbank auf Vollständigkeit
    ''' </remarks>
    Function CheckDB(ByVal dbfile As String) As Boolean
        On Error GoTo CheckDB_Err

        Dim i As Long
        Dim sEnd As String
        Dim lStart As Boolean
        Dim arDaten() As String
        Dim arFields() As String
        Dim arField() As String
        Dim cTabelle As String
        Dim cFields As String
        Dim cField As String
        Dim nMax As Integer
        Dim lStatus As Boolean
        Dim arInhalt() As String
        Dim lFStatus As Boolean



        sEnd = "<End>"
        ReDim arFields(0)
        ReDim arField(0)
        lFStatus = True
        CheckDB = True
100:    arDaten = Split(StruckturBD, vbCrLf)

102:    nMax = UBound(arDaten) - 1
        cTabelle = ""
104:    For i = 0 To nMax
            If lStart = False Then
                'Tabelle extrahieren
                If InStr(1, Trim$(arDaten(i)), "<") <> 0 And Trim$(arDaten(i)) <> sEnd Then
                    cTabelle = Trim$(arDaten(i))
                    cTabelle = Replace(cTabelle, "<", "")
                    cTabelle = Replace(cTabelle, ">", "")
                    lStatus = FindTabelle(cgConn, cTabelle)
                    If lStatus Then
                    Else
                        lgStatusCheck = True
                        ErrReport(cTabelle & " fehlt!", "ADR.moDatenBasis.fktAdd_Fiels", "0")
                    End If
                    lStart = True
                End If
            Else
                'Einzelne Felder übertprüfen
                If InStr(1, Trim$(arDaten(i)), sEnd) = 0 Then
                    arFields(UBound(arFields)) = arDaten(i)
                    ReDim Preserve arFields(UBound(arFields) + 1)
                    If lStatus Then
                        arInhalt = Split(arDaten(i), ",")

                        If Not dbFieldExists(cTabelle, arInhalt(0), dbfile) Then
                            'Feld existiert nicht
                            ErrReport(cTabelle & " " & arInhalt(0) & " fehlt", "ADR.moDatenBasis.fktAdd_Fiels", "0")
                            arField(UBound(arField)) = arDaten(i)
                            ReDim Preserve arField(UBound(arField) + 1)
                            lFStatus = False
                            lgStatusCheck = True
                        End If
                    End If
                Else
                    If lStatus = False Then
                        'Fehlende Tabelle anlegen
                        cFields = Join(arFields, vbCrLf)
                        Call fktCreate_Tabelle2(dbfile, cTabelle, cFields)
                        ErrReport("Fehlende Tabelle: " & cTabelle & " angelegt.", "ADR.moDatenBasis.fktAdd_Fiels", "0")
                    ElseIf lFStatus = False Then
                        'Fehlende Felder anlegen
                        cField = Join(arField, vbCrLf)
                        Call fktAdd_Fiels(dbfile, cTabelle, cField)
                        '4 parameter= andangswert der gesetzt werden soll für gesamte tabelle
                        'arField = Split(cField, ",")
                        Dim arField2() As String
                        For i1 = 0 To arField.Length - 2
                            arField2 = Split(arField(i1), ",")
                            If arField2.Length = 4 Then
                                Dim afeld(0) As String
                                Dim aWert(0) As String
                                afeld(0) = arField2(0)
                                aWert(0) = arField2(3).Trim
                                Call fcUpdateCommand(cTabelle, afeld, aWert, "")
                            End If
                        Next

                        ErrReport("Fehlende Felder in " & cTabelle & " angelegt.", "ADR.moDatenBasis.fktAdd_Fiels", "0")
                    Else
                    End If
                    ReDim arFields(0)
                    ReDim arField(0)
                    lStart = False
                    lFStatus = True
                End If
            End If
        Next

        '<EhFooter>
        Exit Function

CheckDB_Err:
        ErrReport(Err.Description, "ADR.moDatenBasis.CheckDB", Erl)
        CheckDB = False
        Resume Next

    End Function

    ''' <summary>
    ''' Procedure  : FindTabelle
    ''' Created by : Uwe Reich
    ''' Date-Time  : 19.05.2007               Last Upate: 19.05.2007
    ''' </summary>
    ''' <param name="dbhandle"></param>
    ''' <param name="cName"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Prüft ob die Tabelle existiert via ADOX
    ''' </remarks>
    Function FindTabelle(ByVal dbhandle As String, ByVal cName As String) As Boolean

        Dim db, tbl
        db = CreateObject("ADOX.Catalog")
        db.ActiveConnection = dbhandle

        On Error Resume Next
        tbl = db.Tables.Item(cName)
        FindTabelle = True
        If Err.Number <> 0 Then
            FindTabelle = False
        End If
        db = Nothing
    End Function

    ''' <summary>
    ''' Procedure  : dbFieldExists
    ''' Created by : Uwe Reich
    ''' Date-Time  : 19.05.2007               Last Upate: 19.05.2007
    ''' </summary>
    ''' <param name="Table"></param>
    ''' <param name="FieldName"></param>
    ''' <param name="cConn"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Prüfung ob das Feld existiert
    ''' </remarks>
    Function dbFieldExists(ByVal Table As String, _
                           ByVal FieldName As String, _
                           ByVal cConn As String) As Boolean
        'Dim dsTmp As New DataSet("myDataSet")
        'Dim dtTmp As DataTable
        'Dim daTmp As OleDbDataAdapter
        'Dim SQL_String As String
        'Try
        '    SQL_String = "SELECT [" & FieldName & "] FROM " & Table
        '    daTmp = New OleDb.OleDbDataAdapter(SQL_String, conPension)
        '    dtTmp = dsTmp.Tables.Add(Table)
        '    daTmp.Fill(dtTmp)
        dbFieldExists = True
        'Catch ex As Exception
        '    dbFieldExists = False
        'End Try
        'dsTmp = Nothing
    End Function

    ''' <summary>
    ''' Procedure  : fktAdd_Fiels
    ''' Created by : Uwe Reich
    ''' Date-Time  : 19.05.2007               Last Upate: 19.05.2007
    ''' </summary>
    ''' <param name="dbfile"></param>
    ''' <param name="cName"></param>
    ''' <param name="cFields"></param>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Anfügen fehlender Felder in den Tabellen aus einer Resourcen-Datei
    ''' </remarks>
    Function fktAdd_Fiels(ByVal dbfile As String, ByVal cName As String, ByVal cFields As String) As Boolean
        '<EhHeader>
        On Error GoTo fktAdd_Fiels_Err
        '</EhHeader>
        Dim nFelder As Integer
        Dim nLen As Object
        Dim nMax As Integer
        Dim arFelder() As String
        Dim arInhalt() As String
        Dim vType As Object
        Dim db, tbl
        db = New ADOX.Catalog
        db = CreateObject("ADOX.Catalog")
100:    db.ActiveConnection = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & dbfile
102:    tbl = CreateObject("ADOX.Table")
        tbl.Name = cName



        fktAdd_Fiels = True
104:    arFelder = Split(cFields, vbCrLf)
106:    nMax = UBound(arFelder) - 1
        With db
108:        For nFelder = 0 To nMax
110:            If Trim$(arFelder(nFelder)) <> "" Then
112:                arInhalt = Split(arFelder(nFelder), ",")
114:                vType = SetType(Trim$(arInhalt(1)))
                    nLen = 0
116:                If Trim$(arInhalt(1)) = "dbText" Then
118:                    If Trim$(arInhalt(2)) = "" Then
                            nLen = 255
                        Else
120:                        nLen = Val(arInhalt(2))
                        End If
                    End If
                    column = New ADOX.Column
                    With column
132:                    .ParentCatalog = db : .Name = Trim$(arInhalt(0)) : .Type = vType : .DefinedSize = nLen
                    End With
134:                db.Tables(cName).Columns.Append(column)
                End If
            Next
        End With
        db = Nothing
        '<EhFooter>
        Exit Function

fktAdd_Fiels_Err:
        ErrReport(Err.Description, "ADR.moDatenBasis.fktAdd_Fiels", Erl)
        fktAdd_Fiels = False
        '</EhFooter>
    End Function

    ''' <summary>
    ''' Procedure  : CheckINI
    ''' Created by : Uwe Reich
    ''' Date-Time  : 16.05.2007               Last Upate: 16.05.2007
    ''' </summary>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Anfügen fehlender Einträge aus einer Resourcen-Datei
    ''' </remarks>
    Function CheckINI() As Boolean
        '        On Error GoTo CheckINI_Err
        '        Dim i As Long
        '        Dim arStruk(), arZeile(), cTmp As String
        '        Dim nMax As Integer

        '100:    CheckINI = True
        '102:    arStruk = Split(StruckturINI(sgVersion, cgPfad), vbCrLf)

        '104:    nMax = UBound(arStruk) - 1


        '106:    With MyIni
        '108:        .IniFile = IniFile
        '110:        arStruk = Split(StruckturINI(sgVersion, cgPfad), vbCrLf)
        '112:        nMax = UBound(arStruk) - 1
        '114:        For i = 0 To nMax
        '116:            If arStruk(i) <> "" Then
        '118:                arZeile = Split(arStruk(i), ",")
        '120:                cTmp = .ReadEntry(Trim$(arZeile(0)), Trim$(arZeile(1)), 0)
        '122:                If cTmp = "0" Then
        '124:                    .WriteEntry(Trim$(arZeile(0)), Trim$(arZeile(1)), _
        '                                    Chr(34) & arZeile(2) & Chr(34))
        '                    End If
        '                End If
        '            Next
        '        End With

        '        '<EhFooter>
        '        Exit Function

        'CheckINI_Err:
        '        ErrReport(Err.Description, "ADR.moDatenBasis.CheckINI", Erl)
        '        CheckINI = False
        '        Resume Next
        '        '</EhFooter>
    End Function

    ''' <summary>
    ''' Procedure  : fktAutoRepair
    ''' Created by : Uwe Reich
    ''' Date-Time  : 30.03.2007               Last Upate: 30.03.2007
    ''' </summary>
    ''' <returns>
    ''' True / False
    ''' </returns>
    ''' <remarks>
    ''' Description: Führt eine automatische Reparatur der Datenbank durch
    '''              (Felder auf gültige Werte Überprüfen und notfalls Defaultwerte einsetzen.)
    ''' </remarks>
    Function fktAutoRepair() As Boolean
        ''<EhHeader>
        'On Error GoTo fktAutoRepair_Err
        ''</EhHeader>
        Dim i As Long
        Dim sEnd As String
        Dim lStart As Boolean
        Dim arDaten() As String
        Dim arFields() As String
        Dim cTabelle As String
        Dim nMax As Integer

        sEnd = "<End>"
        ReDim arFields(0)

        arDaten = Split(StruckturBD, vbCrLf)

        nMax = UBound(arDaten) - 1
        cTabelle = ""

        Try

            For i = 0 To nMax
                If lStart = False Then
                    'Tabelle extrahieren
                    If InStr(1, Trim$(arDaten(i)), "<") <> 0 And Trim$(arDaten(i)) <> sEnd Then
                        cTabelle = Trim$(arDaten(i))
                        cTabelle = Replace(cTabelle, "<", "")
                        cTabelle = Replace(cTabelle, ">", "")
                        lStart = True
                    End If
                Else
                    'Einzelne Felder ermitteln
                    If InStr(1, Trim$(arDaten(i)), sEnd) = 0 Then
                        arFields(UBound(arFields)) = arDaten(i)
                        ReDim Preserve arFields(UBound(arFields) + 1)
                    Else
                        'Tabelle überprüfen
                        Call fktCheckTabelle(cTabelle, arFields)
                        ReDim arFields(0)
                        lStart = False
                    End If
                End If
            Next

            fktAutoRepair = True

            '            Exit Function
            'fktAutoRepair_Err:
            '            ErrReport(Err.Description, "ADR.moDatenBasis.fktAutoRepair", Erl)
            '            Resume Next


        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            fktAutoRepair = False
        End Try

    End Function

    ''' <summary>
    '''  Procedure  : fktCheckTabelle
    '''  Created by : Uwe Reich
    '''  Date-Time  : 19.05.2007               Last Upate: 19.05.2007
    ''' </summary>
    ''' <param name="cTabelle"></param>
    ''' <param name="arFields"></param>
    ''' <remarks>
    ''' Description: Prüft alle Felder einer Tabelle
    ''' </remarks>
    Private Sub fktCheckTabelle(ByVal cTabelle As String, ByVal arFields() As String)
        '        ''<EhHeader>
        '        'On Error Resume Next 'GoTo fktCheckTabelle_Err
        '        ''</EhHeader>

        '        Dim i As Integer, nMaxField As Integer
        '        Dim arField() As String ',' j As Integer


        '        Dim dsTmp As New DataSet("myDataSet")
        '        Dim dtTmp As DataTable
        '        Dim daTmp As OleDbDataAdapter
        '        Dim SQL_String As String
        '        Dim nRows As DataRowCollection
        '        Dim para As OleDbParameter
        '        Dim paras As OleDbParameterCollection
        '        Try

        '            SQL_String = "SELECT * FROM " & cTabelle
        '            daTmp = New OleDb.OleDbDataAdapter(SQL_String, conPension)
        '100:        dtTmp = dsTmp.Tables.Add(cTabelle)
        '102:        daTmp.Fill(dtTmp)
        '            nRows = dtTmp.Rows
        '            If nRows.Count = 0 Then Exit Sub
        '104:        nMaxField = UBound(arFields) - 1
        '            For Each nRow As DataRow In dtTmp.Rows

        '106:            For i = 0 To nMaxField
        '108:                arField = Split(arFields(i), ",")
        '110:                If nRow.Item(Trim$(arField(0))) Is DBNull.Value Then
        '                        'Update-Kommando selbst definieren
        '                        SQL_String = "UPDATE " & cTabelle & " SET " & Trim$(arField(0)) & " = @newValue"
        '112:                    daTmp.UpdateCommand = New OleDbCommand(SQL_String, conPension)
        '114:                    paras = daTmp.UpdateCommand.Parameters
        '                        If Trim$(arField(1)) = "dbText" Then
        '                            nRow.Item(Trim$(arField(0))) = " "
        '116:                        para = paras.Add("@newValue", OleDbType.VarWChar, 255, Trim$(arField(0)))
        '                        ElseIf Trim$(arField(1)) = "dbMemo" Then
        '                            nRow.Item(Trim$(arField(0))) = " "
        '118:                        para = paras.Add("@newValue", OleDbType.LongVarWChar, 0, Trim$(arField(0)))
        '                        ElseIf Trim$(arField(1)) = "dbInteger" Then
        '                            nRow.Item(Trim$(arField(0))) = 0
        '120:                        para = paras.Add("@newValue", OleDbType.Integer, 0, Trim$(arField(0)))
        '                        ElseIf Trim$(arField(1)) = "dbLong" Then
        '                            nRow.Item(Trim$(arField(0))) = 0
        '122:                        para = paras.Add("@newValue", OleDbType.Integer, 0, Trim$(arField(0)))
        '                        End If
        '                        para.SourceVersion = DataRowVersion.Current
        '                        nRow.EndEdit()
        '124:                    daTmp.Update(dtTmp)
        '                    End If
        '                Next
        '            Next
        '            dsTmp = Nothing

        '        Catch ex As Exception
        '            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        '        End Try

        '        '        Exit Sub

        '        'fktCheckTabelle_Err:
        '        '        ErrReport(Err.Description, "ADR.moDatenBasis.fktCheckTabelle", Erl)
        '        '        Resume Next
    End Sub

#End Region

#Region "Hilfsfunktionen zum lesen und schreiben von Daten"

    ''' <summary>
    ''' Fügt einen neuen Datensatz per SQL in die angegebene Tabelle ein.
    ''' Feldstruktur und Werte werden als Arrays übergeben.
    ''' </summary>
    ''' <param name="cTabelle">Der Name der Ziel-Datenbanktabelle.</param>
    ''' <param name="arFields">Ein Array mit den Spaltennamen.</param>
    ''' <param name="arValue">Ein Array mit den dazugehörigen Werten.</param>
    ''' <returns>True, wenn das Einfügen erfolgreich war, andernfalls False.</returns>
    ''' <remarks>
    ''' 14.12.2007 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Veraltete VB6-Funktionen (UBound, Trim$, Mid, Len) durch moderne .NET-Entsprechungen ersetzt.
    ''' - SQL-Injection-Schutz durch Maskierung von Hochkommas (.Replace) implementiert.
    ''' - 'StringBuilder' für performanten und sauberen Aufbau des SQL-Befehls verwendet.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' </remarks>
    Public Function fcInsertCommand(ByVal cTabelle As String,
                                ByVal arFields() As String,
                                ByVal arValue() As String) As Boolean
        ' Sicherheitsprüfung: Arrays dürfen nicht Nothing sein
        If arFields Is Nothing OrElse arValue Is Nothing Then Return False

        ' Längenprüfung der Arrays (ersetzt UBound)
        If arFields.Length = 0 OrElse arFields.Length <> arValue.Length Then Return False

        Try
            Dim sbSql As New StringBuilder()
            sbSql.Append("INSERT INTO ").Append(cTabelle).Append(" (")

            ' 1. Spaltennamen verketten
            For i As Integer = 0 To arFields.Length - 1
                Dim fieldName As String = If(arFields(i) IsNot Nothing, arFields(i).Trim(), "")
                sbSql.Append(fieldName)

                If i < arFields.Length - 1 Then
                    sbSql.Append(", ")
                End If
            Next

            sbSql.Append(") VALUES (")

            ' 2. Werte verketten und aufbereiten
            For i As Integer = 0 To arValue.Length - 1
                Dim valueText As String = If(arValue(i) IsNot Nothing, arValue(i).Trim(), "")

                ' Boolean-Konvertierung für Access/SQL-Kompatibilität
                If valueText.Equals("True", StringComparison.OrdinalIgnoreCase) Then
                    valueText = "-1"
                ElseIf valueText.Equals("False", StringComparison.OrdinalIgnoreCase) Then
                    valueText = "0"
                Else
                    ' WICHTIG: SQL-Injection-Schutz für Strings (Hochkommas verdoppeln)
                    valueText = valueText.Replace("'", "''")
                End If

                ' Wert in einfache Anführungszeichen setzen
                sbSql.Append("'").Append(valueText).Append("'")

                If i < arValue.Length - 1 Then
                    sbSql.Append(", ")
                End If
            Next

            sbSql.Append(")")

            ' Eigentliches Schreiben in die Datenbank
            UpdateTable(sbSql.ToString())

            Return True

        Catch ex As Exception
            ' Optional: ErrReport nutzen, falls vorhanden, statt MsgBox im Live-Betrieb
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler in fcInsertCommand")
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Aktualisiert Datensätze in einer spezifischen Datenbanktabelle anhand übergebener Feld- und Wert-Arrays.
    ''' </summary>
    ''' <param name="cTabelle">Der Name der zu aktualisierenden Tabelle.</param>
    ''' <param name="arFields">Ein String-Array, das die Namen der zu aktualisierenden Spalten enthält.</param>
    ''' <param name="arValue">Ein String-Array, das die neuen Werte für die entsprechenden Spalten enthält.</param>
    ''' <param name="cBedingung">Die SQL-Filterbedingung (z. B. <c> WHERE ID = '123'</c>).</param>
    ''' <returns><c>True</c>, wenn das Update erfolgreich ausgeführt wurde; andernfalls <c>False</c>.</returns>
    ''' <remarks>
    ''' <para>Die Arrays für Felder und Werte müssen zwingend die gleiche Länge aufweisen. Boolesche Strings ('True'/'False') werden automatisch in SQL-konforme Bit-Werte ('-1'/'0') übersetzt.</para>
    ''' <para>
    ''' <b>Historie:</b><br/>
    ''' 14.12.2007 – Erstellt<br/>
    ''' 22.09.2026 – Optimiert: Umstellung auf den hochperformanten <c>StringBuilder</c> zur String-Generierung. Veraltete VB6-Funktionen (<c>UBound</c>, <c>Mid</c>, <c>Len</c>) durch native .NET-Äquivalente ersetzt. Hochkomma-Verdopplung zur Vorbeugung elementarer SQL-Fehler hinzugefügt.<br/>
    ''' </para>
    ''' </remarks>
    Public Function fcUpdateCommand(ByVal cTabelle As String,
                               ByVal arFields() As String,
                               ByVal arValue() As String,
                               ByVal cBedingung As String) As Boolean
        ' Validierung der Eingaben
        If arFields Is Nothing OrElse arValue Is Nothing Then Return False

        Dim nMax As Integer = arFields.Length - 1
        Dim mMax As Integer = arValue.Length - 1

        ' Validierung: Längenprüfung der Arrays
        If nMax = -1 OrElse nMax <> mMax Then Return False

        Try
            ' WICHTIGE OPTIMIERUNG: StringBuilder statt ressourcenfressender String-Kette (&) in der Schleife
            Dim sbSql As New System.Text.StringBuilder()
            sbSql.AppendFormat("UPDATE {0} SET ", cTabelle)

            For i As Integer = 0 To nMax
                Dim cTmp As String = arValue(i)

                ' Boolesche Werte für Access/SQL-Server kompatibel umwandeln
                If cTmp = "True" Then
                    cTmp = "-1"
                ElseIf cTmp = "False" Then
                    cTmp = "0"
                Else
                    ' MINIMAL-SCHUTZ: Verhindert Abstürze bei Namen wie O'Connor durch Verdopplung des Hochkommas
                    If cTmp IsNot Nothing Then cTmp = cTmp.Replace("'", "''")
                End If

                ' Spaltenname trimmen und Wert hinzufügen
                Dim feldName As String = arFields(i).Trim()
                sbSql.AppendFormat("{0}='{1}',", feldName, cTmp)
            Next

            ' Das letzte, überschüssige Komma entfernen
            If sbSql.Length > 0 Then sbSql.Length -= 1

            ' Bedingung anhängen
            sbSql.Append(cBedingung)

            ' SQL-Befehl an die ausführende Methode übergeben
            UpdateTable(sbSql.ToString())

            Return True

        Catch ex As Exception
            ' Fehlerbehandlung über die interne Schnittstelle (MsgBox durch ErrReport ersetzt, um UI-Blockaden zu vermeiden)
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Neuen Datensatz in die Data Table,
    ''' Feldstruktur und Werte werden per Array übergeben
    ''' </summary>
    ''' <param name="dtTmp"></param>
    ''' <param name="arFields"></param>
    ''' <param name="arValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 30.12.2007 Create
    ''' </remarks>
    Public Function fcInsertTable(ByVal dtTmp As DataTable, _
                                  ByVal arFields() As String, _
                                  ByVal arValue() As String) As Boolean
        Try

            Dim i As Integer
            Dim newRow As DataRow = dtTmp.NewRow()
            Dim mMax As Integer = UBound(arValue)
            Dim nMax As Integer = UBound(arFields)

            If nMax = -1 Then Exit Function
            If nMax <> mMax Then Exit Function
            For i = 0 To nMax
                newRow(Trim$(arFields(i))) = arValue(i)
            Next
            'Add the row to the rows collection.
            dtTmp.Rows.Add(newRow)
            fcInsertTable = True
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            fcInsertTable = False
        End Try
    End Function

    ''' <summary>
    ''' Data Table aktualisieren
    ''' Feldstruktur und Werte werden per Array übergeben
    ''' </summary>
    ''' <param name="dtTmp"></param>
    ''' <param name="arFields"></param>
    ''' <param name="arValue"></param>
    ''' <param name="cBedingung"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 14.12.2007 Create
    ''' </remarks>
    Public Function fcUpdateTable(ByVal dtTmp As DataTable, _
                                  ByVal arFields() As String, _
                                  ByVal arValue() As String, _
                                  ByVal cBedingung As String) As Boolean
        Try
            Dim i As Integer

            Dim cRow() As DataRow = dtTmp.Select(cBedingung)
            Dim mMax As Integer = UBound(arValue)
            Dim nMax As Integer = UBound(arFields)

            If nMax = -1 Then Exit Function
            If nMax <> mMax Then Exit Function

            If cRow.Length <> 0 Then
                For i = 0 To nMax
                    cRow(0)(Trim$(arFields(i))) = arValue(i)
                Next
            End If
            fcUpdateTable = True
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            fcUpdateTable = False
        End Try
    End Function

    '''' <summary>
    '''' Data Table aktualisieren (Datensatzlöschen
    '''' </summary>
    '''' <param name="dtTmp"></param>
    '''' <param name="cBedingung"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' 20.03.2009 Create
    '''' </remarks>
    'Public Function fcDeleteTableRow(ByVal dtTmp As DataTable,
    '                                 ByVal cBedingung As String) As Boolean
    '    Try
    '        Dim cRow() As Data.DataRow = dtTmp.Select(cBedingung)
    '        If cRow.Length <> 0 Then
    '            cRow(0).Delete()
    '        End If

    '        fcDeleteTableRow = True
    '    Catch ex As Exception
    '        ErrReport(ex.Message, ex.Source, ex.StackTrace)
    '        fcDeleteTableRow = False
    '    End Try
    'End Function

    ''' <summary>
    ''' Löscht alle Datensätze aus einer DataTable, die der angegebenen Bedingung entsprechen.
    ''' </summary>
    ''' <param name="dtTmp">Die zu bearbeitende DataTable.</param>
    ''' <param name="cBedingung">Der Filterausdruck (z. B. "ID = '123'").</param>
    ''' <returns>True, wenn der Löschvorgang fehlerfrei durchlaufen wurde, andernfalls False.</returns>
    ''' <remarks>
    ''' 20.03.2009 - Create
    ''' 28.09.2026 - Code-Optimierung:
    ''' - Schleife hinzugefügt, um alle matchenden Zeilen zu löschen (nicht nur die erste).
    ''' - Veraltete Namespace-Deklaration 'Data.DataRow' auf die .NET-Standard-Klasse 'DataRow' verkürzt.
    ''' - Veraltete Zuweisung an den Funktionsnamen durch ein explizites 'Return' ersetzt.
    ''' - Null- und Leerkontrolle für die Parameter integriert.
    ''' </remarks>
    Public Function fcDeleteTableRow(ByVal dtTmp As DataTable, ByVal cBedingung As String) As Boolean
        ' Sicherheitsprüfung: DataTable muss existieren und Bedingung darf nicht leer sein
        If dtTmp Is Nothing OrElse String.IsNullOrWhiteSpace(cBedingung) Then Return False

        Try
            ' Alle Zeilen ermitteln, auf die die Bedingung zutrifft
            Dim foundRows As DataRow() = dtTmp.Select(cBedingung)

            ' Wenn Datensätze gefunden wurden, alle Betroffenen löschen
            If foundRows.Length > 0 Then
                For Each row As DataRow In foundRows
                    row.Delete()
                Next
            End If

            Return True

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Führt eine aktualisierende oder löschende SQL-Anweisung (z. B. INSERT, UPDATE, DELETE) 
    ''' über eine externe PHP-Schnittstelle auf dem Webserver aus.
    ''' </summary>
    ''' <param name="sql_String">Die auszuführende SQL-Anweisung.</param>
    ''' <returns>Die Anzahl der betroffenen Datensätze (sofern vom PHP-Skript zurückgegeben).</returns>
    ''' <remarks>
    ''' Die Funktion leitet den SQL-Befehl zusammen mit der in <c>cgIPWeb</c> hinterlegten 
    ''' Server-IP an die PHP-Komponente weiter.
    ''' 19.01.2012 Create <br/>
    ''' 23.09.2026 Refactored 
    ''' </remarks>
    Public Function UpdateTable(ByVal sql_String As String) As Integer
        ' Validierung: Leere SQL-Strings gar nicht erst an den Server senden
        If String.IsNullOrWhiteSpace(sql_String) Then Return 0

        Dim sIP As String = cgIPWeb

        ' WICHTIG: "Return" hinzugefügt, um das Ergebnis von PHP.Update auch wirklich zurückzugeben
        Return PHP.Update(sql_String, sIP)
    End Function

    ''' <summary>
    ''' Sendet eine SQL-Abfrage an das PHP-Web-Backend und gibt das Ergebnis als DataTable zurück.
    ''' </summary>
    ''' <param name="cSQL">Der auszuführende SQL-Befehl als String.</param>
    ''' <returns>Eine DataTable mit den abgefragten Datensätzen oder eine leere DataTable im Fehlerfall.</returns>
    ''' <remarks>
    ''' 22.09.2026 Code-Optimierung: Integration von Exception-Handling zur Absicherung bei Netzwerk- 
    ''' oder Serverfehlern und Umstellung auf das modernere 'Return'-Schlüsselwort.
    ''' </remarks>
    Public Function fcReadDataTable(ByVal cSQL As String) As DataTable
        ' Vorabprüfung: Wenn kein SQL-Befehl übergeben wurde, direkt abbrechen
        If String.IsNullOrWhiteSpace(cSQL) Then Return New DataTable()

        Try

            ' Abfrage über PHP-Schnittstelle ausführen
            Dim sIP As String = cgIPWeb
            Dim dtErgebnis As DataTable = PHP.DataTable(cSQL, sIP)

            If dtErgebnis IsNot Nothing Then
                Return dtErgebnis
            End If

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
        ' Gibt im Fehlerfall eine leere, aber initialisierte DataTable zurück (verhindert Folge-Abstürze)
        Return New DataTable()
    End Function



    '''' <summary>
    '''' Sucht Datensatz, Rückgabe ist Array, Bei nichtgefunden Array(0)=" "
    ''''  arfeld = fcDataSeekFeld("Select * From Werbung Where Name='", "Test", {"link", "kopf", "Betreff", "Fuss"})
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>
    '''' 03.02.2012 Create
    '''' </remarks>
    'Public Function fcDataSeek(ByRef sSQLText As String, ByRef sVergleichswert As String,
    '                           ByRef nSatz As Integer, ByVal ParamArray arRuekgabeFeld() As String) As Array
    '    Dim sSQL As String
    '    If sVergleichswert = "" Then
    '        sSQL = Mid(sSQLText, 1, Len(sSQLText) - 1)
    '    Else
    '        sSQL = sSQLText & sVergleichswert & "'"
    '    End If
    '    Dim dt As DataTable = fcReadDataTable(sSQL)
    '    Dim sWert As String = ""
    '    If dt.Rows.Count > 0 And dt.Rows.Count - 1 >= nSatz Then
    '        For i = 0 To arRuekgabeFeld.Length - 1
    '            arRuekgabeFeld(i) = dt.Rows(nSatz).Item(arRuekgabeFeld(i)).ToString()
    '        Next
    '    Else
    '        arRuekgabeFeld(0) = " "
    '    End If
    '    fcDataSeek = arRuekgabeFeld
    'End Function

    ''' <summary>
    ''' Sucht einen oder mehrere spezifische Datensätze in der Datenbank.
    ''' Gibt ein String-Array mit den Werten der angeforderten Felder für den angegebenen Datensatz-Index zurück.
    ''' Wenn kein Datensatz gefunden wird, enthält das erste Element des Arrays ein Leerzeichen (" ").
    ''' </summary>
    ''' <param name="sSQLText">Der SQL-Basisstring (z. B. "Select * From Werbung Where Name='").</param>
    ''' <param name="sVergleichswert">Der Wert, der an den SQL-String angehängt wird. Bleibt er leer, wird das öffnende Anführungszeichen entfernt.</param>
    ''' <param name="nSatz">Der zeilenbasierte Index des zurückzugebenden Datensatzes (0 für den ersten Treffer).</param>
    ''' <param name="arRuekgabeFeld">Ein Parameter-Array (ParamArray) mit den Namen der Tabellenspalten, die ausgelesen werden sollen.</param>
    ''' <returns>Ein String-Array mit den Werten der abgefragten Spalten.</returns>
    ''' <remarks>
    ''' <b>Historie:</b><br/>
    ''' 03.02.2012 – Erstellt<br/>
    ''' 24.09.2026 – Uwe: Array-Zuweisung korrigiert (verhindert Indexfehler bei leeren Treffern), auf ByVal umgestellt und Rückgabetyp konkretisiert.<br/>
    ''' </remarks>
    Public Function fcDataSeek(ByVal sSQLText As String,
                           ByVal sVergleichswert As String,
                           ByVal nSatz As Integer,
                           ByVal ParamArray arRuekgabeFeld() As String) As String()

        Dim sSQL As String = ""

        ' 1. SQL-String dynamisch zusammenbauen
        If String.IsNullOrEmpty(sVergleichswert) Then
            If sSQLText.EndsWith("'") OrElse sSQLText.EndsWith("=") Then
                sSQL = sSQLText.Substring(0, sSQLText.Length - 1)
            Else
                sSQL = sSQLText
            End If
        Else
            sSQL = sSQLText & sVergleichswert & "'"
        End If

        ' 2. Daten abrufen
        Dim dt As DataTable = fcReadDataTable(sSQL)

        ' Ergebnis-Array in der passenden Größe initialisieren
        Dim nFeldAnzahl As Integer = If(arRuekgabeFeld IsNot Nothing, arRuekgabeFeld.Length, 0)
        Dim arResult(Math.Max(0, nFeldAnzahl - 1)) As String

        ' 3. Prüfen, ob Datensätze vorhanden sind und der gewünschte Index existiert
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso nSatz < dt.Rows.Count Then
            For i As Integer = 0 To nFeldAnzahl - 1
                ' Spaltenwert sicher auslesen
                If dt.Columns.Contains(arRuekgabeFeld(i)) Then
                    arResult(i) = dt.Rows(nSatz).Item(arRuekgabeFeld(i)).ToString()
                Else
                    arResult(i) = ""
                End If
            Next
        Else
            ' Fallback laut historischer Definition: Wenn nichts gefunden wurde, erstes Feld mit Leerzeichen füllen
            If arResult.Length > 0 Then
                arResult(0) = " "
            End If
        End If

        Return arResult
    End Function


    ''' <summary>
    ''' Sucht Datensatz, Rückgabe ist Array, Bei nichtgefunden Array(0)=" "
    '''  arfeld = fcDataSeekFeld("Select * From Werbung Where Name=Test",0, {"link", "kopf", "Betreff", "Fuss"})
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 26.03.2012 Create
    ''' </remarks>
    Public Function fcGetData(ByRef sSQLText As String, ByRef nSatz As Integer, ByVal ParamArray arRuekgabeFeld() As String) As Array

        Dim dt As DataTable = fcReadDataTable(sSQLText)
        Dim sWert As String = ""
        If dt.Rows.Count > 0 And dt.Rows.Count - 1 >= nSatz Then
            For i = 0 To arRuekgabeFeld.Length - 1
                arRuekgabeFeld(i) = dt.Rows(nSatz).Item(arRuekgabeFeld(i)).ToString()
            Next
        Else
            arRuekgabeFeld(0) = " "
        End If
        fcGetData = arRuekgabeFeld
    End Function
    ''' <summary>
    ''' inhalt aus sql tabelle in String schreiben trennzeiche | und 1013 
    ''' </summary>
    ''' <param name="sTabel"></param>
    ''' <param name="sBedingung"></param>
    ''' <param name="sFeld"></param>
    ''' <param name="sKopf"></param>
    ''' <remarks>
    ''' 20.04.2024 Create Burg Spreewald
    ''' </remarks>
    Public Function fcTabelView(ByVal sTabel As String, ByVal sBedingung As String,
                                ByRef sFeld As String, ByRef sKopf As String) As String
        fcTabelView = ""
        Try
            Dim sText As String = ""
            Dim aKopf() As String = Split(sKopf, ";")
            Dim aFeld() As String = Split(sFeld, ";")

            Dim sSQL As String = "Select * From " & sTabel & "  Where " & sBedingung  'BID='" & sBNr1 & "' and ZimID = '" & sZim1 & "'"
            Dim dt As DataTable = fcReadDataTable(sSQL)
            Dim nMax As Integer = dt.Rows.Count - 1
            Dim nSpalte As Integer = aKopf.Length() - 1
            Dim aLength(nSpalte) As Integer
            Dim aDT(nSpalte, nMax + 2) As String
            For j = 0 To nSpalte
                aDT(j, 1) = aKopf(j)
                aLength(j) = aDT(j, 1).Length
            Next
            For j = 0 To nSpalte
                For i = 0 To nMax
                    aDT(j, i + 2) = dt.Rows(i).Item(aFeld(j))
                    If aLength(j) < aDT(j, i + 2).Length Then aLength(j) = aDT(j, i + 2).Length
                Next
            Next
            For j = 0 To nSpalte
                aDT(j, 0) = aLength(j)
            Next
            For i = 0 To nMax + 2
                For j = 0 To nSpalte
                    fcTabelView = fcTabelView & aDT(j, i) & "|"
                Next
                fcTabelView = Mid(fcTabelView, 1, fcTabelView.Length - 1) & vbCrLf
            Next
        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Function

    ''' <summary>
    ''' Hängt einen leeren Datensatz (Blank) an die angegebene Datei/Tabelle an.
    ''' </summary>
    ''' <param name="sFile">Der Name der Datei oder Tabelle im System.</param>
    ''' <returns>Die ID-Nummer des neu angelegten Datensatzes als String.</returns>
    ''' <remarks>
    ''' 16.09.2013 Create <br/>
    ''' 23.09.2026 Refactored (ByVal-Korrektur und Return-Statement implementiert)
    ''' </remarks>
    Public Function fcAppendBlank(ByVal sFile As String) As String
        Dim sIp As String = cgIPWeb

        ' Explizites Return statt Zuweisung an den Funktionsnamen
        Return PHP.Append(sFile, sIp)
    End Function


#End Region
    Public Function fcTableFind(ByRef dt As DataTable, ByRef sName As String, ByRef sFind As String) As Integer
        fcTableFind = -1
        For i = 0 To dt.Rows.Count - 1
            If sFind = dt.Rows(i).Item(sName).ToString Then
                fcTableFind = i
                Exit For
            End If
        Next
    End Function



    '''' <summary>
    '''' Führt eine MariaDB-Abfrage aus und gibt die erste Spalte der ersten Zeile zurück.
    '''' </summary>
    '''' <param name="sSQL">Die SQL-Abfrage mit optionalen Parametern.</param>
    '''' <param name="params">Eine Liste von MySqlParametern zum Schutz vor SQL-Injection.</param>
    'Public Function fcExecuteScalar(ByVal sSQL As String, Optional ByVal params As List(Of MySqlParameter) = Nothing) As Object
    '    ' Passe den ConnectionString an deine MariaDB-Instanz an
    '    Dim sConnectionString As String = "Server=localhost;Database=deine_db;Uid=dein_user;Pwd=dein_passwort;"
    '    Dim oResult As Object = Nothing

    '    Try
    '        ' Using schließt die Verbindung und gibt Ressourcen auch bei Fehlern sicher frei
    '        Using conn As New MySqlConnection(sConnectionString)
    '            Using cmd As New MySqlCommand(sSQL, conn)

    '                ' Parameter sicher hinzufügen, falls vorhanden
    '                If params IsNot Nothing Then
    '                    cmd.Parameters.AddRange(params.ToArray())
    '                End If

    '                conn.Open()
    '                oResult = cmd.ExecuteScalar()

    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        ' Fehler nach oben werfen, damit er vom prSetToolTip-Catch abgefangen wird
    '        Throw
    '    End Try

    '    Return oResult
    'End Function


End Module