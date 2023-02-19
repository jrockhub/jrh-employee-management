$(() => {
    if ($('#Jrockhub_User').length !== 0) {
        var table = $('#Jrockhub_User').DataTable({
            
            responsive: true,
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            lengthMenu: [[5, 10, 15, 20], [5, 10, 15, 20]],
            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 text-right"p>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            order: [[2, 'desc']],

            ajax: {
                type: "POST",
                url: '/UserGrid',
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "RequestVerificationToken": token
                },
                data: function (data) {
                    let additionalValues = [];
                    additionalValues[0] = $("#txtC1").val();
                    additionalValues[1] = $("#ddlStatus").val();
                    data.AdditionalValues = additionalValues;

                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    orderable: false,
                    width: 50,
                    data: "Action",

                    render: function (data, type, row) {
                        return `<div>
                                                        <button type="button" class="btn btn-sm btn-info mr-2 btnEdit" data-key="${row.Id}" title="Edit"><i class="far fa-edit"></i></button>
                                                        <button type="button" class="btn btn-sm btn-danger btnDelete" data-key="${row.Id}" title="Delete"><i class="far fa-trash-alt"></i></button>
                                            </div>`;
                    }
                },
                
                {
                    data: "Email",
                    width: 100,
                    name: "co"
                },
                {
                    data: "FirstName",
                    width: 100,
                    name: "co"
                },

                {
                    data: "LastName",
                    width: 100,
                    name: "co"
                },
           
              
                {
                    data: "IsActiveFormatted",
                    render: function (data, type, row) {
                        var returnDiv = "";
                        if (row.IsActiveFormatted == 'Active') {
                            returnDiv += `<button type="button" class="btn btn-sm btn-outline-success" >Active</button>`
                        }
                        else {
                            returnDiv += `<button type="button" class="btn btn-sm btn-outline-danger" >In-Active</button>`
                        };
                        return returnDiv
                    },
                    name: "co",
                    width: 100,
                },
                {
                    data: "Id",
                    name: "eq",
                    visible: false,
                    searchable: false,
                    orderable: false
                },
                
            ]
        });

        table.columns().every(function (index) {
            $('#Jrockhub_User thead tr:last th:eq(' + index + ') input')
                .on('keyup',
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ':visible').search(this.value).draw();
                        }
                    });
        });

       

        $(document)
            .off('click', '.btnEdit')
            .on('click', '.btnEdit', function () {
                const id = $(this).attr('data-key');
                window.location.href = `/UserAdd/Edit/${id}`;
               
            });

        $(document)
            .off('click', '.btnDelete')
            .on('click', '.btnDelete', function () {
                const id = $(this).attr('data-key');

                Swal.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    type: "warning",
                    showCancelButton: !0,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, delete it!",
                    confirmButtonClass: "btn btn-primary",
                    cancelButtonClass: "btn btn-danger ml-1",
                    buttonsStyling: !1
                }).then(function (t) {

                    t.value
                    if (t.value == true) {
                        fetch(`/UserAdd/Delete/${id}`)
                            .then((response) => {
                                Swal.fire({
                                    type: "success",
                                    title: "Deleted!",
                                    text: "Your record has been deleted.",
                                    confirmButtonUser: "btn btn-success"
                                }).then(function (t) {

                                    t.value
                                    if (t.value == true) {
                                        table.ajax.reload();
                                    }

                                });

                            })
                            .catch((error) => {
                                console.log(error);
                            });
                    }

                });

            });

        $('#btnExternalSearch').click(function () {
            table.column('0:visible').search($('#txtC1').val()).draw();
           
        });
        $('#btnResetSearch').click(function () {
            $('#txtC1').val("");
            $('#ddlStatus').val();

            table.column('0:visible').search("").draw();
            table.column('1:visible').search("").draw();

        });
    }
    $('#Usertab ul').attr("style", "display:block");
    $('#Usertabgrid').addClass("active");


});