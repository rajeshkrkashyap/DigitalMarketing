'use strict';
var Index = function() {
	var $html = $('html'), $win = $(window), wrap = $('.app-aside'), MEDIAQUERY = {};

	MEDIAQUERY = {
		desktopXL : 1200,
		desktop : 992,
		tablet : 768,
		mobile : 480
	};
	var captionDelay;

	// function to handle swipe slider
	var homeSliderHandler = function() {
		$('.swiper-slider').each(function() {
			var _this = $(this);
			var swiper = new Swiper(_this, {
				pagination : _this.find('.swiper-pagination'),
				paginationClickable : _this.find('.swiper-pagination'),
				loop : true,
				nextButton : _this.find('.swiper-button-next'),
				prevButton : _this.find('.swiper-button-prev'),
				onSlideChangeStart : function(swiper) {
					_this.find('*[data-caption-class]').each(function() {
						var _this = $(this);
						_this.removeClass(_this.attr('data-caption-class')).removeClass('animated');
					});

				},
				onSlideChangeEnd : function(swiper) {

					_this.find('*[data-caption-class]').each(function() {
						var _this = $(this);
						typeof _this.attr('data-caption-delay') !== 'undefined' ? captionDelay = parseInt(_this.attr('data-caption-delay')) : captionDelay = 0;
						setTimeout(function() {
							_this.addClass("animated").addClass(_this.attr('data-caption-class'));
						}, captionDelay);
					});
					_this.find('.swiper-slide').each(function() {
						var _slide = $(this);
						var _slideVideo = _slide.find('video');
						if (_slideVideo.length) {
							if (isMobile()) {
								_slide.css({
									'background-image' : 'url(' + _slideVideo.attr('poster') + ')'
								});
							} else {
								if (_slideVideo.get(0).readyState === 4) {
									if ( typeof _slideVideo.get(0).loop == 'boolean') {// loop supported
										_slideVideo.get(0).loop = true;
									} else {// loop property not supported
										_slideVideo.get(0).on('ended', function() {
											this.currentTime = 0;
											this.play();
										}, false);
									}
									if (_slide.hasClass("swiper-slide-active")) {
										_slideVideo.get(0).currentTime = 1;
										_slideVideo.get(0).play();
									} else {
										_slideVideo.get(0).pause();
										_slideVideo.get(0).currentTime = 1;
									}
								}
							}
						}

					});

				}
			});
		});
		function isMobile() {
			if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
				return true;
			} else {
				return false;
			};
		}

	};

	// function to handle slick carousel
	var homeCarouselHandler = function() {
		$('#portfolio').slick({
			dots : true,
			infinite : false,
			speed : 300,
			slidesToShow : 3,
			slidesToScroll : 3,
			responsive : [{
				breakpoint : 992,
				settings : {
					slidesToShow : 2,
					slidesToScroll : 2,
					infinite : true,
					dots : true
				}
			}, {
				breakpoint : 480,
				settings : {
					slidesToShow : 1,
					slidesToScroll : 1
				}
			}
			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object
			]
		});

		$('#testimonial').slick();

		$('#clients').slick({
			dots : true,
			infinite : false,
			speed : 300,
			slidesToShow : 4,
			slidesToScroll : 4,
			responsive : [{
				breakpoint : 1024,
				settings : {
					slidesToShow : 3,
					slidesToScroll : 3,
					infinite : true,
					dots : true
				}
			}, {
				breakpoint : 600,
				settings : {
					slidesToShow : 2,
					slidesToScroll : 2
				}
			}, {
				breakpoint : 480,
				settings : {
					slidesToShow : 1,
					slidesToScroll : 1
				}
			}
			// You can unslick at a given breakpoint now by adding:
			// settings: "unslick"
			// instead of a settings object
			]
		});
	};
	return {
		init : function() {
			homeSliderHandler();
			homeCarouselHandler();
		}
	};
}();
