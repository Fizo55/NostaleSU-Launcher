<?php

class Dashboard_Model extends Model {

	function __construct() {
		parent::__construct();
	}
	
	function xhrInsert() 
	{
		$title = $_POST['title'];
		$message = $_POST['message'];
		$newslink = $_POST['newslink'];
		$imagelink = $_POST['imagelink'];
		$realms = $_POST['realms'];
		
		$date_today = date("d F Y ");
		
		$this->db->insert('news_launcher', array('title' => $title, 'message' => $message, 'newslink' => $newslink, 'imagelink' => $imagelink, 'realms' => $realms, 'date' => $date_today));

		$data = array('title' => $title, 'ORDER id DESC' => $this->db->lastInsertId());
		echo json_encode($data);
	}
	
	
	/**** HOT NEWS  *****/
	
	
	function xhrHotInsert() 
	{
		$message = $_POST['message'];
		$realms = $_POST['realms'];
		
		$this->db->insert('hot_news', array('message' => $message, 'realms' => $realms));

		$data = array('message' => $message, 'id' => $this->db->lastInsertId());
		echo json_encode($data);
	}
	
	function xhrGetHotListings()
	{
		$result = $this->db->select('SELECT * FROM `hot_news` ORDER BY `id` DESC');
		echo json_encode($result);
	}
	
	function xhrDeleteHotListing()
	{
		$id = (int) $_POST['id'];
		$this->db->delete('hot_news', "id = '$id'");
	}
	
	/**** HOT NEWS  *****/
	
	function xhrGetListings()
	{
		$result = $this->db->select('SELECT * FROM `news_launcher` ORDER BY `id` DESC');
		echo json_encode($result);
	}
	
	function xhrDeleteListing()
	{
		$id = (int) $_POST['id'];
		$this->db->delete('news_launcher', "id = '$id'");
	}

}