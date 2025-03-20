'use strict';
var Animations = function() {

	// function to handle Repeat-Animation Button
	var repeatAnimationHandler = function() {
		$('.repeat-animation').on("click", function(e) {
			var animationClass = firstToLowerCase($(this).closest('section').find('h4').text());

			var imgClass = $(this).closest('section').find('img');
			imgClass.removeClass(animationClass + ' animated');
			setTimeout(function testAnim() {
				imgClass.addClass(animationClass + ' animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function() {
					$(this).removeClass('no-visible');
				});
			}, 100);

			e.preventDefault();
		});
		function firstToLowerCase(str) {
			return str.substr(0, 1).toLowerCase() + str.substr(1);
		}

	};

	return {
		init : function() {
			repeatAnimationHandler();
		}
	};
}();
