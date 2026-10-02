Imports MySql.Data.MySqlClient
''' <summary>
''' Globales Konfigurations- und Datenmodul der Anwendung.
''' Verwaltet INI-Schlüssel, globale DataTables, Umgebungsvariablen und Farbzuweisungen.
''' </summary>
''' <remarks>
''' 17.12.2011 Erstellt<br/>
''' 21.09.2026 Code-Optimierung: Strukturierung, Bereinigung von totem Code, Behebung von Tippfehlern.
''' </remarks>
Public Module Globals

    '==========================================================================
    '#### INI-FILE SEKTIONEN & SCHLÜSSEL ####
    '==========================================================================
    Public Const SEC_COMMON As String = "common"
    Public Const KEY_VERSION As String = "Version"
    Public Const KEY_SDIR As String = "SaveDir"       ' Sicherungsverzeichnis
    Public Const KEY_DDIR As String = "DatevDir"      ' Datevverzeichnis
    Public Const KEY_ADIR As String = "AblageDir"     ' Ablageverzeichnis

    Public Const SEC_BASIC As String = "controls"
    Public Const KEY_COMPANY As String = "Inhaber"
    Public Const KEY_STR As String = "Straße"
    Public Const KEY_PLZ As String = "Plz"
    Public Const KEY_ORT As String = "Ort"
    Public Const KEY_TE1 As String = "Telefon1"
    Public Const KEY_TE2 As String = "Telefon2"
    Public Const KEY_TE3 As String = "Telefon3"
    Public Const KEY_TE4 As String = "Telefon4"
    Public Const KEY_Web As String = "WebAdresse"
    Public Const KEY_UStNr As String = "Umsatzsteuer-Nummer"
    Public Const KEY_UStID As String = "Umsatzsteuer-ID"

    Public Const SEC_EMAIL As String = "EMail"
    Public Const KEY_SMTP As String = "SMTP-Server"
    Public Const KEY_EMAIL As String = "E-Mail Adresse"
    Public Const KEY_SERVER As String = "Am Server anmelden"
    Public Const KEY_USER As String = "UserName"
    Public Const KEY_PASS As String = "Password"
    Public Const KEY_MIME As String = "Kodierung MIME"
    Public Const KEY_UUE As String = "Kodierung UUE"
    Public Const KEY_HTML As String = "Kodierung HTML"
    Public Const KEY_AUTOR As String = "Autor"
    Public Const KEY_BETREFF As String = "Betreff"

    'Public Const KEY_MWST1 As String = "Mwst voll"       ' Getränke
    Public Const KEY_MWST2 As String = "Mwst reduziert"  ' Übernachtung
    'Public Const KEY_MWST3 As String = "Mwst Sonder"     ' Speisen
    'Public Const KEY_MWSTAlt As String = "Mwst Alt"      ' Alter Steuersatz
    'Public Const KEY_MWSTNEW1 As String = "Mwst Neu1"    ' Neuer Steuersatz 1
    'Public Const KEY_MWSTNEW2 As String = "Mwst Neu2"    ' Neuer Steuersatz 2

    Public Const KEY_KNr As String = "Kunden-Nummer"
    Public Const KEY_BNr As String = "Buchungs-Nummer"
    Public Const KEY_RNr As String = "Rechnungs-Nummer"
    Public Const KEY_Fr As String = "Fruestueck"
    Public Const KEY_Ge As String = "Getraenke"

    Public Const KEY_KKonto As String = "Kasse-Konto"
    Public Const KEY_BKonto As String = "Bank-Konto"
    'Public Const KEY_GKonto7 As String = "G-Konto7"
    'Public Const KEY_GKonto19 As String = "G-Konto19"
    'Public Const KEY_GKontoS As String = "G-KontoS"
    'Public Const KEY_GKontoAlt As String = "G-KontoAlt"
    'Public Const KEY_GKontoNeu1 As String = "G-KontoNeu1"
    'Public Const KEY_GKontoNeu2 As String = "G-KontoNeu2"

    Public Const SEC_PREIS As String = "Preise"
    Public Const KEY_UArt As String = "U-Art"
    Public Const KEY_Kate As String = "Kategorie"
    Public Const KEY_NetUser As String = "NetUser"
    Public Const KEY_IPSchloss As String = "IPSchloss"
    Public Const KEY_RFIDPort As String = "RFIDPort"

    '==========================================================================
    '#### DATENBANK & ADO.NET ELEMENTE ####
    '==========================================================================
    Public conPension As New MySqlConnection
    Public conString As String                  ' Verbindungskette für die DB

    Public dtKTO As DataTable                   ' Kontodaten
    Public dtObj As DataTable                   ' Objekte (Häuser)
    Public dtZim As DataTable                   ' Zimmer
    Public dtUser As DataTable                  ' User / Nutzer
    Public dtTxt As DataTable                   ' Makros- und Buchungstexte
    Public dtBuc As DataTable                   ' Tabelle Buchungen
    Public dtDat As DataTable                   ' Tabelle Datev
    Public dtWer As DataTable                   ' Tabelle Werbung
    Public dtPre As DataTable                   ' Tabelle Preise
    Public dtPer As DataTable                   ' Tabelle Personal
    Public dtGas As DataTable                   ' Tabelle Gäste / Nutzer (Erstdeklaration)
    Public dtKun As DataTable                   ' Tabelle Kunde

    '==========================================================================
    '#### GLOBALE SYSTEMVARIABLEN ####
    '==========================================================================
    Public sgVersion As String = "2.0.0.9"
    Public cgPfad As String
    Public cgIPWeb As String                    ' Web-IP-Adresse für den SQL-Server
    Public cgConn As String                     ' Connection String (Redundant zu conString?)
    Public cgTablet As String                   ' Kennzeichnung für Tablet-Betrieb
    Public IniFile As String
    Public DataBaseName As String

    '#### Protokollierung & Fehler ####
    Public cgLogFile As String
    Public cgErrLogFile As String
    Public ngError As Integer                   ' Anzahl der Fehler im Logbuch

    '#### Benutzer & Rechte ####
    Public sgUser As String                     ' Angemeldeter User
    Public lgLogin As Boolean                   ' Login erfolgreich-Flag
    Public ngRechteStatus As Integer = 5        ' Rechte des aktuellen Nutzers
    Public lgStatusCheck As Boolean             ' Datenbank-Statuskontrolle

    '#### Buchungs- & Reservierungskontext ####
    Public sgRNr As String                      ' Aktuelle Rechnungsnummer
    Public lgRech As Boolean                    ' Flag: Rechnung bereits geschrieben
    Public sgRBID As String                     ' Buchungs-ID für Reservierung
    Public sgRZID As String                     ' Zimmer-ID für Reservierung
    Public sgGID As String                      ' Gast-ID 
    Public sgZNr As String                      ' Zimmernummer
    Public sgSasion As String                   ' Kennzeichnung der Saison (N=Neben, V=Vor, H=Haupt)
    Public sgWerbung As String                  ' Werbekanal für Reservierung
    Public lLang As Boolean                     ' Projektsprache: True = Englisch, False = Deutsch

    '#### Hardware / Netzwerk-Schnittstellen ####
    Public sgCodeNew As String                  ' Transpondercode inkl. Zeitstempel
    Public sgNet As String = ""                 ' Netzwerk Übertragungs-Flag
    Public lgSichern As Boolean = True          ' Datensicherung auf FTP ausführen

    '==========================================================================
    '#### ANZEIGE- & FARB-EINSTELLUNGEN ####
    '==========================================================================
    Public cgColorRow As Color = Color.Khaki
    Public arFarbe(20, 2) As String             ' Speicherort für farbliche Bezeichner
    Public arIni(100) As String                 ' Globales INI-Daten-Array
    Public arEMail(14) As String                ' Globales E-Mail-Konfigurations-Array

    '#### Kalender- und Belegungsfarben ####
    Public fColorForeAnreise, fColorBackAnreise As Color
    Public fColorForeFerien1, fColorBackFerien1 As Color
    Public fColorForeFerien2, fColorBackFerien2 As Color
    Public fColorForeFerien3, fColorBackFerien3 As Color
    Public fColorForeFerien4, fColorBackFerien4 As Color
    Public fColorForeFerien5, fColorBackFerien5 As Color
    Public fColorForeNebenSaison1, fColorBackNebenSaison1 As Color
    Public fColorForeNebenSaison2, fColorBackNebenSaison2 As Color
    Public fColorForeHauptSaison, fColorBackHauptSaison As Color
    Public fColorForeFeiertag1, fColorBackFeiertag1 As Color
    Public fColorForeFeiertag2, fColorBackFeiertag2 As Color
    Public fColorForeFeiertag3, fColorBackFeiertag3 As Color
    Public fColorForeFreitag, fColorBackFreitag As Color
    Public fColorForeSamstag, fColorBackSamstag As Color
    Public fColorForeSonstigeFeiertage, fColorBackSonstigeFeiertage As Color
    Public fColorForeFestGebucht, fColorBackFestGebucht As Color
    Public fColorForeVariabel, fColorBackVariabel As Color
    Public fColorForeErlaubt, fColorBackErlaubt As Color
    Public fColorForeNichtErlaubt, fColorBackNichtErlaubt As Color
    Public fColorForeBank, fColorBackBank As Color
    Public fColorForeKasse, fColorBackKasse As Color

End Module
