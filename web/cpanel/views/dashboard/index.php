    <div id="page">
        <div id="tabbed-nav">
            <ul>
                <li><a>News<span>Adds News</span></a></li>
                <li><a>Hot News<span>Adds Hot News</span></a></li>
            </ul>


            <div>

                <div>

<form id="randomInsert" action="dashboard/xhrInsert/" method="post">
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
                 </div>
                <div>

<br><br><br><br>
<form id="randomHotInsert" action="dashboard/xhrHotInsert/" method="post">
<ul class="form-style-1">
    <li>
        <label>Change Realm</label>
        <select name="realms" class="field-select">
        <option value="realm1">Realm1</option>
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
</form>	<br><br><br><br><br><br>
				</div>
            </div>
        </div>



<h2>Lasted News</h2>
<br />
        <div id="tabbed-nav2">
            <ul>
                <li><a>Banner News<span>Show the list of news</span></a></li>
				<li><a>Hot News<span>Show a list of hot news</span></a></li>
            </ul>
            <div>

			<!-- News Tab  -->
			<div>

			<table id="News">
			<tr>
			<th>Title</th>
			<th>Realm</th>
			<th>Settings</th>
			</tr>
			<div id="listInserts"></div>
			</table>
			</div>

			<!-- Hot News Tab  -->
			<div>

			<table id="HotNews">
			<tr>
				<th>Message</th>
				<th>Realm</th>
				<th>Settings</th>
			</tr>
            <div id="listHotInserts"></div>
			</table>
			</div>

            </div>
        </div>
    </div>
