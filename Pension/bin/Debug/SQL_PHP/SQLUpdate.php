<!DOCTYPE html><html><head><meta charset="utf-8">
<?php
 /* Schreiben in ein datensatzes*/
date_default_timezone_set("Europe/Berlin");
include 'SQLfunction.php';
$PW=array("Flipflop","Begegnung","neueLand","sichimmer","Zukunft","zuSorgen","sondern");
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
	$SQL=utf8_encode($SQL);
	$con = fcCon(); //"", "root", "", "Pension"); 
	mysqli_query($con,$SQL); 
	$Err=mysqli_error($con); //SQL ERRORLOG Schreiben
	if($Err=="")
	{
		echo chr(94)."True".chr(94);
	}
	else
	{
		echo chr(94)."False".chr(94);
		fSQLError("Update",$Err,$SQL);
	}
	mysqli_close($con); 
}
else
{    //Errordatei schreiben
	fError("SQLUpdate",$SQL);
}
?>