$('#deleteModal').on('show.mdb.modal', function (event) {
    var button = $(event.relatedTarget);
    var id = button.data("id");
    var modal = $(this);

    modal.find('input[name=key]').val(id);
});
