<h1>Edit User</h1>




<form method="post" action="user/editSave/<?php echo $this->user['id']; ?>">
<ul class="form-style-1">

<li>
<label>Real Name</label>
<input type="text" name="realname"  class="field-long" />
</li>

<li>
<label>Username</label>
<input type="text" name="username" class="field-long" />
</li>

<li>
<label>Password</label>
<input type="text" name="password" class="field-long" />
</li>

<li>
<label>Email</label>
<input type="text" name="password"  class="field-long" />
</li>

<li>

<label>Role</label>
<select name="role">
<option value="default">Default</option>
<option value="admin">Admin</option>
</select><br />

</li>


    <li>
	<center>
        <input type="submit" value="Submit" />
		</center>
    </li>
</ul>
</form>
