$('#removeRoleModal').on('show.mdb.modal', function (event) {
    var button = $(event.relatedTarget);
    var role = button.data("role");
    var modal = $(this);

    modal.find('input[name=role]').val(role);
});