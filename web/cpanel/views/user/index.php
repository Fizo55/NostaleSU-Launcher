<h1>Add User</h1>



<div id="tabbed-nav">
            <ul>
                <li><a>ADD USER<span>Adds a new user</span></a></li>
                <li><a>EDIT USERS<span>Show the list of users</span></a></li>
            </ul>
            <div>
			<div>
<form method="post" action="<?php echo URL; ?>user/create">
<ul class="form-style-1">

<li>	
<label>Real Name</label>
<input type="text" name="realname" class="field-long" />
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
<input type="text" name="email" class="field-long" />
</li>


<label>Role</label>
<center>
<select name="role">
<option value="default">Default</option>
<option value="admin">Admin</option>
</select>
</center>
    <li>
	<center>
        <input type="submit" value="Submit" />
	</center>
    </li>
</ul>
</form>
</div>

<div>
<table>
<tr>
<th>Id</th>
<th>Real Name</th>
<th>Username</th>
<th>Email</th>
<th>Role</th>
<th>Settings</th>
</tr>
<?php
foreach ($this->userList as $key => $value) {


	
	
	echo '<tr>';
	echo '<td>' . $value['id'] . '</td>';
	echo '<td>' . $value['realname'] . '</td>';
	echo '<td>' . $value['username'] . '</td>';
	echo '<td>' . $value['email'] . '</td>';
	echo '<td>' . $value['role'] . '</td>';
	echo '<td>
	<!-- <a href="'.URL.'user/edit/'.$value['id'].'">Edit</a>  -->
	
	<a href="'.URL.'user/delete/'.$value['id'].'">Delete</a></td>';
	echo '</tr>';
	
	
	
	

}
?>
</table>

 </div>



            </div>
        </div>







<hr />
