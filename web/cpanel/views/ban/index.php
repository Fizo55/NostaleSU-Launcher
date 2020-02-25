<?php
  Session::init();
  echo Session::get('Test');
?>

<div id="page">
    <div id="tabbed-nav">
        <ul>
            <li><a>Ban launcher<span>Add ban</span></a></li>
            <li><a>Ban launcher<span>Revoke ban</span></a></li>
        </ul>


        <div>

            <div>

<br><br><br><br>
<form action="ban/add" method="post">
<ul class="form-style-1">

<li>
<label>Username</label>
<input type="text" name="username" class="field-long" />
</li>
<li><center>
    <input type="submit" value="Submit" />
</center>
</li>
</ul>
</form> <br><br><br><br><br><br>
             </div>
            <div>

<br><br><br><br>
<form action="ban/revoke/" method="post">
<ul class="form-style-1">

<li>
  <label>Username</label>
  <input type="text" name="username" class="field-long" />
</li>
<li><center>
    <input type="submit" value="Submit" />
</center>
</li>
</ul>
</form>	<br><br><br><br><br><br>
    </div>
        </div>
    </div>
</div>
