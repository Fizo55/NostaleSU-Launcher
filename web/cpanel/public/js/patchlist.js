$(function() {

	$('#select').on('change', function() {
		alert($("#select :selected").val());
		$('#News').remove();
		$('#toappend').append('<table id="News"><tr><th>Name</th><th>Hash</th><th>Size</th></tr><div id="listInserts"></div></table>');
		$.get('patchlist/getPatchList?ClientLanguage='+$("#select :selected").val(), function(o) {
			for (var i = 0; i < o.length; i++)
			{
				var str = o[i].split(" ");
				$('#News').append('<tr><td>' + str[0] + '</td><td>' + str[1] + '</td><td>' + str[2] + '</td></tr>');
			}

			//$('.del').live('click', function() {
				//delItem = $(this);
				//var id = $(this).attr('rel');

				//$.post('dashboard/xhrDeleteListing', {'id': id}, function(o) {
					//delItem.parent().remove();
				//}, 'json');

				//return false;
			//});
		}, 'json');
	});
});
