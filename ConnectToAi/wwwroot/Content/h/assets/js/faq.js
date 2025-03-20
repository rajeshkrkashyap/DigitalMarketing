'use strict';
var Faq = function() {

	// function to handle Affix
	var affixFaqHandler = function() {
		var offsetTopFn = function() {

			var h = $('#page-title').outerHeight(true) + 60;

			return h;
		};
		var offsetBottomFn = function() {

			var h = -($(document).outerHeight(true) - $(window).outerHeight(true) - $('#footer').outerHeight(true));

			return h;
		};
		$('.faq-sidebar').affix({
			offset : {
				top : offsetTopFn,
				bottom : offsetBottomFn

			}

		});
	};
	// function to check an href for an anchor. If exists scroll to it.
	var anchorHandler = function() {

		var activeAnchor = $('.active-anchor');

		if (activeAnchor.length) {
			activeAnchor.on('click', function() {
				var _this = $(this), anchorHref = _this.attr("href"), $target = $(anchorHref);
				$('html, body').animate({
					scrollTop : $target.offset().top
				});

			});
		}

	};
	return {
		init : function() {
			affixFaqHandler();
			anchorHandler();
		}
	};
}();
