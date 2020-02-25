<?php

define('URL', 'http://localhost/cpanel/');
define('LIBS', 'libs/');

define('DB_TYPE', 'sqlsrv');
define('DB_HOST', 'localhost');
define('DB_NAME', 'opennos');
define('DB_USER', '');
define('DB_PASS', '');

// The sitewide hashkey, do not chaneg this because its used for passwords!
// This is for other hash keys... Not sure yet
define('HASH_GENERAL_KEY', 'MixitUp200');

// This is for database passwords only.
define('HASH_PASSWORD_KEY', 'catsFLYhigh@200miles');
?>
