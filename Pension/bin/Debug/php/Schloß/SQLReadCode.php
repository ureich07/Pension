<?php
set_time_limit(60);
//Daten vom  Schloß holen
 date_default_timezone_set("Europe/Berlin");
 include 'SQLfunction.php';
  $Keys=array_keys($_POST);
  $C=$_POST['Daten'];
  
 Function fcZiffer($Zahl,$z)    //$ID
		{
			$Zahl="000000".$Zahl;
			//$str = 'abcdef';
            $l=strlen($Zahl); // 6
            $Zahl=substr($Zahl,$l-$z);
			return $Zahl;
		}
 // Übertragen auf Schloß Datei system schloß IP hohlen
    $IP="";
    $db=fcCon();
	$Wert="";
	$sSql= "Update System SET RFIDtem=''"; 
	$result=mysqli_query($db,$sSql);
    $sSql= "SELECT * FROM System";   //* from bild where ID='".$id."'";
	$result=mysqli_query($db,$sSql);
    if ($result->num_rows > 0) 
    {
		while($row = $result->fetch_assoc()) 
	    {
	    	$Wert= $row["Pension"];
			
		}
    }
	mysqli_close($db);
	echo "</br>".$sCode."</br>";
	$aWert=explode( chr(13).chr(10), $Wert);
	$IP="xxx";
    for($i=0;$i<=count($aWert)-1;$i++)
	{
		if (substr($aWert[$i],0,10)=="IPSchloss=")
		{
			//$IP= "http://".substr($aWert[$i],10);
			$IP= substr($aWert[$i],10);
		}
	}
	
	//Daten hohlen  http://192.168.178.69/?Daten=Speicher_?
	$IP = gethostbyname ($IP);
	 $sCode='Daten=Speicher_?';//.$C;
	//echo "</br>".$sCode."</br>";
	  $href="http://".$IP."?".$sCode;
	  echo "</br>".$href."</br>";
	Header("Location:".$href);
    // Header($href);

 ?>