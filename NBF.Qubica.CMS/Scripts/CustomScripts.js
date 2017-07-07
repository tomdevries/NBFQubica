$(function () {
    $('.checkall').on('click', function () {
        $(this).closest('fieldset').find(':checkbox').prop('checked', this.checked);
    });
});

$(".allcheckboxes").click(function () {
    if (!this.checked) {
        $(".checkall").attr('checked', false);
    }
    else if (($(".allcheckboxes").length / 2) == $(".allcheckboxes:checked").length) {
        $(".checkall").attr('checked', true);
    }
});