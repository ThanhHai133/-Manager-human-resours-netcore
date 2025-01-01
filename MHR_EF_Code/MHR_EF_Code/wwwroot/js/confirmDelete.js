function confirmDelete(id, entityType, deleteUrl) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This action cannot be undone.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Điều hướng đến URL xóa với các tham số id và entityType
            window.location.href = deleteUrl + '?id=' + id + '&entityType=' + entityType;
        }
    });
}
