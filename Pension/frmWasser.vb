Imports System.Globalization

Public Class frmWasser

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Initialer Aufruf beim Laden des Formulars, um Startwerte (0,00) zu setzen
        prBerechnung()
    End Sub

    ''' <summary>
    ''' Berechnet die prozentuale Verteilung des Restbetrags basierend auf dem Verhältnis 
    ''' von vollem zu ermäßigtem Steuerbetrag (z.B. für die Aufteilung von Abschlagszahlungen).
    ''' </summary>
    ''' <remarks>
    ''' 19.10.2026 - Code-Optimierung:
    ''' - Veraltete 'Val'- und 'Replace'-Befehle durch kultur-bewusstes 'Double.TryParse' ersetzt.
    ''' - Division-by-Zero-Schutz integriert für den Fall, dass die Gesamtsumme 0 ergibt.
    ''' - Textausgaben über 'N2'-Formatierung auf exakt zwei Nachkommastellen vereinheitlicht.
    ''' - Redundante TextChanged-Ereignisse in einer zentralen Methode zusammengefasst.
    ''' </remarks>
    Private Sub prBerechnung()
        Try
            Dim nProz16 As Double = 0
            Dim nProz5 As Double = 0
            Dim nBV As Double = 0
            Dim nBE As Double = 0
            Dim nRest As Double = 0
            Dim nGesammt As Double = 0
            Dim nRBV As Double = 0
            Dim nRBE As Double = 0

            ' Aktuelle Ländereinstellung für das Parsen von Komma/Punkt verwenden
            Dim culture As CultureInfo = CultureInfo.CurrentCulture

            ' Sicheres Einlesen der Textboxen (fängt leere Felder oder Buchstaben ab)
            Double.TryParse(tbRest.Text, NumberStyles.Any, culture, nRest)
            Double.TryParse(tbBetragVoll.Text, NumberStyles.Any, culture, nBV)
            Double.TryParse(tbBetragErmaessigt.Text, NumberStyles.Any, culture, nBE)

            ' Gesamtsumme der Basisbeträge ermitteln
            nGesammt = nBE + nBV
            lbGesammt.Text = nGesammt.ToString("N2", culture)

            ' Division durch 0 verhindern, falls noch keine Beträge eingetragen wurden
            If nGesammt > 0 Then
                ' Prozentuale Anteile berechnen
                nProz16 = fcRound(nBV * 100.0 / nGesammt, 2)
                nProz5 = fcRound(nBE * 100.0 / nGesammt, 2)

                ' Restbetrag prozentual aufteilen
                nRBV = fcRound(nRest * nProz16 / 100.0, 2)
                nRBE = fcRound(nRest * nProz5 / 100.0, 2)
            End If

            ' UI-Labels mit den berechneten Werten befüllen (formatiert auf 2 Nachkommastellen)
            lbPro16.Text = nProz16.ToString("N2", culture)
            lbPro5.Text = nProz5.ToString("N2", culture)

            lbBetragMwstVoll.Text = nRBV.ToString("N2", culture)
            lbBetragMwstErmae.Text = nRBE.ToString("N2", culture)

            lbSumme.Text = (nRBE + nRBV).ToString("N2", culture)

        Catch ex As Exception
            ErrReport(ex.Message, ex.Source, ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Zentrales Ereignis für alle Textänderungen der Eingabefelder.
    ''' </summary>
    Private Sub tbInput_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbRest.TextChanged,
                                                                                          tbBetragVoll.TextChanged,
                                                                                          tbBetragErmaessigt.TextChanged
        prBerechnung()
    End Sub

End Class
