 <?php
 /* SQLRestore list eine datei  aus der FTP und schreibt dise in den SQL Server*/
 

date_default_timezone_set("Europe/Berlin");
include 'SQLfunction.php';
$PW=array("Bootstour","Hinweise","Handbuch","nachhaltig","Highlights","Auflage","unterwegs");
$timestamp = time();
$Keys=array_keys($_POST);
$SQL=$_POST['S'];
$sZeit=substr($SQL,0,2).substr($SQL,3,2);
$nPari=strval(substr($SQL,6,2).substr($SQL,9,2));
$SQL=substr($SQL,12);
$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
$sDatei=fDeCryptPara($SQL,$PWX); // dateinnahme in der FTP
$nPari1=fPari($sDatei);
if ($nPari1==$nPari)
{
   //Später wieder Freigebeb Pension.dat wird sonst nicht geladen
	$hostname = 'elbepension.spdns.de';
	$ip = gethostbyname ($hostname);// DNS-Abfrage durchführen
	$ret="True"; 
    echo clf().$ip.clf();	
	if ($ip != $hostname)  // Auflösen des Namen erfolgreich?
	{
		$conn=ftp_connect($ip);
		if (ftp_login($conn, "ftpuser","38814020"))
		{
			echo clf().$sDatei.clf();    
			if (ftp_get($conn,"./DBSi/Pension.dat",$sDatei, FTP_ASCII)==false) // datei --> ftp
			{
				echo clf()."copy-false".clf();
				$ret="False";
			}
			else
			{
				echo clf()."copy-true ".clf();
			}
		}
		else
		{
			$ret="False";
		}
	}
	else
	{
		$ret="False";
	}
	if ($ret=="True") // Sicherunsdatei auf webserver
	{
		$ftext="";	
		$handle = fopen ("./DBSi/Pension.dat", "r"); //datei lesen
		$con = fcCon(); //"", "root", "", "Pension"); 
		$x=0;
		while (!feof($handle))
		{
			$ftext= fgets($handle);
			if ($x==2) // daten auslesen
			{
				$Value="";
				$Wert = explode(chr(176),$ftext);
				for($j=0; $j <= count($Wert)-2; $j++)   
				{
					$Wert[$j]=str_replace(chr(12),chr(13).chr(10),utf8_encode($Wert[$j]));
					$Value=$Value."'".$Wert[$j]."',";
				}
				$Value=substr($Value,0,strlen($Value)-1);	
				$SQL="INSERT INTO ".$Tabelle." (".$sFeld.") value (".$Value.")";
				fSQL($con,$SQL);

			}
			if ($x==1)  // Feld Name typ Größe
			{
				$x=2;// Tabelle Löschen
				$SQL="DROP TABLE ".$Tabelle;
				fSQL($con,$SQL);
				$SQL="(";
				$Feld = explode(chr(176),$ftext);
				$sFeld=""; // feldstruktur für insert
				for ($j=0;$j<=count($Feld)-1;$j++)
				{
					$Strucktur=explode(",",$Feld[$j]);
					$SQL = $SQL."`".trim($Strucktur[0])."` ";
					$sFeld= $sFeld.trim($Strucktur[0]).",";
					If (trim($Strucktur[1]) =="varchar")
					{
						$SQL = $SQL."varchar(".$Strucktur[2].") NULL, ";
					}
					else
					{
						$SQL = $SQL."text "." NULL, ";
					}
				}
				$sFeld=substr($sFeld,0,strlen($sFeld)-1);
				$SQL = substr($SQL, 0, strlen($SQL) - 2);
				$SQL = "CREATE TABLE ".$Tabelle.$SQL.") CHARACTER SET utf8 COLLATE utf8_general_ci;";
				fSQL($con,$SQL);
			}
			
			if ($x==0 and substr($ftext,0,1)=="<")
			{
				$Tabelle=str_replace(">","",$ftext);
				$Tabelle=substr($Tabelle,1); //TabellenNamme Ermittelt
				$x=1;
			}
			if ($x<>0 and trim($ftext)=="<End>")
			{
				$x=0;
			}
		}
		echo  Chr(94).$ret;
		fclose ($handle);
	}
	else
	{    //Errordatei schreiben
		fError("SQLRestore",$SQL);
	}
}
?>