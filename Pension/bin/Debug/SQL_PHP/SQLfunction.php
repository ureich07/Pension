<?php

function fcCon()    //$ID
		{
			$db= mysqli_connect('127.0.0.1','Reich','02041883-Pi','pension');
			mysqli_set_charset($db,'utf8');
			If (!$db)
			{
				echo "verbindungsfehler".mysqli_connect_error();	
			}
			return $db;
		}
function fSQL($con,$SQL)	
	{	
			mysqli_query($con,$SQL);
			$Err=mysqli_error($con); //SQL ERRORLOG Schreiben
			if($Err<>"")
			{
				fSQLError("Update",$Err,$SQL);
			}
	}
function clf()
	{
		return chr(13).chr(10);
	}
	
	
function fPari($sZeile)
	{
		$nl=0;
		$nStart=strlen($sZeile)-11;
		if ($nStart<0)
		{
			$nStart=0;
		}
		For($i=$nStart;$i<=strlen($sZeile);$i++)
		{
            $nl = $nl + ord(substr($sZeile, $i, 1));
        }
       return $nl;
	}
function fError($Quelle,$SQL)  //Error datei schreiben
	{    

		$datei = fopen("./DBSi/Error.dat","a"); //öffnen
		$Err=strftime("%d.%m.%Y %H:%M:%S",time())." ".$Quelle." ".$SQL.chr(13).chr(10);
		fwrite($datei,$Err); //Text reinschreiben.
		fclose($datei); //Datei schließen.
	}
	
function fSQLError($Quelle,$Error,$SQL)  //SQLError datei schreiben
	{    
        $clf=chr(13).chr(10)."_______________";
		$datei = fopen("./DBSi/SQLError.dat","a"); //öffnen
		$Err=strftime("%d.%m.%Y %H:%M:%S",time())." ".$Quelle.$clf.$SQL.$clf.$Error.chr(13).chr(10);
		fwrite($datei,$Err); //Text reinschreiben.
		fclose($datei); //Datei schließen.
	}
function fDeCrypt($PW,$SW)
	{
		$nMax=strlen($PW)-1;
		$y=strlen($SW);
		$fDeCrypt="";
		For ($n=0;$n<=$nMax;$n++) 
		{
			$x=$n % $y;
			if($x==0)
			{
				 $x=strlen($SW);
			}
			$fDeCrypt=$fDeCrypt.chr(ord(substr($SW,$x,1)) ^  ord(substr($PW,$n,1)));
		}
		return $fDeCrypt;
	}
Function fDeCryptSQL($sString ,$sSecurityValue)
    {
		$nMaxs=strlen($sSecurityValue);
		$nMax=strlen($sString)-1;
		$sCrypt="";	
		for ($n=0;$n <= $nMax; $n++)
		{
			$x=($n+1)%$nMaxs;
			if ($x==0)
			{
				$x=$nMaxs;
			}
			$x=$x-1;
			$Z=ord(substr($sString,$n,1));
			if ($Z==129)
			{
				$Z=61;
			}
			$sCrypt=$sCrypt.chr(ord(substr($sSecurityValue,$x,1)) ^ $Z);
		}
		return $sCrypt;
    }
Function fDeCryptPara($sString ,$sSecurityValue)
    {
		$nMaxs=strlen($sSecurityValue);
		$Zeichen = explode(";", $sString);
		$nMax=count($Zeichen)-1;
		$sCrypt="";	
		for ($n=0;$n <= $nMax; $n++)
		{
			$x=($n+1)%$nMaxs;
			if ($x==0)
			{
				$x=$nMaxs;
			}
			$x=$x-1;
			
			$sCrypt=$sCrypt.chr(ord(substr($sSecurityValue,$x,1)) ^ intval($Zeichen[$n]));
		}
		return $sCrypt;
    }
function fcGetTimeID()
	{  
		return  strftime("%Y%m%d%H%M%S",time());
    }
	
	
	
?>