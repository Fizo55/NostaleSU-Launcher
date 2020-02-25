<?php

class User extends Controller {

	public function __construct() {
		parent::__construct();
		Session::init();
		$logged = Session::get('loggedIn');
		$role = Session::get('role');

		if ($logged == false || $role != 'admin') {
			Session::destroy();
			header('location: login');
			exit;
		}
		
	}
	
	public function index()
	{
		$this->view->userList = $this->model->userList();
		$this->view->render('user/index');
	}

	public function create()
	{
		$data = array();
		$data['realname'] = $_POST['realname'];
		$data['username'] = $_POST['username'];
		$data['password'] = ($_POST['password']);
		$data['email'] = $_POST['email'];
		$data['role'] = $_POST['role'];

		// @TODO: Do your error checking!

		$this->model->create($data);
		header('location: ' . URL . 'user');
	}

	public function edit($id)
	{
		$this->view->user = $this->model->userSingleList($id);
		$this->view->render('user/edit');
	}

	public function editSave($id)
	{
		$data = array();
		$data['id'] = $id;
		$data['realname'] = $_POST['realname'];
		$data['username'] = $_POST['username'];
		$data['password'] = ($_POST['password']);
		$data['email'] = $_POST['email'];
		$data['role'] = $_POST['role'];

		// @TODO: Do your error checking!

		$this->model->editSave($data);
		header('location: ' . URL . 'user');
	}

	public function delete($id)
	{
		$this->model->delete($id);
		header('location: ' . URL . 'user');
	}

}