<!DOCTYPE html>
<html>
<head>
	<title>WoW Suite - Maintened by LDF - Team</title>
	<link rel="stylesheet" href="public/css/default.css" />
	<link rel="stylesheet" href="public/css/zozo.style.min.css" />
	<link rel="stylesheet" href="public/css/zozo.tabs.min.css" />
	<script type="text/javascript" src="public/js/jquery.tabs.min.js"></script>
	<script type="text/javascript" src="public/js/jquery.min.js"></script>
	<script type="text/javascript" src="public/js/zozo.tabs.min.js"></script>

	<?php
		if (isset($this->js))
		{
			foreach ($this->js as $js)
			{
				echo '<script type="text/javascript" src="views/'.$js.'"></script>';
			}
		}
	?>
</head>
<body>

<?php Session::init(); ?>


<div id="header">

	<?php if (Session::get('loggedIn') == false):?>
	<!-- <a href="login">Login</a> -->

	<?php endif; ?>
	<?php if (Session::get('loggedIn') == true):?>
	<a href="dashboard">Dashboard</a>
	<a href="patchlist">PatchList</a>
	<?php if (Session::get('role') == 'admin'):?>
	<a href="user">Users</a>
	<a href="ban">Ban</a>
	<?php endif; ?>
	<a href="help">Help</a>
	<a href="dashboard/logout">Logout</a>


	<?php endif; ?>
</div>

<div id="content">
