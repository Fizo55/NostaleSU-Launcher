<?php

class Patchlist extends Controller {

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

	public function getPatchList() {
		$this->model->getPatchList();
	}

	public function moveFileAndCreatePatchList() {
		$this->model->moveFileAndCreatePatchList();
	}

	public function upload()
	{
		$this->model->upload();
	}

	public function index()
	{
		$this->view->userList = $this->model->userList();
		$this->view->render('patchlist/index');
	}
}
