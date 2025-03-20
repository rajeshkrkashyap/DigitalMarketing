// Initialize a flag to avoid validation on page load
var isPageLoad = true;
// Variables to track time and question
var questionStartTime = null; // When user enters a question
var currentQuestionId = null; // ID of the current question
var testPaperId = "";
// Handle the first step navigation on page load, with no validation
setTimeout(function () {
    var firstStepButton = $("#AllStepButton").find("a").eq(0);
    var goToStepNumber = firstStepButton.attr('questionCount');
    var questionId = firstStepButton.attr('id');
    console.log("setTimeout");
    testPaperId = firstStepButton.attr('testPaperId');
    handleServerCall("GoToStep", goToStepNumber, questionId);
    isPageLoad = false; // Set flag to false after initial load
}, 4000);

// Button click event handlers
$("#btnSaveNext").click(function () {
    console.log("SaveAndNext");
    handleServerCall("SaveAndNext");
});

$("#btnSaveMarkForReview").click(function () {
    console.log("SaveMarkForReview");
    handleServerCall("SaveMarkForReview");
});

$("#btnNext").click(function () {
    console.log("Next");
    handleServerCall("Next");
});

$("#btnBack").click(function () {
    console.log("Back");
    handleServerCall("Back");
});

$(".my-go-ToStep").click(function () {
    var goToStepNumber = $(this).attr('questionCount');
    var questionId = $(this).attr('id');
    console.log("GoToStep");
    handleServerCall("GoToStep", goToStepNumber, questionId);
});

$("#btnClearResponse").click(function () {
    console.log("ClearResponse");
    handleServerCall("ClearResponse");
});

$("#btnMarkForReviewAndNext").click(function () {
    console.log("markedforReview");
    handleServerCall("markedforReview");
});

// Main server call function
function handleServerCall(functionName, goToStepNumber, questionId) {
    console.log("Function is: handleServerCall");

    var currentDiv = "";

    if ((questionId != null || questionId != undefined) && questionId.length > 5) {
        currentDiv = $('div[questionId=' + questionId + '][title="Question"]');
    } else {
        console.log("questionId is null: " + questionId);
        currentDiv = $('div[title="Question"][style="display: block;"]');
    }

    // Determine next or previous question for navigation
    if (functionName === "Next") {
        currentDiv = $(currentDiv).next();
    } else if (functionName === "Back") {
        currentDiv = $(currentDiv).prev();
    }

    //var testPaperId = $(currentDiv).attr("testPaperId");

    console.log("testPaperId: " + testPaperId);

    // Calculate time spent on the current question
    if (currentQuestionId && questionStartTime) {
        var timeSpent = Math.floor((new Date() - questionStartTime) / 1000); // Time in seconds

        // Send the time tracking data to the server
        trackQuestionTime(testPaperId, currentQuestionId, timeSpent);
        console.log("trackQuestionTime is called");
    }

    //var studentPackageId = $(currentDiv).attr("StudentPackageId");
    var questionDiv = $(currentDiv).find("div[qid]");
    currentQuestionId = questionId || $(questionDiv).attr("qid");

    console.log("currentQuestionId: " + currentQuestionId);

    var answerIds = getAnswerIds(currentDiv);
    console.log("answerIds: " + answerIds);

    questionStartTime = new Date(); // Reset start time

    if (["SaveAndNext", "SaveMarkForReview"].includes(functionName)) {

        var questionData = {
            "questionId": currentQuestionId,
            "selectedOptionsId": answerIds //This array will contain multiple selected option IDs
        };

        var cookieKeyForData = currentQuestionId + "_data";

        console.log("questionData In JSON: " + JSON.stringify(questionData));

        CookieManager.setCookie(cookieKeyForData, JSON.stringify(questionData), 1);
    }

    // Only validate for SaveAndNext and SaveMarkForReview actions
    if (["SaveAndNext", "SaveMarkForReview"].includes(functionName) && !validateAnswer(currentDiv, answerIds)) {
        return;
    }

    if (functionName === "markedforReview") {
        var isAnyChecked = false;
        var checkBoxList = $(currentDiv).find("input:checkbox");
        checkBoxList.each(function () {
            if ($(this).prop("checked")) {
                isAnyChecked = true; // Found a checked checkbox
                return false; // Exit the loop early
            }
        });
        if (isAnyChecked) {
            $("#markedforReviewModal").modal(); // Show modal for missing selection
            return false;
        }
    }

    if (functionName === "ClearResponse") {

        //var questionData = {
        //    "questionId": currentQuestionId,
        //    "selectedOptionsId": "" // clear response
        //};

        //CookieManager.setCookie(cookieKeyForData, JSON.stringify(questionData), 1);

    }
    // Perform the server call
    sendRequest(functionName, testPaperId, currentQuestionId, answerIds, goToStepNumber, currentDiv);

    if (functionName === "ClearResponse" || functionName === "markedforReview" || functionName === "Next") {

        var checkBoxList = $(currentDiv).find("input:checkbox");
        // Iterate through each checkbox and uncheck it
        checkBoxList.each(function () {
            $(this).prop('checked', false); // Uncheck the checkbox
        });
    }
}

//Add a helper function trackQuestionTime to send the tracked time for each question:
function trackQuestionTime(testPaperId, questionId, timeSpent) {
    var URL = "/Test/TrackQuestionTime"; // Replace with your API endpoint

    // Make an AJAX request to save time spent on the server
    $.ajax({
        type: 'POST',
        url: URL,
        dataType: 'text',
        data: {
            testPaperId: testPaperId,
            questionId: questionId,
            timeSpentInSeconds: timeSpent
        },
        success: function (result) {
            console.log("Time tracking saved successfully for question:", questionId);
        },
        error: function (ex) {
            console.error("Error saving time tracking:", ex.responseText);
        }
    });
}


// Retrieve selected answers
function getAnswerIds(currentDiv) {
    // Find all checkboxes that are checked
    var selectedCheckboxes = $(currentDiv).find("input[type='checkbox'][name='IsOption']:checked");
    // Map the checked checkboxes to their IDs, join them with a comma
    var ids = selectedCheckboxes.map(function () {
        return this.id;
    }).get().join(",");

    // If no checkbox is selected, return "-1"
    return ids || "-1";
}


// Validate answer inputs for SaveAndNext and SaveMarkForReview
function validateAnswer(currentDiv, answerIds) {
    if (answerIds === "-1" && !isPageLoad) {
        if ($(currentDiv).find("input:checkbox").length > 0) {
            $("#myModal").modal(); // Show modal for missing selection
            return false;
        } else if ($(currentDiv).find("input[type='number']").length === 1) {
            var numberValue = $(currentDiv).find("input[type='number']").val();
            if (!numberValue) {
                $("#IntegerModal").modal(); // Show modal for missing input
                return false;
            }
        }
    }
    return true;
}

// Function to perform the AJAX request
function sendRequest(functionName, testPaperId, questionId, answerIds, goToStepNumber, currentDiv) {
    var URL = "/Test/StartPageButtonAcctions";
    var wizardContent = $('#wizard');
    var cookieKeyForStatus = questionId + "_status";//cookie key

    var questionStatus = CookieManager.getCookie(cookieKeyForStatus);
    console.log("questionStatus: " + questionStatus);

    $.ajax({
        type: 'POST',
        url: URL,
        dataType: 'text',
        data: {
            testPaperId: testPaperId,
            questionId: questionId,
            answerIds: answerIds,
            functionName: functionName
        },
        success: function (result) {
            if (result == "success") {
                if (functionName != "ClearResponse") {
                    if (questionId == undefined) {
                        $("#ExamOverModal").modal(); // Show modal for missing selection
                    }
                    navigateWizard(functionName, goToStepNumber, wizardContent);
                }
                updateButtonState(testPaperId, functionName, questionId, questionStatus);
                updateOptionsState(testPaperId, questionId, currentDiv, functionName);
            }
        },
        error: function (ex) {
            console.log("error: " + ex.responseText);
            if (functionName != "ClearResponse") {
                if (questionId == undefined) {
                    $("#ExamOverModal").modal(); // Show modal for missing selection
                }
                navigateWizard(functionName, goToStepNumber, wizardContent);
            }
            updateButtonState(testPaperId, functionName, questionId, questionStatus);
            updateOptionsState(testPaperId, questionId, currentDiv, functionName);
            //alert('Failed to retrieve states. ' + ex.responseText);
        }
    });
}

// Navigate wizard based on function action
function navigateWizard(functionName, goToStepNumber, wizardContent) {
    if (functionName === "Back") {
        wizardContent.smartWizard("goBackward");
    } else if (functionName === "GoToStep") {
        wizardContent.smartWizard("goToStep", goToStepNumber);
    } else {
        wizardContent.smartWizard("goForward");
    }
}
function updateOptionsState(testPaperId, questionId, currentDiv, functionName) {

    var cookieKeyForData = questionId + "_data";

    if (CookieManager.hasCookie(cookieKeyForData)) {

        var questionData = CookieManager.getCookie(cookieKeyForData);
        try {
            console.log("questionData: " + questionData);

            // Step 2: Parse the JSON string into a JavaScript object
            var parsedQuestionData = JSON.parse(questionData);
            console.log("parsedQuestionData:  " + parsedQuestionData);
            // Step 3: Now you can access the properties
            var questionId = parsedQuestionData.questionId;
            var selectedOptions = parsedQuestionData.selectedOptionsId;
            selectedOptions = JSON.parse(selectedOptions); // Convert string to array if it's in JSON format
        } catch (e) {
            console.error("Parsing error:", e);
        }
        var checkBoxList = $(currentDiv).find("input[type='checkbox'][name='IsOption']");
        checkBoxList.prop('checked', false);

        if (functionName != "ClearResponse") {

            if (Array.isArray(selectedOptions)) {
                // Iterate over each option in selectedOptions and check the matching checkbox
                selectedOptions.forEach(function (optionId) {
                    // Check the checkbox with the matching ID
                    checkBoxList.filter('#' + optionId).prop('checked', true);
                });
            } else {
                checkBoxList.filter('#' + selectedOptions).prop('checked', true);
            }
        } else {
            var cookieKeyForStatus = questionId + "_status";//cookie key
            if (CookieManager.hasCookie(cookieKeyForStatus) && CookieManager.hasCookie(cookieKeyForData)) {
                CookieManager.deleteCookie(cookieKeyForData);
                CookieManager.setCookie(cookieKeyForStatus, "Visited", 1);
            }
        }
    }
}

// Update button state based on action taken
function updateButtonState(testPaperId, functionName, questionId, questionStatus) {

    var cookieKeyForStatus = questionId + "_status"; // cookie Key

    var questionButton = $("#AllStepButton").find("#" + questionId).eq(0);

    if (functionName === "SaveAndNext") {
        questionButton.removeClass("btn-primary Visited answeredMarkedForReview markedforReview").addClass("answered");
        CookieManager.setCookie(cookieKeyForStatus, "answered", 1);
        UpdateStausOfNextButton(testPaperId, questionButton);
    } else if (functionName === "SaveMarkForReview") {
        questionButton.removeClass("btn-primary answered Visited markedforReview").addClass("answeredMarkedForReview");
        CookieManager.setCookie(cookieKeyForStatus, "answeredMarkedForReview", 1);
        UpdateStausOfNextButton(testPaperId, questionButton);
    } else if (functionName === "markedforReview") {
        questionButton.removeClass("btn-primary answered Visited answeredMarkedForReview").addClass("markedforReview");
        CookieManager.setCookie(cookieKeyForStatus, "markedforReview", 1);
        UpdateStausOfNextButton(testPaperId, questionButton);
    } else if (functionName === "ClearResponse") {
        questionButton.removeClass("answered answeredMarkedForReview markedforReview").addClass("Visited");
        CookieManager.deleteCookie(cookieKeyForStatus);
        CookieManager.setCookie(cookieKeyForStatus, "Visited", 1);
    }
    else {
        if (!questionButton.hasClass("Visited")) {
            if (questionStatus == "" || questionStatus == null || questionStatus == "Visited") {
                questionButton.removeClass("btn-primary").addClass("Visited");
                CookieManager.setCookie(cookieKeyForStatus, "Visited", 1);
            }
        }
    }
}
function UpdateStausOfNextButton(testPaperId, questionButton) {
    if (questionButton.length > 0) {
        var questionCounter = $(questionButton).attr('questioncount');
        questionCounter = parseInt(questionCounter) + 1;
        var nextQuestionButton = $("#AllStepButton").find("a[questioncount='" + questionCounter + "']").eq(0);

        if ($(nextQuestionButton).length > 0 && nextQuestionButton.hasClass("btn-primary")) {
            nextQuestionButton.removeClass("btn-primary").addClass("Visited");
            var questionId = $(nextQuestionButton).attr('id');
            var cookieKeyForStatus = questionId + "_status"; // cookie Key
            CookieManager.setCookie(cookieKeyForStatus, "Visited", 1);
        }
    }
}
