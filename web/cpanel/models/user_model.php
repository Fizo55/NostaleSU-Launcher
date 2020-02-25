<?php

class User_Model extends Model
{
	public function __construct()
	{
		parent::__construct();
	}

	public function userList()
	{
		return $this->db->select('SELECT id, realname, username, email, role FROM users');
	}

	public function userSingleList($id)
	{
		return $this->db->select('SELECT id, realname, username, email, role FROM users WHERE id = :id', array(':id' => $id));
	}

	public function create($data)
	{
		$this->db->insert('users', array(
			'realname' => $data['realname'],
			'username' => $data['username'],
			'password' =>  Hash::create('sha256', $data['password'], HASH_PASSWORD_KEY),
			'email' => $data['email'],
			'role' => $data['role']
			));
	}

	public function editSave($data)
	{

		$postData = array(
			'realname' => $data['realname'],
			'username' => $data['username'],
			'password' =>  Hash::create('sha256', $data['password'], HASH_PASSWORD_KEY),
			'email' => $data['email'],
			'role' => $data['role']
			);
		
		$this->db->update('users', $postData, "`id` = {$data['id']}");
	}

	public function delete($id)
	{
		$result = $this->db->select('SELECT role FROM users WHERE id = :id', array(':id' => $id));
		if($result[0]['role'] == 'admin')
			return false;

		$this->db->delete('users', "id = '$id'");
	}
	
}