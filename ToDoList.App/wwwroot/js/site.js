﻿$(document).ready(function() {
	$("#cancelButton").click(function() {
		Redirect("/");
	});
});

function Redirect(path, id) {
	let route = id ? `${path}/${id}` : path;
	
	window.location.href = route;
}