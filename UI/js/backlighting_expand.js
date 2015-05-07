$(document).ready(function(){
	$('a.header-logo').mousedown(function(){
		window.external.FormMouseDown();
	})

	$('#GithubFork').click(function(){
		window.external.GetGithub();
	})

	$('#GithubForkMenu').click(function(){
		window.external.GetGithub();
	})

	$('#CloseWindow').click(function(){
		window.external.CloseWindow();
	})
})


