<!DOCTYPE html><html><head><meta charset="utf-8">
 <?php
 /* SQL Kunde sucht nach gleichen datensätzen über web an VB aus*/
 
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
	echo Chr(94);
	$SQL1="SELECT * FROM buchung order by KunID asc";
	$SQL="SELECT * FROM kunden order by ID asc";
	$con = fcCon(); //"", "root", "", "Pension"); 
	$dtb= mysqli_query($con,$SQL1); 
	$dtk = mysqli_query($con,$SQL); 
//	$nKunde= mysqli_num_rows($dt);  //Kunden datensätze lesen

	$i=0;
    while ($dSatzk=mysqli_fetch_assoc($dtk))
	{
		$arKunde[$i][0]=trim($dSatzk["ID"]);
		$arKunde[$i][1]=trim($dSatzk["Name1"]);
		$arKunde[$i][2]="0";	
		$i++;
	}
	$ii=$i;
	$i=0;
	While ($dSatzb =mysqli_fetch_assoc($dtb))
	{
		
		if (trim($dSatzb["KunID"])=="")
		{
			if (trim($dSatzb["Kunde"])<>"Unbekannt")
			{
				// Kunden=Unbekannt
				$ID =$dSatzb["Kunde"];
				$SQL="UPDATE buchung SET Kunde='Unbekannt' WHERE ID = '".$ID."'";
				mysqli_query($con,$SQL);
			}
			
		}
		else
		{
			$ID=$dSatzb["ID"];
			$KunID=trim($dSatzb["KunID"]);
			$Kunde=trim($dSatzb["Kunde"]);
			$Abruch="0";
			do  
			{
				if ($KunID==$arKunde[$i][0])
				{
					$Abruch="1";
					$arKunde[$i][2]="1";
					if ($Kunde<>$arKunde[$i][1])
					{	// kunde in buchung eintragen
						$Ku=$arKunde[$i][1];
						$SQL="UPDATE buchung SET Kunde='$Ku' WHERE ID = '".$ID."'";
						mysqli_query($con,$SQL);
					}
				}
				else
				{
					$i++;
				
					if ($i==$ii)
					{
						
						echo $ID."," .$KunID." ".$Kunde.Chr(94);
						$SQL="UPDATE buchung SET Kunde='Unbekannt',KunID='' WHERE ID = '".$ID."'";
						mysqli_query($con,$SQL);
						$i=0;
						$Abruch="1";
						break;
					}	
				}
			} while ($Abruch=="0");
		}
	}
	for($i=0;$i<=$ii-1;$i++)
	{
		if($arKunde[$i][2]=="0")
		{
			// Delete ID $arKunde[$i][0]
			$ID=$arKunde[$i][0];
			$SQL="DELETE FROM kunden WHERE ID = '".$ID."'";
			mysqli_query($con,$SQL);
			
		}
	}
	echo "True".Chr(94);
	mysqli_close($con); 
}
else
{    //Errordatei schreiben
	fError("SQLKunID",$SQL);
}
/* SQL Read list eine datenbank aus und giebt diese ?ber web an VB aus*/	
?>