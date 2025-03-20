
$("#btnNext").click(function () {
    ServerCalls("Next");
});
$("#btnBack").click(function () {
    ServerCalls("Back");
});

$(".my-go-ToStep").click(function () {
    var goToStepNumer = $(this).attr('questionCount');
    var questionId = $(this).attr('id');
    ServerCalls("GoToStep", goToStepNumer, questionId);
});


function ServerCalls(_functionName, goToStepNumer, questionId) {

    var wizardDiv = $('DIV[title="Question"][style*="display: block"]')

    if (_functionName == "Next") {
        wizardDiv = wizardDiv.next();
    }
    if (_functionName == "Back") {
        wizardDiv = wizardDiv.prev();
    }

    var _testPaperId = $(wizardDiv).attr("testPaperId");
    var _StudentPackageId = $(wizardDiv).attr("StudentPackageId");

    var questionDiv = $(wizardDiv).eq(0).find("DIV[qId]");

    var _questionId = -1;

    if (_functionName == "GoToStep") {
        //_questionId = goToStepNumer;
        _questionId = questionId;
    } else {
        _questionId = $(questionDiv).attr("qId");
    }

    var checkboxlist = $(wizardDiv).find("input:checkbox");
    var _answerId = "-1";

    var ids = "";
    var count = 0;
    $.each(checkboxlist, function (i, checkbox) {

        if ($(this).is(":checked")) {
            if (count == 0) {
                ids = $(this).prop("id");
            } else {
                ids = ids + "," + $(this).prop("id");
            }
            count++;
        }
        _answerId = ids;
    });

    if (_functionName == "Next" || _functionName == "Back" || _functionName == "GoToStep" || _functionName == "MarkForReviewAndNext") {
        _answerId = "-2";
    }

    if (_functionName == "ClearResponse") {

        $.each(checkboxlist, function (i, checkbox) {
            if ($(this).is(":checked")) {
                $(this).removeAttr("checked");
            }
        });
    }

    if ((_answerId.length == 0 || _answerId == "-1") && _functionName != "ClearResponse") {
        $("#myModal").modal();//alert("Please choose a option!");
        return;
    }
    var URL = "/Student/Test/StartPageButtonAcctions";

    var wizardContent = $('#wizard');



    if (_functionName == "Back") {
        wizardContent.smartWizard("goBackward");
    } else if (_functionName == "GoToStep" || _functionName == "ClearResponse") {
        wizardContent.smartWizard("goToStep", goToStepNumer);
    } else {
        wizardContent.smartWizard("goForward");
    }

    var questionButton = $("#AllStepButton").find("#" + _questionId).eq(0);

    if (_functionName == "Next" || _functionName == "Back" || _functionName == "GoToStep") {
        if (!questionButton.hasClass("Visited")) {
            questionButton.removeClass("btn-primary").addClass("Visited");
        }
    }

    $('DIV[title="Question"][style*="display: block"]').focus();


}