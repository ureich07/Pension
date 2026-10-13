<!DOCTYPE html><html><head><meta charset="utf-8">
 <?php
 /* SQL Read list eine datenbank aus und giebt diese ?ber web an VB aus*/
 
date_default_timezone_set("Europe/Berlin");
include 'SQLfunction.php';
$PW=array("Gabriela","Gewuenscht","Karriere","Elternzeit","Truemmer","Abstand","Bodensee");
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
	$con = fcCon(); //"", "root", "", "Pension"); 
	$dt = mysqli_query($con,$SQL); 
	$nField=mysqli_num_fields($dt);
    $Text="";
	$i=0;
	echo chr(94);
	while ($finfo = mysqli_fetch_field($dt))  //ermiteln der Feldnamen
	{
		$aField[$i]=$finfo->name;
		$Text=$Text.$aField[$i].chr(126);
		$i++;
	}
	$Text=substr($Text,0,strlen($Text)-1);
	echo $Text.chr(94);

	While ($dSatz =mysqli_fetch_assoc($dt))    //inhalt ausgeben
	{
		$Text="";
		for($j=0;$j<=$nField-1;$j++)
		{
			$Text=$Text.$dSatz[$aField[$j]].chr(126);
		}
		$Text=substr($Text,0,strlen($Text)-1);
		echo $Text.chr(94);	
	}
	mysqli_close($con); 
}
else
{    //Errordatei schreiben
	fError("SQLRead",$SQL);
	
}
/* SQL Read list eine datenbank aus und giebt diese ?ber web an VB aus*/	





?>
