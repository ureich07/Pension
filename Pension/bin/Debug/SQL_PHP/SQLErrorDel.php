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
	$P=substr($SQL,strlen($SQL)-1,1);
	$Datei="SQLError.dat";
	if ($P==0)
	{
		$Datei="Error.dat";
	}
	$ret="x";
	$handle = fopen ("./DBSi/".$Datei, "w"); //datei lesen
	fwrite($handle,""); //Text reinschreiben.
	fclose($handle); //Datei schließen.
}	
else
{    //Errordatei schreiben
	fError("SQLErrorDel",$SQL);
}		
?>