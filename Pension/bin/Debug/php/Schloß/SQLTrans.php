<?php
set_time_limit(60);
 date_default_timezone_set("Europe/Berlin");
 include 'SQLfunction.php';
 
 Function fcZiffer($Zahl,$z)    //$ID
		{
			$Zahl="000000".$Zahl;
			//$str = 'abcdef';
            $l=strlen($Zahl); // 6
            $Zahl=substr($Zahl,$l-$z);
			return $Zahl;
		}
		
 Function logFile($wert)
		{
	         $handle=fopen("/home/sqlpension/LogText.txt","w");
			 fwrite($handle,$wert);
			// echo $wert;
			 fclose($handle);
		}
 $toDay=strftime("%Y%m%d",time());
 $con = fcCon(); //"", "root", "", "Pension"); 
 $dt = mysqli_query($con,"DELETE FROM code WHERE bis < '".$toDay."'" );
 mysqli_close($con); 
 
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
			$IP= substr($aWert[$i],10);
		
		}
	}
	//Daten hohlen
	$Day=date("Ymd", strtotime("2 days"));
	 $sCode="Daten=";
	 $db=fcCon();
	
	$sSql= "SELECT * FROM Code WHERE von <= '".$Day."'";   //* from bild where ID='".$id."'";
	//echo $sSql."</br>";
	$result=mysqli_query($db,$sSql);
    if ($result->num_rows > 0) 
    {
	
		while($row = $result->fetch_assoc()) 
	    {
			if (strlen($row["code"])<>5)
			{
				
				$sCode= $sCode.$row["code"].";";  // ";"
				//$sCode=str_replace(";","x",$sCode.$row["code"])."x";
			}else
			{
				$Wert= $row["code"];
				
				for ($i=0;$i<=4;$i++)
				{
					$sCode=$sCode.fcZiffer(substr($Wert,$i,1),3).";";// ";"
				}
			}
			$Wert= $row["von"];
			for ($i=0;$i<=7;$i=$i+2)
			{
				$sCode=$sCode.fcZiffer(substr($Wert,$i,2),3).";";// ";"
			}
				
			$Wert= $row["vonzeit"];
			$sCode=$sCode.fcZiffer(substr($Wert,0,2),3).";";// ";"
			$sCode=$sCode.fcZiffer(substr($Wert,2,2),3).";";// ";"
			
			$Wert= $row["bis"];
			for ($i=0;$i<=7;$i=$i+2)
			{
				$sCode=$sCode.fcZiffer(substr($Wert,$i,2),3).";";// ";"
			}
			$Wert= $row["biszeit"];
			$sCode=$sCode.fcZiffer(substr($Wert,0,2),3).";";// ";"
			$sCode=$sCode.fcZiffer(substr($Wert,2,2),3)."|";
			//echo $sCode."</br>";
		}
    }
	mysqli_close($db);
	//echo "</br>".$sCode."</br>";
	logFile($sCode);
	//echo $IP."</br>";
	$IP = "http://".gethostbyname($IP);
	//echo $IP."</br>";
	 //echo "</br>".$sCode."</br>";
    $href=$IP."?".$sCode;
	echo "</br>".$href."</br>";
	Header("Location:".$href);

 ?>