<?php
 /* SQL Append neuen datensatz anlegen ID über  web an VB aus*/
 include 'SQLfunction.php';
 date_default_timezone_set("Europe/Berlin");
$PW=array("Rucksack","waehrend","zuHause","unterwegs","Berufung","Chickenbus","Speedboot");
$timestamp = time(); 

$Keys=array_keys($_POST);
$SQL=$_POST['S'];
$sZeit=substr($SQL,0,2).substr($SQL,3,2);
$nPari=strval(substr($SQL,6,2).substr($SQL,9,2));
$SQL=substr($SQL,12);
$PWX =fDeCryptSQL( $PW[date("w")],$sZeit.date("Ymd", $timestamp));
$Tabelle=fDeCryptPara($SQL,$PWX);
$nPari1=fPari($Tabelle);
if ($nPari1==$nPari)
{
	$con = fcCon(); //"", "root", "", "Pension"); 
	$SQL="SELECT * FROM ".$Tabelle;
	$dt = mysqli_query($con,$SQL); 
	$nField=mysqli_num_fields($dt);
	$i=0;
	while ($finfo = mysqli_fetch_field($dt))  //ermiteln der Feldnamen
	{
		$aField[$i]=$finfo->name;
		$i++;
	}
	$ID=fcGetTimeID();//JJMMTThhmmss
	$Value="";
	$sFeld="";
	for($i=0; $i <= count($aField)-1; $i++)   
	{
		$sFeld= $sFeld.$aField[$i].",";
		$sWert="0";
		if ($aField[$i]=="ID")
		{
			$sWert=$ID;
		}	
		$Value=$Value."'".$sWert."',";
	}
	$sFeld=substr($sFeld,0,strlen($sFeld)-1);
	$Value=substr($Value,0,strlen($Value)-1);
	$SQL="INSERT INTO ".$Tabelle." (".$sFeld.") value (".$Value.")";
	mysqli_query($con,$SQL);
	$Err=mysqli_error($con); //SQL ERRORLOG Schreiben
	if($Err=="")
	{
		//echo chr(94)."True".chr(94);
		echo chr(94).$ID.chr(94);
	}
	else
	{
		echo chr(94)."False".chr(94);
		fSQLError("Append",$Err,$SQL);
	}
	mysqli_close($con); 

}
else
{    //Errordatei schreiben
	fError("SQLAppend",$SQL);
	
}
?>