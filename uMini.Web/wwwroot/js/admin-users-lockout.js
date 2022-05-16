$('#dateTimePicker').datetimepicker({
    format: 'd/m/y H:i',
    step: 1,
});

$('#removeLockoutModal').on('show.mdb.modal', function (event) {
    var button = $(event.relatedTarget);
    var id = button.data("id");
    var modal = $(this);

    modal.find('input[name=userId]').val(id);
});
