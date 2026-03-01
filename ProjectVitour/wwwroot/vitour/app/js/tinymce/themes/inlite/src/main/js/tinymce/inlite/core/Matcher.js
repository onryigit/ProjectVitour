

define('tinymce/inlite/core/Matcher', [
], function () {
	var result = function (id, rect) {
		return {
			id: id,
			rect: rect
		};
	};
	var match = function (editor, matchers) {
		for (var i = 0; i < matchers.length; i++) {
			var f = matchers[i];
			var result = f(editor);

			if (result) {
				return result;
			}
		}

		return null;
	};

	return {
		match: match,
		result: result
	};
});
