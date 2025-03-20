var TextEditor = function() {"use strict";
	//function to initiate ckeditor
	var ckEditorHandler = function() {
		//CKEDITOR.disableAutoInline = true;
		$('textarea.ckeditor').ckeditor();
	};
	var ckEditorHandlerWithoutControls = function () {
		CKEDITOR.disableAutoInline = true;
		$('textarea.ckeditor').ckeditor({
			toolbar: [
				'bold', 'italic', '|', 'undo', 'redo'
			]
		})
			.catch(error => {
				console.error(error);
			});
	};
	var ckEditorHandlerInline = function (id) {
		CKEDITOR.disableAutoInline = true;
		CKEDITOR.inline(id);
	};
	return {
		//main function to initiate template pages
		init: function() {
			ckEditorHandler();
		},
		//main function to initiate template pages
		initWithoutControls: function () {
			ckEditorHandlerWithoutControls();
		},
		//main function to initiate template pages
		initEditorInline: function (id) {
			ckEditorHandlerInline(id);
		}
	};
}();
