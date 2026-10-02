<!DOCTYPE html><html><head><meta charset="utf-8">
 <?php
 /* SQL Kunde sucht nach gleichen kunden datensätzen über web an VB aus*/
 
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
	$SQL="SELECT * FROM kunden order by trim(PLZ) asc, trim(Name1) asc, trim(Strasse) asc";
//	$SQL="SELECT * FROM kunden order by PLZ asc";
	$con = fcCon(); //"", "root", "", "Pension"); 
	$dt = mysqli_query($con,$SQL); 
//	$nKunde= mysqli_num_rows($dt);  //Kunden datensätze lesen
	echo chr(13).chr(10);

	$oPLZ="";
	$oName1="";
	$oName2="";
	$oVorname="";
	$oID="";
	$oStrasse="";
	$oOrt="";
	$oMail="";
	While ($dSatz =mysqli_fetch_assoc($dt))
	{
		$aID=$dSatz["ID"];
		$aName1=trim($dSatz["Name1"]);
		$aName2=trim($dSatz["Name2"]);
		$aVorname=trim($dSatz["Vorname"]);
		$aPLZ=trim($dSatz["PLZ"]);
		$aOrt=trim($dSatz["Ort"]);
		$aStrasse=trim($dSatz["Strasse"]);
		$aMail=Trim($dSatz["EMail"]);
		if($P=="1" and $oPLZ==$aPLZ and $oName1==$aName1 and $oName2==$aName2 and substr($oStrasse,0,2)==substr($aStrasse,0,2))
		{
				echo $oName1.";".$oName2.";".$oVorname.";".$oPLZ.";".$oOrt.";".$oStrasse.";".$oMail.";".$oID.chr(13).chr(10);
				echo $aName1.";".$aName2.";".$aVorname.";".$aPLZ.";".$aOrt.";".$aStrasse.";".$aMail.";".$aID.chr(13).chr(10);
				echo "#".chr(13).chr(10);
		}
		if($P=="0" and $oPLZ==$aPLZ and substr($oStrasse,0,2)==substr($aStrasse,0,2))	
		{
			echo $oName1.";".$oName2.";".$oVorname.";".$oPLZ.";".$oOrt.";".$oStrasse.";".$oMail.";".$oID.chr(13).chr(10);
			echo $aName1.";".$aName2.";".$aVorname.";".$aPLZ.";".$aOrt.";".$aStrasse.";".$aMail.";".$aID.chr(13).chr(10);
			echo "#".chr(13).chr(10);
			
			
			
		//echo $dSatz["PLZ"]."---".$dSatz["Name1"]."---".$dSatz["Strasse"]."---".$dSatz["Name2"]."---".$dSatz["Ort"]."---".$dSatz["ID"].chr(13).chr(10);
		}
		
		$oID=$dSatz["ID"];
		$oName1=trim($dSatz["Name1"]);
		$oName2=trim($dSatz["Name2"]);
		$oVorname=trim($dSatz["Vorname"]);
		$oPLZ=trim($dSatz["PLZ"]);
		$oOrt=trim($dSatz["Ort"]);
		$oStrasse=trim($dSatz["Strasse"]);
	    $oMail=Trim($dSatz["EMail"]);
	}
	
	
	mysqli_close($con); 
}
else
{    //Errordatei schreiben
	fError("SQLKunde",$SQL);
	
}
/* SQL Read list eine datenbank aus und giebt diese ?ber web an VB aus*/	
?>