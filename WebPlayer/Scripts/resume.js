var $_GET = {};
var apiRoot = "https://textadventures.co.uk/";

document.location.search.replace(/\??(?:([^=]+)=([^&]*)&?)/g, function () {
    function decode(s) {
        return decodeURIComponent(s.split("+").join(" "));
    }

    $_GET[decode(arguments[1])] = decode(arguments[2]);
});

$(function () {
    var id = $_GET["id"];
    if(localStorage.hasOwnProperty(id + "-quest-webplayer-save")){

        function base64ToBytes(base64) {
          const binString = atob(base64);
          return Uint8Array.from(binString, (m) => m.codePointAt(0));
        }

        function bytesToBase64(bytes) {
          const binString = Array.from(bytes, (byte) =>
            String.fromCodePoint(byte),
          ).join("");
          return btoa(binString);
        }

        var data = bytesToBase64(new TextEncoder().encode(new TextDecoder().decode(base64ToBytes(localStorage.getItem(id + "-quest-webplayer-save")))));
        var result = {
          "Data": data
        };
        $("#status").text("Loading...");
        //console.log('result:');
        //console.log(result);
        //alert('Loading...');
        $.post("/Resume", result, function() {
          window.location = "/Play.aspx?id=" + id;
        });

    }
    else {
      $("#status").text("No data found.");
      alert('No data found.\n\nRestarting game.');
      $("#status").text("Loading...");
      window.location = "/Play.aspx?id=" + id;
    }

});