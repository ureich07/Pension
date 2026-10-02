
Public Class frmLogin


    ''' <summary>
    ''' Setzt beim Laden des Login-Formulars den Fokus initial auf das Benutzernamen-Textfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die <see cref="System.EventArgs"/>-Daten des Ereignisses.</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Fokus direkt in das erste Eingabefeld setzen
        tbNutzer.Focus()
        tbNutzer.Text = "Erik"
        tbPassWD.Text = "02041883"
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf den OK-Button. 
    ''' Führt die Login-Prüfung durch und schließt das Formular nur bei erfolgreicher Anmeldung.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die <see cref="System.EventArgs"/>-Daten des Ereignisses.</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ' Login-Prüfung mit den eingegebenen Daten aufrufen
        prCheckResult(tbNutzer.Text, tbPassWD.Text)

        ' Das Fenster nur schließen, wenn der Login erfolgreich war (lgLogin = True)
        If lgLogin Then
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Verarbeitet den Klick auf den Abbrechen-Button und schließt das Formular.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses.</param>
    ''' <param name="e">Die <see cref="System.EventArgs"/>-Daten des Ereignisses.</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        ' Zur Sicherheit den Login-Status auf Fehlgeschlagen/Abgebrochen setzen
        lgLogin = False

        ' Formular schließen
        Me.Close()
    End Sub


    ''' <summary>
    ''' Überprüft die eingegebenen Login-Daten (Benutzername und Passwort).
    ''' Prüft zuerst auf den Hardcoded-Admin und gleicht andernfalls die Daten mit der Datenbank ab.
    ''' </summary>
    ''' <param name="sUser">Der eingegebene Benutzername.</param>
    ''' <param name="sPassWD">Das eingegebene Passwort (wird für die DB-Abfrage entschlüsselt).</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub prCheckResult(ByVal sUser As String, ByVal sPassWD As String)
        ' 1. Prüfung auf den fest hinterlegten Administrator
        If sUser = "admin" AndAlso sPassWD = "sysman" Then
            frmMain.Text = "Pension am Radweg => angemeldet als Administrator!"
            ngRechteStatus = 0
            lgLogin = True
            prLoginStatus(0)

            ' 2. Wenn nicht Admin, in der Datenbank nachschlagen
        Else
            ' Passwort für den Abgleich entschlüsseln
            Dim sDecryptedPass As String = fDeCrypt(sPassWD, "UrSoft")

            ' SQL-Abfrage vorbereiten (Hinweis: Idealerweise in fcReadDataTable Parameter nutzen!)
            Dim sSQL As String = String.Format("Select * from Nutzer Where Name='{0}' and PassWD='{1}'",
                                           sUser.Replace("'", "''"),
                                           sDecryptedPass.Replace("'", "''"))

            Dim dt As DataTable = fcReadDataTable(sSQL)

            ' Prüfen, ob genau ein passender Benutzer gefunden wurde
            If dt IsNot Nothing AndAlso dt.Rows.Count = 1 Then
                ' Status sicher in Ganzzahl konvertieren
                ngRechteStatus = Convert.ToInt32(dt.Rows(0).Item("Status"))
                lgLogin = True

                ' Menürechte anpassen
                prLoginStatus(ngRechteStatus)

                ' Begrüßung im Hauptfenster anzeigen
                frmMain.Text = "Pension am Radweg => Guten Tag " & sUser
            Else
                ' Login fehlgeschlagen: Variablen zurücksetzen und Benutzer informieren
                lgLogin = False
                ngRechteStatus = -1 ' Undefinierter Zustand
                MessageBox.Show("Benutzername oder Passwort falsch!", "Login fehlgeschlagen", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub


    ''' <summary>
    ''' Steuert die Sichtbarkeit und Verfügbarkeit der Menüfunktionen im Hauptformular (frmMain) 
    ''' basierend auf dem übergebenen Login-Status des Benutzers.
    ''' </summary>
    ''' <param name="nStatus">Der Berechtigungsstatus des Benutzers (z. B. 0 = Admin, 1 = Supervisor, etc.).</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub prLoginStatus(ByVal nStatus As Integer)
        ' 1. Sicherheits-Standard: Erst einmal alle relevanten Menüpunkte sperren
        frmMain.tsbSystem.Enabled = False
        frmMain.tsbStatistik.Enabled = False
        frmMain.tsbPersonen.Enabled = False
        frmMain.tsbDruck.Enabled = False
        frmMain.tsbKunde.Enabled = False

        ' 2. Rechte basierend auf dem Status gezielt freischalten
        Select Case nStatus
            Case 0, 1 ' Admin / Supervisor (Volle Rechte)
                frmMain.tsbSystem.Enabled = True
                frmMain.tsbStatistik.Enabled = True
                frmMain.tsbPersonen.Enabled = True
                frmMain.tsbDruck.Enabled = True
                frmMain.tsbKunde.Enabled = True

            Case 2 ' Standardbenutzer (Eingeschränkte Rechte, keine Statistik/Datev)
                frmMain.tsbSystem.Enabled = True
                frmMain.tsbPersonen.Enabled = True
                frmMain.tsbDruck.Enabled = True
                frmMain.tsbKunde.Enabled = True

            Case 3 ' Nur Drucken erlaubt
                frmMain.tsbDruck.Enabled = True

            Case 4
                ' Status 4 hat aktuell keine Rechte (bleibt alles gesperrt)

            Case Else
                ' Unbekannter Status: Zur Sicherheit bleibt alles gesperrt
        End Select

        ' Login-Button Text aktualisieren
        frmMain.tsbLogin.Text = "Logout"
    End Sub


    ''' <summary>
    ''' Verarbeitet die Tastatureingaben im Benutzer-Textfeld.
    ''' Wenn die Eingabetaste (Enter) gedrückt wird, wechselt der Fokus in das Passwort-Textfeld.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (tbNutzer).</param>
    ''' <param name="e">Die <see cref="System.Windows.Forms.KeyPressEventArgs"/>-Daten, die das eingegebene Zeichen enthalten.</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub tbNutzer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbNutzer.KeyPress
        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Den Standard-Warnton (Piepen) von Windows unterdrücken
            e.Handled = True

            ' Fokus in das Passwort-Textfeld setzen
            tbPassWD.Focus()
        End If
    End Sub


    ''' <summary>
    ''' Verarbeitet die Tastatureingaben im Passwort-Textfeld.
    ''' Wenn die Eingabetaste (Enter) gedrückt wird, wechselt der Fokus auf den OK-Button.
    ''' </summary>
    ''' <param name="sender">Die Quelle des Ereignisses (tbPassWD).</param>
    ''' <param name="e">Die <see cref="System.Windows.Forms.KeyPressEventArgs"/>-Daten, die das eingegebene Zeichen enthalten.</param>
    ''' <remarks>
    ''' Autor: Uwe
    ''' Datum: 24.09.2026
    ''' </remarks>
    Private Sub tbPassWD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPassWD.KeyPress
        ' Prüfen, ob die Eingabetaste (Enter) gedrückt wurde
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            ' Den Standard-Warnton (Piepen) von Windows unterdrücken
            e.Handled = True

            ' Fokus auf den OK-Button setzen
            OK_Button.Focus()
        End If
    End Sub


End Class
