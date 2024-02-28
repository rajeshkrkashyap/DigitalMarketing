const chatInput = document.querySelector("#chat-input");
const sendButton = document.querySelector("#send-btn");
const chatContainer = document.querySelector(".chat-container");
const deleteButton = document.querySelector("#delete-btn");
const toipcList = document.querySelector("#toipcList");

let userText = null;

const loadDataFromLocalstorage = () => {
    // Load saved chats and theme from local storage and apply/add on the page
    const themeColor = localStorage.getItem("themeColor");
    document.body.classList.toggle("light-mode", themeColor === "light_mode");

    const defaultText = `<div class="default-text">
                            <h1>Start a conversation</h1>
                        </div>`

    chatContainer.innerHTML = localStorage.getItem("all-chats") || defaultText;
    chatContainer.scrollTo(0, chatContainer.scrollHeight); // Scroll to bottom of the chat container

    let paragraphs = document.querySelectorAll('p');
    paragraphs.forEach(function (paragraph) {
        if (paragraph.innerHTML.trim() === "") {
            paragraph.remove();
        }
    });
}

const createChatElement = (content, className) => {
    // Create new div and apply chat, specified class and set html content of div
    const chatDiv = document.createElement("div");
    chatDiv.classList.add("chat", className);
    chatDiv.innerHTML = content;
    return chatDiv; // Return the created chat div
}

const getImage = async (id) => {

    var urls = [];
    setTimeout(function () {
        urls = [];
        var count = 0;
        let baseUrl = window.location.origin;
        const API_URL = baseUrl + "/student/gpt/GetImageURL";
        $('#' + id).find('img').each(function () {
            const altText = $(this).attr('alt');

            $.ajax({
                type: 'POST',
                url: API_URL, // we are calling json method
                dataType: 'json',
                data: { altPrompt: altText },
                success: function (result) {
                    $('#' + id).find('img[alt="' + result.altText + '"]').attr("src", result.url);
                    urls[count++] = result.url;
                    localStorage.setItem("all-chats", chatContainer.innerHTML);
                },
                error: function (ex) {
                    console.log(ex);
                    alert('Failed to retrieve status.' + ex.responseText);
                }
            });
        });
    },
        200);
    var joinedString = urls.join(',');
}

const getChatResponse = async (incomingChatDiv, userPrompt, childInstructionId) => {
    const inputType = chatInput.getAttribute("inputType");
    let baseUrl = window.location.origin;
    const API_URL = baseUrl + "/student/gpt/SendButtonClicked";  //'@Url.Action("AddTopicContent", "Question")';
    $.ajax({
        type: 'POST',
        url: API_URL,
        dataType: 'json',
        data: {
            inputType: inputType,
            userPrompt: userPrompt,
            childInstructionId: childInstructionId
        },
        success: function (result) {
            var altTexts = result.altTexts;
            var id = generateGUID();
            var response = result.content;

            try {

                var divElement = document.createElement("div");
                divElement.classList.add("copy-content");
                divElement.style.float = "left";

                if (inputType == "image") {
                    divElement.style.fontSize = "initial";
                }

                divElement.marginTop = "-35px";
                divElement.setAttribute("id", id);

                var originalString = response;
                if (originalString.charAt(0) === "'" || originalString.charAt(0) === ",") {
                    originalString = originalString.substr(1);
                }

                if (inputType == "image") {
                    divElement.innerHTML = originalString;
                    window.location.reload();
                } else {
                    divElement.innerHTML = marked(originalString);
                    //typeText(divElement, marked(originalString), originalString.length);
                }

            } catch (error) { // Add error class to the paragraph element and set error text
                divElement.classList.add("error");
                divElement.textContent = "Oops! Something went wrong while retrieving the response. Please try again.";
            }

            // Remove the typing animation, append the paragraph element and save the chats to local storage
            incomingChatDiv.querySelector(".typing-animation").remove();
            incomingChatDiv.querySelector(".chat-details-inner").appendChild(divElement);

            let paragraphs = document.querySelectorAll('p');
            paragraphs.forEach(function (paragraph) {
                if (paragraph.innerHTML.trim() === "") {
                    paragraph.remove();
                }
            });

            if (altTexts != null) {
                getImage(id);
            }
            localStorage.setItem("all-chats", chatContainer.innerHTML);
            chatContainer.scrollTo(0, chatContainer.scrollHeight);

            getSpeechFromAzure(originalString);
        },
        error: function (ex) {
            console.log(ex);
            alert('Failed to retrieve status.' + ex.responseText);
        }
    });

    // Send POST request to API, get response and set the reponse as paragraph element text
}

const copyResponse = (copyBtn, ishtml) => {
    // Copy the text content of the response to the clipboard
    const reponseTextElement = copyBtn.parentElement.querySelector(".copy-content");
    if (ishtml) {
        navigator.clipboard.writeText(reponseTextElement.outerHTML);
    } else {
        navigator.clipboard.writeText(reponseTextElement.textContent);
    }
    copyBtn.textContent = "done";
    setTimeout(() => copyBtn.textContent = "content_copy", 1000);
}

const saveResponse = async (copyBtn, userText) => {
    // Copy the text content of the response to the clipboard
    console.log(copyBtn);
    const reponseTextElement = copyBtn.parentElement.querySelector(".copy-content");
    console.log(reponseTextElement);
    var outerHTML = reponseTextElement.outerHTML;

    let baseUrl = window.location.origin;
    const API_URL = baseUrl + "/student/gpt/AddContentToTopic";  //'@Url.Action("AddTopicContent", "Question")';

    //Ajax Call Start 
    $.ajax({
        type: 'POST',
        url: API_URL, // we are calling json method
        dataType: 'json',
        data: {
            title: userText,
            content: outerHTML
        },
        success: function (result) {

            //const myCookieValue = getCookie('cookie_topics'); // Replace 'myCookie' with your cookie name
            //if (myCookieValue !== null) {
            //    const parsedCookieValue = JSON.parse(myCookieValue);
            //    const name = parsedCookieValue.Name;
            //    console.log("Name:", name);
            //} else {
            //    console.log('Cookie not found');
            //}

            var liItem = `<li>
                            <a href="javascript:;" id='`+ result.Id + `'> <span title=` + result.title + `>` + result.title.substr(0, 23) + `...</span> </a>
                        </li>`;
            var ul = $('#subjectItems').children('ul')

            if ($(ul).find("li[id='" + result.topicId + "']").length == 0) {

                $(ul).append(liItem);
            }

            copyBtn.textContent = "done";
        },
        error: function (ex) {
            console.log(ex);
            alert('Failed to retrieve status.' + ex.responseText);
        }
    });
    //Ajax call end

    setTimeout(() => copyBtn.textContent = "content_copy", 1000);
}

const textToSpeeach = async (speakerBtn, audioPlayId) => {
    // Copy the text content of the response to the clipboard
    const reponseTextElement = speakerBtn.parentElement.querySelector(".copy-content");
    var textContent = reponseTextElement.textContent;
    var audioPlayerControl = document.getElementById(audioPlayId);
    speakerBtn.style.display = "none";
    audioPlayerControl.style.display = "block";
    getSpeechFromAzure(textContent);

    //let baseUrl = window.location.origin;
    //const API_URL = baseUrl + "/student/gpt/Speaker";

    ////Ajax Call Start
    //$.ajax({
    //    type: 'POST',
    //    url: API_URL, // we are calling json method
    //    dataType: 'arraybuffer',
    //    data: {
    //        webText: textContent
    //    },
    //    success: function (result) {
    //        //speakerBtn.textContent = "done";
    //        var audioData = result;
    //        //console.log(result);
    //        console.log('audio');
    //        var blob = new Blob([audioData], { type: "audio/wav" }); // Adjust the content type as per your audio format
    //        var url = URL.createObjectURL(blob);
    //        var audioElement = document.getElementById("audioElement");
    //        audioElement.src = url;
    //        audioElement.load();
    //        audioElement.play();
    //    },
    //    error: function (ex) {
    //        //console.log(ex);
    //        var audioData = ex.responseText;
    //        //console.log(result);
    //        console.log('audio');
    //        var blob = new Blob([audioData], { type: "audio/wav" }); // Adjust the content type as per your audio format
    //        var url = URL.createObjectURL(blob);
    //        var audioElement = document.getElementById("audioElement");
    //        audioElement.src = url;
    //        audioElement.load();
    //        audioElement.play();
    //        console.log('Failed to retrieve status.' + ex.responseText);
    //    }
    //});
    ////Ajax call end

    //setTimeout(() => speakerBtn.textContent = "", 1000);
}

const showTypingAnimation = (userText, childInstructionId) => {
    console.log(userText);
    var id = generateGUID();
    // Display the typing animation and call the getChatResponse function
    const html = `<div class="chat-content">
                    <div class="chat-details">
                        <div class="chat-details-inner" style="float:left">
                            <img src="/assets/images/response.png" alt="chatbot-img" style="float:left;
                            width: 35px;height: 35px;align-self: flex-start;object-fit: cover;border-radius: 2px;">
                            <div class="typing-animation">
                                <div class="typing-dot" style="--delay: 0.2s"></div>
                                <div class="typing-dot" style="--delay: 0.3s"></div>
                                <div class="typing-dot" style="--delay: 0.4s"></div>
                            </div>
                        </div>
                    </div>
                     <span onclick="copyResponse(this, false)" class="material-symbols-rounded span" title="Copy text only">content_copy</span>
                     <span onclick="copyResponse(this, true)" class="material-symbols-rounded span" style="color:#51c6ea;margin-left:5px" title="Copy with html">content_copy</span>
                     <span onclick="saveResponse(this, '`+ userText + `' )" class="material-symbols-rounded span" style="margin-left:5px" title="Save content">save</span>
                     <span onclick="textToSpeeach(this, '`+ id + `')" class="material-symbols-rounded span" style="margin-left:5px;font-size:2em" title="Speaker"></span>
                     <span id='`+ id + `' onclick="playAudio(this)" class="material-symbols-rounded span" style="margin-left:5px;font-size:2em;display:block" title="Speaker">play_pause</span>
                 </div>`;

    // Create an incoming chat div with typing animation and append it to chat container
    const incomingChatDiv = createChatElement(html, "incoming");
    chatContainer.appendChild(incomingChatDiv);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);

    getChatResponse(incomingChatDiv, userText, childInstructionId);
}

var audioElement = document.getElementById("audioElement");
const playAudio = (playBtn) => {
    
    if (playBtn.textContent == 'play_pause') {
        audioElement.pause();
        playBtn.textContent = 'play_arrow'
        console.log('play_arrow');

    } else {
        audioElement.play();
        playBtn.textContent = 'play_pause'
        console.log('play_pause');
    }
}
 
const handleOutgoingTextChat = () => {
    userText = chatInput.value.trim(); // Get chatInput value and remove extra spaces
    if (!userText) return; // If chatInput is empty return from here
    var radios = document.getElementsByName("topicRadio");
    var isTopicSelected = false;
    var childInstructionId = "";
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked == true) {
            isTopicSelected = true;
            childInstructionId = radios[i].getAttribute('id'); // Assuming the attribute is 'data-custom'
        }
    }
    console.log("childInstructionId: " + childInstructionId);
    if (!isTopicSelected) {
        alert("Please choose a topic.");
        $('#appSidebarTopics').click();
        return;
    }

    // Clear the input field and reset its height
    chatInput.value = "";
    chatInput.style.height = `${initialInputHeight}px`;
    var chatId = generateGUID();
    const html = `<div class="chat-content" id="` + chatId + `">
                    <div class="chat-details">
                       <div style="font-size: 1.05rem;word-break: break-word;"> 
                            <img src="/assets/images/avatar-1.png" alt="user-img" style="width: 35px;height: 35px;
                            align-self: flex-start;object-fit: cover;border-radius: 2px;">
                            ${userText}
                        </div>
                    </div>
                </div>`;

    // Create an outgoing chat div with user's message and append it to chat container
    const outgoingChatDiv = createChatElement(html, "outgoing");
    chatContainer.querySelector(".default-text")?.remove();
    chatContainer.appendChild(outgoingChatDiv);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);
    setTimeout(showTypingAnimation(userText, childInstructionId), 500);
}

const handleOutgoingImageChat = () => {
    //fileSelected();
    userText = chatInput.value.trim(); // Get chatInput value and remove extra spaces
    if (!userText) return; // If chatInput is empty return from here
    //console.log(userText);
    var radios = document.getElementsByName("topicRadio");
    var isTopicSelected = false;
    var childInstructionId = "";
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked == true) {
            isTopicSelected = true;
            childInstructionId = radios[i].getAttribute('id'); // Assuming the attribute is 'data-custom'
        }
    }
    console.log("childInstructionId: " + childInstructionId);
    if (!isTopicSelected) {
        alert("Please choose a topic.");
        $('#appSidebarTopics').click();
        return;
    }
    setTimeout(showTypingAnimation(userText, childInstructionId), 500);
}

deleteButton.addEventListener("click", () => {
    // Remove the chats from local storage and call loadDataFromLocalstorage function
    if (confirm("Are you sure you want to delete all the chats?")) {
        localStorage.removeItem("all-chats");
        loadDataFromLocalstorage();
    }
});

const initialInputHeight = chatInput.scrollHeight;

chatInput.addEventListener("input", () => {
    // Adjust the height of the input field dynamically based on its content
    chatInput.style.height = `${initialInputHeight}px`;
    chatInput.style.height = `${chatInput.scrollHeight}px`;
});

chatInput.addEventListener("keydown", (e) => {
    // If the Enter key is pressed without Shift and the window width is larger 
    // than 800 pixels, handle the outgoing chat
    if (e.key === "Enter" && !e.shiftKey && window.innerWidth > 800) {
        e.preventDefault();
        handleOutgoingTextChat();
    }
});

loadDataFromLocalstorage();

if (sendButton.title == "text") {
    sendButton.addEventListener("click", handleOutgoingTextChat);
}

if (sendButton.title == "image") {
    sendButton.addEventListener("click", handleOutgoingImageChat);
}

if (sendButton.title == "audio") {
    console.log("audio");
    recording();
}
function generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        let r = Math.random() * 16 | 0,
            v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function fileSelected(_formData) {
    // Your code here
    console.log("File Start Processing!");

    var radios = document.getElementsByName("topicRadio");
    var isTopicSelected = false;
    var childInstructionId = "";
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked == true) {
            isTopicSelected = true;
            childInstructionId = radios[i].getAttribute('id'); // Assuming the attribute is 'data-custom'
        }
    }
    console.log("childInstructionId: " + childInstructionId);
    if (!isTopicSelected) {
        alert("Please choose a topic.");
        $('#appSidebarTopics').click();
        return;
    }

    var formData = new FormData();
    formData = _formData; //formData.append("file", $("#fileInput")[0].files[0]);

    let baseUrl = window.location.origin;
    const API_URL = baseUrl + "/student/gpt/UploadAction";

    chatInput.value = "";
    chatInput.style.height = `${initialInputHeight}px`;
    var chatId = generateGUID();
    const html = `<div class="chat-content" id="` + chatId + `">
                            <div class="chat-details">
                               <div style="font-size: 1.05rem;word-break: break-word;">
                                    <img src="/assets/images/avatar-1.png" alt="user-img" style="width: 35px;height: 35px;
                                    align-self: flex-start;object-fit: cover;border-radius: 2px;">
                                     <div class="typing-animation">
                                        <div class="typing-dot" style="--delay: 0.2s"></div>
                                        <div class="typing-dot" style="--delay: 0.3s"></div>
                                        <div class="typing-dot" style="--delay: 0.4s"></div>
                                     </div>
                                    <img src="" id="promptImage"/>
                                </div>
                            </div>
                        </div>`;

    // Create an outgoing chat div with user's message and append it to chat container
    const outgoingChatDiv = createChatElement(html, "outgoing");
    chatContainer.querySelector(".default-text")?.remove();
    chatContainer.appendChild(outgoingChatDiv);
    chatContainer.scrollTo(0, chatContainer.scrollHeight);

    $.ajax({
        type: 'POST',
        url: API_URL, //'@Url.Action("UploadAction", "Gpt")', // we are calling json method
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log("File uploaded successfully:", response);
            //if (response.success) {
            // Clear the input field and reset its height
            var blobPath = response.blobPath;
            //console.log(response.blobPath);
            //console.log(response.imageContent);
            $('#chat-input').val(response.imageContent);
            $('#' + chatId).find("[id='promptImage']").attr("src", response.blobPath);
            chatContainer.querySelector(".typing-animation")?.remove();
            handleOutgoingImageChat();

            //}
            //Perform actions after successful upload
        },
        error: function (xhr, status, error) {
            console.error("Error uploading file:", error);
            // Handle upload error
        }
    });
}

function recording() {
    let recognition = null;
    let recordedChunks = [];
    let audioPlayer = document.getElementById("recordedAudio"); // Get the audio player element

    document.getElementById("startRecording").addEventListener("click", () => {
        flag = true;
        $('#startRecording').hide();
        $('#stopRecording').show();
        $('#iRecording').show();
        setTimeout(initFunction(), 300);
    });

    var flag = true;

    function initFunction() {
        //var promtQueryText = document.getElementById("chat-input");
        chatInput.value = "";

        window.SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        recognition = new SpeechRecognition();
        recognition.interimResults = true;

        recognition.addEventListener("result", (e) => {
            var text = Array.from(e.results)
                .map((result) => result[0])
                .map((result) => result.transcript)
                .join("");

            //console.log(text);

            if (e.results[0].isFinal) {
                console.log(e);
                if (text.toLocaleLowerCase() == "clear recording text") {
                    chatInput.value = "";
                } else {
                    chatInput.value += text + " ";
                }
            }
        });

        recognition.addEventListener("end", () => {
            if (flag) {
                recognition.start();
            } else {

            }
        });
        recognition.start();
    }

    document.getElementById("stopRecording").addEventListener("click", () => {
        $('#startRecording').show();
        $('#stopRecording').hide();
        $('#iRecording').hide();
        recognition.stop();
        //console.log("Logging Audio Text");
        handleOutgoingTextChat();
        flag = false;
    });
}

async function typeText(divElement, textToType, contentLenght) {
    console.log(textToType);
    index = 0;
    console.log($(textToType).length);
    if (index < $(textToType).length) {
        let currentContent = '';
        for (var i = 0; i < contentLenght; i++) {

            console.log(textToType.charAt(index));
            currentContent += textToType.charAt(index);
            if (currentContent.endsWith('>')) {
                divElement.innerHTML = currentContent;
            }
            //divElement.innerHTML += textToType.charAt(index);
            index++;
            await sleep(100);
        }
    }
}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
