<?php


//if (version_compare(PHP_VERSION, '5.4.0') < 0) {
if (version_compare(PHP_VERSION, '5.3.0') < 0) {
    die('[My PHP version('.PHP_VERSION.')] << [APP PHP version(5.4.0)]');
}

require_once ROOT.'api'.DS.'Autoload.php';

Autoload::register(ROOT);

$get = isset($_GET['_url']) ? $_GET['_url'] : false;

\App::$key = 'asc312f8826gieb51504483ee8c19ta2';

$api = \App::__Init();

if($get === 'online'){
    $api->getOnline();
}
else if($get === 'news'){
    $api->getNews();
}
else if($get === 'hot_news') {
    $api->getHotNews();
}
else if($get === 'count_online'){
    $api->CountOnline();
}
else if($get === 'getNewsTab'){
    $api->getNewsTab();
}
else if($get === 'auth'){
    $api->getLogin();


} else {
    header('HTTP/1.1 404 Not Found');
    header('Status: 404 Not Found');
    exit();
}
