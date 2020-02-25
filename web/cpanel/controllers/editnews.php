<?php

class Editnews extends Controller {

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
		$this->view->js = array('dashboard/js/default.js');
	}
	
	public function index()
	{
		$this->view->newsList = $this->model->newsList();
		$this->view->render('editnews/index');
	}

	public function create()
	{
		$data = array();
		$data['title'] = $_POST['title'];
		$data['message'] = $_POST['message'];
		$data['newslink'] = ($_POST['newslink']);
		$data['imagelink'] = $_POST['imagelink'];
		$data['realms'] = $_POST['realms'];

		// @TODO: Do your error checking!

		$this->model->create($data);
		header('location: ' . URL . 'editnews');
	}

	public function edit($id)
	{
		$this->view->editnews = $this->model->newsSingleList($id);
		$this->view->render('editnews/edit');
	}

	public function editSave($id)
	{
		$data = array();
		$data['id'] = $id;
		$data['title'] = $_POST['title'];
		$data['message'] = $_POST['message'];
		$data['newslink'] = ($_POST['newslink']);
		$data['imagelink'] = $_POST['imagelink'];
		$data['realms'] = $_POST['realms'];

		// @TODO: Do your error checking!

		$this->model->editSave($data);
		header('location: ' . URL . 'editnews');
	}

	public function delete($id)
	{
		$this->model->delete($id);
		header('location: ' . URL . 'editnews');
	}
	
	
}