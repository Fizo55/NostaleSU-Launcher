<?php

class Dashboard extends Controller {

	function __construct() {
		parent::__construct();
		Session::init();
		$logged = Session::get('loggedIn');
		if ($logged == false) {
			Session::destroy();
			header('location: login');
			exit;
		}

		$this->view->js = array('dashboard/js/default.js');

	}

	function index()
	{
		$this->view->render('dashboard/index');
	}

	function logout()
	{
		Session::destroy();
		header('location: ../login');
		exit;
	}
	/////////  HOT NEWS  ////////
	function xhrHotInsert()
	{
		$this->model->xhrHotInsert();
	}

	function xhrGetHotListings()
	{
		$this->model->xhrGetHotListings();
	}

	function xhrDeleteHotListing()
	{
		$this->model->xhrDeleteHotListing();
	}
	/////////  HOT NEWS  ////////

	function xhrInsert()
	{
		$this->model->xhrInsert();
	}

	function xhrGetListings()
	{
		$this->model->xhrGetListings();
	}

	function xhrDeleteListing()
	{
		$this->model->xhrDeleteListing();
	}
}
