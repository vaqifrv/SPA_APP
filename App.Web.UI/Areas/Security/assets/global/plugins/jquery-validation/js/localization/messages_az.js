(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: AZ (Az_Latin; Azərbaycan dili)
 */
$.extend($.validator.messages, {
	required: "Bu sahəni doldurmaq vacibdir.",
	remote: "Lütfən bu sahəni düzəldin.",
	email: "Lütfən doğru bir e-poçt adresi daxil edin.",
	url: "Lütfən doğru bir web adresi (URL) daxil edin.",
	date: "Lütfən doğru bir tarix daxil edin.",
	dateISO: "Lütfən doğru bir tarix daxil edin(ISO formatında).",
	number: "Lütfən doğru bir ədəd daxil edin.",
	digits: "Lütfən sadəcə rəqəmsal simvollar daxil edin.",
	creditcard: "Lütfən doğru bir kredit kartı daxil edin.",
	equalTo: "Lütfən eyni dəyəri təkrar daxil edin.",
	extension: "Lütfən doğru uzantıya sahib bir dəyər daxil edin.",
	maxlength: $.validator.format("Lütfən ən çox {0} simvol uzunluğunda bir dəyər daxil edin."),
	minlength: $.validator.format("Lütfən ən az {0} simvol uzunluğunda bir dəyər daxil edin."),
	rangelength: $.validator.format("Lütfən ən az {0} və ən çox {1} uzunluğunda bir dəyər daxil edin."),
	range: $.validator.format("Lütfən {0} ilə {1} arasında bir dəyər daxil edin."),
	max: $.validator.format("Lütfən {0} dəyərinə bərabər ya da daha kiçik bir dəyər daxil edin."),
	min: $.validator.format("Lütfən {0} dəyərinə bərabər ya da daha böyük bir dəyər daxil edin."),
	require_from_group: "Lütfən bu sahələrin ən az {0} dənəsini doldurun."
});

}));