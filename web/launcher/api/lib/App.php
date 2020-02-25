<?php

class App {

    public static $key;
    private static $_conf = false;
    protected static $_init;
    const PATH = ROOT;

    public static function __Init(){
        $key = isset($_GET['_key']) ? $_GET['_key'] : false;

        if(self::$key !== $key){
            header('HTTP/1.1 404 Not Found');
            header('Status: 404 Not Found');
            exit();
        }

        if(self::$_init === null){
            self::$_init = new self();
        }
        return self::$_init;
    }

    protected function __construct(){
        $file = self::PATH.'api'.DS.'config.php';
        if(is_file($file)){
            self::$_conf = require_once $file;
        }else{
            $this->error('Not Found File config!',E_USER_ERROR);
        }
    }

    private function error($msg,$type){
        if(DISPLAY_ERROR === true){
            trigger_error($msg,$type);
        }
        exit();
    }

    private function TestConnect($host,$port){
        $fp = @fsockopen($host, $port, $errno, $errstr, 10);
        if($fp){
            return true;
        }else{
            return false;
        }
    }

    protected function Connect(){
      return $this->connexion_bdd = new PDO('sqlsrv:Server=localhost;Database=opennos', '', '');
    }

    protected function XMLRender($keys,$data,$multi = false){
        if(is_array($keys) and is_array($data)) {

            //header("Content-Type: text/xml");

            $xml = new XMLWriter();
            $xml->openMemory();
            $xml->startDocument();

            if($multi === false) {
                foreach ($keys as $key) {
                    $xml->startElement($key);
                }

                foreach ($data as $key => $val) {
                    $xml->writeElement($key, $val);
                }

                foreach ($keys as $key) {
                    $xml->endElement();
                }
            }else{
                $elem = array_pop($keys);

                foreach ($keys as $key) {
                    $xml->startElement($key);
                }

                foreach($data as $line){
                    $xml->startElement($elem);
                    foreach($line as $k=>$l){
                        $xml->writeElement($k, $l);
                    }
                    $xml->endElement();
                }

                foreach ($keys as $key) {
                    $xml->endElement();
                }
            }

           return $xml->outputMemory();
        }else{
            $this->error('XML Data ERROR',E_USER_ERROR);
        }
    }
    public function getOnline(){
        $db = $this->Connect(self::$_conf['characters']);
        $online = '';
        $online_list = $db->query("SELECT name,race,class,level,gender FROM `characters` WHERE `online` = 1 AND NOT `extra_flags` & 16 ORDER BY `name`")->fetchAll();
        if($online_list !== false and count($online_list) > 0){
            foreach($online_list as $key=>$onl){
                $class = $this->ClassState($onl['class']);
                $online[$key] = array(
                    'Name' => $onl['name'],
                    'Level' => $onl['level'],
                    'Race' => self::$_conf['site_url'].$this->OnlineIcon((int) $onl['race'],(int) $onl['gender']),
                    'Class' => $class['class'],
                    'Side' => $this->Fraction((int) $onl['race']),
                    'TotalTime' => $this->totalTime($onl['totaltime'])
                );
            }
            if(is_array($online)){
                echo $this->XMLRender(array('Characters','Stat','CharBlock'),$online,true);
            }else{
                echo 0;
            }

        }else{
            echo 0;
        }
    }

    public function getHotNews(){
        $db = $this->Connect();
        $hot_news = $db->query("SELECT TOP 1 * FROM hot_news WHERE realms = 'realm1'");
        $result = $hot_news->fetch();
	      if(!empty($result)){
            echo $result['message'];
        }else{
            echo 'note';
        }
    }

    public function getNews(){
        $db = $this->Connect();
        $news = $db->query("SELECT TOP 4 * FROM news_launcher WHERE realms = 'realm1'");
        $result = $news->fetchAll();
        $count = 0;
        if(!empty($result)){
            if (count($result) == 1) {
              $new = '';
            }
            else {
              $new = [];
            }
            foreach($result as $key => $new_post){
                if (count($result) == 1) {
                  $new = array(
                    'NewsTitle' => $new_post['title'],
                    'Text'      => $new_post['message'],
                    'ImageLink' => $new_post['imagelink'],
                    'NewsLink'  => $new_post['newslink'],
                    'Realms'    => $new_post['realms'],
                    'Date'      => $new_post['date_time'],
                  );
                }
                else {
                  $test = array(
                    'NewsTitle' => $new_post['title'],
                    'Text'      => $new_post['message'],
                    'ImageLink' => $new_post['imagelink'],
                    'NewsLink'  => $new_post['newslink'],
                    'Realms'    => $new_post['realms'],
                    'Date'      => $new_post['date_time'],
                  );

                  array_push($new, $test);
                }
                $count++;
            }
            if(!empty($new)) {
                echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), $new, $count == 1 ? false : true);
            } else {
                echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), array('NewsTitle'=>'No news !'));
            }
        } else {
            echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), array('NewsTitle'=>'No news !'));
        }
    }

	///  COUNT ONLINE     ///
	public function CountOnline(){
        $db = $this->Connect(self::$_conf['characters']);
        $online = $db->count('characters',array('online' => 1));
        if(is_numeric($online) and $online !== 0){
            echo $online;
        }else{
            echo 0;
        }
    }

	//////// LOGIN  //////////
	function hashPass($pass) {
	   return hash('sha512', $pass);
	}

	public function getLogin(){
		$login = $_GET['login'];
		$pass = $_GET['password'];
		$db = $this->Connect();
    $req = $db->prepare('SELECT * FROM Account WHERE Name = ? AND Password = ?');
    $req->execute([$login, $this->hashPass($pass)]);
    $result = $req->fetch();
		sleep(1);
		if (!empty($result)) {
		    echo 'true';
		} else {
		    echo 'false';
		}
	}

	public function getNewsTab(){
    $db = $this->Connect();
    $news = $db->query("SELECT TOP 20 * FROM news_launcher WHERE realms = 'realm1' ORDER BY id DESC");
    $result = $news->fetchAll();
    $count = 0;
    if(!empty($result)){
        if (count($result) == 1) {
          $new = '';
        }
        else {
          $new = [];
        }
        foreach($result as $key => $new_post){
            if (count($result) == 1) {
              $new = array(
                'NewsTitle' => $new_post['title'],
                'Text'      => $new_post['message'],
                'ImageLink' => $new_post['imagelink'],
                'NewsLink'  => $new_post['newslink'],
                'Realms'    => $new_post['realms'],
                'Date'      => $new_post['date_time'],
              );
            }
            else {
              $test = array(
                'NewsTitle' => $new_post['title'],
                'Text'      => $new_post['message'],
                'ImageLink' => $new_post['imagelink'],
                'NewsLink'  => $new_post['newslink'],
                'Realms'    => $new_post['realms'],
                'Date'      => $new_post['date_time'],
              );

              array_push($new, $test);
            }
            $count++;
        }
        if(!empty($new)) {
            echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), $new, $count == 1 ? false : true);
        } else {
            echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), array('NewsTitle'=>'No news !'));
        }
    } else {
        echo $this->XMLRender(array('NewsRoot','ExpressNews','NewsItem'), array('NewsTitle'=>'No news !'));
    }
  }
}
