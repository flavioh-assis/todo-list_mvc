$(document).ready(function() {
	$("#cancelButton").click(function() {
			window.location.href = "/Home/Index";
	});
});

function Redirect(path, id) {
	let route = id ? `${path}/${id}` : path;
	
	window.location.href = route;
}