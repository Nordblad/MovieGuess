$('#search').on('click', function () {
    $.get('http://www.omdbapi.com/?t=' + $('#title').val() + '&y=&plot=short&r=json', function (result) {
        if (result['Response'] == 'False') {
            alert("Ingen sån");
        }
        else {
            $('#result').html(result.Title + ': ' + result['Year']);
            movie = result;
            movie.imdbRating = parseFloat(result.imdbRating) * 10;
            ko.applyBindings(movie); //Knockout!!
        }
    })
})

$('#addbtn').on('click', function () {
    $.post('/Admin/AddMovie', movie, function (data) {
        if (data.success) {
            var listItem = document.createElement('li');
            $(listItem).html('<p>' + movie.Title + '</p><div>' + movie.Year + ' - ' + movie.Genre + '</div>').hide();
            $('.movie-list').prepend(listItem);
            $(listItem).fadeIn('fast');
            //$('.movie-list').prepend('<li><p>' + movie.Title + '</p><div>' + movie.Year + ' - ' + movie.Genre + '</div></li>');
        }
        showMessage(data.success, data.message);
    });
})

function showMessage(success, message) {
    if (success == true) {
        $("#message").css('background-color', 'lightgreen');
    }
    else {
        $("#message").css('background-color', 'red');
    }
    $("#message").html(message);
    $("#message").show();
}

$('#title').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#search').click();
        return false;
    }
});