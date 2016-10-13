// Write your Javascript code.

function removePrompt(callback) {
    BootstrapDialog.show({
        title: "Verwijderen",
        message: "Weet je het zeker?",
        closable: false,
        buttons: [
        {
            label: "Ja",
            cssClass: "btn btn-success",
            action: function (dialog) {
                dialog.close();
                callback(true);
            }
        },
        {
            label: "Nee!",
            cssClass: "btn btn-danger",
            action: function (dialog) {
                dialog.close();
                callback(false);
            }
        }]
    });
}

function copyToClipBoard(inputElement) {
    var inputField = $(inputElement);
    inputField.select();
    // Copy to the clipboard
    document.execCommand('copy');
}