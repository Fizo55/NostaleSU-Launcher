<?php

require 'config.php';

// This is autoloader for libs files.
spl_autoload_register(function($class)
{
	require LIBS . $class .".php";
});

$app = new Bootstrap();
