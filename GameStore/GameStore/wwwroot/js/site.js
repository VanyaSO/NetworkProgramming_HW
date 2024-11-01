$(document).ready(function () {
    // Подтверждение и удаление
    let productId;
    
    $('#confirmDeleteModal').on('show.bs.modal', function (event) {
        productId = $(event.relatedTarget).data('product-id');
        $(event.target).find('#confirmDeleteModalBody').text(`Вы действительно хотите удалить обьект под номером ${productId} ?`);
    });

    $('#confirmDeleteButton').on('click', function () {
        if (!productId) return;
        $(`#deleteForm-${productId}`).submit();
    });
    
    
});
