<?php
  Session::init();
  echo Session::get('error');
?>
<script src="public/js/dropzone.js"></script>

<script type="text/javascript" src="public/js/patchlist.js"></script>

<!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" integrity="sha384-HSMxcRTRxnN+Bdg0JdbxYKrThecOKuH5zCYotlSAcp1+c8xmyTe9GYg1l9a69psu" crossorigin="anonymous">

<!-- Optional theme -->
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap-theme.min.css" integrity="sha384-6pzBo3FDv/PJ8r2KRkGHifhEocL+1X2rVCTTkUfGk7/0pbek5mMa1upzvWbrUbOZ" crossorigin="anonymous">

<!-- Latest compiled and minified JavaScript -->
<script src="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" integrity="sha384-aJ21OjlMXNL5UyIl/XNwTMqvzeRMZH2w8c5cRVpzpU8Y5bApTppSuUkhZXN0VxHd" crossorigin="anonymous"></script>

<h1>Patchlist Editor</h1>
<div id="page">
    <div id="tabbed-nav">
        <ul>
            <li><a>Patch<span>Adds files</span></a></li>
        </ul>
        <div>
            <div>
                Note : If you want to add a file in a other folder than the actual one you'll need to refresh the page this is the same for the language. No folder = Don't put folder name
                <center>
                    <br>
                    <form action="patchlist/upload" class="dropzone">
                        <div class="fallback">
                            <input name="file" type="file" />
                        </div>
                    </form>
                    <br>
                    <form method="POST" action="patchlist/moveFileAndCreatePatchList">
                      <select name="select" style="width:800px;">
                          <option value="en">English</option>
                          <option value="fr">Français</option>
                          <option value="es">Español</option>
                          <option value="it">Italiano</option>
                          <option value="de">Deutsch</option>
                          <option value="cz">Český</option>
                          <option value="tr">Türk</option>
                          <option value="ru">Русский</option>
                      </select>
                      <br>
                      <br>
                      <input style="width:800px;" type="text" name="path" placeholder="Enter the name of the folder where the file will be installed">
                      <ul class="form-style-1">
                        <li>
                          <input type="submit" value="Submit" />
                        </li>
                      </ul>
                    </form>
                </center>
            </div>
        </div>
    </div>

    <h2>Patch List</h2>
    <br />
    <center>
      <select id="select" style="width:800px;">
          <option value="en">English</option>
          <option value="fr">Français</option>
          <option value="es">Español</option>
          <option value="it">Italiano</option>
          <option value="de">Deutsch</option>
          <option value="cz">Český</option>
          <option value="tr">Türk</option>
          <option value="ru">Русский</option>
      </select>
    </center>
    <div id="tabbed-nav2">
        <ul>
            <li><a>Files Information<span>Show the information about the files</span></a></li>
        </ul>
        <div>
            <div id="toappend">
                <table id="News">
                    <tr>
                        <th>Name</th>
                        <th>Hash</th>
                        <th>Size</th>
                    </tr>
                    <div id="listInserts"></div>
                </table>
            </div>
        </div>
    </div>
</div>
