<?php

class Editnews_Model extends Model {

	public function __construct()
	{
		parent::__construct();
	}

	public function newsList()
	{
		return $this->db->select('SELECT id, title, message, newslink, imagelink, realms FROM news_launcher');
		//$this->newsList;  

	}

	public function newsSingleList($id)
	{
		return $this->db->select('SELECT id, title, message, newslink, imagelink, realms FROM news_launcher WHERE id = :id', array(':id' => $id));
	}

	public function create($data)
	{
		$this->db->insert('news_launcher', array(
			'title' => $data['title'],
			'message' => $data['message'],
			'newslink' => $data['newslink'],
			'imagelink' => $data['imagelink'],
			'realms' => $data['realms']
			));
	}

	public function editSave($data)
	{

		$postData = array(
			'title' => $data['title'],
			'message' => $data['message'],
			'newslink' => $data['newslink'],
			'imagelink' => $data['imagelink'],
			'realms' => $data['realms']
			);
		
		$this->db->update('news_launcher', $postData, "`id` = {$data['id']}");
	}

	public function delete($id)
	{
		$result = $this->db->select('SELECT role FROM news_launcher WHERE id = :id', array(':id' => $id));
		if($result[0]['role'] == 'admin')
			return false;

		$this->db->delete('news_launcher', "id = '$id'");
	}

}