
var notification = function ($sce) {

    this.showSuccess = function (message, button1Name, button1Link, button2Name, button2Link) {
        var button1 = "";
        var button2 = "";

        if (button1Link && button1Name) {
            button1 = "<a href='" + button1Link + "' class='btn green'>" + button1Name + "</a>";
        }

        if (button2Link && button2Name) {
            button2 = "<a href='" + button2Link + "' class='btn red'>" + button2Name + "</a>";
        }

        var result = "";

        if (button1 || button2) {
            result = $sce.trustAsHtml("<div class='alert alert-success ag-success fade in' style='margin-top:18px;'><a href='#' class='close' data-dismiss='alert' aria-label='close' title='close'>×</a><p style='margin-bottom: 21px;'>" + message + " </p><p>" + button1 + "&nbsp" + button2 + "</p></div>");
        } else {
            result = $sce.trustAsHtml("<div class='alert alert-success ag-success fade in' style='margin-top:18px;'><a href='#' class='close' data-dismiss='alert' aria-label='close' title='close'>×</a><p>" + message + " </p></div>");
        }

        return result;
    }

    this.showError = function (message, button1Name, button1Link, button2Name, button2Link) {
        var button1 = "";
        var button2 = "";

        if (button1Link && button1Name) {
            button1 = "<a href='" + button1Link + "' class='btn green'>" + button1Name + "</a>";
        }

        if (button2Link && button2Name) {
            button2 = "<a href='" + button2Link + "' class='btn red'>" + button2Name + "</a>";
        }
        
        var result = "";

        if (button1 || button2) {
            result = $sce.trustAsHtml("<div class='alert alert-danger fade in' style='margin-top:18px;'><a href='#' class='close' data-dismiss='alert' aria-label='close' title='close'>×</a><p style='margin-bottom: 21px;'>" + message + " </p><p>" + button1 + "&nbsp" + button2 + "</p></div>");
        } else {
            result = $sce.trustAsHtml("<div class='alert alert-danger fade in' style='margin-top:18px;'><a href='#' class='close' data-dismiss='alert' aria-label='close' title='close'>×</a><p>" + message + " </p></div>");
        }

        return result;
    }

    this.scrollTop = function () {
        $('body').animate({ scrollTop: $('#mainBody').offset().top }, 500);
    }

}
