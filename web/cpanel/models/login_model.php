<?php

class Login_Model extends Model
{
	public function __construct()
	{
		parent::__construct();
	}

	public function run()
	{
		$sth = $this->db->prepare("SELECT * FROM Account WHERE
				Name = ? AND Password = ?");
		$sth->execute([$_POST['username'], hash('sha512', $_POST['password'])]); // Anyway in our case HASH_KEY_PASSWORD it's useless

		$data = $sth->fetch();

		if (!empty($data)) {
			// login
			Session::init();
			Session::set('role', $data['Authority'] >= 2 ? "admin" : 'user');
			Session::set('loggedIn', true);
			header('location: ../dashboard');
		} else {
			header('location: ../login');
		}

	}

}
