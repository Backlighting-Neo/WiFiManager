$(document).ready(function(){
	$('a.header-logo').mousedown(function(){
		window.external.FormMouseDown();
	})

	$('#GithubFork').click(function(){
		window.external.GetGithub();
	})
})

