$(document).ready(function ()
{
    $(document).ready(function ()
    {
        $("#cat-dropdown").change(function ()
        {
            selectElement = document.querySelector('#cat-dropdown');
            output = selectElement.value;
            document.querySelector('#cat-name-forum-add').value = output;
        });
    });
});