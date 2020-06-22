var dataTable;

$(document).ready(function () {
    
    loadDataTable();

});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({

        "ajax": {
            
            "url": "GetListUsers",
            "type": "GET",
            "datatype": "Json"
        },

        "columns": [
            {"data": "name", "width": "20%" }, 
            {"data": "email", "width": "20%" },
            {"data": "isVerified", "width": "20%" },
            {"data": "emailConfirmed", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/admin/Approve/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class="fa fa-check" aria-hidden="true"></i></a>
                                    &nbsp;
                                <a onclick=Delete("/admin/DeleteUser/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "20%"
            }
        ]

    });
}


function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data",
        icon: "Warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }

    });
} 