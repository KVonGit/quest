var webPlayer = false;
var canSendCommand = true;

(async function () {
    await CefSharp.BindObjectAsync("questCefInterop");
})();

history.pushState(null, null, null);
window.addEventListener('popstate', function () {
    history.pushState(null, null, null);
});

function sendCommand(text, metadata) {
    markScrollPosition();
    var data = new Object();
    data["command"] = text;
    if (typeof metadata != "undefined") {
        data["metadata"] = metadata;
    }
    UIEvent("RunCommand", JSON.stringify(data));
}

function ASLEvent(event, parameter) {
    UIEvent("ASLEvent", event + ";" + parameter);
}

// RestartGame added by KV
function RestartGame() {
    UIEvent("RestartGame", "");
}

// Write/append to GAMENAME-transcript.txt in Documents\Quest Transcripts
function WriteToTranscript(data) {
  if (data != '' && typeof (data) == 'string') {
    UIEvent("WriteToTranscript", data);
  }
}

// Write/append to GAMENAME-log.txt in Documents\Quest Logs
function WriteToLog(data) {
    if (typeof (data) != 'string') {
        data = "[" + typeof(data) + "]";
    }
    UIEvent("WriteToLog", getTimeAndDateForLog() + " " + data);
}

function goUrl(href) {
    UIEvent("GoURL", href);
}

function sendEndWait() {
    UIEvent("EndWait", "");
}

function doSave() {
    UIEvent("Save", $("#divOutput").html());
}

function UIEvent(cmd, parameter) {
    questCefInterop.uiEvent(cmd, parameter);
}

function disableMainScrollbar() {
    $("body").css("overflow", "hidden");
}

function ui_init() {
    $("#gameTitle").remove();
    $("#cmdExitFullScreen").click(function () {
        UIEvent("ExitFullScreen", "");
    });
}

function updateListEval(listName, listData) {
    updateList(listName, eval("(" + listData + ")"));
}

function showExitFullScreenButton(show) {
    if (eval(show)) {
        $("#cmdExitFullScreen").show();
    }
    else {
        $("#cmdExitFullScreen").hide();
    }
    updateStatusVisibility();
}

function panesVisibleEval(visible) {
    panesVisible(eval(visible));
}

function setCompassDirectionsEval(list) {
    setCompassDirections(eval(list));
}

function selectText(containerid) {
    var range = document.createRange();
    range.selectNodeContents(document.getElementById(containerid));
    var sel = window.getSelection();
    sel.removeAllRanges();
    sel.addRange(range);
}

function writeGameState(key, value) {
    UIEvent("WriteGameState", key + "___STATEDATA___" + value);
}

function readGameState(key) {
    UIEvent("ReadGameState", key);
}

function onGameStateRead(key, value) {
    // This function will be called when state is read
    // Game scripts can override this to handle the response
    if (typeof window.questGameStateCallback === 'function') {
        window.questGameStateCallback(key, value);
    }
}

/* EXAMPLE:

// Save some state
writeGameState("completed_tutorial", "true");
writeGameState("highest_score", "1000");

// Read state (asynchronous)
window.questGameStateCallback = function(key, value) {
    switch(key) {
        case "completed_tutorial":
            if (value === "true") {
                // Skip tutorial
            }
            break;
        case "highest_score":
            var score = parseInt(value) || 0;
            if (score > 1000) {
                // Unlock bonus content
            }
            break;
    }
};
readGameState("completed_tutorial");

*/