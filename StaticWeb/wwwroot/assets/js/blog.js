'use strict';
var Blog = function() {
	// function to handle fluid width Video
	var fluidVideoHandler = function() {
		//video
		var $allVideos = $(".blog-posts iframe, .blog-posts object, .blog-posts embed"), $fluidEl = $(".post-media");

		$allVideos.each(function() {
			$(this)
			// jQuery .data does not work on object/embed elements
			.attr('data-aspectRatio', this.height / this.width).removeAttr('height').removeAttr('width');

		});
		$(window).resize(function() {
			var newWidth = $fluidEl.width();
			$allVideos.each(function() {

				var $el = $(this);
				$el.width(newWidth).height(newWidth * $el.attr('data-aspectRatio'));

			});

		}).resize();
	};

	return {
		init : function() {
			fluidVideoHandler();
		}
	};
}();
