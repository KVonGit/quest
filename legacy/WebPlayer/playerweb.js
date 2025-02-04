﻿$(function () {
    var width = $_GET["w"];
    if (width) {
        setGameWidth(width);
    }
});

function ui_init() {
}

function sendEndWait() {
    window.setTimeout(function () {
        $("#fldUIMsg").val("endwait");
        $("#cmdSubmit").click();
    }, 100);
    waitEnded();
}

function sessionTimeout() {
    disableInterface();
}

function afterSendCommand() {
}

function afterSave() {
}

var _currentPlayer = "";

function playWav(filename, sync, looped) {
    if (!document.createElement('audio').canPlayType) {
        // no <audio> support, so we must play WAVs using <embed> as the 
        // jPlayer Flash fallback does not support WAV.

        var extra = "";

        if (looped) {
            extra = " loop=\"true\"";
        }

        $("#audio_embed").html("<embed src=\"" + filename + "\" autostart=\"true\" width=\"0\" height=\"0\" type=\"audio/wav\"" + extra + ">");
        _currentPlayer = "embed";
    }
    else {
        playAudio(filename, "wav", sync, looped);
    }
}

function playMp3(filename, sync, looped) {
    playAudio(filename, "mp3", sync, looped);
}

var showCommandDiv = isElementVisible("#txtCommandDiv");
function playAudio(filename, format, sync, looped) {
    _currentPlayer = "jplayer";

    $("#jquery_jplayer").unbind($.jPlayer.event.ended);

    if (looped) {
        // This works in Firefox and Chrome.
        endFunction = function () { $("#jquery_jplayer").jPlayer("play"); };
    }
    else if (sync) {
        // Added the following line to set the variable properly. 3-4-2018
		showCommandDiv = isElementVisible("#txtCommandDiv");
        _waitingForSoundToFinish = true;
        // Altered finishSync to use the showCommandDiv variable.
        // It was using txtCommandDiv visibility, which the last line sets to false!
        endFunction = function () { finishSync(showCommandDiv); };
        $("#txtCommandDiv").hide();
    }
    else {
        endFunction = null;
    }

    //$("#jquery_jplayer").bind($.jPlayer.event.error, function (event) { alert(event.jPlayer.error.type); });

    if (endFunction != null) {
        $("#jquery_jplayer").bind($.jPlayer.event.ended, function (event) { endFunction(); });
    }

    if (format == "wav") $("#jquery_jplayer").jPlayer("setMedia", { wav: filename });
    if (format == "mp3") $("#jquery_jplayer").jPlayer("setMedia", { mp3: filename });
    $("#jquery_jplayer").jPlayer("play");
}

function stopAudio() {
    if (_currentPlayer == "jplayer") {
        $("#jquery_jplayer").jPlayer("stop");
    }
    else if (_currentPlayer == "embed") {
        $("#audio_embed").html("");
    }
}

function finishSync(showCommandDiv) {
    _waitingForSoundToFinish = false;
    window.setTimeout(function () {
        if (showCommandDiv) $("#txtCommandDiv").show();
        $("#fldUIMsg").val("endwait");
        $("#cmdSubmit").click();
    }, 100);
}

var platform = "webplayer";