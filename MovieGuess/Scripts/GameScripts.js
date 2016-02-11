$(document).ready(function () {
    var chat = $.connection.gameHub;
    //playerName = prompt('Username please', 'ThugQueen' + Math.floor((Math.random() * 100) + 1));
    $.connection.hub.start()
    //$.connection.hub.start().done(function () {
    //    chat.server.setUserName('ThugQueen' + Math.floor((Math.random() * 100) + 1));
    //});
    messages = 0;
    chat.client.BroadcastMessage = function (name, message) {
        //$('#chat').append(name + ': ' + message + '<br>');
        var m = $('<div class="chat-msg" id="msg' + messages + '"><p>' + name + ':</p><span>' + message + '</span></div>')
        addToChat(m);
    }

    chat.client.addClue = function (property, value) {
        $('#' + property).html(value).hide().fadeIn(200);
    }
    chat.client.startCountdown = function (text, milliSeconds) {
        $('#timer-bar').stop();
        $('#timer-bar').css('width', '100%')
        $('#timer-text').html(text);
        $('#timer-bar').animate({ width: 0 }, milliSeconds, 'linear');
    }
    chat.client.resetClues = function () {
        $('.clue').html('?');
    }

    function addToChat(item) {
        messages++;
        $('#chat').append(item.fadeIn(100));
        if (messages >= 15) {
            $('#msg' + (messages - 15)).fadeOut(300, function () { this.remove(); });
        }
    }
    chat.client.announceWinner = function (winnerName) {
        var d = $('<div class="chat-msg-winner" id="msg' + messages + '"><span>' + (winnerName != "" ? winnerName + ' won' : 'Game Over') + '</span></div>');
        //$('.big-container').animate({ opacy: 1 }, 50, function () { $('.big-container').animate({ opacy: 0 }, 50) });
        addToChat(d);

        var $el = $(".big-container");

        $el.css("background", "#191c1e");

        setTimeout(function () {
            $el.css("background", "transparent");
        }, 275);
    }
    

    $('#send').on('click', function () {
        //chat.server.send(name, $('#message').val());
        chat.server.addChatMessage($('#message').val());
        $('#message').val('').focus();
    })

    $('#cheat').on('click', function () {
        chat.server.cheat("you");
    })

    $('#message').keyup(function (e) {
        if (e.keyCode == 13) {
            $('#send').click();
        }
    });
    $('#username').keyup(function (e) {
        if (e.keyCode == 13) {
            $('#join').click();
        }
    });

    $('#join').on('click', function () {
        chat.server.setUserName($('#username').val());
        $('#login').fadeOut(100, function () { $('#chat-input').fadeIn(100); $('#message').focus() });
    })
})
