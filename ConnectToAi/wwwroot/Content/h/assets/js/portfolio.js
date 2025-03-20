'use strict';
var Portfolio = function() {

	// function to handle mixItUp Plugin
	var mixItUpHandler = function() {
		$('#Grid').mixItUp();
	};

	return {
		init : function() {
			mixItUpHandler();
		}
	};
}();
