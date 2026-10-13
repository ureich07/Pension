<?php

//http://192.168.178.55:81/SQLSend.php?Daten=000;000;000;000;000°20240725°1200°20240726°1200|
 date_default_timezone_set("Europe/Berlin");
 include 'SQLfunction.php';
 $Keys=array_keys($_POST);

 $C=$_POST['Daten'];
 // $C=$_GET['Daten'];
 Echo $C."</br>";
 Function fcZiffer($Zahl,$z)    //$ID
		{
			$Zahl="000000".$Zahl;
			//$str = 'abcdef';
            $l=strlen($Zahl); // 6
            $Zahl=substr($Zahl,$l-$z);
			return $Zahl;
		}
		
 $T= explode("°", $C);
  $sCode="Daten=".$T[0].";";
 
 for ($i=0;$i<=7;$i=$i+2)
 {
	$sCode=$sCode.fcZiffer(substr($T[1],$i,2),3).";";
 }
 $sCode=$sCode.fcZiffer(substr($T[2],0,2),3).";";
 $sCode=$sCode.fcZiffer(substr($T[2],2,2),3).";";
 for ($i=0;$i<=7;$i=$i+2)
 {
	$sCode=$sCode.fcZiffer(substr($T[3],$i,2),3).";";
 }
 $sCode=$sCode.fcZiffer(substr($T[4],0,2),3).";";
 $sCode=$sCode.fcZiffer(substr($T[4],2,2),3)."|";
 
 
 
 
 // Übertragen auf Schloß Datei system schloß IP hohlen
    $IP="";
    $db=fcCon();
	$Wert="";
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
	$aWert=explode( chr(13).chr(10), $Wert);
    for($i=0;$i<=count($aWert)-1;$i++)
	{
		if (substr($aWert[$i],0,10)=="IPSchloss=")
		{
			//$IP= "http://".substr($aWert[$i],10);
			$IP= substr($aWert[$i],10);
			echo $IP."</br>";
		}
	}
	
     echo "</br>".$C."</br>";
	 echo "</br>".$sCode."</br>";
	echo "</br>// dpddxjamtr339hza.myfritz.net</br>";
	echo "</br>   ".$IP."</br>";
	 //$IP = gethostbyname ($IP);
	  $IP = gethostbyname ("dpddxjamtr339hza.myfritz.net");
	 $href="http://".$IP."?".$sCode;
	 echo "</br>".$href."</br>";
	
	 Header("Location:".$href);
 
    // Header($href);
 
 ?>