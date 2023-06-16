$(document).ready(function ()
{
    $('#cat-dropdown a').on('click', function ()
    {
        var txt = ($(this).text());
        document.getElementById('cat-name-forum-add').value = txt;
    });
});