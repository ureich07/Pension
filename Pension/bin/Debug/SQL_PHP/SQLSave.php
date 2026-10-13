<?php
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
//	echo "Sicherung";
	$sDatei="DBSi".fcGetTimeID().".dat";
	$fclf=chr(13).chr(10);
	$fft=chr(176);
	$ftext="";
	$ret="True";
	$handle = fopen ("./DBSi/Pension1.dat", "w+"); //datei löschen
	fwrite($handle,""); //Text reinschreiben.
	fclose($handle); //Datei schließen.
	$handle = fopen("./DBSi/Pension1.dat","a"); //öffnen
	$con = fcCon(); //"", "root", "", "Pension"); 
	$sql = "SHOW TABLES FROM pension";
	$result = mysqli_query($con,$sql);
	$i=0;
	while ($row = mysqli_fetch_row($result)) //Tabelle nanen ermitteln
	{
		$Tabelle[$i]=$row[0];
		$i++;
	}	
	$i--;
	for ($i=0;$i<=count($Tabelle)-1;$i++)
	{
		
		$ftext="<".ucfirst($Tabelle[$i]).">".$fclf;
		echo $ftext;
		fwrite($handle,$ftext); //Text reinschreiben.
		$SQL="SELECT * FROM ".$Tabelle[$i];
		$dt = mysqli_query($con,$SQL); 
		$nField=mysqli_num_fields($dt);
		$j=0;
		$ffeld="";
		while ($finfo = mysqli_fetch_field($dt))  //ermiteln der Feldnamen und Strucktur
		{
			$aField[$j]=$finfo->name;
			$Typ="varchar";
			if ($finfo->type==252)
			{
				$Typ="text";
			}
			$ffeld=$ffeld. $finfo->name.",".$Typ.",".(($finfo->length)/3).$fft;
			$j++;
		}
		$ffeld=substr($ffeld,0,strlen($ffeld)-1);
	
		$ftext=$ffeld.$fclf;
		fwrite($handle,$ftext); //Text reinschreiben.
		While ($dSatz =mysqli_fetch_assoc($dt))    //inhalt ausgeben
		{
			$ffeld="";
			$Text="";
			for($j=0;$j<=$nField-1;$j++)
			{
				$T=utf8_decode($dSatz[$aField[$j]]);
				$Text1=str_replace(chr(13).chr(10),chr(12),$T);
				$ffeld=$ffeld.$Text1.$fft;
			}
			$ftext=$ffeld.$fclf;
			fwrite($handle,$ftext); //Text reinschreiben.
		}
		$ftext="<End>".$fclf;
		fwrite($handle,$ftext); //Text reinschreiben.
	}
	fclose ($handle);
	
	mysqli_close($con);
	$hostname = 'elbepension.spdns.de';
	$ip = gethostbyname ($hostname);// DNS-Abfrage durchführen
	if ($ip != $hostname)  // Auflösen des Namen erfolgreich?
	{
		$conn=ftp_connect($ip);
		if (ftp_login($conn, "ftpuser","38814020"))
		{
			if (ftp_put($conn,"./DBSicherung/".$sDatei, "./DBSi/Pension1.dat", FTP_ASCII)==false) // datei --> ftp
			{
				$ret="False";
			}
			$d= ftp_nlist($conn,"DBSicherung");
			$nMax=count($d)-1;
			for ($x=1 ; $x<= 20 ; $x++) // x anzahl der kopien
			{
				$dd="0";
				$nSatz=-1;
				for($i=0 ; $i <= $nMax ; $i++)
				{
					if ($d[$i]<>"")
					{
						$dd1=substr($d[$i],16,14);
						if (strcmp($dd,$dd1)<0)
						{
							$dd=$dd1;
							$nSatz=$i;
						}
					}
				}
				if ($nSatz > -1)
				{
					$d[$nSatz]="";
				}	
			}
			for($i=0 ; $i <= $nMax ; $i++) // Löschen
			{
				if ($d[$i]<>"")
				{ 
					ftp_delete($conn,$d[$i]);// Ftp Löschen 
				}
			}
		}
	}	
	else
	{    
		$ret="False";	
	}
	
	
	
	//echo $ret;
}
else
{    //Errordatei schreiben
	fError("SQLSave",$SQL);
	
}	

?>