<?php

class Ban extends Controller {

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
		$this->view->render('ban/index');
	}

	public function add()
	{
		$this->model->add();
	}

	public function read()
	{
		$this->model->read();
	}

	public function revoke()
	{
		$this->model->revoke();
	}
}
