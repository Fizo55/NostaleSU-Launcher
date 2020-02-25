<?php

class Ban_Model extends Model {
	public function __construct()
	{
		parent::__construct();
	}

  function isa_count_datafile_lines($file) {
    set_time_limit(300);
    ini_set('memory_limit', '-1');
    $arr = file($file, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
    $c = ( false === $arr) ? 0 : count($arr);
    set_time_limit(30);// restore to default
    ini_set('memory_limit','128M');// restore to default
    return $c;
  }

  public function read()
  {
    foreach(file('../launcher/ban.txt') as $line) {
       echo $line. "\n";
    }
  }

  public function revoke()
  {
    $saveline = '';

    $fn = fopen("../launcher/ban.txt","r");

    while(!feof($fn))  {
    	$saveline .= fgets($fn);
    }

    fclose($fn);

    $hwid = $_POST['username']."=";
    $hwid .= str_replace('ProcessorId', '', shell_exec("wmic cpu get ProcessorId")); // If the code bellow (with the database) is uncommented delete this !
    $hwid .= ";"; // Add a delimitation to delete the ban

    /*
      $req = $this->db->prepare('SELECT * FROM Account WHERE Name = ?');
      $req->execute([$_POST['username']]);
      $result = $req->fetch();

      $hwid = $result["hwid"];
    */

    $file = fopen("../launcher/ban.txt", "w");
    fwrite($file, str_replace(str_replace(array("\r", "\n", ' '), '', trim($hwid)), '', str_replace(array("\r", ' '), '', trim($saveline))));
    fclose($file);
    header('Location: ../ban');
  }

  public function add()
  {
    $hwid = $_POST['username']."=";
    $hwid .= str_replace('ProcessorId', '', shell_exec("wmic cpu get ProcessorId")); // If the code bellow (with the database) is uncommented delete this !
    $hwid .= ";"; // Add a delimitation to delete the ban

    /*
      $req = $this->db->prepare('SELECT * FROM Account WHERE Name = ?');
      $req->execute([$_POST['username']]);
      $result = $req->fetch();

      $hwid = $result["hwid"];
    */

    if (!file_exists("../launcher/ban.txt")) {
      die('File doesn\'t exist anymore');
    }

    $file = fopen("../launcher/ban.txt", "a+");
    if (number_format($this->isa_count_datafile_lines("../launcher/ban.txt")) == 0) {
      fprintf($file, str_replace(array("\r", "\n", ' '), '', trim($hwid)));
    }
    else {
      fprintf($file, "\n");
      fprintf($file, str_replace(array("\r", "\n", ' '), '', trim($hwid)));
    }
    fclose($file);
    header('Location: ../ban');
  }
}
