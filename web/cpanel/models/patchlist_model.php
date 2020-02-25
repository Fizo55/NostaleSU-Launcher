<?php

class Patchlist_Model extends Model
{
	public function __construct()
	{
		parent::__construct();
	}

	public function userList()
	{
		return $this->db->select('SELECT * FROM Account');
	}

	public function userSingleList($id)
	{
		return $this->db->select('SELECT * FROM Account WHERE AccountId = :id', array(':id' => $id));
	}

	public function getPatchList () {
		#include file with data
		$arg = file('../launcher/client_'.$_GET['ClientLanguage'].'/patchlist.txt');

		#create array for our json
		$res = [];

		#start the cycle
		foreach ($arg as $k => $v) {
		    #take first element
		    $res[$k] = $v;
		}

		echo json_encode($res);
	}

	public function moveFileAndCreatePatchList() {
		Session::init();

		if (!empty($_POST['path'])) {
			if (!is_dir('../launcher/client_'.$_POST['select'].'/'.$_POST['path'].'/')) {
				mkdir('../launcher/client_'.$_POST['select'].'/'.$_POST['path'].'/', 0777, true);
			}
		}

		if (!is_dir('../launcher/client_'.$_POST['select'].'/')) {
			mkdir('../launcher/client_'.$_POST['select'].'/', 0777, true);
		}

		// Just to create the PID file xDDD
		if (!is_file('../launcher/client_'.$_POST['select'].'/pid.uls')) {
			$file = fopen('../launcher/client_'.$_POST['select'].'/pid.uls', "w");
			fwrite($file, "x");
			fclose($file);
		}

		// Just to create the patchlist file xDDD
		if (!is_file('../launcher/client_'.$_POST['select'].'/patchlist.txt')) {
			$file = fopen('../launcher/client_'.$_POST['select'].'/patchlist.txt', "w");
			fwrite($file, "");
			fclose($file);
		}

		if (is_array(Session::get('tmp_name'))) {
			for ($i = 0; $i < count(Session::get('tmp_name')); $i++) {
				if (!empty($_POST['path'])) {
					move_uploaded_file(Session::get('tmp_name')[$i], '../launcher/client_'.$_POST['select'].'/'.$_POST['path'].'/'.Session::get('path')[$i]);
					unlink(Session::get('tmp_name')[$i]);
				}
				else {
					move_uploaded_file(Session::get('tmp_name')[$i], '../launcher/client_'.$_POST['select'].'/'.Session::get('path')[$i]);
					unlink(Session::get('tmp_name')[$i]);
				}
			}
		}
		else if (!empty(Session::get('tmp_name'))) {
			if (!empty($_POST['path'])) {
				copy(Session::get('tmp_name'), '../launcher/client_'.$_POST['select'].'/'.$_POST['path'].'/'.Session::get('path'));
				unlink(Session::get('tmp_name'));
			}
			else {
				copy(Session::get('tmp_name'), '../launcher/client_'.$_POST['select'].'/'.Session::get('path'));
				unlink(Session::get('tmp_name'));
			}
		}

		$files = scandir("../launcher/client_".$_POST['select']."/");
		$data = array();
		$hideName = array('.','..','.DS_Store','patchlist.txt','pid.uls');
		$i = -1;

		$path = "../launcher/client_".$_POST['select']."/";
		$dirHandle = opendir($path);
	  while($item = readdir($dirHandle)) {
	    $newPath = $item;
	    if (($item == '.') || ($item == '..') || ($item == 'patchlist.txt') || ($item == 'pid.uls') || ($item == '.DS_Store')) {
	        continue;
	    }
	    if (is_dir($path.$newPath)) {
				$path2 = $path.$newPath.'/';
				$dirHandle2 = opendir($path2);
				while ($item2 = readdir($dirHandle2)) {
					if (($item2 == '.') || ($item2 == '..') || ($item2 == 'patchlist.txt') || ($item2 == 'pid.uls') || ($item2 == '.DS_Store')) {
							continue;
					}

					$i++;
					$data[$i] = $newPath.'/'.$item2;
				}
	    } else {
				$i++;
				$data[$i] = $item;
	    }
	  }

		var_dump($data);

		$data = array(
			"FileName" => $data,
			"ClientLanguage" => $_POST['select']
		);
		$data_string = json_encode($data);
		$ch = curl_init('http://localhost:9000/patchlist/index');
		curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
		curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
		curl_setopt($ch, CURLOPT_HTTPHEADER, array(
			'Content-Type: application/json',
			'Content-Length: ' . strlen($data_string))
		);
		$result2 = curl_exec($ch);
		curl_close($ch);

		$fp = fopen('../launcher/client_'.$_POST['select'].'/patchlist.txt', 'w');
		fwrite($fp, str_replace(['"', '\r\n'], ['', PHP_EOL], $result2));
		fclose($fp);
		Session::set('tmp_name', '');
		Session::set('path', '');
		header('Location: /patchlist');
	}

	public function upload() {
		if ($_FILES['file']['name'] != '')
		{
			$target_dir = '../launcher/client_en/'; // That's normal don't change the language here :)
			$file = $_FILES['file']['name'];
			$path = pathinfo($file);
			$ext = $path['extension'];
			$temp_name = $_FILES['file']['tmp_name'];
			$path_filename_ext = $target_dir.$file;
			if (file_exists($path_filename_ext))
			{
				unlink($path_filename_ext);
			}

			move_uploaded_file($temp_name, '../launcher/'.$file); // PHP / 20 ?

			Session::init();
			if (!empty(Session::get('tmp_name'))) {
				$array = [];
				array_push($array, '../launcher/'.$file, Session::get('tmp_name'));
				Session::set('tmp_name', $array);

				$array1 = [];
				array_push($array1, $file, Session::get('path'));
				Session::set('path', $array1);
			}
			else {
				Session::set('tmp_name', '../launcher/'.$file);
				Session::set('path', $file);
			}
		}
	}

	public function create($data)
	{
		$this->db->insert('Account', array(
			'Name' => $data['username'],
			'Password' =>  hash('sha512', $data['password']),
			'Email' => $data['email'],
			'Authority' => $data['role'] == 'admin' ? 2 : 0
		));
	}

	public function editSave($data)
	{
		$postData = array(
			'Name' => $data['username'],
			'Password' =>  hash('sha512', $data['password']),
			'Email' => $data['email'],
			'Authority' => $data['role'] == 'admin' ? 2 : 0
		);

		$this->db->update('Account', $postData, "AccountId = {$data['id']}");
	}

	public function delete($id)
	{
		$result = $this->db->select('SELECT Authority FROM Account WHERE id = :id', array(':id' => $id));
		if($result[0]['Authority'] == 2)
			return false;

		$this->db->delete('Account', "id = '$id'");
	}

}
