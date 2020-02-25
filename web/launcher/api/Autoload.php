<?php

class Autoload {

    private static $_path;

    private static $_loadFile = array();

    private static $_prefix = array('\\',"\0",'_');

    private static $_load;

    private static $directory = array(
        'api/lib/',
    );

    public static function register($path = null){
        self::$_path = ($path === null) ? realpath(__DIR__).DIRECTORY_SEPARATOR : $path;
        spl_autoload_register(array(__CLASS__, 'load'));
    }

    public static function setDirectory($directory){
        self::$directory[] = $directory;
    }

    public static function load($class)
    {

        $name = str_replace(self::$_prefix, '/', $class) . '.php';

        foreach (array_reverse(self::$directory) as $dirs) {
            $path = self::$_path . $dirs . $name;

            if (is_file($path) and !isset(self::$_loadFile[$class])) {
                self::$_loadFile[$class] = $path;
            }
        }

        if (is_file(self::$_loadFile[$class])) {
            self::$_load[] = self::$_loadFile[$class];
            require_once self::$_loadFile[$class];
        } else {
            throw new \Exception('not found class "' . $class . '"');
        }

    }
}


