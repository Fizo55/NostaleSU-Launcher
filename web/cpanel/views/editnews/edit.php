<h1>Edit News</h1>




<?php



/*
$this->newsList = array("id", "title", "message", "newslink", "imagelink", "realms");

print_r (array_values($this->newsList));

echo ($this->newsList[0]);
*/
?>








<form method="post" action="<?php echo URL; ?>editnews/editSave/<?php  ?>  ">
<ul class="form-style-1">

<li>	
<label>News Title</label>
<input type="text" name="title" class="field-long" />
</li>

<li>	
<label>News Link</label>
<input type="text" name="newslink" class="field-long" />
</li>

<li>
<label>Image Link</label>
<input type="text" name="imagelink" class="field-long" />
</li>

    <li>
        <label>Change Realm</label>
        <select name="realms" class="field-select">
        <option value="realm1">Realm1</option>
        <option value="realm2">Realm2</option>
        <option value="realm3">Realm3</option>
		<option value="realm4">Realm4</option>
        </select>
    </li>
    <li>
        <textarea name="message" id="field5" placeholder="Enter a Message" class="field-long field-textarea"></textarea>
    </li>
    <li><center>
        <input type="submit" value="Submit" />
		</center>
    </li>
</ul>
</form>