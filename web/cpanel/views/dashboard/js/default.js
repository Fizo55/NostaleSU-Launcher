$(function() {

	$.get('dashboard/xhrGetListings', function(o) {

		for (var i = 0; i < o.length; i++)
		{
			$('#News').append('<tr><td>' + o[i].title + '</td><td>' + o[i].realms + '</td><td>' + '<a class="del" rel="'+o[i].id+'" href="#">Delete</a>' + '</td></tr>');
		}

		$('.del').live('click', function() {
			delItem = $(this);
			var id = $(this).attr('rel');

			$.post('dashboard/xhrDeleteListing', {'id': id}, function(o) {
				delItem.parent().remove();
			}, 'json');

			return false;
		});

	}, 'json');


	$.get('dashboard/xhrGetHotListings', function(o) {

		for (var i = 0; i < o.length; i++)
		{
			$('#HotNews').append('<tr><td>' + o[i].message + '</td><td>' + o[i].realms + '</td><td>' +'<a class="del" rel="'+o[i].id+'" href="#">Delete</a>' + '</td></tr>');
		}

		$('.del').live('click', function() {
			delItem = $(this);
			var id = $(this).attr('rel');

			$.post('dashboard/xhrDeleteHotListing', {'id': id}, function(o) {
				delItem.parent().remove();
			}, 'json');

			return false;
		});

	}, 'json');


	$('#randomHotInsert').submit(function() {
		var url = $(this).attr('action');
		var data = $(this).serialize();
		$.post(url, data, function(o) {
			$('#HotNews').append('<tr><td>' + o.message + '</td><td>' + o.realms + '</td><td>' + '<a class="del" rel="'+ o.id +'" href="#">Delete</a>' + '</td></tr>');
		}, 'json');
		$("#randomHotInsert")[0].reset();

		return false;
	});


	$('#randomInsert').submit(function() {
		var url = $(this).attr('action');
		var data = $(this).serialize();

		$.post(url, data, function(o) {
			$('#News').append('<tr><td>' + o.title + '</td><td>' + o.realms + '</td><td>' + '<a class="del" rel="'+ o.id +'" href="#">Delete</a>' + '</td></tr>');
		}, 'json');
		$("#randomInsert")[0].reset();

		return false;
	});

});
