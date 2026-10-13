
Imports System.IO
Imports System.Net
Public Class FTP
    ''' <summary>
    ''' IP vom DNS Server hohlen
    ''' </summary>
    ''' <param name="sHost">Servername mit Endung (z.B. meinServer.de)</param>
    ''' <param name="sPort">Port</param>
    ''' <remarks></remarks>

    Public Shared Function getIP(ByRef sHost As String, ByRef sPort As String) As String
        Dim mIP As String = ""
        Dim host = Net.Dns.GetHostEntry(sHost)
        For Each ip In host.AddressList
            If ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                mIP = ip.ToString()
                mIP = "ftp://" & ip.ToString() & ":" & sPort
            End If
        Next
        Return mIP
    End Function
    ''' <summary>
    ''' IP vom DNS Server hohlen ohne port
    ''' </summary>
    ''' <param name="sHost">Servername mit Endung (z.B. meinServer.de)</param>
    ''' <param name="sPort">Port</param>
    ''' <remarks></remarks>

    Public Shared Function getWebIP(ByRef sHost As String) As String
        Dim mIP As String = ""
        Dim host = Net.Dns.GetHostEntry(sHost)
        For Each ip In host.AddressList
            If ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                mIP = ip.ToString()
                '  mIP = "ftp://" & ip.ToString() & ":" & sPort
            End If
        Next
        Return mIP
    End Function
    ''' <summary>
    ''' Upload einer Datei
    ''' </summary>
    ''' <param name="LocalPhat ">Quellphat z.B. "c:/daten_erik"</param>
    ''' <param name="file ">QuellDatei z.B. "konto.txt"</param>
    ''' <param name="ftpuri">Zielphat z.B."IP+Port z.B."80.132.79.123:21/konto.txt" </param>
    ''' <param name="ftpusername">Username </param>
    ''' <param name="ftppassword">Userpasswort </param>
    ''' <remarks></remarks>
    Public Shared Function UploadFile(ByVal LocalPhat As String, ByVal file As String, ByVal ftpuri As String, ByVal ftpusername As String, ByVal ftppassword As String) As Boolean
        ' FtpUploadFile("c:/daten_erik/konto.txt", hortfile, us, pas)
        ftpuri = ftpuri & "/" & file
        Dim filetoupload As String = LocalPhat & "\" & file
        ' Dim filetoupload As String = file
        ' Create a web request that will be used to talk with the server and set the request method to upload a file by ftp.
        Dim ftpRequest As FtpWebRequest = CType(WebRequest.Create(ftpuri), FtpWebRequest)

        Try
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile

            ' Confirm the Network credentials based on the user name and password passed in.
            ftpRequest.Credentials = New NetworkCredential(ftpusername, ftppassword)

            ' Read into a Byte array the contents of the file to be uploaded 
            Dim bytes() As Byte = System.IO.File.ReadAllBytes(filetoupload)

            ' Transfer the byte array contents into the request stream, write and then close when done.
            ftpRequest.ContentLength = bytes.Length
            Using UploadStream As Stream = ftpRequest.GetRequestStream()
                UploadStream.Write(bytes, 0, bytes.Length)
                UploadStream.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
            Exit Function
        End Try
        Return True
        ' MessageBox.Show("Process Complete")
    End Function
    ''' <summary>
    ''' Download einer Datei
    ''' </summary>
    ''' <param name="LocalPhat ">Zielphat z.B. "c:/daten_erik"</param>
    ''' <param name="file ">Zieldatei z.B. "konto.txt"</param>
    ''' <param name="ftpuri">IP+Port z.B."80.132.79.123:21" </param>
    ''' <param name="ftpusername">Username </param>
    ''' <param name="ftppassword">Userpasswort </param>
    ''' <remarks></remarks>


    Public Shared Function DownloadFile(ByRef LocalPhat As String, ByVal file As String, ByVal ftpuri As String, ByVal ftpusername As String, ByVal ftppassword As String) As Boolean

        ftpuri = ftpuri & "/" & file
        Dim downloadpath As String = LocalPhat & "\" & file
        'Create a WebClient.
        Dim request As New WebClient()

        ' Confirm the Network credentials based on the user name and password passed in.
        request.Credentials = New NetworkCredential(ftpusername, ftppassword)

        'Read the file data into a Byte array
        Dim bytes() As Byte = request.DownloadData(ftpuri)

        Try
            '  Create a FileStream to read the file into
            Dim DownloadStream As FileStream = IO.File.Create(downloadpath)
            '  Stream this data into the file
            DownloadStream.Write(bytes, 0, bytes.Length)
            '  Close the FileStream
            DownloadStream.Close()

        Catch ex As Exception
            ' MessageBox.Show(ex.Message)
            Return False
            Exit Function
        End Try

        ' MessageBox.Show("Process Complete")
        Return True
    End Function
End Class

