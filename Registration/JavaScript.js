$(document).ready(function () {
    // Applying Select2 to your DropDownList with custom options
    $('#ddlInstituteType').select2({
        placeholder: '--select--', // Placeholder text
        allowClear: true, // Allow clearing the selected option
        
        //theme: 'classic', You can change the theme to 'bootstrap', 'classic', etc.
    });

   


    // Reinitialize Select2 after UpdatePanel partial postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {

        setTimeout(function () {

            // vendor
            $('#ddlInstituteType').select2({
                theme: 'classic',
                placeholder: 'Select here.....',
                allowClear: false,
            });

          


        }, 0);
    });


    
    // Attaching a change event handler to trigger the postback
    $('#ddlInstituteType').on('select2:select', function (e) {
        __doPostBack('<%= ddlInstituteType.ClientID %>', ''); // Trigger postback
    });
});