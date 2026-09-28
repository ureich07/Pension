<?php
date_default_timezone_set("Europe/Berlin");

include 'SQLfunction.php';
$PW=array("Fahrwasser","leuchttonne","mobtaste","Dresden","Wasserwerk","flughafen","flugzeug");
$timestamp = time();
$Keys=array_keys($_POST);
$SQL=$_POST['S'];
$sZeit=substr($SQL,0,2).substr($SQL,3,2);
$nPari=strval(substr($SQL,6,2).substr($SQL,9,2));
$SQL=substr($SQL,12);
$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
$SQL=fDeCryptPara($SQL,$PWX);
$nPari1=fPari($SQL);
if ($nPari1==$nPari)
{
    // Name des Rechners, dessen IP-Adresse ermittelt werden soll DNS-Abfrage
    $hostname = 'elbepension.spdns.de';
    $ip = gethostbyname ($hostname);// DNS-Abfrage durchführen
    // Auflösen des Namen erfolgreich?
    if ($ip != $hostname) // Auflösen des Namen erfolgreich?
	{
		$conn=ftp_connect($ip);
		if (ftp_login($conn, "ftpuser","38814020"))
		{
		  $d= ftp_nlist($conn,"DBSicherung");
		  $db="";
		  echo chr(94);
		  for($i=0 ; $i <= count($d)-1 ; $i++)
		  {
			  $db=$db.$d[$i].chr(94);
		  }
		  echo substr($db,0,strlen($db)-1);
		}
	}	
    else
    {    // Ansonsten entsprechende Fehlermeldung
        echo 'False';
		
	}	
}
else
{    //Errordatei schreiben
	fError("SQLDir",$SQL);
	
}	
?>
	