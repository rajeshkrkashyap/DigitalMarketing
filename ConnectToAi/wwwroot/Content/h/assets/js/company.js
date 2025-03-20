'use strict';
var Company = function() {

	// function to handle slick carousel
	var testimonialCarouselHandler = function() {
		$('#testimonial').slick();
	};
	return {
		init : function() {
			testimonialCarouselHandler();
		}
	};
}();
